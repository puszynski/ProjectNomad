using AccountModule;
using AccountModule.DtoModels;
using GameModule;
using Microsoft.AspNetCore.Mvc;

namespace ProjectNomad.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountModule _accountModule;
        private readonly IGameModule _gameModule;
        public AccountController(IAccountModule accountModule,
            IGameModule gameModule)
        {
            _accountModule = accountModule;
            _gameModule = gameModule;
        }

        //todo guests
        [HttpPost("login")]
        public async Task<ActionResult> LogIn(AccountDto model)
        {
            var accountId = await _accountModule.LogIn(model.Name, model.Password, null);

            if (accountId == null)
                return NotFound();

            return Ok(accountId);
        }

        //todo guests
        [HttpPost("register")]
        public async Task<ActionResult> Register(AccountDto model) 
        {
            var accountId = await _accountModule.Register(model.Name, model.Password);

            if (accountId == Guid.Empty)
                return Problem();

            await _gameModule.InitPlayerGameObjects(accountId);

            return Ok(accountId);
        }

        [HttpPost("generateNewTribeMembers")]
        public async Task<ActionResult> GenerateNewTribeMembers([FromBody]Guid accountId)
        {
            await _gameModule.InitPlayerGameObjectsForExistingTribe(accountId);
            return Ok();
        }
    }
}
