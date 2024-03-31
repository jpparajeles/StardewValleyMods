using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildFlowersReimagined
{
    
    public sealed class SaveDataItem
    {
        public int Vector2X { get; set; }
        public int Vector2Y { get; set;}

        public int PhaseToShow { get; set; }
        public int CurrentPhase {  get; set; }
        public string IndexOfHarvest { get; set; }
        public byte TintColorR { get; set; }
        public byte TintColorG { get; set; }
        public byte TintColorB { get; set; }
        public byte TintColorA { get; set; }
        public bool Dead { get; set; }


    }

    public sealed class SaveData
    {
        public Dictionary<string, List<SaveDataItem>> patchMapData {  get; set; }
    }
}
