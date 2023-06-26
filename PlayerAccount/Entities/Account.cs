using AccountModule.Entities.ValueObject;

namespace AccountModule.Entities
{
    internal class Account
    {
        public int Id { get; set; }

        public GuestAccount? GuestAccount { get; set; }
        public RegisteredAccount? RegisteredAccount { get; set; }
    }
}
