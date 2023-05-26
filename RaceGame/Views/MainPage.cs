using System.Linq;
using System.Threading;
using CommunityToolkit.Maui.Animations;
using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using RaceGame.Models;
using RaceGame.ViewModels;

namespace RaceGame.Views;

public class MainPage : ContentPage
{

    

    // page elements
    
    Button raceBtn = new Button { Text = "Start Race", HeightRequest = 80, WidthRequest = 200 };

    
    AbsoluteLayout endLine = new AbsoluteLayout {
        Children =
        {
            new Image { Source = "race.jpeg", Aspect = Aspect.AspectFill }
        },
        Margin = new Thickness(-85, 0, 0, 0), // todo: relative value
    };


    VerticalStackLayout resultLayout = new VerticalStackLayout { };
    Frame resultBox = new Frame { VerticalOptions = LayoutOptions.Fill, HorizontalOptions = LayoutOptions.End, MaximumWidthRequest=250};

    StackLayout raceWrapper = new StackLayout
    {
        VerticalOptions = LayoutOptions.End,
        HorizontalOptions = LayoutOptions.Fill,
        Orientation = StackOrientation.Vertical,
        Margin = new(0,50,0,0),

    };

    // contructor

    public MainPage(MainViewModel vm)
    {
        BindingContext = vm;
        // results
        var resultList = new ListView { ItemsSource = vm.Horses , ItemTemplate = new DataTemplate(() =>
        {
            var resultText = new Label { FontSize = 20, };
            resultText.SetBinding(Label.TextProperty, new Binding("Name"));
            var animatedResultSpeed = new Label { FontSize = 20, };
            animatedResultSpeed.SetBinding(Label.TextProperty, new Binding("RemainingTime", stringFormat: "{0:ss\\:ff}"));

            var layout =  new FlexLayout
            {
                Direction = Microsoft.Maui.Layouts.FlexDirection.Row,
                JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceBetween,
                Children = { resultText, animatedResultSpeed }
            };
            return new ViewCell { View = layout };
        })};

   

        resultBox.Content = resultList;



        var horseView = new CollectionView
        {
            ItemsSource = vm.Horses,
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                ItemSpacing = 50,
                SnapPointsAlignment = SnapPointsAlignment.Center,
                 
            },
            
            ItemTemplate = new DataTemplate(() =>
            {
                RadioButton horseRadioBtn = new RadioButton { GroupName = "horseGroup" };
                horseRadioBtn.SetBinding(RadioButton.ContentProperty,new Binding("Img"));
                
                horseRadioBtn.CheckedChanged += (sender, args) => {
                    if (((RadioButton)sender).IsChecked)
                    {
                        RadioButton r = (RadioButton)sender;
                        vm.SelectedHorse = (Horse)r.BindingContext;
                    }
                    else
                    {
                        horseRadioBtn.SetValue(RadioButton.IsCheckedProperty, false);
                    }
                  
                  
                };
                horseRadioBtn.WidthRequest = 120;
                return new HorizontalStackLayout { Children = { horseRadioBtn } };
            }),



        };
  
        raceBtn.Clicked +=   async (sender, args) =>
        {

            vm.IsBusy = true;
            // apply some random delay
            if(vm.SelectedHorse is null)
            {
                await DisplayAlert("Check", "Choose a horse to race", "Ok");
                vm.IsBusy = false;
                return;
            }
            foreach (Horse h in vm.Horses)
            {
                var random = new Random();
                int delay = random.Next(4000, 8000); // random magic numbers
                h.Speed = (double)delay;
                MainThread.BeginInvokeOnMainThread(async () => await vm.animateSeconds(h));
            }

            await ApplyAnimation(horseView);
            await Task.Delay((int)vm.Horses.OrderBy((elm) => elm.Speed).Last().Speed); // cheap hack todo: create a custom Task scheduler needed
            await AnnounceWinner(vm, horseView);

            vm.IsBusy = false;
        };

        Grid gridView = new Grid
        {
            RowDefinitions = {
                new RowDefinition {Height = new GridLength(.2, GridUnitType.Star)} ,
                new RowDefinition { Height = new GridLength(.1, GridUnitType.Star)}, // spacer
                new RowDefinition { Height = new GridLength(.5, GridUnitType.Star)},
                new RowDefinition {Height = new GridLength(.1, GridUnitType.Star)}
            },
            ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
        };



        var horsesGrid = new Grid
        {
            VerticalOptions = LayoutOptions.End,
            ColumnSpacing = 0,
            RowSpacing = 0,
            RowDefinitions =
                {

                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },

                },

            ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(.95, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(.05, GridUnitType.Star) },
                },
        };

        // score board
        gridView.Add(resultBox, 0, 0);

        // race area
        raceWrapper.Add(horseView);
        horsesGrid.Add(raceWrapper, 0, 0);
        horsesGrid.Add(endLine, 1, 0);
        gridView.Add(horsesGrid, 0, 2);

        // button
        raceBtn.SetBinding(Button.IsEnabledProperty, new Binding("IsNotBusy", source: vm));
        gridView.Add(new HorizontalStackLayout
        {

            Children = { raceBtn },
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        },
            0, 3);
        //gridView.Add(btn, 0, 2);
        Content = gridView;

    
    }


    async Task AnimateRectangle(View view, uint duaration=2000)
    {
        if (view.Width > 0)
        {
            //double maxX = DeviceDisplay.MainDisplayInfo.Width - view.Width; // maui bug
            double maxX =(uint) Shell.Current.Window.Width/2 - view.Width;
            await view.TranslateTo(maxX, 0, duaration); // Animating the translation from left to right
            
        };

}

    private async Task<Task[]> ApplyAnimation(View horseView, bool reset=false)
    {
        Task[] tasks = new Task[] {};

        foreach (var v in horseView.GetVisualTreeDescendants())
        {

            if (v is RadioButton)
            {
                RadioButton r = (RadioButton)v;
                var horse = (Horse)r.BindingContext;
                if (reset)
                {
                    await r.TranslateTo(0, (uint)0);
                }
                else
                {
                    tasks.Append(AnimateRectangle(r, (uint)horse.Speed));
                }

            }
        }
        await Task.WhenAll(tasks);
        return tasks;
        
    }

    private async Task AnnounceWinner(MainViewModel vm, CollectionView view)
    {
        
        Horse winner = vm.Horses.OrderBy((elm) => elm.Speed).FirstOrDefault(); // cheap sorting todo: handle two or more winners
        var result = await DisplayAlert("Winner", $"{winner.Name}", "Restart Game", "Quit");

        if (result)
        {

            vm.RestartGame();
            await ApplyAnimation(view, reset: true); // hard reset
        }
        else
        {
            App.Current.Quit();
        }
    }
}
