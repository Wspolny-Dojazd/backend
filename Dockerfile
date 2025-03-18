# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the entire source code into the container
COPY . .

# Restore dependencies
RUN dotnet restore

# Generate certificates required for https functionality
RUN dotnet dev-certs https --trust

# Run the application
CMD ["dotnet", "run", "-lp", "https", "--project", "API/API.csproj"]