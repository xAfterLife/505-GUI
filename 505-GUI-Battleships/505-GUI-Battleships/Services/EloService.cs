using System;

namespace _505_GUI_Battleships.Services;

internal sealed class EloService : ServiceBase
{
    /// <summary>
    ///     Weigthing for the EloCalculation
    /// </summary>
    public enum EloCalculationTarget
    {
        Winner,
        Looser
    }

    /// <summary>
    ///     Type of Protection against Elo-Loss
    /// </summary>
    public enum EloLossProtection
    {
        None,
        Fixxed,
        Percentual
    }

    /// <summary>
    ///     ctor
    /// </summary>
    /// <param name="winner">Elo of the Winner</param>
    /// <param name="looser">Elo of the Looser</param>
    /// <param name="calculationTarget">Target</param>
    /// <param name="lossProtection">Type of Protection</param>
    /// <param name="lossProtectionThreashold">Optional Protection Threshold</param>
    /// <returns></returns>
    public long CalculateElo(long winner, long looser, EloCalculationTarget calculationTarget, EloLossProtection lossProtection = EloLossProtection.None, long lossProtectionThreashold = 0)
    {
        try
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
        catch ( Exception e )
        {
            OnError(e);
            throw;
        }
    }
}
