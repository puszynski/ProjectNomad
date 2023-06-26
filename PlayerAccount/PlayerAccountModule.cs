namespace AccountModule
{
    public class PlayerAccountModule : IPlayerAccountModule
    {
        public int Register(string? accountName,
            string? password,
            bool isGuest = false)
        {
            throw new NotImplementedException();
        }

        public int LogIn(string? accountName,
            string? password,
            string? guestToken)
        {
            throw new NotImplementedException();
        }
    }
}