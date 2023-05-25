using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RaceGame.Models;

public class Horse: ObservableObject
{
	public string Name { get; set; }
	public Coordinates Cords { get; set; }
	public Image Img { get; set; }

    private TimeSpan remainingTime;
    public TimeSpan RemainingTime { get => remainingTime; set => SetProperty(ref remainingTime, value); }

    private double speed=0;
	public double Speed { get=>speed; set=>SetProperty(ref speed, value); }


    public Horse()
	{

	}

    public override string ToString()
    {
        return $"{this.Name.ToString()}\t{this.Speed.ToString()}";
    }
}


