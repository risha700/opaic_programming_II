

using Microsoft.Maui.Graphics;

namespace PacManApp.Models;

public class Pacman:GameShape
{
    public string Img { get; set; }
    //public PathF Mouth { get; set; }

	public Pacman(float w = 50, float h = 50, float x = 0, float y = 0, Color clr = null, String img=null)
    {

        Dimension = new(w, h);
        Position = new(x, y); // set it by height of canvas
        FillColor = clr ?? Colors.Yellow;
        Img = img ?? "pacman.png";
    }

    public void Render(ICanvas canvas, RectF dirtyRect)
    {
        canvas.StrokeColor = Colors.Violet;
        canvas.StrokeSize = 6;
        canvas.FillColor = FillColor;
        Element = new RectF(Position.X+200, Position.Y - 200, Dimension.Height, Dimension.Width);
        canvas.FillEllipse(Element);



        //PathF p = new PathF();
        //var r = Dimension.Width / 2;
        //canvas.FillColor = Colors.Orange;

        


        ////ctx.beginPath();
        //var x = Position.X;
        //var y = Position.Y;
        //var h = Dimension.Height;
        //var w = Dimension.Width;

        //canvas.BlendMode = BlendMode.Overlay;
        

        //canvas.FillArc(x, y, w,h, (float)(0.25 *Math.PI), (float)(1.25 *Math.PI), true); // fill circle

        //canvas.FillColor = Colors.Transparent;
        

        //// mouth
        //p.MoveTo(x + r, y + r);
        //p.LineTo(x + w-1, (float)(y +(r*0.7)));
        //p.LineTo(x + w-1, (float)(y +(r*1.5)));
        //p.Close();
        //canvas.FillPath(p);
        
        
        //// eye
        //canvas.FillColor = Colors.Black;
        //canvas.FillArc(x+r/2, y+r/3, 10, 10, 0, (float)(2 * Math.PI), true); // fill circle


    }
}

