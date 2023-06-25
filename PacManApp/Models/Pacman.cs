

using Microsoft.Maui.Graphics;

namespace PacManApp.Models;

public class Pacman:GameShape
{
    public bool IsEating = false;
    public RectF CollissionElement;
    public int Speed;

    //public PathF Mouth { get; set; }

    public Pacman(float w = 30, float h = 30, float x = 0, float y = 0, Color clr = null)
    {

        Dimension = new(w, h);
        Position = new(x, y); // set it by height of canvas
        FillColor = clr ?? Colors.Gold;
        Direction = Direction.Right;
        Speed = 10;
        
    }



    public void Render(ICanvas canvas, RectF dirtyRect, PointF WallBrickDimensions)
    {
        double radius = Dimension.Height / 2;
        

        canvas.StrokeColor = Colors.GreenYellow;
        canvas.StrokeSize = 6;
        canvas.FillColor = FillColor;
        Element = new RectF(Position.X, Position.Y, Dimension.Width, Dimension.Height);
        CollissionElement = new RectF(Position.X, Position.Y, WallBrickDimensions.X, WallBrickDimensions.Y);


        //canvas.StrokeColor = Colors.Red;
        //canvas.DrawRectangle(CollissionElement);

        switch (this.Direction)
        {
            case Direction.Right:
                // open mouth right
                canvas.FillArc(Element, 45, 225, false);
                canvas.FillArc(Element, 135, 315, false);

                break;
            case Direction.Left:
                //open mouth left
                canvas.FillArc(Element, 225, 45, false);
                canvas.FillArc(Element, 315, 135, false);
                break;
            case Direction.Down:
                // mouth down open
                canvas.FillArc(Element, 325, 135, false);
                canvas.FillArc(Element, 45, 225, false);
                break;
            case Direction.Up:
                // mouth up open
                canvas.FillArc(Element, 135, 325, false);
                canvas.FillArc(Element, 225, 45, false);
                break;
        }

        //full circle
        if(IsEating)
        canvas.FillArc(Element, 0, 359, false);









    }
}

