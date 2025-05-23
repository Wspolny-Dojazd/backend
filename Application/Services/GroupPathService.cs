﻿using Application.DTOs.Path;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Model;
using Shared.Enums.ErrorCodes;

namespace Application.Services;

/// <summary>
/// Provides operations for managing accepted group travel paths.
/// </summary>
/// <param name="proposedPathRepository">The repository for accessing <see cref="ProposedPath"/> data.</param>
/// <param name="groupPathRepository">The repository for accessing <see cref="GroupPath"/> data.</param>
/// <param name="groupService">The service for validating group existence.</param>
/// <param name="mapper">The object mapper.</param>
public class GroupPathService(
    IProposedPathRepository proposedPathRepository,
    IGroupPathRepository groupPathRepository,
    IGroupService groupService,
    IMapper mapper)
    : IGroupPathService
{
    /// <inheritdoc/>
    public async Task<GroupPathDto> ConfirmFromProposalAsync(int groupId, Guid proposalId)
    {
        _ = await groupService.GetByIdAsync(groupId);

        var acceptedPath = await groupPathRepository.GetByGroupIdAsync(groupId);
        if (acceptedPath is not null)
        {
            throw new AppException(400, GroupPathErrorCode.PATH_ALREADY_ACCEPTED);
        }

        var allProposals = await proposedPathRepository.GetAllByGroupIdAsync(groupId);
        var selectedProposal = allProposals.FirstOrDefault(p => p.Id == proposalId)
            ?? throw new AppException(404, GroupPathErrorCode.PATH_NOT_FOUND);

        var entity = new GroupPath
        {
            Id = Guid.NewGuid(),
            GroupId = groupId,
            CreatedAt = DateTime.UtcNow,
            SerializedDto = selectedProposal.SerializedDto,
        };

        await groupPathRepository.AddAsync(entity);
        await proposedPathRepository.RemoveRangeAsync(allProposals);

        return mapper.Map<GroupPathDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<GroupPathDto> GetByGroupIdAsync(int groupId)
    {
        _ = await groupService.GetByIdAsync(groupId);

        var path = await groupPathRepository.GetByGroupIdAsync(groupId)
            ?? throw new AppException(404, GroupPathErrorCode.PATH_NOT_FOUND);

        return mapper.Map<GroupPathDto>(path);
    }
}
