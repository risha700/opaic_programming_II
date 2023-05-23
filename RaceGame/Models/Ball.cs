using System;
namespace RaceGame.Models;

public class Ball
{
	public Coordinates Cords { get; set; }
	public Coordinates TargetPosition { get; set; }
	public double Direction { get; set; }
    public double Speed { get; set; }


    public Ball()
	{
	}
}


