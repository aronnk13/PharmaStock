using PharmaStock.Core.DTO.Dashboard;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IInventoryDashboardService
    {
        Task<InventoryDashboardDTO> GetDashboardStatsAsync();
    }
}
