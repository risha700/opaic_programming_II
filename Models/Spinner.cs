using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SlotGame.Models;

public class Spinner
{

    public ObservableCollection<Image> SoltImages { get; set; }
    public ObservableCollection<Image> OriginalImages { get; private set; }
    public uint Balance { get; set; }
    public uint Bet { get; set; }
    public double Score { get; set; }
    public bool IsWinner { get; set; }


    public Spinner()
    {

        LoadResources();
        Bet = 10; //original bet
        Balance = 200; // opening balance
        IsWinner = false;

    }

    public void LoadResources()
    {
        // load resources
        Image sheepImg = new Image { Source = "sheep.bmp" };
        Image coyoteImg = new Image { Source = "coyote.bmp" };
        Image treeImg = new Image { Source = "tree.bmp" };

        SoltImages = new (){ sheepImg, coyoteImg, treeImg };
        OriginalImages = new() { sheepImg, coyoteImg, treeImg };
    }

    public void CheckWinner()
    {
        //var fileName = ((FileImageSource)img.Source).File;
        IsWinner = SoltImages.All(n => n.Equals(SoltImages.First()));
    }

    public void PayOut()
    {

        Balance += 50;
    }

    public void CollectBet()
    {
        Balance -= Bet;
    }

}


