using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface ITransferService
    {
        public Task UpsertTransferOrder();
        public Task DeleteTransferOrder();

        public Task UpsertTransferItem();
        public Task DeleteTransferItem();
    }
}