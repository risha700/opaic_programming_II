using System;
namespace BallBreaker.Models;

public class GameShape
{

    public SizeF Dimension { get; set; } // height and width
    public PointF Position { get; set; } // x,y
    public Color FillColor { get; set; }
    public RectF Element { get; set; }

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