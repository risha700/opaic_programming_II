using CommunityToolkit.Mvvm.ComponentModel;
using PacManApp.Models;

namespace PacManApp.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    


    [ObservableProperty]
    public Game activeGame;

    public GameViewModel(GameAudioViewModel audioModel)
    {
        ActiveGame = new Game(audioModel);


    }





}
