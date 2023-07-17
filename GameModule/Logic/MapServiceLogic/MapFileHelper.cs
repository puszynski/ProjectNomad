using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace GameModule.Logic.MapServiceLogic
{
    internal static class MapFileHelper
    {
        //maps file range 0_0-99_99
        //x y means middle of 7x7 square
        internal static IEnumerable<string> GetMapFilesNames(int x, int y) 
        {
            var x_start = Math.Floor(x / 100d) * 100;
            var x_end = x_start + 99;

            var y_start = Math.Floor(y / 100d) * 100;
            var y_end = y_start + 99;

            if (x_start <= x - 3 && x_end >= x + 3
                && y_start <= y - 3 && y_end >= y + 3)
                return new List<string> { $"{x_start}_{y_start}-{x_end}_{y_end}" };

            else                
                throw new NotImplementedException(); //todo get more then one
        }
    }
}
