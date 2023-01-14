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

    /// <summary>
    ///     Size of the Image
    /// </summary>
    public string ShipImageSize
    {
        get => _shipImageSize;
        set => Update(ref _shipImageSize, value);
    }

    /// <summary>
    ///     The Currently selected Images size
    /// </summary>
    public int ShipBoardSpaceSize
    {
        get => _shipBoardSpaceSize;
        set => Update(ref _shipBoardSpaceSize, value);
    }

    /// <summary>
    ///     The different Sizes of the Images
    /// </summary>
    public string[] ShipSizeValues { get; set; } = { "60", "85", "110", "135", "160" };

    /// <summary>
    ///     EnlargeShipSizeCommand Visibility
    /// </summary>
    public Visibility EnlargeShipSizeVisibility
    {
        get => _enlargeShipSizeVisibility;
        set => Update(ref _enlargeShipSizeVisibility, value);
    }

    /// <summary>
    ///     ReduceShipSizeCommand Visibility
    /// </summary>
    public Visibility ReduceShipSizeVisibility
    {
        get => _reduceShipSizeVisibility;
        set => Update(ref _reduceShipSizeVisibility, value);
    }

    /// <summary>
    ///     Instance of the Object
    /// </summary>
    public ShipSizeSelectorModel Instance { get; }

    /// <summary>
    ///     The Command to increase the current Ships size
    /// </summary>
    public static ICommand? EnlargeShipSizeCommand => new RelayCommand(instance =>
    {
        if ( instance is not ShipSizeSelectorModel shipSizeSelector )
            return;

        shipSizeSelector._shipImageListIndex++;
        shipSizeSelector.UpdateImagePath();
    });

    /// <summary>
    ///     The Command to decrease the current Ships size
    /// </summary>
    public static ICommand? ReduceShipSizeCommand => new RelayCommand(instance =>
    {
        if ( instance is not ShipSizeSelectorModel shipSizeSelector )
            return;

        shipSizeSelector._shipImageListIndex--;
        shipSizeSelector.UpdateImagePath();
    });

    /// <summary>
    ///     The Image of the Ship
    /// </summary>
    public string ShipImagePath
    {
        get => _shipImagePath;
        set => Update(ref _shipImagePath, value);
    }

    /// <summary>
    ///     The List Index of the Ship Image
    /// </summary>
    public int ShipImageListIndex
    {
        get => _shipImageListIndex;
        set => Update(ref _shipImageListIndex, value);
    }

    /// <summary>
    ///     Constructor of ShipSizeSelectorModel
    /// </summary>
    public ShipSizeSelectorModel()
    {
        Instance = this;
        _shipImageListIndex = 0;
        ShipBoardSpaceSize = _shipImageListIndex + 1;
        ShipImageSize = ShipSizeValues[_shipImageListIndex];
        ShipImagePath = _shipImagePathList[_shipImageListIndex];
        ReduceShipSizeVisibility = Visibility.Hidden;
    }

    /// <summary>
    ///     Updates the current Image & Size
    /// </summary>
    private void UpdateImagePath()
    {
        ShipImagePath = _shipImagePathList[_shipImageListIndex];
        ShipImageSize = ShipSizeValues[_shipImageListIndex];
        ShipBoardSpaceSize = _shipImageListIndex + 1;

        //Ship is at max size
        if ( _shipImageListIndex == 4 )
            EnlargeShipSizeVisibility = Visibility.Hidden;
        else
            EnlargeShipSizeVisibility = Visibility.Visible;

        //Ship is at min size
        if ( _shipImageListIndex == 0 )
            ReduceShipSizeVisibility = Visibility.Hidden;
        else
            ReduceShipSizeVisibility = Visibility.Visible;
    }
}
