using System.Net.Http.Json;

namespace ProjectNomad.Client.ViewModels.Game
{
    public class TribeGameObjectsViewModel
    {
        public TribeViewModel Tribe { get; set; }
        public IEnumerable<HumanUnitViewModel> HumanUnits { get; set; }

        readonly HttpClient _httpClient;

        public TribeGameObjectsViewModel()
        {            
        }
        public TribeGameObjectsViewModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        internal async Task TrigerRecalculation()
        {
            await _httpClient.PostAsJsonAsync("api/game/triggerPlayerGameObjectRecalculation", Tribe.Id);

            //await _httpClient.GetAsync($"api/game/triggerPlayerGameObjectRecalculation/{Tribe.Id}");
        }

        internal async Task ReBornTribeMembers()
        {
            await _httpClient.PostAsJsonAsync("api/account/generateNewTribeMembers", Tribe.Id);
        }
    }
}