using System;
using System.Collections.ObjectModel;

namespace RaceGame.Models;

public class Brick
{
	public Coordinates Cords { get; set; }
	public Color TileColor { get; set; }

	public Brick()
	{
	}
}


public enum HitStatus
{
	Hit,

}