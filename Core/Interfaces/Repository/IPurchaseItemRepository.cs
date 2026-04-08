using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Models;

namespace PharmaStock.Core.Interfaces.Repository
{
    public interface IPurchaseItemRepository: IGenericRepository<PurchaseItem>
    {
        Task<bool> IsPurchaseOrderIdValid(int purchaseOrderId);
        Task<bool> IsItemIdValid(int itemId);
        Task<bool> HasGRNAsync(int purchaseItemId);
    }
}