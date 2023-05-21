using System.Collections.ObjectModel;
using SlotGame.Models;
using SlotGame.ViewModels;


namespace SlotGame;

public partial class MainPage : ContentPage
{

    public bool buttonLoading { get; set; } 

    public MainPage(SpinnerViewModel vm)
	{
		Title = "Slot Game";
		BindingContext = vm;

        // predefine ui elements
        Label betLabel = new Label { };
        Label balanceLable = new Label {  };
        Label prizeBox = new Label { Text = "Match 3 drawings to win $50" };
        Label resultLabel = new Label { };
        Frame resultFrame = new Frame { Content = resultLabel };
        
        Button spinBtn = new Button { Text = "Spin it", HeightRequest = 80, WidthRequest = 200, Margin = new(0, 100, 0, 0) };

        betLabel.SetBinding(Label.TextProperty, new Binding("Bet", stringFormat: "Your bet is {0:C}", source: vm.Game, mode: BindingMode.OneTime));
        balanceLable.SetBinding(Label.TextProperty, new Binding("Balance", stringFormat: "Your Balance is {0:C}", source: vm.Game, mode: BindingMode.OneWay));
        resultLabel.SetBinding(Label.TextProperty, new Binding("ResultMessage", mode: BindingMode.OneWay));
        spinBtn.SetBinding(IsEnabledProperty, new Binding("IsButtonEnabled", mode: BindingMode.TwoWay));

        spinBtn.Clicked += async (sender, args) => await vm.Spin();

        Content = new VerticalStackLayout
		{
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.Center,
			Children = {

                new HorizontalStackLayout
                {
                    Children =
                    {
                        new StackLayout
                        {
                                Children = { betLabel, balanceLable, prizeBox },
                                Spacing = 30,
                                Padding = 50,


                        },
                        new StackLayout{
                            Children = {resultLabel },
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Center,
                        },
                    }
                },
            
                new CollectionView
                {
                    ItemsSource = vm.Game.SoltImages,
                    ItemsLayout = LinearItemsLayout.Horizontal,

                    ItemTemplate = new DataTemplate(() =>
                    {
                        Image img = new Image
                        {
                            Margin = new(150, 0, 0, 0),
                            WidthRequest = 300,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center,

                        };
                        img.SetBinding(Image.SourceProperty, path: "Source");
                        return img;
                    }),

                    MaximumHeightRequest = 350,

                },
                spinBtn,

            }
		};




	}
}
