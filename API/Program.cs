using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using API.Conventions;
using API.Middleware;
using Application;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.Repositories;
using PublicTransportService.Infrastructure;
using PublicTransportService.Infrastructure.Data;
using Shared.Enums.ErrorCodes;
using Shared.Enums.ErrorCodes.Auth;
using Shared.JsonConverters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transport API",
        Version = "v1",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    options.UseOneOfForPolymorphism();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Your Bearer token (without 'Bearer' prefix)",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            },
            Array.Empty<string>()
        },
    });
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddPTSInfrastructure(builder.Configuration);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFriendInvitationRepository, FriendInvitationRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupPathRepository, GroupPathRepository>();
builder.Services.AddScoped<IFriendRepository, FriendRepository>();
builder.Services.AddScoped<IProposedPathRepository, ProposedPathRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUserConfigurationRepository, UserConfigurationRepository>();
builder.Services.AddScoped<IUserLocationRepository, UserLocationRepository>();
builder.Services.AddScoped<IGroupInvitationRepository, GroupInvitationRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<IFriendInvitationService, FriendInvitationService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupPathService, GroupPathService>();
builder.Services.AddScoped<IProposedPathService, ProposedPathService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IJWTTokenService, JWTTokenService>();
builder.Services.AddScoped<IUserConfigurationService, UserConfigurationService>();
builder.Services.AddScoped<IUserLocationService, UserLocationService>();
builder.Services.AddScoped<IGroupInvitationService, GroupInvitationService>();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IPathAssembler, PathAssembler>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IGroupAuthorizationService, GroupAuthorizationService>();

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(new AuthorizeFilter());
        options.Conventions.Add(new UnauthorizedProducesResponseConvention());
        options.Conventions.Add(new InternalServerErrorProducesResponseConvention());
        options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseParameterTransformer()));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.Converters.Add(new DateTimeUtcJsonConverter());
    });

var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddDbContext<PTSDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ClockSkew = TimeSpan.Zero,
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var authHeaderExists = context.Request.Headers.ContainsKey("Authorization");

            var error = context.AuthenticateFailure;

            var code = error switch
            {
                SecurityTokenExpiredException => AuthErrorCode.EXPIRED_TOKEN,
                SecurityTokenException => AuthErrorCode.INVALID_TOKEN,
                _ when !authHeaderExists => AuthErrorCode.MISSING_TOKEN,
                _ => AuthErrorCode.INVALID_TOKEN,
            };

            var response = new ErrorResponse<AuthErrorCode>(code, error?.Message);
            await context.Response.WriteAsJsonAsync(response);
        },
    };
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transport API v1");
    });
}

await app.Services.InitializePTSInfrastructureAsync();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
