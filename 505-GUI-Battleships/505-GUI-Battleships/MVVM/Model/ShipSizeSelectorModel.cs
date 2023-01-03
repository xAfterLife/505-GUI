using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.View.UserControls;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class ShipSizeSelectorModel : ObservableObject
{
    private readonly string[] _shipImagePathList;
    private int _shipImageListIndex;
    private string _shipImagePath;

    public Visibility EnlargeShipSizeVisibility { get; set; }
    public Visibility ReduceShipSizeVisibility { get; set; }
    public ShipSizeSelectorModel Instance { get; }

    public static ICommand? EnlargeShipSizeCommand =>
        new RelayCommand(instance =>
        {
            if ( instance is not ShipSizeSelectorModel shipSizeSelector )
                return;

            shipSizeSelector._shipImageListIndex++;
            shipSizeSelector.UpdateImagePath();
        });

    public static ICommand? ReduceShipSizeCommand =>
        new RelayCommand(instance =>
        {
            if ( instance is not ShipSizeSelectorModel shipSizeSelector )
                return;

            shipSizeSelector._shipImageListIndex--;
            shipSizeSelector.UpdateImagePath();
        });

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
        _shipImagePathList = new string[5] { "../../../Ressources/Ships/1ShipPatrolHorizontal.png", "../../../Ressources/Ships/2ShipRescueHorizontal.png", "../../../Ressources/Ships/3ShipSupHorizontal.png", "../../../Ressources/Ships/4ShipDestroyerHorizontal.png", "../../../Ressources/Ships/5ShipBattleshipHorizontal.png" };
        Instance = this;
        _shipImageListIndex = 1;
        _shipImagePath = _shipImagePathList[_shipImageListIndex];
    }

    void UpdateImagePath() 
    {
        _shipImagePath = _shipImagePathList[_shipImageListIndex];

        if (_shipImageListIndex == 4) EnlargeShipSizeVisibility = Visibility.Hidden;
        else EnlargeShipSizeVisibility = Visibility.Visible;

        if (_shipImageListIndex == 0) ReduceShipSizeVisibility = Visibility.Hidden;
        else ReduceShipSizeVisibility = Visibility.Visible;

        OnPropertyChanged(nameof(ShipImagePath));
        OnPropertyChanged(nameof(EnlargeShipSizeVisibility));
        OnPropertyChanged(nameof(ReduceShipSizeVisibility));
    }
}
