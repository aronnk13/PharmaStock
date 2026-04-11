using PharmaStock.Core.DTO.GRNItem;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IGRNItemRepository : IGenericRepository<GoodsReceiptItem>
    {

        Task<GoodsReciept?> GetGoodsReceiptWithDetailsAsync(int goodsReceiptId);
        Task<GoodsReceiptItem?> GetItemWithDetailsAsync(int goodsReceiptItemId);
        Task<Item?> GetItemByIdAsync(int itemId);
        Task<PurchaseItem?> GetPurchaseItemByIdAsync(int purchaseItemId);
        Task<(List<GRNItemListDTO>, int)> GetFilteredItemsAsync(int goodsReceiptId, GRNItemFilterDTO filter);
        Task<bool> IsDuplicateBatchAsync(int goodsReceiptId, int itemId, int batchNumber, int? excludeId = null);
        Task<int> CountActiveItemsInGrnAsync(int goodsReceiptId);
        Task<bool> HasActiveTaskAsync(int goodsReceiptItemId);
    }
}
