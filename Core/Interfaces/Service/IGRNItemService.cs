using PharmaStock.Core.DTO.GRNItem;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IGRNItemService
    {
        Task<GRNItemResponseDTO> CreateAsync(int goodsReceiptId, CreateGRNItemDTO dto);
        Task<GRNItemsPagedResponseDTO> GetAllAsync(int goodsReceiptId, GRNItemFilterDTO filter);
        Task<GRNItemResponseDTO> GetByIdAsync(int goodsReceiptId, int goodsReceiptItemId);
        Task<GRNItemResponseDTO> UpdateAsync(int goodsReceiptId, int goodsReceiptItemId, UpdateGRNItemDTO dto);
        Task<GRNItemDeleteResponseDTO> DeleteAsync(int goodsReceiptId, int goodsReceiptItemId, DeleteGRNItemDTO dto);
    }
}
