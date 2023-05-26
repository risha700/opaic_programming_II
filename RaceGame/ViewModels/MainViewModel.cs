using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using RaceGame.Models;

namespace RaceGame.ViewModels;

public partial class MainViewModel:BaseViewModel
{


    [ObservableProperty]
    public Board gameBoard;


    public ObservableCollection<Horse> Horses=new();


    [ObservableProperty]
    public Horse selectedHorse;


    public MainViewModel(Coordinates coordinates)
	{
        // create board
        GameBoard = new();
        LoadHorses();
        
    }

    private void LoadHorses()
    {

        //Horses.Clear();
        
        List<string> HorsesNames = new List<string>() { "daffy", "pepe", "taz", "wiley" };

        HorsesNames.ForEach(horseName =>
        {
            var horse = new Horse
            {
                Name = horseName,
                Img = new Image { Source = $"{horseName}.jpg", Aspect = Aspect.AspectFill, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, WidthRequest = 50, HeightRequest = 50 },
                Speed = 0

            };

            
            Horses.Add(horse);
            
        });
    }

    public async Task animateSeconds(Horse horse)
    {

        await Task.CompletedTask;
        horse.RemainingTime = TimeSpan.FromMilliseconds(0);
        while (horse.RemainingTime.TotalMilliseconds < horse.Speed)
        {
            horse.RemainingTime += TimeSpan.FromMilliseconds(100);
            await Task.Delay(100);
        }

    }

    public void RestartGame()
    {

        Horses.Clear();
        LoadHorses();
        SelectedHorse = null;
        

    }
}


