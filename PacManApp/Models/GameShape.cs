using System;

namespace PacManApp.Models;

public class GameShape
{

    public SizeF Dimension; // height and width
    public PointF Position; // x,y
    public Direction Direction;
    public Color FillColor = Colors.SlateBlue;


    public RectF Element;

    public void MoveTo(float XPoint, float YPoint)
    {
        this.Element.Y = YPoint;
        this.Element.X = XPoint;
    }

    public void SwitchDirection(Direction direction)
    {
        this.Direction = direction;
    }
}


public struct SizeF
{
    public float Height;
    public float Width;

    public SizeF(float width, float height)
    {
        Width = width;
        Height = height;
    }


}

public enum Direction
{
    Left,
    Up,
    Right,
    Down
}