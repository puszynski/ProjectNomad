using GameModule.Entities;
using GameModule.Entities.ValueObjects;
using GameModule.Repositories;
using ProjectNomad.Shared.Enums;
using ProjectNomad.Shared;

namespace GameModule.Logic.GameLooperLogic.TasksLogic.Helpers
{
    internal class MapTileFetcher
    {
        readonly IMapTileRepository _mapTileRepository;

        public MapTileFetcher(IMapTileRepository mapTileRepository)
        {
            _mapTileRepository = mapTileRepository;
        }

        internal async Task<ICollection<MapTile>> GetMapTilesCurrentlyAssigned(Tribe tribe)
        {
            if (tribe.HumanTasks == null || tribe.HumanTaskOrders == null)
                throw new NullReferenceException();

            var mapTileLocalizationsForTasks = tribe.HumanTasks
                .Where(x => x.Localization != null)
                .Select(y => y.Localization)
                .ToList();

            var mapTileLocalizationsForTaskOrders = tribe.HumanTaskOrders
                .Where(x => x.Localization != null)
                .Select(y => y.Localization)
                .ToList();

            var mapTileFromCurrentLocalization = tribe.Localization;

            var allMapTileLocalization = mapTileLocalizationsForTasks.Union(mapTileLocalizationsForTaskOrders).ToList();

            if (!allMapTileLocalization.Any(x => x.Equals(mapTileFromCurrentLocalization)))
                allMapTileLocalization.Add(mapTileFromCurrentLocalization);

            return await _mapTileRepository.GetByLocalizations(allMapTileLocalization);
        }

        internal MapTile? AssignMapTileToJobAndFetchMissingMapTiles(
            IEnumerable<MapTile> mapTiles,
            ETaskType taskType,
            Localization tribeLocalization)
        {
            switch (taskType)
            {
                case ETaskType.GatheringFood:
                    var mapTileInTribeLocalization = mapTiles
                        .FirstOrDefault(x => x.Localization.Equals(tribeLocalization)) //sypie ale czemu?
                        ?? throw new ArgumentNullException(nameof(mapTiles));

                    if (mapTileInTribeLocalization.Food.ActualPoints >= GameSETTINGS.Food.MapTileFoodGathered)
                        return mapTileInTribeLocalization;

                    for (int radius = 1; radius <= 3; radius++)
                    {
                        var mapTileNearByTribeLocalization = GetMapTilesWithRadiusCondition(mapTiles, GetConditionForRadius(radius))
                            .FirstOrDefault(x => x.Food.ActualPoints >= GameSETTINGS.Food.MapTileFoodGathered)
                            ??
                            _mapTileRepository.GetQueryFromCondition(
                                GetConditionForRadius(radius),
                                x => x.Food.ActualPoints >= GameSETTINGS.Food.MapTileFoodGathered)
                                .FirstOrDefault();

                        if (mapTileNearByTribeLocalization != null)
                            return mapTileNearByTribeLocalization;
                    }
                    return null;

                case ETaskType.GatheringWood:
                    mapTileInTribeLocalization = mapTiles
                        .FirstOrDefault(x => x.Localization.Equals(tribeLocalization))
                        ?? throw new ArgumentNullException(nameof(taskType));

                    if (mapTileInTribeLocalization.Wood.ActualPoints >= GameSETTINGS.Wood.WoodAmountGatheredFromMap)
                        return mapTileInTribeLocalization;

                    for (int radius = 1; radius <= 3; radius++)
                    {
                        var mapTileNearByTribeLocalization = GetMapTilesWithRadiusCondition(mapTiles, GetConditionForRadius(radius))
                            .FirstOrDefault(x => x.Wood.ActualPoints >= GameSETTINGS.Wood.WoodAmountGatheredFromMap)
                            ??
                            _mapTileRepository.GetQueryFromCondition(
                                GetConditionForRadius(radius),
                                x => x.Wood.ActualPoints >= GameSETTINGS.Wood.WoodAmountGatheredFromMap)
                                .FirstOrDefault();

                        if (mapTileNearByTribeLocalization != null)
                            return mapTileNearByTribeLocalization;
                    }
                    return null;

                default:
                    throw new NotImplementedException();
            }

            Func<MapTile, bool> GetConditionForRadius(int radius)
            {
                return x =>
                    x.Localization.X >= tribeLocalization.X - radius
                    && x.Localization.X <= tribeLocalization.X + radius
                    && x.Localization.Y >= tribeLocalization.Y - radius
                    && x.Localization.Y <= tribeLocalization.Y + radius;
            }

            IEnumerable<MapTile> GetMapTilesWithRadiusCondition(
                IEnumerable<MapTile> mapTiles,
                Func<MapTile, bool> condition)
            {
                return mapTiles.Where(condition);
            }
        }

    }
}
