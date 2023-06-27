using AccountModule;
using Microsoft.AspNetCore.Mvc;
using ProjectNomad.Shared.Models;

namespace ProjectNomad.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountModule _accountModule;
        public AccountController(IAccountModule accountModule)
        {
            _accountModule = accountModule;
        }

        //todo guests
        [HttpPost("login")]
        public async Task<ActionResult> LogIn(Account model)
        {
            var accountId = await _accountModule.LogIn(model.AccountName, model.Password, null);

            if (accountId == null)
                return NotFound();

            return Ok(accountId);
        }

        //todo guests
        [HttpPost("register")]
        public async Task<ActionResult> Register(Account model) 
        {
            var accountId = await _accountModule.Register(model.AccountName, model.Password);

            if (accountId == null)
                return Problem();

            return Ok(accountId);
        }
    }
}
