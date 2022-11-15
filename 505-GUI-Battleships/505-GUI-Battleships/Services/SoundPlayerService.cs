using System;
using System.Media;
using System.Reflection;

namespace _505_GUI_Battleships.Services;

internal sealed class SoundPlayerService : ServiceBase
{
    /// <summary>
    ///     Type of Sound (Name)
    /// </summary>
    public enum SoundType
    {
        FinalTreffer,
        Geschoss,
        Treffer,
        Wassertreffer
    }

    /// <summary>
    ///     Random
    /// </summary>
    private static readonly Random Rnd = new((int)(Environment.TickCount * DateTime.Now.ToFileTimeUtc()));

    /// <summary>
    ///     Plays Sound based on SoundType enum
    /// </summary>
    /// <param name="type">SoundType</param>
    public void PlaySound(SoundType type)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream($@"_505_GUI_Battleships.Resources.Sounds.{type switch
            {
                SoundType.FinalTreffer  => $"Final_Treffer_{Rnd.Next(1, 3)}.wav",
                SoundType.Geschoss      => $"Geschoss_{Rnd.Next(1, 2)}.wav",
                SoundType.Treffer       => $"Treffer_{Rnd.Next(1, 4)}.wav",
                SoundType.Wassertreffer => $"Wassertreffer_{Rnd.Next(1, 3)}.wav",
                _                       => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            }}");
            var player = new SoundPlayer(stream);
            player.Play();
        }
        catch ( Exception e )
        {
            OnError(e);
            throw;
        }
    }
}
