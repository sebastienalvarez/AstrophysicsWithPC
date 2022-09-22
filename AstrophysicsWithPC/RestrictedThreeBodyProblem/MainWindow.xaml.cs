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

namespace RestrictedThreeBodyProblem
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

        private void ProblemSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            vm.UpdateInitialValues(comboBox.SelectedIndex);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Name == "AnimationButton")
            {
                vm.ComputeAnimationDisplay();
            }
            else if (button.Name == "StaticButton")
            {
                vm.ComputeStaticDisplay();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            vm.DrawStaticDisplay();
            //if (vm.IsStaticComputationExists)
            //{
            //    vm.DrawStaticDisplay();
            //}
            //else
            //{
            //    canvas.Children.Clear();
            //}

        }
    }
}
