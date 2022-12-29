using _505_GUI_Battleships.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class ShipSizeSelectorModel : ObservableObject
{
    private string _shipImagePath;
    private readonly string[] _shipImagePathList;
    private int _shipImageListIndex;
    public ShipSizeSelectorModel Instance { get; }


    //TODO: Fix this lol
   public static ICommand? EnlargeShipSizeCommand => 
        new RelayCommand(instance =>
        {
            if ( instance is not ShipSizeSelectorModel ShipSizeSelector )
                return;
            Trace.WriteLine(ShipSizeSelector._shipImageListIndex);

            EnlargeShipSizeCommandPressed?.Invoke(ShipSizeSelector, EventArgs.Empty);
        });

    public static ICommand? ReduceShipSizeCommand =>
        new RelayCommand(instance =>
        {
            if (instance is not ShipSizeSelectorModel ShipSizeSelector)
                return;
            Trace.WriteLine(ShipSizeSelector._shipImageListIndex);

            ReduceShipSizeCommandPressed?.Invoke(ShipSizeSelector, EventArgs.Empty);
        });

    public static event EventHandler? EnlargeShipSizeCommandPressed;
    
    public static event EventHandler? ReduceShipSizeCommandPressed;

    public string ShipImagePath 
    { 
        get => _shipImagePath;
        set => Update(ref _shipImagePath, value);
    }

    public int ShipImageListIndex 
    {
        get => _shipImageListIndex;
        set => Update(ref _shipImageListIndex, value);
    }

    public ShipSizeSelectorModel()
    {
        _shipImagePathList = new string[5] { "../../../Ressources/Ships/1ShipRescueHorizontal.png",
                                             "../../../Ressources/Ships/2ShipRescueHorizontal.png",
                                             "../../../Ressources/Ships/3ShipRescueHorizontal.png",
                                             "../../../Ressources/Ships/4ShipRescueHorizontal.png",
                                             "../../../Ressources/Ships/5ShipRescueHorizontal.png"};
        Instance = this;
        _shipImageListIndex = 1;
        _shipImagePath = _shipImagePathList[_shipImageListIndex];
    }

    public void IncrementIndex()
    {
        _shipImageListIndex++;
    }

    public void DecrementIndex()
    {
        _shipImageListIndex--;
    }


    /*public void UpdateImage() 
    {
        OnPropertyChanged(nameof(ShipImagePath));
    }*/

}
