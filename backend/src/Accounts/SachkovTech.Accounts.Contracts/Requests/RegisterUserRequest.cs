namespace SachkovTech.Accounts.Contracts.Requests;

public record RegisterUserRequest(string Email, string UserName, string Password);