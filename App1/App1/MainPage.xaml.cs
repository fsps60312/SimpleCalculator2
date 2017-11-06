using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
	public partial class MainPage : ContentPage
	{
        Entry resultLabel = new Entry();//View: Button Label BoxView Grid 
        Grid mainGrid = new Grid();//Grid 也是 View
		public MainPage()
        {
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            resultLabel.Text = "Result";
            resultLabel.FontSize = 50;
            mainGrid.Children.Add(resultLabel, 0, 0);
            Grid.SetColumnSpan(resultLabel, mainGrid.ColumnDefinitions.Count);//=4

            for(int i=1;i<=9;i++)
            {
                Button numberButton = new Button { Text = $"{i}" };
                numberButton.Clicked += NumberButton_Clicked;
                mainGrid.Children.Add(numberButton, (i - 1) % 3, 1 + (i - 1) / 3);
            }

            {
                Button numberButton = new Button { Text = "0" };
                numberButton.Clicked += NumberButton_Clicked;
                mainGrid.Children.Add(numberButton, 1, 4);
            }

            {
                Button button = new Button { Text = "=" };
                button.Clicked += EqualSignClicked;
                mainGrid.Children.Add(button, 0, 4);
            }

            {
                Button button = new Button { Text = "←" };
                button.Clicked += DeleteButtonClicked;
                mainGrid.Children.Add(button, 2, 4);
            }

            char[] signs = { '+', '-', '*', '/' };
            for(int i=0;i<signs.Length;i++)
            {
                Button button = new Button { Text = $"{signs[i]}" };
                button.Clicked += NumberButton_Clicked;
                mainGrid.Children.Add(button, 3, 1 + i);
            }

            this.Content = mainGrid;
        }

        private void DeleteButtonClicked(object sender, EventArgs e)
        {
            if(resultLabel.Text.Length>0)
            {
                resultLabel.Text = resultLabel.Text.Remove(resultLabel.Text.Length - 1);
            }
        }

        private async void EqualSignClicked(object sender, EventArgs e)
        {
            try
            {
                var text = resultLabel.Text;
                var numbers = text.Split(new char[] { '+', '-', '*', '/' });
                if (numbers.Length != 2) throw new Exception();
                await Application.Current.MainPage.DisplayAlert("", string.Join("\r\n", numbers), "OK");
                var a = double.Parse(numbers[0]);
                var b = double.Parse(numbers[1]);
                var sign = text[numbers[0].Length];
                double answer = 0;
                switch (sign)
                {
                    case '+':
                        {
                            answer = a + b;
                            break;
                        }
                    case '-':
                        {
                            answer = a - b;
                            break;
                        }
                    case '*':
                        {
                            answer = a * b;
                            break;
                        }
                    case '/':
                        {
                            answer = a / b;
                            break;
                        }
                    default:
                        {
                            //不會執行到
                            break;
                        }
                }
                resultLabel.Text = answer.ToString();
            }
            catch (Exception error)
            {
                await App.Current.MainPage.DisplayAlert("", $"你亂打字\r\n{error}", "我認輸");
            }
        }

        private void NumberButton_Clicked(object sender, EventArgs e)
        {
            var text = (sender as Button).Text;
            resultLabel.Text += text;
        }
    }
}
