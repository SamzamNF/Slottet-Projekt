using System;
using System.Collections.Generic;
using System.Text;

namespace Slottet.Domain.Entities;

public class Risk
{
    public int Id { get; set; }
    // Set as Neutral until actual assessment is made
    public string? RiskAssessment { get; set; } = RiskLevels.Neutral;

    // Foreign key
    public int PostItId { get; set; }
    public PostIt? PostIt { get; set; }

    private Risk() { }

    public static Risk Create(string? riskAssessment = RiskLevels.Neutral)
    {
        var assessment = string.IsNullOrWhiteSpace(riskAssessment) ? RiskLevels.Neutral : riskAssessment;
        ValidateRiskLevel(assessment);

        return new Risk
        {
            RiskAssessment = assessment
        };
    }

    public void Update(string? newRiskAssessment)
    {
        var assessment = string.IsNullOrWhiteSpace(newRiskAssessment) ? RiskLevels.Neutral : newRiskAssessment;
        ValidateRiskLevel(assessment);

        RiskAssessment = assessment;
    }
    
    private static void ValidateRiskLevel(string assessment)
    {
        if (assessment != RiskLevels.Neutral && 
            assessment != RiskLevels.Low && 
            assessment != RiskLevels.Medium && 
            assessment != RiskLevels.High)
        {
            throw new ArgumentException($"Ugyldigt risikoniveau: {assessment}");
        }
    }

}
