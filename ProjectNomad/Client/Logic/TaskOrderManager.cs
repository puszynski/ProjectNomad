using ProjectNomad.Client.DtoModels;
using System.Net.Http.Json;

namespace ProjectNomad.Client.Logic
{
    internal class TaskOrderManager
    {
        readonly HttpClient _httpClient;
        public TaskOrderManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        internal async Task<IEnumerable<HumanUnitTaskOrder>> GetAll(int tribeId)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<HumanUnitTaskOrder>>($"api/taskOrder/{tribeId}");
            return response.OrderBy(x => x.Added);
        }

        internal async Task Add(int tribeId, ProjectNomad.Shared.Enums.EHumanUnitTaskType type, int mapTileX, int mapTileY)
        {
            var request = new AddHumanUnitTaskOrderRequest(tribeId, type, mapTileX, mapTileY);
            await _httpClient.PostAsJsonAsync("api/taskOrder/", request);
        }

        //todo cancel order.. 
    }
}
