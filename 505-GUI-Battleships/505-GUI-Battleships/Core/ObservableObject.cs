using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace _505_GUI_Battleships.Core;

public class ObservableObject : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void Update<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if ( EqualityComparer<T>.Default.Equals(field, value) )
            return;
        field = value;
        OnPropertyChanged(propertyName);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
