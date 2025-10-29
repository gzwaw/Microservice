using Microsoft.AspNetCore.Mvc;
using MilleniumTest.Interfaces;
using MilleniumTest.Model;

namespace MilleniumTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountHandler _handler;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountHandler handler, ILogger<AccountController> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAll()
        {
            _logger.LogInformation("GetAll");
            IEnumerable<Account> result = await _handler.GetAccountsAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetById(int id)
        {
            _logger.LogInformation($"GetById({id})");
            Account? account = await _handler.GetByIdAsync(id);
            
            if (account == null) 
                return NotFound();
            
            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(Account account)
        {
            //Tutaj warto by rozdzielić Account na AccountCreateDTO i AccountUpdateDTO, żeby przy Create niepotrzebnie nie przyjmować Id, ale zabrakło mi czasu
            _logger.LogInformation($"Create, {account.FirstName} {account.LastName}");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Account? created = await _handler.CreateAsync(account);

            if (created == null)
                return StatusCode(500);
            
            return Ok(created.Id);
        }

        [HttpPut()]
        public async Task<ActionResult> Update(Account updated)
        {
            _logger.LogInformation($"Update, id: {updated.Id}");

            //Tutaj warto by rozdzielić Account na AccountCreateDTO i AccountUpdateDTO, z różną wymagalnością atrybutów, żeby niepotrzebnie nie wymagać imienia i nazwiska, ale zabrakło mi czasu
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool result = await _handler.UpdateAsync(updated);
            
            if (!result) 
                return NotFound();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Delete({id})");
            bool deleted = await _handler.DeleteAsync(id);
            
            if (!deleted) 
                return NotFound();
            
            return NoContent();
        }
    }
}
