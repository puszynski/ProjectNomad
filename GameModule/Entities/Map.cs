using ProjectNomad.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModule.Entities
{
    internal class Map
    {
        //how to store map?
    }

    internal class MapTile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public EMapType Type { get; set; }

        public int FoodPoints { get; set; }
        public int MaxFoodPoints { get; set; }
    }
}
