using TrainingPlanner.Services.Api;
using TrainingPlanner.Services.Contracts;
using TrainingPlanner.Models;
using System.Net.Mail;

namespace TrainingPlanner.Services.Implementation;

public sealed class UserService(ITrainingPlannerApiClient apiClient) : IUserService
{
    ITrainingPlannerApiClient _apiClient = apiClient;

    public async Task<UserDTO> CreateUser(UserDTO user, CancellationToken cancellationToken = default)
    {
        if (!IsUserValid(user))
            throw new ArgumentException("The user details are invalid.", nameof(user));

        UserDTO? result = await _apiClient.PostAsync<UserDTO, UserDTO>("/users", user, cancellationToken);

        if (result == null)
            throw new InvalidOperationException("The API did not return the created user.");

        Console.WriteLine($"USER SERVICE CREATED {user}");
        return result;
    }

    public Task<UserDTO> GetUserByEMail(string eMail, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    private bool IsUserValid(UserDTO user)
    {
        if (user is null ||
            string.IsNullOrWhiteSpace(user.FirstName) ||
            string.IsNullOrWhiteSpace(user.LastName) ||
            string.IsNullOrWhiteSpace(user.Email) ||
            string.IsNullOrWhiteSpace(user.LoginProvider) ||
            string.IsNullOrWhiteSpace(user.NameIdentifier))
        {
            return false;
        }

        try
        {
            MailAddress address = new(user.Email);
            return string.Equals(address.Address, user.Email.Trim(), StringComparison.OrdinalIgnoreCase);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}