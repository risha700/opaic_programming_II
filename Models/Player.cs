using System;
namespace BallBreaker.Models;

public class Player
{

	public readonly Guid Id  = new();
	public uint Score { get; set; } 
	  
	public Player()
	{
	}
}


