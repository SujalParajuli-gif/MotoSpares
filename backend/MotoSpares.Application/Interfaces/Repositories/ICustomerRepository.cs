using System.Threading.Tasks;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task AddVehicleForUserAsync(UserVehicle userVehicle);
}
