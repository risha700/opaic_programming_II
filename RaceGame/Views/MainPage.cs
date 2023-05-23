
using Microsoft.Maui.Controls;
using RaceGame.Models;
using RaceGame.ViewModels;

namespace RaceGame.Views;

public class MainPage : ContentPage
{


	Button raceBtn = new Button { Text="Start Race", HeightRequest=80, WidthRequest=200};

    Image endLine = new Image { Source = "race.jpeg" };
	Entry entryPosX = new Entry { Placeholder="Enter x"};
    Entry entryPosY = new Entry { Placeholder = "Enter y"  };

	Label posX = new Label { };
    Label posY = new Label { };
    Label blabla = new Label { Text="Hi there!"};

    

    public MainPage(MainViewModel vm)
	{
		BindingContext = vm;

        //posX.SetBinding(Label.TextProperty, new Binding("PositionX", mode: BindingMode.TwoWay, stringFormat: "x cords {0}",source:vm));
        //      posY.SetBinding(Label.TextProperty, new Binding("PositionY", mode: BindingMode.TwoWay, stringFormat: "y cords {0}", source: vm));

        raceBtn.Clicked += (sender, args) =>
		{
            //var element = collectionView.GetItemAt(itemIndex);
        };
		Grid gridView = new Grid
        {
            RowDefinitions = {
                new RowDefinition() ,
                new RowDefinition { Height = new GridLength(4, GridUnitType.Star) },
                new RowDefinition { }
            },
            ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) } },
            BackgroundColor = Colors.Aqua,
        };

        // score board
		gridView.Add(entryPosX, 0, 0);
        var horsesGrid = new Grid
        {
            //BackgroundColor = Colors.Beige,
            VerticalOptions = LayoutOptions.Center,
            ColumnSpacing = 0,
            RowSpacing = 0,
            RowDefinitions =
                {
           
                    new RowDefinition { Height = new GridLength(.25, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(.25, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(.25, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(.25, GridUnitType.Star) },
                },

            ColumnDefinitions = {
                    new ColumnDefinition { Width = new GridLength(.9, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(.1, GridUnitType.Star) },
                },

            

        };

        // race area
        

        gridView.Add(new HorizontalStackLayout
        {
            VerticalOptions = LayoutOptions.Fill,
            HorizontalOptions = LayoutOptions.Start,
            
            //Spacing = 50,
            //ColumnDefinitions = {new ColumnDefinition()},
            //RowDefinitions = {new RowDefinition()},
            Children = {new CollectionView {
                ItemsSource = vm.Horses,
                ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
                {
                    ItemSpacing = 25 // Adjust the value (10) to set the vertical spacing between items
                },
                ItemTemplate = new DataTemplate(() =>
                {
                    RadioButton horseRadioBtn = new RadioButton { GroupName = "horseGroup" , BackgroundColor=Colors.Chocolate};
                    horseRadioBtn.SetBinding(RadioButton.ContentProperty, "Img");
                    horseRadioBtn.SetBinding(RadioButton.ValueProperty, new Binding("SelectedHorse", source:vm));

                    
                    //Console.WriteLine();
                    return new HorizontalStackLayout{ Children={horseRadioBtn } };
                }),


            }}
        }, 0, 1);

        // bottom
        gridView.Add(new HorizontalStackLayout {
            //BackgroundColor = Colors.DarkCyan,
            Children = { raceBtn },
            HorizontalOptions=LayoutOptions.Center,
            VerticalOptions=LayoutOptions.Center },
            0, 2);
        //gridView.Add(btn, 0, 2);
        Content = gridView;
	}

    private async void AnimateRectangle(View view)
    {
        await Task.Delay(1000); // Delay before animation starts

        double maxX = DeviceDisplay.MainDisplayInfo.Width - view.Width;
        await view.TranslateTo(maxX, 0, 2000); // Animating the translation from left to right
    }
}
