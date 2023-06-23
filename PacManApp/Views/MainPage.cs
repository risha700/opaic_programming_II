﻿using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using SkiaSharp.Views.Maui.Controls;
using Microsoft.Maui.Controls;

using System;
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace PacManApp.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        Content = new VerticalStackLayout
        {
            //HeightRequest = Shell.Current.Window.Height,
            
            VerticalOptions = LayoutOptions.Center,
            Children = {
                new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "Welcome to PacMan Game"},
                new Button{Text="Play Now", Command=PlayNowCommand, WidthRequest=300, HeightRequest=300}
            }
        };
    }

    [RelayCommand]
    private async Task PlayNow()
    {
        await Shell.Current.GoToAsync(nameof(GamePage), true);
    }


}