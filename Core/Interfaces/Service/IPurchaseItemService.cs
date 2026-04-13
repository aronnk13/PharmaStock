using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Item;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IPurchaseItemService
    {
        Task<PurchaseItemResponseDTO> AddPIAsync(CreatePurchaseItemDTO dto);
        Task<PurchaseItemResponseDTO> UpdatePIAsync(int id, UpdatePurchaseItemDTO dto);
        System.Threading.Tasks.Task DeletePIAsync(int id);
    }
}