using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IColdChainLogRepository : IGenericRepository<ColdChainLog>
    {
        Task<IEnumerable<ColdChainLog>> GetAllWithDetailsAsync();
        Task<int> CountActiveExcursionsAsync();
    }
}
