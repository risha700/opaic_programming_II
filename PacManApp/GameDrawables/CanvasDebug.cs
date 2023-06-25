using System;
using PacManApp.Models;

namespace PacManApp.GameDrawables;

[AttributeUsage(AttributeTargets.Method)]
public class CanvasDebug:Attribute
{
    ICanvas Canvas;
    Kibble Element;
    public CanvasDebug(ICanvas canvas, Kibble element)
    {
        Canvas = canvas;
        Element = element;
    }

    public void OnMethodExecution()
    {

        // Custom logic to be executed before or after the decorated method
        Canvas.FillColor = Colors.Aqua;
        Canvas.FillRectangle(Element.CollissionElement);
        var k = Element;
        // Invoke the original method
        // (To be implemented depending on your specific use case)
        // Option 1: Invoke the original method using reflection
        var methodInfo = typeof(CanvasDrawable).GetMethod("MyDecoratedMethod");
        methodInfo.Invoke(null, null);
        // Custom logic to be executed after the decorated method
        //k.Dimension.Width = WallBrickDimensions.X;
        //k.Dimension.Height = WallBrickDimensions.Y;
        Canvas.FontColor = Colors.Black;
        Canvas.DrawString($"({k.Position.X},{k.Position.Y})", k.CollissionElement.X, k.CollissionElement.Y, k.CollissionElement.Width, k.CollissionElement.Height, HorizontalAlignment.Left, VerticalAlignment.Top);
        Canvas.DrawString($"({k.MatrixPosition.X},{k.MatrixPosition.Y})", k.CollissionElement.X, k.CollissionElement.Y, k.CollissionElement.Width, k.CollissionElement.Height, HorizontalAlignment.Left, VerticalAlignment.Bottom);
    }
}

