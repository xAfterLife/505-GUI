using System;

namespace _505_GUI_Battleships.Services;

internal class EloService : ServiceBase
{
    public enum EloCalculationTarget
    {
        Winner,
        Looser
    }

    public enum EloLossProtection
    {
        None,
        Fixxed,
        Percentual
    }

    public static long CalculateElo(long winner, long looser, EloCalculationTarget calculationTarget, EloLossProtection lossProtection = EloLossProtection.None, long lossProtectionThreashold = 0)
    {
        var combined = winner + looser;

        var calculatedPercentage = 100 / combined * calculationTarget switch
        {
            EloCalculationTarget.Looser => looser,
            EloCalculationTarget.Winner => winner,
            _                           => throw new ArgumentOutOfRangeException(nameof(calculationTarget), calculationTarget, null)
        };

        var calculatedValue = calculatedPercentage * (combined * 0.01);

        switch ( lossProtection )
        {
            case EloLossProtection.None:
                return (long)calculatedValue;
            case EloLossProtection.Fixxed:
                return (long)(calculatedValue > lossProtectionThreashold ? lossProtectionThreashold : calculatedValue);
            case EloLossProtection.Percentual:
                var threshold = lossProtectionThreashold / 100 * calculationTarget switch
                {
                    EloCalculationTarget.Winner => winner,
                    EloCalculationTarget.Looser => looser,
                    _                           => throw new ArgumentOutOfRangeException(nameof(calculationTarget), calculationTarget, null)
                };

                return (long)(calculatedValue > threshold ? threshold : calculatedValue);
            default:
                throw new ArgumentOutOfRangeException(nameof(lossProtection), lossProtection, null);
        }
    }
}
