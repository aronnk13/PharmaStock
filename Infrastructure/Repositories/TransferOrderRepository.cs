using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class TransferOrderRepository : GenericRepository<TransferOrder>, ITransferOrderRepository
    {
          private readonly PharmaStockContext _pharmaStockContext;
       public TransferOrderRepository(PharmaStockContext pharmaStockContext) 
            : base(pharmaStockContext)
        {
            _pharmaStockContext = pharmaStockContext;
        }
    }
}