using Microsoft.CodeAnalysis;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Crops;
using StardewValley.ItemTypeDefinitions;
using System.Dynamic;

namespace WildFlowersReimagined
{
    sealed class SeedMap
    {
        /// <summary>
        /// Reformatted game data for faster lookups
        /// Flower Name -> ( Flower Data, List[(Seed id, Crop Data) ] )
        /// </summary>
        private readonly Dictionary<string, (ItemMetadata flowerData, List<(string seedId, CropData cropData)> seeds)> mapData = new();

        /// <summary>
        /// Map to check if a seed has conflicts
        /// </summary>
        private readonly Dictionary<ItemMetadata, string> checkMap = new();

        /// <summary>
        /// Flag to mark the seed map as initialized
        /// </summary>
        private bool initialized = false;

        /// <summary>
        /// Creates a new seedMap with the game data already formatted
        /// </summary>
        public SeedMap() {}

        /// <summary>
        /// Initializes the seed map, this needs to be done after all the mods have loaded to ensure we have access to modded data
        /// </summary>
        /// <param name="force">Force the initialization of the map even if it's supposed to be full</param>
        public void Init(IMonitor monitor, IgnoreList? ignoreList=null, bool force = false)
        {
            if (initialized && !force)
            {
                return;
            }
            if (ignoreList == null)
            {
                ignoreList = new IgnoreList();
            }
            initialized = true;
            mapData.Clear();
            checkMap.Clear();
            foreach (var (seedId, cropData) in Game1.cropData)
            {   
                // skip the item if it's in the seeds ignore list
                if (ignoreList.Seeds.Contains(seedId))
                {
                    monitor.Log($"Skipping {seedId}, as requested by the seed ignore list", LogLevel.Trace);
                    continue;
                }
                // skip the item if the crop / flower is on the flower ignore list
                if (ignoreList.Flowers.Contains(cropData.HarvestItemId))
                {
                    monitor.Log($"Skipping {seedId}, as requested by the flower ignore list", LogLevel.Trace);
                    continue;
                }

                // if there is no crop data, initialize the crop data
                if (!mapData.TryGetValue(cropData.HarvestItemId, out var seedValues))
                {
                    var localList = new List<(string seedId, CropData cropData)>();
                    var itemInfo = ItemRegistry.GetMetadata(cropData.HarvestItemId);
                    
                    
                    if (itemInfo == null || !itemInfo.Exists()) {
                        monitor.Log($"{seedId} doesn't have a valid item info", LogLevel.Info);
                        continue;
                    }
                    if (itemInfo.GetParsedData().Category != StardewValley.Object.flowersCategory)
                    {
                        continue;
                    }
                    if (checkMap.TryGetValue(itemInfo, out var knownName))
                    {
                        monitor.Log($"{seedId} has a conflict with {knownName} skipping", LogLevel.Warn);
                        continue;
                    }
                    seedValues = (itemInfo, localList);
                    mapData[cropData.HarvestItemId] = seedValues;
                    checkMap[itemInfo] = cropData.HarvestItemId;
                }
                
                seedValues.seeds.Add((seedId, cropData));
            }

        }

        /// <summary>
        /// Returns a list of all known flowers
        /// </summary>
        /// <returns>Map of flower item data to seed data</returns>
        public Dictionary<ItemMetadata, List<ItemMetadata>> GetFlowerConfigMap()
        {
            return mapData.ToDictionary(p => p.Value.flowerData, 
                p => p.Value.seeds.Select(s => ItemRegistry.GetMetadata(s.seedId))
                    .Where(i => i.Exists())
                    .ToList()
                    );
        }

        /// <summary>
        /// Return a list of all flowers for this location
        /// </summary>
        /// <param name="location"> Location to filter down the list</param>
        /// <returns>List of seeds for this location</returns>
        public List<string> GetSeedCandidatesForLocation(GameLocation location)
        {
            var candidates = new List<string>();
            if (location.SeedsIgnoreSeasonsHere())
            {
                candidates = mapData.SelectMany(p => p.Value.seeds.Select(s => s.seedId)).ToList();
            }
            else
            {
                var localSeason = location.GetSeason();
                candidates = mapData.SelectMany(p => p.Value.seeds.Where(p => p.cropData.Seasons.Contains(localSeason)).Select(p => p.seedId)).ToList();
            }
            return candidates;

        }

        /// <summary>
        /// Gets the seeds for this location scaled to the probability set
        /// </summary>
        /// <param name="location">Location to filter the seeds</param>
        /// <param name="flowerProbabilityMap">Map of the "probability" of each seed</param>
        /// <returns>List of seed for this location with the probability map applied</returns>
        public List<string> GetSeedsForLocation(GameLocation location, Dictionary<string, int> flowerProbabilityMap) {
            var seeds = new List<string>();
            var candidates = GetSeedCandidatesForLocation(location);
            foreach (var seed in candidates)
            {
                for ( var i = 0; i < flowerProbabilityMap.GetValueOrDefault(seed, 3); i++)
                {
                    seeds.Add(seed);
                }
            }
            return seeds;
        }
    }

    public sealed class IgnoreList
    {
        public IgnoreList() { }
        public List<string> Seeds { get; set; } = new List<string>();
        public List<string> Flowers { get; set; } = new List<string>();
    }
}
