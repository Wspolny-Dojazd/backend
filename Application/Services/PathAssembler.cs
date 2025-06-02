using Application.DTOs.Path;
using Application.Interfaces;
using PublicTransportService.Application.Interfaces;
using PublicTransportService.Application.PathFinding;
using PublicTransportService.Domain.Entities;
using PublicTransportService.Domain.Interfaces;

namespace Application.Services;

/// <summary>
/// Provides operations for assembling or mapping path results into DTOs.
/// </summary>
/// <param name="tripRepository">The repository for accessing trip data.</param>
/// <param name="routeRepository">The repository for accessing route data.</param>
/// <param name="shapeRepository">The repository for accessing shape data.</param>
/// <param name="walkingTimeEstimator">The repository for accessing time data.</param>
public class PathAssembler(
    ITripRepository tripRepository,
    IRouteRepository routeRepository,
    IShapeRepository shapeRepository,
    IWalkingTimeEstimator walkingTimeEstimator) : IPathAssembler
{
    /// <inheritdoc/>
    public async Task<IEnumerable<UserPathDto>> AssemblePaths(
        Dictionary<Guid, PathResult> paths,
        IReadOnlyDictionary<string, Stop> stopLookup,
        IReadOnlyDictionary<Guid, (double Latitude, double Longitude)> userLocations,
        (double Latitude, double Longitude) destination,
        DateTime arrivalTime)
    {
        var userPaths = new List<UserPathDto>();
        foreach (var (userId, pathResult) in paths)
        {
            var segmentsDto = await this.BuildSegments(
                pathResult, stopLookup, userLocations[userId], destination);

            if (segmentsDto.Count > 0)
            {
                var initWalkSeg = segmentsDto[0] as WalkSegmentDto;
                DateTime departureTime;
                if (initWalkSeg != null)
                {
                    var firstStopSeg = segmentsDto[1] as RouteSegmentDto;
                    if (firstStopSeg != null)
                    {
                        var firstStop = firstStopSeg.Stops[0];
                        departureTime = firstStop.DepartureTime!.Value.AddMinutes(-initWalkSeg.Duration);
                    }
                    else
                    {
                        departureTime = arrivalTime.AddMinutes(-initWalkSeg.Duration);
                    }
                }
                else
                {
                    var firstStop = (segmentsDto[0] as RouteSegmentDto)!.Stops[0];
                    departureTime = firstStop.DepartureTime!.Value;
                }

                userPaths.Add(new UserPathDto(userId, departureTime, segmentsDto));
            }
            else
            {
                userPaths.Add(new UserPathDto(userId, arrivalTime, segmentsDto));
            }
        }

        return userPaths;
    }

    private static SegmentType DetermineSegmentType(PathSegment segment)
    {
        return segment.TripId is null
            ? SegmentType.Walk
            : SegmentType.Route;
    }

    private async Task<WalkSegmentDto> BuildWalkSegmentDto(
        List<string> stopsOrdered,
        IReadOnlyDictionary<string, Stop> stopLookup)
    {
        var fromStop = stopLookup[stopsOrdered[0]];
        var fromName = string.IsNullOrEmpty(fromStop.Code)
            ? fromStop.Name
            : $"{fromStop.Name} {fromStop.Code}";
        var fromDto = new WalkLocationDto(
            Id: fromStop.Id,
            Latitude: fromStop.Latitude,
            Longitude: fromStop.Longitude,
            Name: fromName);

        var toStop = stopLookup[stopsOrdered[1]];
        var toName = string.IsNullOrEmpty(toStop.Code)
            ? toStop.Name
            : $"{toStop.Name} {toStop.Code}";
        var toDto = new WalkLocationDto(
            Id: toStop.Id,
            Latitude: toStop.Latitude,
            Longitude: toStop.Longitude,
            Name: toName);

        var walkingPathInfo = await walkingTimeEstimator.GetWalkingPathInfoAsync(
            fromStop.Latitude, fromStop.Longitude, toStop.Latitude, toStop.Longitude);

        var duration = (int)MathF.Ceiling(walkingPathInfo.Duration / 60f);

        var coordinates = walkingPathInfo.Coordinates;

        var shapeCoords = coordinates
            .Select(c => new ShapeCoordDto(c.Latitude, c.Longitude))
            .ToList();

        var shapeSection = new ShapeSectionDto(
            From: fromDto.Id,
            To: toDto.Id,
            Coords: shapeCoords);

        return new WalkSegmentDto(
            fromDto, toDto, duration, walkingPathInfo.Distance, [shapeSection]);
    }

    private async Task<WalkSegmentDto> BuildInitWalkSegmentDto(
        (double Latitude, double Longitude) departure,
        string stop,
        IReadOnlyDictionary<string, Stop> stopLookup)
    {
        var toStop = stopLookup[stop];
        var toName = string.IsNullOrEmpty(toStop.Code)
            ? toStop.Name
            : $"{toStop.Name} {toStop.Code}";

        var fromDto = new WalkLocationDto(
            Id: "init",
            Latitude: departure.Latitude,
            Longitude: departure.Longitude,
            Name: string.Empty);

        var toDto = new WalkLocationDto(
            Id: toStop.Id,
            Latitude: toStop.Latitude,
            Longitude: toStop.Longitude,
            Name: toName);

        var walkingPathInfo = await walkingTimeEstimator.GetWalkingPathInfoAsync(
            departure.Latitude, departure.Longitude, toStop.Latitude, toStop.Longitude);

        var duration = (int)MathF.Ceiling(walkingPathInfo.Duration / 60f);

        var coordinates = walkingPathInfo.Coordinates;

        var shapeCoords = coordinates
            .Select(c => new ShapeCoordDto(c.Latitude, c.Longitude))
            .ToList();

        var shapeSection = new ShapeSectionDto(
            From: fromDto.Id,
            To: toDto.Id,
            Coords: shapeCoords);

        return new WalkSegmentDto(
            fromDto, toDto, duration, walkingPathInfo.Distance, [shapeSection]);
    }

    private async Task<WalkSegmentDto> BuildFinalWalkSegmentDto(
        string stop,
        (double Latitude, double Longitude) destination,
        IReadOnlyDictionary<string, Stop> stopLookup)
    {
        var fromStop = stopLookup[stop];
        var fromName = string.IsNullOrEmpty(fromStop.Code)
            ? fromStop.Name
            : $"{fromStop.Name} {fromStop.Code}";
        var fromDto = new WalkLocationDto(
            Id: fromStop.Id,
            Latitude: fromStop.Latitude,
            Longitude: fromStop.Longitude,
            Name: fromName);

        var toDto = new WalkLocationDto(
            Id: "final",
            Latitude: destination.Latitude,
            Longitude: destination.Longitude,
            Name: string.Empty);

        var walkingPathInfo = await walkingTimeEstimator.GetWalkingPathInfoAsync(
            fromStop.Latitude, fromStop.Longitude, destination.Latitude, destination.Longitude);

        var duration = (int)MathF.Ceiling(walkingPathInfo.Duration / 60f);

        var coordinates = walkingPathInfo.Coordinates;

        var shapeCoords = coordinates
            .Select(c => new ShapeCoordDto(c.Latitude, c.Longitude))
            .ToList();

        var shapeSection = new ShapeSectionDto(
            From: fromDto.Id,
            To: toDto.Id,
            Coords: shapeCoords);

        return new WalkSegmentDto(
            fromDto, toDto, duration, walkingPathInfo.Distance, [shapeSection]);
    }

    private async Task<List<SegmentDtoBase>> BuildSegments(
        PathResult pathResult,
        IReadOnlyDictionary<string, Stop> stopLookup,
        (double Latitude, double Longitude) userLocation,
        (double Latitude, double Longitude) destination)
    {
        var originalSegments = pathResult.Segments;
        var merged = new List<SegmentDtoBase>();

        if (originalSegments.Count == 0)
        {
            return merged;
        }

        var firstStopSeg = originalSegments[0];
        var firstStop = stopLookup[firstStopSeg.FromStopId];
        if (firstStop.Longitude != userLocation.Longitude || firstStop.Latitude != userLocation.Latitude)
        {
            merged.Add(await this.BuildInitWalkSegmentDto(userLocation, firstStopSeg.FromStopId, stopLookup));
        }

        var currentGroup = new List<PathSegment> { originalSegments[0] };
        SegmentType currentType = DetermineSegmentType(originalSegments[0]);
        string? currentTripId = originalSegments[0].TripId;

        for (int i = 1; i < originalSegments.Count; i++)
        {
            var seg = originalSegments[i];
            var segType = DetermineSegmentType(seg);
            var segTripId = seg.TripId;

            if (segType == currentType && segTripId == currentTripId)
            {
                currentGroup.Add(seg);
            }
            else
            {
                merged.Add(await this.BuildSingleSegmentDto(currentGroup, stopLookup, currentType));
                currentGroup = [seg];
                currentType = segType;
                currentTripId = seg.TripId;
            }
        }

        if (currentGroup.Count != 0)
        {
            merged.Add(await this.BuildSingleSegmentDto(currentGroup, stopLookup, currentType));
        }

        var lastStopSeg = originalSegments[^1];
        var lastStop = stopLookup[lastStopSeg.ToStopId];
        if (lastStop.Longitude != destination.Longitude || lastStop.Latitude != destination.Latitude)
        {
            merged.Add(await this.BuildFinalWalkSegmentDto(lastStopSeg.ToStopId, destination, stopLookup));
        }

        return merged;
    }

    private async Task<SegmentDtoBase> BuildSingleSegmentDto(
        List<PathSegment> segmentsGroup,
        IReadOnlyDictionary<string, Stop> stopLookup,
        SegmentType segmentType)
    {
        var stopsOrdered = new List<string> { segmentsGroup.First().FromStopId };
        stopsOrdered.AddRange(segmentsGroup.Select(s => s.ToStopId));

        return segmentType switch
        {
            SegmentType.Route => await this.BuildRouteSegmentDto(segmentsGroup, stopsOrdered, stopLookup),
            SegmentType.Walk => await this.BuildWalkSegmentDto(stopsOrdered, stopLookup),
            _ => throw new ArgumentException($"Unknown segment type: {segmentType}"),
        };
    }

    private async Task<SegmentDtoBase> BuildRouteSegmentDto(
        List<PathSegment> segmentsGroup,
        List<string> stopsOrdered,
        IReadOnlyDictionary<string, Stop> stopLookup)
    {
        var stopDtos = new List<StopDto>();
        int count = stopsOrdered.Count;
        for (int i = 0; i < count; i++)
        {
            bool isFirst = i == 0;
            bool isLast = i == count - 1;
            var stopId = stopsOrdered[i];
            var stop = stopLookup[stopId];

            DateTime? arrTime = null;
            DateTime? depTime = null;
            if (isFirst)
            {
                depTime = segmentsGroup.First().DepartureTime;
            }
            else if (isLast)
            {
                arrTime = segmentsGroup.Last().ArrivalTime;
            }
            else
            {
                arrTime = segmentsGroup[i - 1].ArrivalTime;
                depTime = segmentsGroup[i].DepartureTime;
            }

            stopDtos.Add(new StopDto(
                Id: stop.Id,
                Name: stop.Name,
                Code: stop.Code,
                Latitude: stop.Latitude,
                Longitude: stop.Longitude,
                WheelchairAccessible: stop.WheelchairBoarding,
                ArrivalTime: arrTime,
                DepartureTime: depTime));
        }

        var shapeList = new List<ShapeSectionDto>();
        foreach (var seg in segmentsGroup.Where(s => s.TripId is not null))
        {
            var shapes = await shapeRepository.GetSegmentShapesAsync(seg);
            var coords = shapes.Select(s => new ShapeCoordDto(s.PtLatitude, s.PtLongitude)).ToList();

            shapeList.Add(new ShapeSectionDto(seg.FromStopId, seg.ToStopId, coords));
        }

        var tripId = TripIdUtils.GetBaseTripId(segmentsGroup.First().TripId!, '@');

        var trip = await tripRepository.GetByIdAsync(tripId)
            ?? throw new KeyNotFoundException($"Trip with ID {tripId} not found.");

        var route = await routeRepository.GetByIdAsync(trip.RouteId)
            ?? throw new KeyNotFoundException($"Route with ID {trip.RouteId} not found.");

        var lineDto = new RouteLineDto(
            Type: route.Type,
            ShortName: route.ShortName,
            LongName: route.LongName,
            HeadSign: trip.HeadSign,
            Color: route.Color,
            TextColor: route.TextColor);

        return new RouteSegmentDto(
            Line: lineDto,
            Stops: stopDtos,
            Shapes: shapeList);
    }
}
