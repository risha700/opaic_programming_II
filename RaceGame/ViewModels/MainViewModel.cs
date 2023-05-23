using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using RaceGame.Models;

namespace RaceGame.ViewModels;

public partial class MainViewModel:ObservableObject
{
    //[ObservableProperty]
    //public Coordinates coordinates;

    [ObservableProperty]
    public Board gameBoard;

    public ObservableCollection<Horse> Horses=new();

    [ObservableProperty]
    public Horse selectedHorse;


    public MainViewModel(Coordinates coordinates)
	{
        // create board
        
        GameBoard = new();
        //GameBoard.GameTimer

        // get width and height

        // assign endpoint todo: simply width of gameboard

        // create list of horses


        //LoadHorses();
        // add Horses to collection or list view


        // assign random speed to endpoint

        //Coordinates = coordinates;
        //Horses = new();
        LoadHorses();

    }

    private void LoadHorses()
    {
        List<string> HorsesNames = new List<string>() { "daffy", "pepe", "taz", "wiley" };

        HorsesNames.ForEach(horseName =>
        {
            Horses.Add(new Horse
            {
                Name = horseName,
                Img = new Image { Source = $"{horseName}.jpg" },

            });
        });
    }
}


