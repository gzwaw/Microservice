namespace WebAPI.Requests.Account
{
    public record CreateAccountRequest(string FirstName, string LastName, string CompanyName, string Email, string PhoneNo, string Location);
}