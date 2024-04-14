using System.Xml.Serialization;
using StardewValley;
using StardewValley.TerrainFeatures;
using Netcode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WildFlowersReimagined
{
    public class FlowerGrass : Grass
    {
        private readonly HoeDirt fakeDirt = new();

        private readonly NetRef<Crop> netCrop = new();
        
        private List<Action<GameLocation, Vector2>> queuedActions = new();
        
        public Crop Crop
        {
            get
            {
                return netCrop.Value;
            }
            set
            {
                netCrop.Value = value;
            }
        }
        

        //public Crop Crop { get; set; }

        public bool CropInit { get; set; } = false;

        public FlowerGrassConfig FlowerGrassConfig { get; set; }

        
        public override void initNetFields()
        {
            base.initNetFields();
            base.NetFields.AddField(netCrop, "netCrop");
            netCrop.fieldChangeVisibleEvent += delegate
            {
                if (netCrop.Value != null)
                {
                    netCrop.Value.Dirt = fakeDirt;
                    netCrop.Value.currentLocation = Location;
                    netCrop.Value.updateDrawMath(Tile);
                }
            };
            netCrop.Interpolated(interpolate: false, wait: false);
            netCrop.OnConflictResolve += delegate (Crop rejected, Crop accepted)
            {
                if (Game1.IsMasterGame && rejected != null && rejected.netSeedIndex.Value != null)
                {
                    queuedActions.Add(delegate (GameLocation gLocation, Vector2 tileLocation)
                    {
                        Vector2 vector = tileLocation * 64f;
                        gLocation.debris.Add(new Debris(rejected.netSeedIndex, vector, vector));
                    });
                    base.NeedsUpdate = true;
                }
            };

        }
        

        public FlowerGrass() : base()
        {
            Location = Game1.currentLocation;
            FlowerGrassConfig = new FlowerGrassConfig();
            
            this.CropInit = false;

        }

        /*

        public void initFromModData()
        {
            if (!this.CropInit)
            {
                if (this.modData.TryGetValue("fd/SeedIndex", out var seedId))
                {
                    this.numberOfWeeds.Value = 3;
                    this.Crop = new(seedId, 10, 10, Game1.getFarm());
                    this.Crop.growCompletely();
                    this.Crop.newDay(0);
                    
                    this.CropInit = true;

                }
                else
                {
                    this.CropInit = false;
                }
            }
        }
        */

        public FlowerGrass(int which, int numberOfWeeds, Crop crop, FlowerGrassConfig flowerGrassConfig) : base()
        {
            grassType.Value = (byte)which;
            loadSprite();
            this.numberOfWeeds.Value = numberOfWeeds;
            this.Crop = crop;
            this.FlowerGrassConfig = flowerGrassConfig;

            //
            this.CropInit = true;


            /*
            this.modData["fd/PhaseToShow"] = $"{crop.phaseToShow.Value}";
            this.modData["fd/CurrentPhase"] = $"{crop.currentPhase.Value}";
            this.modData["fd/SeedIndex"] = $"{crop.netSeedIndex.Value}";
            this.modData["fd/TintColorR"] = $"{crop.tintColor.Value.R}";
            this.modData["fd/TintColorG"] = $"{crop.tintColor.Value.G}";
            this.modData["fd/TintColorB"] = $"{crop.tintColor.Value.B}";
            this.modData["fd/TintColorA"] = $"{crop.tintColor.Value.A}";
            */

        }


        

        public override void draw(SpriteBatch spriteBatch)
        {
            base.draw(spriteBatch);
            Vector2 tile = Tile;
            if (this.Crop != null)
            {
                Crop.draw(spriteBatch, tile, Color.White, shakeRotation);
            }
        }

        /// <summary>
        /// Method for harvesting the flowers, 
        /// </summary>
        /// <param name="tileLocation"> tile location </param>
        /// <param name="useScythe">if it was scythe or hand. The game code has different behaviors for this</param>
        private void Harvest(Vector2 tileLocation, bool useScythe)
        {
            this.Crop.harvest((int)tileLocation.X, (int)tileLocation.Y, fakeDirt, isForcedScytheHarvest: useScythe);
            this.Crop = null;
        }

        public override bool performUseAction(Vector2 tileLocation)
        {
            if (this.Crop != null && !this.FlowerGrassConfig.UseScythe)
            {
                Harvest(tileLocation, false);
            }

            return false;
        }

        public override bool performToolAction(Tool tool, int damage, Vector2 tileLocation)
        {
            if (this.Crop != null && this.FlowerGrassConfig.UseScythe && tool != null && tool.isScythe())
            {
                Harvest(tileLocation, true);
            }
            return base.performToolAction(tool, damage, tileLocation);
        }





    }
}
