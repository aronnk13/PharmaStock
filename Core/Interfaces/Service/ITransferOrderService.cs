using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Transfer;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface ITransferOrderService
    {
        Task<TransferOrderResponseDTO> CreateTransferOrderAsync(CreateTransferOrderDTO dto);
        Task<TransferItemResponseDTO> AddTransferItemAsync(CreateTransferItemDTO dto);
        Task<IEnumerable<TransferOrderResponseDTO>> GetAllTransferOrdersAsync();
        Task<IEnumerable<TransferItemResponseDTO>> GetItemsByTransferOrderIdAsync(int transferOrderId);
    }
}