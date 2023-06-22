using System;
namespace PacManApp.Models;

public class Ghost:GameShape
{

	public Ghost(float w = 20, float h = 20, float x = 0, float y = 0, Color clr = null)
    {
        Dimension = new(w, h);
        Position = new(x, y); // set it by height of canvas
        FillColor = clr ?? Colors.Red;
        
    }

    public void Render(ICanvas canvas, RectF dirtyRect)
    {
        canvas.FillColor = FillColor;
        Element = new RectF(Position.X, Position.Y, Dimension.Height, Dimension.Height);
        //canvas.FillRectangle(Element);

        PathF path = new ();
        //path.AppendRectangle(Element);
        var r = Dimension.Height / 2;
        var x = Element.X;
        var y = Element.Y;


        // skeleton
        path.MoveTo(x, y);
        path.LineTo(x, y-14);
        path.CurveTo(x, y-22, x+6, y-28, x+14, y-28);
        path.CurveTo(x+22, y-28, x+28, y-22, x+28, y-14);
        path.LineTo(x+28, y);
        path.LineTo(x+23.333f, y-5.777f);
        path.LineTo(x+18.666f, y);
        path.LineTo(x+14, y - 5.777f);
        path.LineTo(x+9.333f, y);
        path.LineTo(x+4.666f, y - 5.777f);
        path.LineTo(x, y);

        canvas.StrokeColor = Colors.DeepSkyBlue;
        canvas.DrawPath(path);
        canvas.FillPath(path);
        path.Close();

        path = new();
        //// EYE balls

        canvas.FillColor = Colors.White;
        path.MoveTo(x + 8, y - 20);
        path.CurveTo(x + 5, y - 20, x + 4, y - 17, x + 4, y - 15); 
        path.CurveTo(x + 4, y - 13, x + 5, y - 10, x + 8, y - 10); 

        path.CurveTo(x + 11, y - 10, x + 12, y - 13, x + 12, y - 15);

        path.CurveTo(x + 12, y - 17, x + 11, y - 20, x + 8, y - 20);

        path.MoveTo(x + 20, y - 20);
        path.CurveTo(x + 17, y - 20, x + 16, y - 17, x + 16, y - 15);
        path.CurveTo(x + 16, y - 13, x + 17, y - 10, x + 20, y - 10);
        path.CurveTo(x + 23, y - 10, x + 24, y - 13, x + 24, y - 15);
        path.CurveTo(x + 24, y - 17, x + 23, y - 20, x + 20, y - 20);
        path.Close();
        canvas.DrawPath(path);
        canvas.FillPath(path);


        //eyes
        canvas.FillColor = Colors.Black;
        canvas.FillEllipse(x+20, y-14, 2, 2);
        canvas.FillEllipse(x +6, y - 14, 2, 2);

    }
}

