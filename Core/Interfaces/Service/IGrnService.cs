using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.GoodsReceipt;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IGrnService
    {
        Task<GetGrnDTO> CreateGrnAsync(CreateGrnDTO request, int userId);
        Task<GetGrnDTO> UpdateGrnAsync(int grnId, UpdateGrnDTO request);
        Task<GetGrnDTO?> GetGrnByIdAsync(int grnId);
        Task<GetGrnWithItemsDTO?> GetGrnWithItemsAsync(int grnId);
        Task<PaginatedResult<GetGrnDTO>> GetAllGrnsAsync(GrnFilterDTO filter);
        Task<List<GetGrnDTO>> GetPendingQcAsync();
        Task<CompleteQcResultDTO> CompleteQcAsync(int grnId, CompleteQcDTO dto, int userId);
    }
}
