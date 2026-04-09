using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class ReplenishmentRequestRepository : GenericRepository<ReplenishmentRequest>, IReplenishmentRequestRepository
    {
        private readonly PharmaStockContext _pharmaStockContext;
        public ReplenishmentRequestRepository(PharmaStockContext pharmaStockContext)
             : base(pharmaStockContext)
        {
            _pharmaStockContext = pharmaStockContext;
        }
    }
}