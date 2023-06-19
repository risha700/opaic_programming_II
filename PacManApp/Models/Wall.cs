using System;
namespace PacManApp.Models
{
	public class Wall:GameShape
	{
		public Wall(float w = 20, float h = 20, float x = 0, float y = 0, Color clr = null)
        {
            Dimension = new(w, h);
            Position = new(x, y); // set it by height of canvas
            FillColor = clr ?? Colors.Blue;
        }
	}
}

