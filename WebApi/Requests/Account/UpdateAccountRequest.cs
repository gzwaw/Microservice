namespace WebAPI.Requests.Account
{
    public record UpdateAccountRequest(string FirstName, string LastName, string CompanyName, string Email, string PhoneNo, string Location);
}
