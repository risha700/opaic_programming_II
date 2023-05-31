using System;
using CommunityToolkit.Mvvm.ComponentModel;
namespace BallBreaker.Models;

public class GameShape
{

    public SizeF Dimension; // height and width
    public PointF Position; // x,y
    public Color FillColor;

    
    public RectF Element;

    public void Move(float XPoint)
    {

        this.Element.X = XPoint;
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