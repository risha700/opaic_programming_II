using System;
namespace BallBreaker.Models;

public class Ball:GameShape
{
    
    public float Radius { get; set; } = 50;
    public float Speed { get; set; }
    public BallDirection Direction;

    public Ball(float width = 50, float height = 50, float x = 0, float y = 0, Color color = null)
    {
        Dimension = new(width, height);
        Position = new(x, y); // set it by height of canvas
        FillColor = color ?? Colors.YellowGreen;
        Direction = BallDirection.DownRight;
    }

    public void Render(ICanvas canvas, RectF dirtyRect, int CornerRadius=50)
    {
        canvas.StrokeColor = Colors.Violet;
        canvas.StrokeSize = 6;
        canvas.FillColor = FillColor;
        Element = new RectF(Position.X, Position.Y - 200, Dimension.Height, Dimension.Width);// todo: set ball cords dynamic
        canvas.FillRoundedRectangle(Element, CornerRadius);
    }

    public void SwitchDirection(BallDirection direction)
    {
        this.Direction = direction;
    }
}


public enum BallDirection
{
    UpRight,
    DownRight,
    UpLeft,
    DownLeft
}
