using GameModule.Logic.MapServiceLogic;
using ProjectNomad.Shared.Interfaces;
using System.Reflection;
using System.Text.Json;

namespace GameModule.Logic
{
    internal class MapService
    {
        public async Task<IEnumerable<IMapTile>> GetMapData(int x, int y)
        {
            if (x != 0 && y != 0)
                throw new NotImplementedException();

            var fileNames = MapFileHelper.GetMapFilesNames(x, y);

            var allMapTilesNeeded = new List<IMapTile>();

            foreach ( var fileName in fileNames)
            {
                var filePath = MapDirectoryPathName(fileName);
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line = await reader.ReadToEndAsync();
                    var dataFromFiles = JsonSerializer.Deserialize<IEnumerable<IMapTile>>(line);
                    allMapTilesNeeded.AddRange(dataFromFiles); 
                }
            }

            throw new NotImplementedException(); //todo return 7x7 
        }

        

        static string MapDirectoryPathName(string fileName)//todo rename - or remove to other place.. ?
        {
            var dirPath = Assembly.GetExecutingAssembly().Location;
            dirPath = Path.GetDirectoryName(dirPath);
            return Path.GetFullPath(Path.Combine(dirPath, $"/Modules/GameModule/Logic/MapService/MapFiles/{fileName}.txt"));
        }
    }
}
