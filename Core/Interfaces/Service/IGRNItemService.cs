using PharmaStock.Core.DTO.GRNItem;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IGRNItemService
    {
        System.Threading.Tasks.Task<GRNItemResponseDTO> CreateAsync(CreateGRNItemDTO dto);
        System.Threading.Tasks.Task<GRNItemResponseDTO> GetByIdAsync(int id);
        System.Threading.Tasks.Task<GRNItemsPagedResponseDTO> GetAsync(GRNItemFilterDTO filter);
        System.Threading.Tasks.Task UpdateAsync(UpdateGRNItemDTO dto);
    }
}
