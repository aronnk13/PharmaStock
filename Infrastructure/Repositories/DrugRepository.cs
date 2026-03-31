using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaStock.Core.DTO.Auth;
using PharmaStock.Core.DTO.Common;
using PharmaStock.Core.DTO.Drug;
using PharmaStock.Core.Interfaces;
using PharmaStock.Models;

namespace PharmaStock.Infrastructure.Repositories
{
    public class DrugRepository(PharmaStockContext context) : GenericRepository<Drug>(context), IDrugRepository
    {
        
        public async Task<(List<Drug>,int)> GetDrugsByFilterAsync(DrugFilterDTO filter)
        {
            var result = context.Drugs.AsQueryable();
            if (filter.GenericName != null)
            {
                result = result.Where(q => q.GenericName.Contains(filter.GenericName));
            }
            if (filter.StorageClass != null)
            {
                result = result.Where(q => q.StorageClass == filter.StorageClass);
            }
            if (filter.ControlClass != null)
            {
                result = result.Where(q => q.ControlClass == filter.ControlClass);
            }
            if (filter.Status != null)
            {
                result = result.Where(q => q.Status == filter.Status);
            }

            var totalCount = await result.CountAsync();

            var drugs = await result.Skip((filter.Page-1)*filter.PageSize)
                                    .Take(filter.PageSize)
                                    .ToListAsync();
            
            return (drugs, totalCount);
        }
         public async Task<DrugDeletedResponseDTO> DeleteDrug(int DrugId)
        {
            try
            {
                // 1. Check for drug availability
                var drug = await context.Drugs.FindAsync(DrugId);

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
                context.Drugs.Update(drug);

                var rowsAffected = await context.SaveChangesAsync();

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