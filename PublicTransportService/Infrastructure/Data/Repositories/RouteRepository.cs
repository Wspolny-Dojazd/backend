﻿using Microsoft.EntityFrameworkCore;
using PublicTransportService.Domain.Entities;
using PublicTransportService.Domain.Interfaces;

namespace PublicTransportService.Infrastructure.Data.Repositories;

/// <summary>
/// Provides data access operations for <see cref="Route"/> entities.
/// </summary>
/// <param name="dbContext">The database context used to access route data.</param>
internal class RouteRepository(PTSDbContext dbContext) : IRouteRepository
{
    /// <inheritdoc/>
    public async Task<Route?> GetByIdAsync(string id)
    {
        return await dbContext.Routes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}
