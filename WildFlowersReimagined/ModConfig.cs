﻿namespace WildFlowersReimagined
{
    public sealed class ModConfig
    {
        public bool ModEnabled { get; set; } = true;
        public FlowerGrassConfig FlowerGrassConfig { get; set; } = new FlowerGrassConfig();
        public float WildflowerGrowChance { get; set; } = 0.005f;

        public Dictionary<string, int> FlowerProbabilityMap { get; set; } = new Dictionary<string, int>();

        public bool PreserveFlowersOnProbability0 { get; set; } = true;

        public bool CheckAllLocations { get; set; } = false;
        
    }

    public sealed class FlowerGrassConfig
    {
        public bool UseScythe { get; set; } = true;

        public bool KeepRegrowFlower {  get; set; } = true;
    }
}
