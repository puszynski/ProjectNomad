using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ProjectNomad.Shared.Interfaces;
using System.Net.Http.Json;

namespace ProjectNomad.Client.Logic
{
    public class AccountManager
    {
        readonly HttpClient _httpClient;
        readonly ILocalStorageService _localStorage;
        readonly NavigationManager _navigationManager;

        public AccountManager(ILocalStorageService localStorage,
            HttpClient httpClient,
            NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _navigationManager = navigationManager;
        }

        internal async Task LogIn(IAccount logInData)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account/register", logInData);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                //todo
                throw new Exception();
            else
            {
                var accountId = await result.Content.ReadFromJsonAsync<Guid>();
                await _localStorage.SetItemAsync("id", accountId);
                _navigationManager.NavigateTo("game");
            }
        }

        internal async Task Logout()
        {
            await _localStorage.RemoveItemAsync("id");
        }
    }
}
