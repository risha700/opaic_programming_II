using System;
namespace BallBreaker.Models;

public class Brick:GameShape
{

    public Brick(float width = 100, float height = 50, float x = 0, float y = 0, Color color = null)
	{
		Dimension = new (width,height);
		Position = new (x, y); // set it by height of canvas
		FillColor = color?? Colors.BlueViolet;
	}
}

