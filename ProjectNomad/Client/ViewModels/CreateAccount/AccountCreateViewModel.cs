using Microsoft.AspNetCore.Components;
using ProjectNomad.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace ProjectNomad.Client.ViewModels.CreateAccount
{
    public class AccountCreateViewModel
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
        public AccountCreateViewModel(HttpClient httpClient, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public async Task Register()
        {
            if (!Password.Equals(PasswordConfirmation))
                throw new Exception();//todo

            Account sharedModel = this;

            var result = await _httpClient.PostAsJsonAsync("api/account/register", sharedModel);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                //todo
                //_clientStateService.AddNotification("Dane logowania są błędne.", ENotificationType.Notification);
                throw new Exception(); //todo remove
            else
            {
                //_clientStateService.AddNotification("Zgłoszenie zostało wysłane", ENotificationType.Success);
                _navigationManager.NavigateTo("counter"); //todo
            }
        }

        public static implicit operator Account(AccountCreateViewModel model)
            => new Account
            {
                AccountName = model.Name,
                Password = model.Password
            };
    }
}
