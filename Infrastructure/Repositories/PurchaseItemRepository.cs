using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class PurchaseItemRepository(PharmaStockContext context) :GenericRepository<PurchaseItem>(context), IPurchaseItemRepository
    {
        public Task<bool> HasGRNAsync(int purchaseItemId)
        {
            return context.GoodsReceiptItems.AnyAsync(e => e.PurchaseOrderItemId == purchaseItemId);
        }

        public Task<bool> IsItemIdValid(int itemId)
        {
            return context.Items.AnyAsync(e => e.ItemId == itemId);
        }

        public Task<bool> IsPurchaseOrderIdValid(int purchaseOrderId)
        {
            return context.PurchaseOrders.AnyAsync(e=> e.PurchaseOrderId == purchaseOrderId);
        }

    }
}