using MediatR;
using Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Application.Accounts.Querries;
using Application.Accounts.Querries.GetAllAccounts;
using Application.Accounts.Querries.GetAccountById;
using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.UpdateAccount;
using Application.Accounts.Commands.DeleteAccount;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IMediator mediator, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
        {
            _logger.LogInformation("GetAll");

            IEnumerable<AccountDto> accounts = await _mediator.Send(new GetAllAccountsQuery());
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetById(int id)
        {
            _logger.LogInformation($"GetById({id})");
            AccountDto? account = await _mediator.Send(new GetAccountByIdQuery(id));
            
            if (account == null) 
                return NotFound();
            
            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateAccountCommand command)
        {
            _logger.LogInformation($"Create, {command.FirstName} {command.LastName}");

            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = id }, null);
        }

        [HttpPut()]
        public async Task<ActionResult> Update(UpdateAccountCommand command)
        {
            _logger.LogInformation($"Update, id: {command.Id}");

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAccountCommand(id));
            return NoContent();
        }
    }
}