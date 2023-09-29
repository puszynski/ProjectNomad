using ProjectNomad.Client.Models.Response;
using ProjectNomad.Shared.Interfaces;
using System.Net.Http.Json;

namespace ProjectNomad.Client.Logic
{
    internal class TasksManager
    {
        readonly HttpClient _httpClient;
        public TasksManager(HttpClient httpClient) => _httpClient = httpClient;

        internal async Task<IEnumerable<IHumanUnitTaskDto>> GetAll(int tribeId) 
            => await _httpClient.GetFromJsonAsync<IEnumerable<HumanUnitTask>>($"api/task/get-actual-tasks/{tribeId}");

        internal async Task AddToServer(IAddHumanUnitTaskDto dto) 
            => await _httpClient.PostAsJsonAsync("api/task/add-task", dto);
    }
}
