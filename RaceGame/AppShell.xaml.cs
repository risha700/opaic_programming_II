﻿namespace RaceGame;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Shell.SetNavBarIsVisible(this, false);
	}
}

