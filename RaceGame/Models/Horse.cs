using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RaceGame.Models;

public class Horse: ObservableObject
{
	public string Name { get; set; }
	public Coordinates Cords { get; set; }
	public Image Img { get; set; }

	private double speed;
	public double Speed { get=>speed; set=>SetProperty(ref speed, value); }

	public Horse()
	{

	}

    public override string ToString()
    {
        return Name;
    }
}


