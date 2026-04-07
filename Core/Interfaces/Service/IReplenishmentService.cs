using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaStock.Core.DTO.Register;
using PharmaStock.Core.DTO.Replenishment;

namespace PharmaStock.Core.Interfaces.Service
{
    public interface IReplenishmentService
    {
        public Task<UpsertResponse> UpsertReplenishmentRequest(UpsertReplenishmentRequestDTO upsertReplenishmentRequestDTO);
        public Task DeleteReplenishmentRequest(int requestId);

        public Task<UpsertResponse> UpsertReplenishmentRule(UpsertReplenishmentRuleDTO upsertReplenishmentRuleDTO);
        public Task DeleteReplenishmentRule(int ruleId);


    }
}