using System;
namespace BallBreaker.Models;

public class Ball:GameShape
{
    
    public float Radius { get; set; } = 50;
    public float Speed { get; set; }
    public float Direction { get; set; }

    public Ball(float width = 50, float height = 50, float x = 0, float y = 0, Color color = null)
    {
        Dimension = new(width, height);
        Position = new(x, y); // set it by height of canvas
        FillColor = color ?? Colors.YellowGreen;
    }

}


