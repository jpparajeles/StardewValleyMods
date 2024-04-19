﻿using StardewValley;
using StardewValley.TerrainFeatures;
using Netcode;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;

namespace WildFlowersReimagined
{
    [XmlType("Mods_jppWildFlowersReimagined_FlowerGrass")]
    public class FlowerGrass : Grass
    {
        [XmlIgnore]
        private readonly HoeDirt fakeDirt = new();

        public readonly NetRef<Crop> netCrop = new(new());

        [XmlIgnore]
        private List<Action<GameLocation, Vector2>> queuedActions = new();

        [XmlIgnore]
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

        public FlowerGrassConfig FlowerGrassConfig { get; set; }

        public override void initNetFields()
        {
            base.initNetFields();
            base.NetFields.AddField(netCrop, "netCrop");
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
        }

        public FlowerGrass(int which, int numberOfWeeds, Crop crop, FlowerGrassConfig flowerGrassConfig) : this()
        {
            grassType.Value = (byte)which;
            loadSprite();
            this.numberOfWeeds.Value = numberOfWeeds;
            this.Crop = crop;
            this.FlowerGrassConfig = flowerGrassConfig;

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
            var successful = this.Crop.harvest((int)tileLocation.X, (int)tileLocation.Y, fakeDirt, isForcedScytheHarvest: useScythe);
            if (successful)
            {
                this.Crop = null;
            }
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
