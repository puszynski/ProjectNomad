using Blazored.LocalStorage;
using ProjectNomad.Client.ViewModels.Tasks;
using ProjectNomad.Shared.Interfaces;
using System.Net.Http.Json;

namespace ProjectNomad.Client.Logic
{
    internal class HumanUnitTasksLocalStorageManager
    {
        const string LOCAL_STORAGE_NAME = "HumanUnitTasks";

        readonly ILocalStorageService _localStorageService;
        readonly HttpClient _httpClient;


        public HumanUnitTasksLocalStorageManager(ILocalStorageService localStorageService, 
            HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        internal async Task<IEnumerable<IHumanUnitTaskDto>> GetAllFromLocalStorageAndCleanOutdated()
        {
            var tasks = await _localStorageService.GetItemAsync<IEnumerable<HumanUnitTaskViewModel>>(LOCAL_STORAGE_NAME);

            if (tasks == null)
                return new List<HumanUnitTaskViewModel>();

            var actualTasks = tasks
                .Where(task => task.To > DateTime.UtcNow)
                .ToList();

            await _localStorageService.SetItemAsync(LOCAL_STORAGE_NAME, actualTasks);
            return actualTasks;
        }

        internal async Task SyncTasksWithServer(int tribeId)
        {
            var actualTasksFromServer = await _httpClient
                .GetFromJsonAsync<IEnumerable<HumanUnitTaskViewModel>>($"api/task/get-actual-tasks/{tribeId}");//PROBLEM.. TODO

            if (actualTasksFromServer?.Any() == true)
                await _localStorageService.SetItemAsync(LOCAL_STORAGE_NAME, actualTasksFromServer);
        }

        internal async Task AddToServer(IAddHumanUnitTaskDto dto)
        {
            await _httpClient.PostAsJsonAsync("api/task/add-task", dto);
            await SyncTasksWithServer(dto.TribeId);
        }
    }
}
