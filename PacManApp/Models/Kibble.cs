using System;
namespace PacManApp.Models;

public class Kibble:GameShape
{
    public RectF CollissionElement;

    public Kibble(float w = 10, float h = 10, float x = 0, float y = 0, Color clr = null, Point matrixPos = new())
    {
        Dimension = new(w, h);
        Position = new(x, y);
        FillColor = clr ?? Colors.Orange;
        MatrixPosition = matrixPos;


    }
}

