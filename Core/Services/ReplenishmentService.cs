using PharmaStock.Core.DTO.Register;
using PharmaStock.Core.DTO.Replenishment;
using PharmaStock.Core.Interfaces.Repository;
using PharmaStock.Core.Interfaces.Service;
using PharmaStock.Models;

namespace PharmaStock.Core.Services
{
    public class ReplenishmentService : IReplenishmentService
    {
        private readonly IReplenishmentRequestRepository _replenishmentRepository;
        private readonly IReplenishmentRuleRepository _replenishmentRuleRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IItemRepository _itemRepository;


        public ReplenishmentService(IReplenishmentRequestRepository replenishmentRepository, IReplenishmentRuleRepository replenishmentRuleRepository, ILocationRepository locationRepository, IItemRepository itemRepository)
        {
            _replenishmentRepository = replenishmentRepository;
            _replenishmentRuleRepository = replenishmentRuleRepository;
            _locationRepository = locationRepository;
            _itemRepository = itemRepository;
        }
        public async System.Threading.Tasks.Task DeleteReplenishmentRequest(int requestId)
        {
            var request = await _replenishmentRepository.GetByIdAsync(requestId);
            if(request.Status != 1)
            {
                throw new InvalidOperationException("Only requests that are opened can be deleted.");
            }
             await _replenishmentRepository.DeleteAsync(requestId);
                
        }

        public async System.Threading.Tasks.Task DeleteReplenishmentRule(int ruleId)
        {
            await _replenishmentRuleRepository.DeleteAsync(ruleId);
        }

        public async Task<UpsertResponse> UpsertReplenishmentRequest(UpsertReplenishmentRequestDTO upsertReplenishmentRequestDTO)
        {
            try
            {
                if(await _locationRepository.GetByIdAsync(upsertReplenishmentRequestDTO.LocationId) == null)
                {
                    return new UpsertResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Location ID."
                    };
                }
                if(await _itemRepository.GetByIdAsync(upsertReplenishmentRequestDTO.ItemId) == null)
                {
                    return new UpsertResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Item ID."
                    };
                }
                if (upsertReplenishmentRequestDTO.IsCreate)
                {
                    ReplenishmentRequest request = new ReplenishmentRequest
                    {
                        LocationId = upsertReplenishmentRequestDTO.LocationId,
                        ItemId = upsertReplenishmentRequestDTO.ItemId,
                        SuggestedQty = upsertReplenishmentRequestDTO.SuggestedQuantity,
                        CreatedDate = DateTime.UtcNow,
                        RuleId = upsertReplenishmentRequestDTO.RuleId,
                        Status = upsertReplenishmentRequestDTO.StatusId,

                    };
                    await _replenishmentRepository.AddAsync(request);
                    return new UpsertResponse { IsSuccess = true, Message = $"Request created successfully." };
                }
                else
                {
                    ReplenishmentRequest? existingRequest = await _replenishmentRepository.GetByIdAsync(upsertReplenishmentRequestDTO.RequestId);
                    if (existingRequest == null)
                    {
                        return new UpsertResponse { IsSuccess = false, Message = $"Request with ID {upsertReplenishmentRequestDTO.RequestId} not found!" };
                    }

                    existingRequest.LocationId = upsertReplenishmentRequestDTO.LocationId;
                    existingRequest.ItemId = upsertReplenishmentRequestDTO.ItemId;
                    existingRequest.SuggestedQty = upsertReplenishmentRequestDTO.SuggestedQuantity;
                    existingRequest.CreatedDate = DateTime.UtcNow;
                    existingRequest.RuleId = upsertReplenishmentRequestDTO.RuleId;
                    existingRequest.Status = upsertReplenishmentRequestDTO.StatusId;

                    _replenishmentRepository.Update(existingRequest);

                    return new UpsertResponse { IsSuccess = true, Message = $"Request updated successfully." };
                }
            }
            catch (Exception ex)
            {
                return new UpsertResponse
                {
                    IsSuccess = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }

        public async Task<UpsertResponse> UpsertReplenishmentRule(UpsertReplenishmentRuleDTO upsertReplenishmentRuleDTO)
        {
            if(await _locationRepository.GetByIdAsync(upsertReplenishmentRuleDTO.LocationId) == null)
                {
                    return new UpsertResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Location ID."
                    };
                }
                if(await _itemRepository.GetByIdAsync(upsertReplenishmentRuleDTO.ItemId) == null)
                {
                    return new UpsertResponse
                    {
                        IsSuccess = false,
                        Message = "Invalid Item ID."
                    };
                }
            try
            {
                if (upsertReplenishmentRuleDTO.IsCreate)
                {
                     if (await _replenishmentRuleRepository.IsRuleExist(upsertReplenishmentRuleDTO.LocationId, upsertReplenishmentRuleDTO.ItemId))
                    {
                        return new UpsertResponse
                        {
                            IsSuccess = false,
                            Message = "A rule with this Location and Item already exists."
                        };
                    }

                    ReplenishmentRule rule = new ReplenishmentRule
                    {
                        LocationId = upsertReplenishmentRuleDTO.LocationId,
                        ItemId = upsertReplenishmentRuleDTO.ItemId,
                        MinLevel = upsertReplenishmentRuleDTO.MinLevel,
                        MaxLevel = upsertReplenishmentRuleDTO.MaxLevel,
                        ParLevel = upsertReplenishmentRuleDTO.ParLevel,
                        ReviewCycle = upsertReplenishmentRuleDTO.ReviewCycle

                    };
                    await _replenishmentRuleRepository.AddAsync(rule);
                    return new UpsertResponse { IsSuccess = true, Message = $"Request created successfully." };
                }
                else
                {
                    ReplenishmentRule? existingRule = await _replenishmentRuleRepository.GetByIdAsync(upsertReplenishmentRuleDTO.RuleId);
                    if (existingRule == null)
                    {
                        return new UpsertResponse { IsSuccess = false, Message = $"Rule with ID {upsertReplenishmentRuleDTO.RuleId} not found!" };
                    }

                    existingRule.LocationId = upsertReplenishmentRuleDTO.LocationId;
                    existingRule.ItemId = upsertReplenishmentRuleDTO.ItemId;
                    existingRule.MinLevel = upsertReplenishmentRuleDTO.MinLevel;
                    existingRule.MaxLevel = upsertReplenishmentRuleDTO.MaxLevel;
                    existingRule.ParLevel = upsertReplenishmentRuleDTO.ParLevel;
                    existingRule.ReviewCycle = upsertReplenishmentRuleDTO.ReviewCycle;

                    _replenishmentRuleRepository.Update(existingRule);

                    return new UpsertResponse { IsSuccess = true, Message = $"Rule updated successfully." };
                }
            }
            catch (Exception ex)
            {
                return new UpsertResponse
                {
                    IsSuccess = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
        }
    }
}