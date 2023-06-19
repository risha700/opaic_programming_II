using System;
namespace PacManApp.Models;

public class Ghost:GameShape
{
	public string Img;

	public Ghost(float w = 50, float h = 50, float x = 0, float y = 0, Color clr = null, String img = null)
    {
        Dimension = new(w, h);
        Position = new(x, y); // set it by height of canvas
        FillColor = clr ?? Colors.Blue;
        Img = img ?? "ghost.png";
    }

    public void Render(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = FillColor;
        //float X = dirtyRect.Width / 2;
        //float Y = dirtyRect.Height - Dimension.Height * 2;
        Element = new RectF(Position.X, Position.Y, Dimension.Width, Dimension.Height);
        canvas.FillRectangle(Element);

    }
}

