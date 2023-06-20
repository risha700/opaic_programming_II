using System;
namespace PacManApp.Models;

public class Kibble:GameShape
{
    private double x;
    private double y;

    public Kibble(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public Kibble(float w = 10, float h = 10, float x = 0, float y = 0, Color clr = null)
    {
        Dimension = new(w, h);
        Position = new(x, y);
        FillColor = clr ?? Colors.Orange;
        
    }
}

