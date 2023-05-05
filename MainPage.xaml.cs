using System.Collections.ObjectModel;

namespace CalculatorApp;

public partial class MainPage : ContentPage
{
	public int count = 0;


    public MainPage()
	{
		InitializeComponent();
        BindingContext = this;

    }


    // handle event
    void ButtonClicked(System.Object sender, System.EventArgs e)
    {
        Button button = (Button)sender;
        string buttonText = button.Text;
        HandleMathOperation(buttonText);
    }

    void ClearAll(System.Object sender, System.EventArgs e)
    {
        sum.Text = "";
        fnum.Text = "";
        snum.Text = "";
    }

    // calculator brain
    void HandleMathOperation(string btn)
    {
        //double FirstInput = Convert.ToDouble(fnum.Text);
        //double SecondInput = Convert.ToDouble(snum.Text);
        double temp;
        bool hasNum1 = Double.TryParse(fnum.Text.ToString(), out double FirstInput);
        bool hasNum2 = Double.TryParse(snum.Text.ToString(), out double SecondInput);

        if(hasNum1&&hasNum2)
            switch (btn)
            {
                case "-":
                    // do
                    temp = FirstInput - SecondInput;
                    PlotNumbers(temp);
                    break;
                case "+":
                    temp = FirstInput + SecondInput;
                    PlotNumbers(temp);
                    break;
                case "x":
                    temp = FirstInput * SecondInput;
                    PlotNumbers(temp);
                    break;
                case "/":
                    temp = FirstInput / SecondInput;
                    PlotNumbers(temp);
                    break;
            }
    }
    // plot text

    void PlotNumbers(dynamic text)
    {
        string t = (string)text.ToString();
        string fmt = t.Length > 9 ? "0.00000#E+0" : null; // high precision :)
        sum.Text = (string)text.ToString(fmt);
    }
    void ValidateInput(object sender, TextChangedEventArgs e)
    {
        bool isValid = double.TryParse(e.NewTextValue, out double result) || String.IsNullOrEmpty(e.NewTextValue);
        if (!isValid)
        {
            Entry entry = (Entry)sender;

            //entry.Text = e.OldTextValue; //  bug crashes
            entry.Text = entry.Text[..^1]; // remove last char
            
        }


    }
}

