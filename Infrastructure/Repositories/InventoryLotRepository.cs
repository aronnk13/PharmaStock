using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class InventoryLotRepository 
        : GenericRepository<InventoryLot>, IInventoryLotRepository
    {
        public InventoryLotRepository(PharmaStockContext context)
            : base(context)
        {
        }
    }
}