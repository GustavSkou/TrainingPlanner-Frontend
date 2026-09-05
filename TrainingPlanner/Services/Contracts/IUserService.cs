using TrainingPlanner.Models;

namespace TrainingPlanner.Services.Contracts;

public interface IUserService
{
    Task<UserDTO> CreateUser(UserDTO user, CancellationToken cancellationToken = default);
    Task<UserDTO> GetUserByEMail(string eMail, CancellationToken cancellationToken = default);
}