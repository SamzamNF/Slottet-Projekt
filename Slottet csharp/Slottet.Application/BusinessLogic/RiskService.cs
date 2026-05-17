using Slottet.Application.Interfaces;
using Slottet.Domain.Entities;
using Slottet.Shared.DTO;

namespace Slottet.Application.BusinessLogic;

public class RiskService
{
    private readonly IRiskRepository _riskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RiskService(IRiskRepository riskRepository, IUnitOfWork unitOfWork)
    {
        _riskRepository = riskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RiskDTO?> GetRiskAsync(int id)
    {
        var risk = await _riskRepository.GetByIdAsync(id);

        if (risk == null) return null;

        return new RiskDTO
        {
            Id = risk.Id,
            RiskAssessment = risk.RiskAssessment ?? "Neutral"
        };
    }

    public async Task<bool> UpdateRiskAsync(RiskDTO riskDto)
    {
        var existingRisk = await _riskRepository.GetByIdAsync(riskDto.Id);

        if (existingRisk == null) return false;

        existingRisk.RiskAssessment = riskDto.RiskAssessment;

        await _riskRepository.Update(existingRisk);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}