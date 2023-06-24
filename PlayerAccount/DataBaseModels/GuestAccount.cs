namespace AccountModule.DataBaseModels
{
    internal class GuestAccount
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public string Name { get; set; }
        public string ReLoginToken { get; set; }

    }
}
