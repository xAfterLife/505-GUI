﻿using _505_GUI_Battleships.Core;
using _505_GUI_Battleships.MVVM.Model;
using _505_GUI_Battleships.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _505_GUI_Battleships.MVVM.ViewModel;

internal sealed class BoardAttackViewModel: ObservableObject
{
    private PlayerModel _targetedPlayerCard;
    private PlayerModel _attackerPlayerCard;
    private Canvas _playerBoard;
    private Grid _boardContainer;
    private readonly (int Width, int Height) _boardDimensions;
    private readonly GameDataService _gameService;

    public Grid BoardContainer
    {
        get => _boardContainer;
        set => Update(ref _boardContainer, value);
    }
    public Canvas PlayerBoard
    {
        get => _playerBoard;
        set => Update(ref _playerBoard, value);
    }

    public PlayerModel TargetedPlayerCard
    {
        get => _targetedPlayerCard;
        set => Update(ref _targetedPlayerCard, value);
    }

    public PlayerModel AttackerPlayerCard
    {
        get => _attackerPlayerCard;
        set => Update(ref _attackerPlayerCard, value);
    }

    public void SetupBoardAttack (PlayerModel targetedPlayerCard, PlayerModel attackerPlayerCard, Canvas playerBoard)
    {
        TargetedPlayerCard = targetedPlayerCard;
        AttackerPlayerCard = attackerPlayerCard;
        PlayerBoard = playerBoard;
    }
    public BoardAttackViewModel()
    {
        _gameService = GameDataService.GetInstance();
        _boardDimensions = (_gameService.GameBoard!.Width, _gameService.GameBoard!.Height);
        SetupBoardContainer();
    }

    private void SetupBoardContainer()
    {
        BoardContainer = new Grid();
        BoardContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        BoardContainer.RowDefinitions.Add(new RowDefinition { Height = new GridLength(_boardDimensions.Height, GridUnitType.Star) });
        BoardContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        BoardContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_boardDimensions.Width, GridUnitType.Star) });

        Canvas xAxis = new() { Width = _boardDimensions.Width, Height = 1, LayoutTransform = new ScaleTransform(1, -1) };
        Canvas yAxis = new() { Width = 1, Height = _boardDimensions.Height, LayoutTransform = new ScaleTransform(1, -1) };

        for (var i = 0; i < _boardDimensions.Width; i++)
        {
            var textBlock = new TextBlock
            {
                Text = ((char)('A' + i)).ToString(),
                FontSize = 0.5,
                Foreground = new SolidColorBrush(Colors.LightGreen),
                LayoutTransform = new ScaleTransform(1, -1),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 1,
                Height = 1
            };

            xAxis.Children.Add(textBlock);
            Canvas.SetLeft(xAxis.Children[i], i);
        }

        Grid.SetRow(xAxis, 0);
        Grid.SetColumn(xAxis, 1);

        for (var i = 0; i < _boardDimensions.Height; i++)
        {
            var textBlock = new TextBlock
            {
                Text = (i + 1).ToString(),
                FontSize = 0.5,
                Foreground = new SolidColorBrush(Colors.White),
                LayoutTransform = new ScaleTransform(1, -1),
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = 1,
                Height = 1
            };
            yAxis.Children.Add(textBlock);
            Canvas.SetTop(yAxis.Children[i], i - 0.2);
        }

        Grid.SetRow(yAxis, 1);
        Grid.SetColumn(yAxis, 0);

        BoardContainer.Children.Add(xAxis);
        BoardContainer.Children.Add(yAxis);

        var playerBoard = _gameService.GameBoard!.Board;
        Grid.SetRow(playerBoard, 1);
        Grid.SetColumn(playerBoard, 1);

        BoardContainer.Children.Add(playerBoard);
    }
}
