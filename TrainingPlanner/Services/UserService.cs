using TrainingPlanner.Services.Api;
using TrainingPlanner.Services.Contracts;
using TrainingPlanner.Models;

namespace TrainingPlanner.Services.Implementation;

public sealed class UserService(ITrainingPlannerApiClient apiClient) : IUserService
{
    ITrainingPlannerApiClient _apiClient = apiClient;

    public async Task CreateUser(UserDTO user, CancellationToken cancellationToken = default)
    {
        
        UserDTO result = await _apiClient.PostAsync<UserDTO, UserDTO>("/users", user, cancellationToken);
    }

    public Task<UserDTO> GetUserByEMail(string eMail, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}