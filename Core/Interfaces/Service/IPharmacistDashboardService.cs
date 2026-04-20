using PharmaStock.Core.DTO.Pharmacist;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IPharmacistDashboardService
    {
        Task<PharmacistDashboardDTO> GetDashboardAsync(int locationId);
    }
}
