using System;
using CommunityToolkit.Mvvm.ComponentModel;
using SlotGame.Models;

namespace SlotGame.ViewModels;


public partial class SpinnerViewModel:ObservableObject
{
    [ObservableProperty]
    public bool isButtonEnabled;
    

    [ObservableProperty]
    public static Spinner game;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Game))]
    public double balance;


    public bool canSpin => Game.Balance >= Game.Bet;

    [ObservableProperty]
    public string resultMessage;


    public SpinnerViewModel()
	{
        Game = new();
        IsButtonEnabled = true;
        
        Balance = Game.Balance;
    }

    private void RestartGame() => Game = new();

    public async Task Spin()
    {
        ResultMessage = "";
        IsButtonEnabled = false;

        if (canSpin) {
            Balance -= Game.Bet;
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                List<Image> temp = BuildImgArr();

                await DoSpin(temp);

                Game.CheckWinner();
                ResultMessage = Game.IsWinner ? "You Won" : "You lost";

                if (Game.IsWinner)
                {
                    Balance += 50;
                    await Shell.Current.DisplayAlert("WINNER", "YOU GOT $50", "OK");
                }

            }); // end thread async
        }
        else
        {
            await Shell.Current.DisplayAlert("Hmmmmm", "You are broke dude!", "Restart Game");
        }
        
        IsButtonEnabled = true;
    }

    private async Task DoSpin(List<Image> temp)
    {
        // assign images randomly

        for (int i = 0; i < 57; i += 3)
        {
            Game.SoltImages[0] = temp[i];
            Game.SoltImages[1] = temp[i + 1];
            Game.SoltImages[2] = temp[i + 2];
            await Task.Delay(100);
        }
    }

    private List<Image> BuildImgArr()
    {
        // create sudo list if images
        List<Image> temp = new();

        for (int i = 0; i < 60; i++)
        {
            Random random = new Random();
            temp.Add(Game.OriginalImages[random.Next(0, (int)Game.SoltImages.Count)]);
        }

        return temp;
    }

  

}


