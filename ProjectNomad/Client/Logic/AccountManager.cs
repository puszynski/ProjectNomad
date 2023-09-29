using Blazored.LocalStorage;

namespace ProjectNomad.Client.Logic
{
    internal class AccountManager
    {
        readonly ILocalStorageService _localStorage;

        public AccountManager(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        //todo move login

        internal async Task Logout()
        {
            //todo remove await _localStorage.SetItemAsync("id", accountId);
            await _localStorage.RemoveItemAsync("id");
        }
    }
}
