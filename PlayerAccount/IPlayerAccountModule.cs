namespace AccountModule
{
    public interface IPlayerAccountModule
    {
        int Register(string? accountName, 
            string? password, 
            bool isGuest = false);

        int LogIn(string? accountName,
            string? password,
            string? guestToken);
    }
}
