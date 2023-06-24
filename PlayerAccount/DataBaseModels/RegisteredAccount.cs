namespace AccountModule.DataBaseModels
{
    internal class RegisteredAccount
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
    }
}
