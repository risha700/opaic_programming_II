using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RaceGame.Models;

public class Coordinates:ObservableObject
{

	private int positionX=0;
    private int positionY=0;

    public int PositionX {
		get => positionX;
		set => SetProperty(ref positionX, value);
	}

    public int PositionY
    {
        get => positionY;
        set => SetProperty(ref positionY, value);
    }


    public Coordinates()
	{
		
	}

	public void setPosition(int posX, int posY)
	{
		PositionX = posX;
		PositionY = posY;
	}


}


