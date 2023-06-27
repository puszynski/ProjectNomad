namespace AccountModule.Logic.AccountRegistration
{
    internal interface IAccountRegistration
    {
        internal Task<Guid> Register();
    }
}
