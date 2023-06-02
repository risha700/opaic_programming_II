using BallBreaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace BallBreaker.ViewModels;

public partial class GameViewModel : BaseViewModel
{
	


	[ObservableProperty]
	public Game activeGame;

	public GameViewModel()
	{
		ActiveGame = new Game();


    }

    
    

	
}


