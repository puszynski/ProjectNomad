namespace AccountModule.DataBaseModels
{
    internal class Account
    {
        public int Id { get; set; }

        public GuestAccount? GuestAccount { get; set; }
        public RegisteredAccount? RegisteredAccount { get; set; }
    }
}
