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
            await RemoveOutdated();
            var tasks = await _localStorageService.GetItemAsync<IEnumerable<HumanUnitTaskViewModel>>(LOCAL_STORAGE_NAME) ?? new List<HumanUnitTaskViewModel>();
            return tasks;


            async Task RemoveOutdated()
            {
                var tasks = await _localStorageService.GetItemAsync<IEnumerable<HumanUnitTaskViewModel>>(LOCAL_STORAGE_NAME);

                if (tasks == null)
                    return;

                var tasksList = tasks.ToList();

                var actualTasks = tasksList.Where(task => task.To > DateTime.UtcNow).ToList();
                await _localStorageService.SetItemAsync(LOCAL_STORAGE_NAME, actualTasks);
            }
        }

        internal async Task SyncTasksWithServer(int tribeId)
        {
            var actualTasksFromServer = await _httpClient
                .GetFromJsonAsync<IEnumerable<HumanUnitTaskViewModel>>($"api/task/get-actual-tasks/{tribeId}");//PROBLEM.. TODO

            if (actualTasksFromServer?.Any() == true)
                await _localStorageService.SetItemAsync(LOCAL_STORAGE_NAME, actualTasksFromServer);
        }

        internal async Task Add(IAddHumanUnitTaskDto dto)
        {
            var result = await _httpClient.PostAsJsonAsync("api/task/add-task", dto);

            if (result.IsSuccessStatusCode)
            {
                //todo notification
            }

            await SyncTasksWithServer(dto.TribeId);
        }
    }
}
