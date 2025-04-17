using System.Text.Json;
using Application.DTOs.Path;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using PublicTransportService.Application.Interfaces;
using PublicTransportService.Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Provides operations for generating and managing proposed paths.
/// </summary>
/// <param name="groupService">The service for validating group existence.</param>
/// <param name="pathPlanningService">The service for computing group paths.</param>
/// <param name="stopRepository">The repository for accessing stop data.</param>
/// <param name="pathAssembler">The service for assembling paths from raw data.</param>
/// <param name="groupPathRepository">The repository for accessing confirmed group paths.</param>
/// <param name="proposedPathRepository">The repository for accessing proposed paths.</param>
/// <param name="mapper">The object mapper.</param>
public class ProposedPathService(
    IGroupService groupService,
    IPathPlanningService pathPlanningService,
    IStopRepository stopRepository,
    IPathAssembler pathAssembler,
    IGroupPathRepository groupPathRepository,
    IProposedPathRepository proposedPathRepository,
    IMapper mapper)
    : IProposedPathService
{
    /// <inheritdoc/>
    public async Task<IEnumerable<ProposedPathDto>> GenerateAsync(int groupId, PathRequestDto request)
    {
        _ = await groupService.GetByIdAsync(groupId);

        var acceptedPath = await groupPathRepository.GetByGroupIdAsync(groupId);
        if (acceptedPath is not null)
        {
            var excMsg = "Cannot generate new paths when one is already accepted.";
            throw new AppException(400, "PATH_ALREADY_ACCEPTED", excMsg);
        }

        await this.ResetAllForGroupAsync(groupId);

        var userCoordinates = request.UserLocations
            .Select(x => (x.UserId, x.Latitude, x.Longitude));

        var allPaths = await pathPlanningService.ComputeGroupPathsAsync(
            request.DestinationLatitude,
            request.DestinationLongitude,
            request.ArrivalTime,
            userCoordinates);

        if (!allPaths.Any())
        {
            return [];
        }

        var stopIds = allPaths
            .SelectMany(p => p.Values.SelectMany(r => r.Segments))
            .SelectMany(s => new[] { s.FromStopId, s.ToStopId })
            .ToHashSet();

        var stops = await stopRepository.GetByIdsAsync(stopIds);
        var stopLookup = stops.ToDictionary(stop => stop.Id, stop => stop);

        var proposedPaths = new List<ProposedPath>();
        foreach (var paths in allPaths)
        {
            var assembledPaths = await pathAssembler.AssemblePaths(paths, stopLookup);
            var proposalId = Guid.NewGuid();

            var proposedPath = new ProposedPath
            {
                Id = proposalId,
                GroupId = groupId,
                CreatedAt = DateTime.UtcNow,
                SerializedDto = JsonSerializer.Serialize(assembledPaths),
            };

            proposedPaths.Add(proposedPath);
        }

        await proposedPathRepository.AddRangeAsync(proposedPaths);
        return mapper.ProjectTo<ProposedPathDto>(proposedPaths.AsQueryable());
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ProposedPathDto>> GetRangeAsync(int groupId)
    {
        _ = await groupService.GetByIdAsync(groupId);

        var acceptedPath = await groupPathRepository.GetByGroupIdAsync(groupId);
        if (acceptedPath is not null)
        {
            var excMsg = "Cannot generate new paths when one is already accepted.";
            throw new AppException(400, "PATH_ALREADY_ACCEPTED", excMsg);
        }

        var proposedPaths = await proposedPathRepository.GetAllByGroupIdAsync(groupId);
        return mapper.ProjectTo<ProposedPathDto>(proposedPaths.AsQueryable());
    }

    /// <inheritdoc/>
    public async Task ResetAllForGroupAsync(int groupId)
    {
        var existing = await proposedPathRepository.GetAllByGroupIdAsync(groupId);
        await proposedPathRepository.RemoveRangeAsync(existing);
    }
}
