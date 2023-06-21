

using Microsoft.Maui.Graphics;

namespace PacManApp.Models;

public class Pacman:GameShape
{
    private double x;
    private float y;

    public string Img { get; set; }
    //public PathF Mouth { get; set; }

	public Pacman(float w = 25, float h = 25, float x = 0, float y = 0, Color clr = null, String img=null)
    {

        Dimension = new(w, h);
        Position = new(x, y); // set it by height of canvas
        FillColor = clr ?? Colors.Blue;
        Img = img ?? "pacman.png";
    }



    public void Render(ICanvas canvas, RectF dirtyRect)
    {
        double radius = Dimension.Width / 2;
        

        canvas.StrokeColor = Colors.GreenYellow;
        canvas.StrokeSize = 6;
        canvas.FillColor = FillColor;
        Element = new RectF(Position.X, Position.Y, Dimension.Height, Dimension.Width);
        //canvas.FillEllipse(Element);

        // open mouth right
        canvas.FillArc(Element, 45, 225, false);
        canvas.FillArc(Element, 135, 315, false);

        //full circle
        //canvas.FillArc(Element, 0, 359, false);

        // open mouth left
        //canvas.FillArc(Element, 225, 45, false);
        //canvas.FillArc(Element, 315, 135, false);

        // mouth down open
        //canvas.FillArc(Element, 325, 135, false);
        //canvas.FillArc(Element, 45, 225, false);

        // mouth up open
        //canvas.FillArc(Element, 135, 325, false);
        //canvas.FillArc(Element, 225, 45, false);




    }
}

