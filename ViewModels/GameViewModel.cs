using BallBreaker.Models;
using CommunityToolkit.Mvvm.ComponentModel;

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


