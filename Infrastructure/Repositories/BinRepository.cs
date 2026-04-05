using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Bin;
using PharmaStock.Core.Interfaces;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class BinRepository : GenericRepository<Bin>, IBinRepository
    {
        private readonly PharmaStockContext _context;

        public BinRepository(PharmaStockContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsBinCodeExistsInLocation(int locationId, string code, int? excludeBinId = null)
        {
            return await _context.Bins.AnyAsync(b =>
                b.LocationId == locationId &&
                b.Code == code &&
                (!excludeBinId.HasValue || b.BinId != excludeBinId.Value));
        }

        public async Task<Location?> GetLocationByIdAsync(int locationId)
        {
            return await _context.Locations.FindAsync(locationId);
        }

        public async Task<BinStorageClass?> GetBinStorageClassByIdAsync(int storageClassId)
        {
            return await _context.BinStorageClasses.FindAsync(storageClassId);
        }

        public async Task<bool> HasInventoryAsync(int binId)
        {
            return await _context.InventoryBalances
                .AnyAsync(ib => ib.BinId == binId && ib.QuantityOnHand > 0);
        }

        public async Task<bool> HasOpenPutAwayTasksAsync(int binId)
        {
            // Status = false means pending (not yet completed)
            return await _context.Tasks
                .AnyAsync(t => t.TargetBinId == binId && t.Status == false);
        }

        public async Task<GetBinDTO?> GetBinDtoByIdAsync(int binId)
        {
            return await _context.Bins
                .Where(b => b.BinId == binId)
                .Select(b => new GetBinDTO
                {
                    BinId = b.BinId,
                    LocationId = b.LocationId,
                    LocationName = b.Location.Name,
                    Code = b.Code,
                    BinStorageClassId = b.BinStorageClass,
                    StorageClass = b.BinStorageClassNavigation.StorageClass,
                    IsQuarantine = b.IsQuarantine,
                    MaxCapacity = b.MaxCapacity,
                    IsActive = b.StatusId
                })
                .FirstOrDefaultAsync();
        }
    }
}
