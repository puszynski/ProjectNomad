using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ProjectNomad.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ProjectNomad.Client.ViewModels.CreateAccount
{
    public class AccountCreateViewModel : IAccount
    {
        [Required]
        [MinLength(4)]
        public string? Name { get; set; }

        [Required]
        [MinLength(4)]
        public string? Password { get; set; }

        [Required]
        [MinLength(4)]        
        public string? PasswordConfirmation { get; set; }

        readonly HttpClient _httpClient;
        readonly NavigationManager _navigationManager;
        readonly ILocalStorageService _localStorage;
        public AccountCreateViewModel(HttpClient httpClient,
            NavigationManager navigationManager,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorage = localStorage;
        }

        public async Task Register()
        {
            if (!Password.Equals(PasswordConfirmation))
                throw new Exception();//todo

            IAccount sharedModel = this;

            var result = await _httpClient.PostAsJsonAsync("api/account/register", sharedModel);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                //todo
                //_clientStateService.AddNotification("Dane logowania są błędne.", ENotificationType.Notification);
                throw new Exception();
            else
            {
                //_clientStateService.AddNotification("Zgłoszenie zostało wysłane", ENotificationType.Success);

                var accountId = await result.Content.ReadFromJsonAsync<Guid>();
                await _localStorage.SetItemAsync("id", accountId);
                _navigationManager.NavigateTo("game");
            }
        }
    }
}
