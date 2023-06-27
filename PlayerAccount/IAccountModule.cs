namespace AccountModule
{
    public interface IAccountModule
    {
        Task<Guid> Register(string? accountName, 
            string? password, 
            bool isGuest = false);

        Task<Guid?> LogIn(string? accountName,
            string? password,
            string? guestToken);
    }
}
