using PharmaStock.Core.DTO.GoodsReceipt;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IGrnRepository : IGenericRepository<GoodsReciept>
    {
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId);
        Task<bool> HasGrnItemsAsync(int grnId);
        Task<GetGrnDTO?> GetGrnDtoByIdAsync(int grnId);
        Task<GetGrnWithItemsDTO?> GetGrnWithItemsAsync(int grnId);
        Task<(List<GetGrnDTO>, int)> GetGrnsByFilterAsync(GrnFilterDTO filter);
        Task<List<GetGrnDTO>> GetPendingQcGrnsAsync();
        Task<GoodsReceiptStatus?> GetGrnStatusByCodeAsync(string statusCode);
        Task<List<int>> GetReceivablePoStatusIdsAsync();
        Task<string?> GetUsernameByIdAsync(int userId);

        // Lookup helpers used by CompleteQc
        Task<int?> GetInventoryLotStatusIdByNameAsync(string statusName);
        Task<int?> GetPoStatusIdByNameAsync(string statusName);
        Task<int?> GetStockTransitionTypeIdByNameAsync(string typeName);
        Task<int?> GetFirstBinIdAtLocationAsync(int locationId);
        Task<GoodsReciept?> GetGrnWithPoAndItemsAsync(int grnId);
    }
}
