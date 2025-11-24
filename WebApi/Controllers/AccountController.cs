using Application.Accounts.Commands.CreateAccount;
using Application.Accounts.Commands.DeleteAccount;
using Application.Accounts.Commands.UpdateAccount;
using Application.Accounts.Querries;
using Application.Accounts.Querries.GetAccountById;
using Application.Accounts.Querries.GetAllAccounts;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Requests.Account;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IMediator mediator, IMapper mapper, ILogger<AccountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Pobiera listę wszystkich kont
        /// </summary>
        /// <remarks>
        /// To zapytanie zwraca pełną listę kont z bazy danych.
        /// </remarks>
        /// <response code="200">Lista kont została pobrana poprawnie</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
        {
            _logger.LogInformation("GetAll");

            IEnumerable<AccountDto> accounts = await _mediator.Send(new GetAllAccountsQuery());
            return Ok(accounts);
        }

        /// <summary>
        /// Pobiera konto o podanym id
        /// </summary>
        /// <response code="200">Pobrano konto o podanym id</response>
        /// <response code="404">Nie odnaleziono konta o podanym id</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AccountDto>> GetById(int id)
        {
            _logger.LogInformation("GetById({id})", id);
            AccountDto? account = await _mediator.Send(new GetAccountByIdQuery(id));

            if (account == null)
                return NotFound();

            return Ok(account);
        }

        /// <summary>
        /// Tworzy nowe konto
        /// </summary>
        /// <returns>id utworzonego rekordu</returns>
        /// <response code="201">Utworzono konto</response>
        /// <response code="400">Nieprawidłowe dane wejściowe</response>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Create([FromBody] CreateAccountRequest request)
        {
            _logger.LogInformation("Create, {FirstName} {LastName}", request.FirstName, request.LastName);

            CreateAccountCommand command = _mapper.Map<CreateAccountCommand>(request);

            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        /// <summary>
        /// Aktualizuje istniejące konto
        /// </summary>
        /// <response code="204">Zaktualizowano</response>
        /// <response code="404">Nie znaleziono konta o podanym id</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateAccountRequest request)
        {
            _logger.LogInformation($"Update, id: {id}");

            UpdateAccountCommand command = _mapper.Map<UpdateAccountCommand>(request);
            command.Id = id;

            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Usuwa istniejące konto
        /// </summary>
        /// <response code="204">Usunięto</response>
        /// <response code="404">Nie znaleziono konta o podanym id</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteAccountCommand(id));
            return NoContent();
        }
    }
}