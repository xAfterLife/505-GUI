using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _505_GUI_Battleships.MVVM.ViewModel;

public class GameOptionsViewModel : ObservableObject
{
    public static ICommand? AddShipCommand { get; set; }
    public static ICommand? DeleteShipCommand { get; set; }
}
