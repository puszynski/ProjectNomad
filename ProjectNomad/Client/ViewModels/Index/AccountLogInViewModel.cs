using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ProjectNomad.Client.ViewModels.Shared;
using ProjectNomad.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ProjectNomad.Client.ViewModels.Index
{
    public class AccountLogInViewModel : BaseViewModel, IAccount
    {
        [Required]
        [MinLength(3)]
        public string? Name { get; set; }

        [Required]
        [MinLength(3)]
        public string? Password { get; set; }


        readonly HttpClient _httpClient;
        readonly NavigationManager _navigationManager;
        readonly ILocalStorageService _localStorageService;
        public AccountLogInViewModel(HttpClient httpClient, 
            NavigationManager navigationManager, 
            ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
        }

        public async Task LogIn()
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/login", this);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                ErrorMessage = "Login failed. Please try again.";
            else
            {
                //_clientStateService.AddNotification("Zgłoszenie zostało wysłane", ENotificationType.Success);
                var accountId = await result.Content.ReadFromJsonAsync<Guid>();
                _localStorageService.SetItemAsync("id", accountId);

                _navigationManager.NavigateTo("game"); //todo
            }
        }
    }
}
