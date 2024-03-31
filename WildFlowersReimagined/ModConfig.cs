namespace WildFlowersReimagined
{
    public sealed class ModConfig
    {
        public bool ModEnabled { get; set; } = true;
        public FlowerGrassConfig FlowerGrassConfig { get; set; } = new FlowerGrassConfig();
        public float WildflowerGrowChance { get; set; } = 0.005f;
        
    }

    public sealed class FlowerGrassConfig
    {
        public bool UseScythe { get; set; } = true;
    }
}
