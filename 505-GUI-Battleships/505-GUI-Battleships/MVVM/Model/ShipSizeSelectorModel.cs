using _505_GUI_Battleships.Core;
using System;
using System.Diagnostics;
using System.Windows.Input;

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

            EnlargeShipSizeCommandPressed?.Invoke(ShipSizeSelector, EventArgs.Empty);
            Trace.WriteLine(ShipSizeSelector._shipImageListIndex);
        });

    public static ICommand? ReduceShipSizeCommand => 
        new RelayCommand(instance =>
        {
            if (instance is not ShipSizeSelectorModel ShipSizeSelector)
                return;

            ReduceShipSizeCommandPressed?.Invoke(ShipSizeSelector, EventArgs.Empty);
            Trace.WriteLine(ShipSizeSelector._shipImageListIndex);
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
        _shipImagePathList = new string[5] { "../../../Ressources/Ships/1ShipPatrolHorizontal.png",
                                             "../../../Ressources/Ships/2ShipRescueHorizontal.png",
                                             "../../../Ressources/Ships/3ShipSupHorizontal.png",
                                             "../../../Ressources/Ships/4ShipDestroyerHorizontal.png",
                                             "../../../Ressources/Ships/5ShipBattleshipHorizontal.png"};
        Instance = this;
        _shipImageListIndex = 1;
        _shipImagePath = _shipImagePathList[_shipImageListIndex];
    }

    public void IncrementIndex()
    {
        _shipImageListIndex++;
 /*       _shipImagePath = _shipImagePathList[_shipImageListIndex];
        OnPropertyChanged(nameof(_shipImagePath));*/
    }

    public void DecrementIndex()
    {
        _shipImageListIndex--;
/*        _shipImagePath = _shipImagePathList[_shipImageListIndex];
        OnPropertyChanged(nameof(_shipImagePath));*/
    }


    /*public void UpdateImage() 
    {
        OnPropertyChanged(nameof(ShipImagePath));
    }*/

}
