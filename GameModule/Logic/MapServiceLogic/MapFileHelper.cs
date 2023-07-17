using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace GameModule.Logic.MapServiceLogic
{
    internal static class MapFileHelper
    {
        internal static IEnumerable<string> GetMapFilesNames(int x, int y) //todo UT!
        {
            //maps file range 0_0-99_99
            //x y means middle of 7x7 square

            //todo check if one or more file maps needed, get theirs names

            var x_start = Math.Floor(x / 100d) * 100;
            var x_end = x_start + 99;

            var y_start = Math.Floor(y / 100d) * 100;
            var y_end = y_start + 99;

            //todo check if +/- 3 tiles are in
            if (x_start <= x - 3 && x_end >= x + 3
                && y_start <= y - 3 && y_end >= y + 3)
            {
                return new List<string> { $"{x_start}_{y_start}-{x_end}_{y_end}" };
            }
            else
            {
                //todo get more then one
                throw new NotImplementedException();
            }

            //1 to nearest rounded hundred
            var n = 76d;
            var test1 = Math.Round(n / 100d, 0) * 100;
        }
    }
}
