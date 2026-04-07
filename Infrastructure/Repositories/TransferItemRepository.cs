using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class TransferItemRepository : GenericRepository<TransferItem>, ITransferItemRepository
    {
        private readonly PharmaStockContext _pharmaStockContext;
        public TransferItemRepository(PharmaStockContext pharmaStockContext)
             : base(pharmaStockContext)
        {
            _pharmaStockContext = pharmaStockContext;
        }
    }
}