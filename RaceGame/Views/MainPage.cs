using CommunityToolkit.Maui.Animations;
using CommunityToolkit.Maui.Behaviors;
using Microsoft.Maui.Controls;
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
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Fill,
        BackgroundColor = Colors.AntiqueWhite,
        Orientation = StackOrientation.Vertical,

    };

    public MainPage(MainViewModel vm)
    {
        BindingContext = vm;

        // results
        BuildResultBoard(vm);

        resultBox.Content = resultLayout;



        var horseView = new CollectionView
        {
            ItemsSource = vm.Horses,
            ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                ItemSpacing = 50,
            },

            ItemTemplate = new DataTemplate(() =>
            {
                RadioButton horseRadioBtn = new RadioButton { GroupName = "horseGroup" };
                horseRadioBtn.SetBinding(RadioButton.ContentProperty, "Img");
                horseRadioBtn.SetBinding(RadioButton.XProperty, "Name");
                horseRadioBtn.SetBinding(RadioButton.ValueProperty, new Binding("SelectedHorse", source: vm));
                horseRadioBtn.WidthRequest = 120;
                return new HorizontalStackLayout { Children = { horseRadioBtn } };
            }),



        };

        raceBtn.Clicked += (sender, args) =>
        {
            // apply some random delay
            foreach (Horse h in vm.Horses)
            {
                var random = new Random();
                int delay = random.Next(4000, 8000); // random magic numbers
                h.Speed = (double)delay;
                MainThread.BeginInvokeOnMainThread(async () => await vm.animateSeconds(h));
            }
            ApplyAnimation(horseView);
        };

        Grid gridView = new Grid
        {
            RowDefinitions = {
                new RowDefinition() ,
                new RowDefinition { Height = new GridLength(4, GridUnitType.Star) },
                new RowDefinition { }
            },
            ColumnDefinitions = {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
        };



        var horsesGrid = new Grid
        {
            VerticalOptions = LayoutOptions.Center,
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
        gridView.Add(horsesGrid, 0, 1);

        // button
        gridView.Add(new HorizontalStackLayout
        {

            Children = { raceBtn },
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        },
            0, 2);
        //gridView.Add(btn, 0, 2);
        Content = gridView;

    
    }

    void BuildResultBoard(MainViewModel vm)
    {
        foreach (Horse horse in vm.Horses)
        {
            var resultText = new Label { FontSize = 20, };
            resultText.SetBinding(Label.TextProperty, new Binding("Name", source: horse));
            var animatedResultSpeed = new Label {FontSize = 20, };
            animatedResultSpeed.SetBinding(Label.TextProperty, new Binding("RemainingTime", source: horse, stringFormat: "{0:ss\\:ff}"));

            resultLayout.Add(new FlexLayout
            {
                Direction = Microsoft.Maui.Layouts.FlexDirection.Row,
                JustifyContent = Microsoft.Maui.Layouts.FlexJustify.SpaceBetween,
                Children = { resultText, animatedResultSpeed }
            });

        }
    }

    async Task AnimateRectangle(View view, uint duaration=2000)
    {
        if (view.Width > 0) 
        await MainThread.InvokeOnMainThreadAsync(async() =>
        {
         
            //double maxX = DeviceDisplay.MainDisplayInfo.Width - view.Width; // maui bug
            double maxX =(uint) Shell.Current.Window.Width/2 - view.Width;
            await view.TranslateTo(maxX, 0, duaration); // Animating the translation from left to right
            
            // show winner name and image
            //await DisplayAlert("info", $"window width{Shell.Current.Window.Width} \n  max width{maxX} \n elment width {view.Width}", "ok");
        });

    }

    private void ApplyAnimation(View horseView)
    {
        List<Task> tasks = new();

        foreach (var v in horseView.GetVisualTreeDescendants())
        {

            if (v is RadioButton)
            {
                RadioButton r = (RadioButton)v;
                Console.WriteLine(r.Width);
                var horse = (Horse)r.BindingContext;
                tasks.Add(AnimateRectangle(r, (uint)horse.Speed));

            }



        }
    }
}
