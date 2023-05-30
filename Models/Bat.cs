using System;
using Microsoft.Maui.Graphics.Platform;

namespace BallBreaker.Models;

public class Bat:GameShape
{

    public Bat(float width = 100, float height = 50, float x = 0, float y = 0, Color color = null)
    {
        Dimension = new(width, height);
        Position = new(x, y); // set it by height of canvas
        FillColor = color ?? Colors.BlueViolet;
    }

    public void Draw(PlatformCanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = FillColor;
        float X = dirtyRect.Width / 2;
        float Y = dirtyRect.Height - Dimension.Height * 2;
        // bat
        Element = new RectF(X, Y, Dimension.Width, Dimension.Height);
        canvas.FillRectangle(Element);
    }
}



