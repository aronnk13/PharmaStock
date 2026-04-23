using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class ColdChainLogService : IColdChainLogService
    {
        private readonly IColdChainLogRepository _repo;

        public ColdChainLogService(IColdChainLogRepository repo) => _repo = repo;

        public async Task<IEnumerable<ColdChainLogDTO>> GetAllAsync()
        {
            var items = await _repo.GetAllWithDetailsAsync();
            return items.Select(Map);
        }

        public async Task<ColdChainLogDTO> CreateAsync(CreateColdChainLogDTO dto)
        {
            var entity = new ColdChainLog
            {
                LocationId = dto.LocationId,
                SensorId = dto.SensorId,
                Timestamp = dto.Timestamp,
                TemperatureC = dto.TemperatureC,
                Status = dto.Status
            };
            await _repo.AddAsync(entity);
            var created = await _repo.GetAllWithDetailsAsync();
            var full = created.FirstOrDefault(c => c.LogId == entity.LogId) ?? entity;
            return Map(full);
        }

        private static ColdChainLogDTO Map(ColdChainLog c) => new()
        {
            LogId = c.LogId,
            LocationId = c.LocationId,
            LocationName = c.Location?.Name,
            SensorId = c.SensorId,
            Timestamp = c.Timestamp,
            TemperatureC = c.TemperatureC,
            Status = c.Status
        };
    }
}
