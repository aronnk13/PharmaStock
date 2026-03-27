using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class DrugRepositoy : IDrugRepository
    {
        private readonly PharmaStockContext _context;
        public DrugRepositoy(PharmaStockContext context)
        {
            _context = context;
        }
        public async Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId)
        {
            try
            {
                // 1. Check for drug availability
                var drug = await _context.Drugs.FindAsync(DrugId);

                if (drug == null)
                {
                    return new DrugDeletedResponseDTO
                    {
                        IsDeleted = false,
                        Message = "Drug not found."
                    };
                }

                // 2. Change status
                drug.Status = false;
                _context.Drugs.Update(drug);

                var rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return new DrugDeletedResponseDTO
                    {
                        IsDeleted = true,
                        Message = "Drug Deleted Successfully."
                    };
                }

                return new DrugDeletedResponseDTO
                {
                    IsDeleted = false,
                    Message = "Delete failed: No changes were saved to the database."
                };
            }
            catch (Exception ex)
            {
                // Capture the exception message or a custom one
                return new DrugDeletedResponseDTO
                {
                    IsDeleted = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}