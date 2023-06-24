using System;
using Microsoft.Maui.Graphics;

namespace PacManApp.Models;

public class Wall:GameShape
{
    public PatternPaint WallPattern;
    

	public Wall(float w = 40, float h = 40, float x = 0, float y = 0, Color clr = null, Point matrixPos=new())
    {
        Dimension = new(w, h);
        Position = new(x, y); // set it by height of canvas
        FillColor = clr ?? Colors.DarkKhaki;
        WallPattern = WallPaintPattern();
        MatrixPosition = matrixPos;
    }

internal PatternPaint WallPaintPattern()
{
    IPattern pattern;

    // Create a 10x10 template for the pattern
    using (PictureCanvas picture = new PictureCanvas(0, 0, 10, 10))
    {
        picture.StrokeColor = FillColor;
        picture.DrawLine(0, 0, 10, 10);
        picture.DrawLine(0, 10, 10, 0);
        pattern = new PicturePattern(picture.Picture, 10, 10);
    }
    // Fill the rectangle with the 10x10 pattern
    PatternPaint patternPaint = new PatternPaint
    {
        Pattern = pattern
    };

    return patternPaint;
}

}


