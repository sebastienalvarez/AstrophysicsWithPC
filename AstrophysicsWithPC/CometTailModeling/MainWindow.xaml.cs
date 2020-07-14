using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CometTailModeling
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel(canvas);
            DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if(button.Name == "AnimationButton")
            {
                vm.ComputeAnimationDisplay();
            }
            else if(button.Name == "StaticButton")
            {
                vm.ComputeStaticDisplay();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (vm.IsStaticComputationExists)
            {
                vm.DrawStaticDisplay();
            }
            else
            {
                canvas.Children.Clear();
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox texbox = sender as TextBox;
            bool isDoubleValue = double.TryParse(texbox.Text, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out _);
            switch (texbox.Name)
            {
                case "PerihelionTextBox":
                    vm.IsPerihelionInputValid = isDoubleValue;
                    break;
                case "EccentricityTextBox":
                    vm.IsEccentricityInputValid = isDoubleValue;
                    break;
                case "ForceParameterTextBox":
                    vm.IsForceParameterInputValid = isDoubleValue;
                    break;
                case "OutFlowVelocityTextBox":
                    vm.IsOutFlowVelocityInputValid = isDoubleValue;
                    break;
            }
            vm.IsInputValid = vm.IsInputValid;
        }
    }
}