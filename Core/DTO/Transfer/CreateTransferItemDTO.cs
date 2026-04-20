using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaStock.Core.DTO.Transfer
{
    public class CreateTransferItemDTO
{
    [Required]
    public int TransferOrderId { get; set; }
    [Required]
    public int ItemId { get; set; }
    [Required]
    public int InventoryLotId { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
    public int Quantity { get; set; }
}
}