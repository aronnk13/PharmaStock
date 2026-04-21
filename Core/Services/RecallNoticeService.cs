using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class RecallNoticeService : IRecallNoticeService
    {
        private readonly IRecallNoticeRepository _repo;

        public RecallNoticeService(IRecallNoticeRepository repo) => _repo = repo;

        public async Task<IEnumerable<RecallNoticeDTO>> GetAllAsync()
        {
            var items = await _repo.GetAllWithDetailsAsync();
            return items.Select(Map);
        }

        public async Task<RecallNoticeDTO?> GetByIdAsync(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            return item == null ? null : Map(item);
        }

        public async Task<RecallNoticeDTO> CreateAsync(CreateRecallNoticeDTO dto)
        {
            var entity = new RecallNotice
            {
                DrugId     = dto.DrugId,
                Reason     = dto.Reason,
                Action     = dto.Action,
                NoticeDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Status     = true   // true = Active/Open recall
            };
            await _repo.AddAsync(entity);
            // re-fetch with navigation properties
            var all    = await _repo.GetAllWithDetailsAsync();
            var created = all.FirstOrDefault(r => r.RecallNoticeId == entity.RecallNoticeId);
            return Map(created ?? entity);
        }

        public async Task<bool> ResolveAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.Status = false;   // false = Resolved
            return await _repo.UpdateAsync(entity);
        }

        private static RecallNoticeDTO Map(RecallNotice r) => new()
        {
            RecallNoticeId = r.RecallNoticeId,
            DrugId         = r.DrugId,
            DrugName       = r.Drug?.GenericName,
            NoticeDate     = r.NoticeDate,
            Reason         = r.Reason,
            Action         = r.Action,
            ActionName     = r.ActionNavigation?.Action,
            Status         = r.Status
        };
    }
}
