namespace AccountModule.Logic.AccountLogIn
{
    internal interface IAccountLogInHandler
    {
        internal Task<Guid?> LogIn();
    }
}
