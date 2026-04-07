using PharmaStock.Core.DTO.Bin;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IBinRepository : IGenericRepository<Bin>
    {
        Task<bool> IsBinCodeExistsInLocation(int locationId, string code, int? excludeBinId = null);
        Task<Location?> GetLocationByIdAsync(int locationId);
        Task<BinStorageClass?> GetBinStorageClassByIdAsync(int storageClassId);
        Task<bool> HasInventoryAsync(int binId);
        Task<bool> HasOpenPutAwayTasksAsync(int binId);
        Task<GetBinDTO?> GetBinDtoByIdAsync(int binId);
        Task<(List<GetBinDTO>, int)> GetBinsByFilterAsync(BinFilterDTO filter);
    }
}
