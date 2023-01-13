using System;
using System.Media;
using System.Reflection;

namespace _505_GUI_Battleships.Services;

public static class SoundPlayerService
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
    ///     Plays Sound based on SoundType enum
    /// </summary>
    /// <param name="type">SoundType</param>
    public static void PlaySound(SoundType type)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream($@"_505_GUI_Battleships.Resources.Sounds.{type switch
        {
            SoundType.FinalTreffer  => $"Final_Treffer_{Random.Shared.Next(1, 3)}.wav",
            SoundType.Geschoss      => $"Geschoss_{Random.Shared.Next(1, 2)}.wav",
            SoundType.Treffer       => $"Treffer_{Random.Shared.Next(1, 4)}.wav",
            SoundType.Wassertreffer => $"Wassertreffer_{Random.Shared.Next(1, 3)}.wav",
            _                       => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        }}");
        var player = new SoundPlayer(stream);
        player.Load();
        player.Play();
    }
}
