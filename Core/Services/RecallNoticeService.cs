using PharmaStock.Core.DTO.QCO;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;

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
            var entity = new PharmaStock.Models.RecallNotice
            {
                DrugId = dto.DrugId,
                NoticeDate = dto.NoticeDate,
                Reason = dto.Reason,
                Action = dto.Action,
                Status = false  // false = Open
            };
            await _repo.AddAsync(entity);
            var items = await _repo.GetAllWithDetailsAsync();
            var full = items.FirstOrDefault(r => r.RecallNoticeId == entity.RecallNoticeId) ?? entity;
            return Map(full);
        }

        public async Task<bool> CloseAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.Status = true;  // true = Closed
            return await _repo.UpdateAsync(entity);
        }

        private static RecallNoticeDTO Map(PharmaStock.Models.RecallNotice r) => new()
        {
            RecallNoticeId = r.RecallNoticeId,
            DrugId = r.DrugId,
            DrugName = r.Drug?.GenericName,
            NoticeDate = r.NoticeDate,
            Reason = r.Reason,
            Action = r.Action,
            ActionName = r.ActionNavigation?.Action,
            Status = r.Status
        };
    }
}
