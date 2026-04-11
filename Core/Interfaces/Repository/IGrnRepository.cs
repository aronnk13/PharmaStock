using PharmaStock.Core.DTO.GoodsReceipt;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IGrnRepository : IGenericRepository<GoodsReciept>
    {
        Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId);
        Task<bool> HasGrnItemsAsync(int grnId);
        Task<GetGrnDTO?> GetGrnDtoByIdAsync(int grnId);
        Task<(List<GetGrnDTO>, int)> GetGrnsByFilterAsync(GrnFilterDTO filter);
        Task<GoodsReceiptStatus?> GetGrnStatusByCodeAsync(string statusCode);
        Task<List<int>> GetReceivablePoStatusIdsAsync();
    }
}
