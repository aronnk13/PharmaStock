using PharmaStock.Core.DTO.Bin;
using PharmaStock.Core.DTO.Common;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IBinService
    {
        Task<GetBinDTO> CreateBinAsync(CreateBinDTO request);
        Task<GetBinDTO> UpdateBinAsync(int binId, UpdateBinDTO request);
        Task<GetBinDTO> DeleteBinAsync(int binId);
        Task<GetBinDTO?> GetBinByIdAsync(int binId);
        Task<PaginatedResult<GetBinDTO>> GetAllBinsAsync(BinFilterDTO filter);
        Task<GetBinDTO?> GetBinByIdAsync(int binId);
    }
}
