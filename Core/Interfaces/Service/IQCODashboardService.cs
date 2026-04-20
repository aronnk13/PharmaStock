using PharmaStock.Core.DTO.QCO;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IQCODashboardService
    {
        Task<QCODashboardDTO> GetDashboardAsync();
    }
}
