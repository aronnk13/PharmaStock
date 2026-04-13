using PharmaStock.Core.DTO.GRNItem;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IGRNItemService
    {
        Task<GRNItemResponseDTO> CreateAsync(CreateGRNItemDTO dto);
        Task<object> GetAsync(GRNItemFilterDTO filter);
        Task<GRNItemResponseDTO> UpdateAsync(UpdateGRNItemDTO dto);
    }
}
