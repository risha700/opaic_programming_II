using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RaceGame.Models
{
	public class Board:ObservableObject
	{
		public double Width { get; set; }
		public double Height { get; set; }

		private Timer gametimer;

		public Timer GameTimer {
			get => gametimer;
			set => SetProperty(ref gametimer, value);
		}

		public Board(double? width=null, double? height = null)
		{
            // set board bounds
            //Width = width?? DeviceDisplay.Current.MainDisplayInfo.Width;
            //Height = height?? DeviceDisplay.Current.MainDisplayInfo.Height;
            Width = width?? Shell.Current.Window.Width;
			Height = height ?? Shell.Current.Window.Height;

		}

        ~Board()
		{
			gametimer.Dispose();
		}
    }
}

