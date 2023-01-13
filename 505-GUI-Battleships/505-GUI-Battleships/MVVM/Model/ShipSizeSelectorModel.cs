using System.Windows;
using System.Windows.Input;
using _505_GUI_Battleships.Core;

namespace _505_GUI_Battleships.MVVM.Model;

public sealed class ShipSizeSelectorModel : ObservableObject
{
    private readonly string[] _shipImagePathList = { "../../../Resources/Ships/1ShipPatrolHorizontal.png", "../../../Resources/Ships/2ShipRescueHorizontal.png", "../../../Resources/Ships/3ShipSubMarineHorizontal.png", "../../../Resources/Ships/4ShipDestroyerHorizontal.png", "../../../Resources/Ships/5ShipBattleshipHorizontal.png" };
    private Visibility _enlargeShipSizeVisibility;
    private Visibility _reduceShipSizeVisibility;
    private int _shipBoardSpaceSize;
    private int _shipImageListIndex;
    private string _shipImagePath;

    private string _shipImageSize = "60";

    public string ShipImageSize
    {
        get => _shipImageSize;
        set => Update(ref _shipImageSize, value);
    }

    public int ShipBoardSpaceSize
    {
        get => _shipBoardSpaceSize;
        set => Update(ref _shipBoardSpaceSize, value);
    }

    public string[] ShipSizeValues { get; set; } = { "60", "85", "110", "135", "160" };

    public Visibility EnlargeShipSizeVisibility
    {
        get => _enlargeShipSizeVisibility;
        set => Update(ref _enlargeShipSizeVisibility, value);
    }

    public Visibility ReduceShipSizeVisibility
    {
        get => _reduceShipSizeVisibility;
        set => Update(ref _reduceShipSizeVisibility, value);
    }

    public ShipSizeSelectorModel Instance { get; }

    public static ICommand? EnlargeShipSizeCommand => new RelayCommand(instance =>
    {
        if ( instance is not ShipSizeSelectorModel shipSizeSelector )
            return;

        shipSizeSelector._shipImageListIndex++;
        shipSizeSelector.UpdateImagePath();
    });

    public static ICommand? ReduceShipSizeCommand => new RelayCommand(instance =>
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
        Instance = this;
        _shipImageListIndex = 0;
        ShipBoardSpaceSize = _shipImageListIndex + 1;
        ShipImageSize = ShipSizeValues[_shipImageListIndex];
        _shipImagePath = _shipImagePathList[_shipImageListIndex];
        ReduceShipSizeVisibility = Visibility.Hidden;
    }

    private void UpdateImagePath()
    {
        _shipImagePath = _shipImagePathList[_shipImageListIndex];
        ShipImageSize = ShipSizeValues[_shipImageListIndex];
        ShipBoardSpaceSize = _shipImageListIndex + 1;

        if ( _shipImageListIndex == 4 )
            EnlargeShipSizeVisibility = Visibility.Hidden;
        else
            EnlargeShipSizeVisibility = Visibility.Visible;

        if ( _shipImageListIndex == 0 )
            ReduceShipSizeVisibility = Visibility.Hidden;
        else
            ReduceShipSizeVisibility = Visibility.Visible;
        OnPropertyChanged(nameof(ShipImagePath));
    }
}
