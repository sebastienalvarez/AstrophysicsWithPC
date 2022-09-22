using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AstrophysicsAlgorithms.RestrictedThreeBodyProblem;

namespace RestrictedThreeBodyProblem
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // PROPRIETES

        private string[] problemSelections;

        public string[] ProblemSelections
        {
            get { return problemSelections; }
            set 
            { 
                problemSelections = value;
                OnPropertyChanged(nameof(ProblemSelections));
            }
        }

        private int problemSelectedIndex;

        public int ProblemSelectedIndex
        {
            get { return problemSelectedIndex; }
            set 
            { 
                problemSelectedIndex = value;
                OnPropertyChanged(nameof(problemSelectedIndex));
            }
        }

        private double massParameter;

        public double MassParameter
        {
            get { return massParameter; }
            set
            {
                massParameter = value;
                OnPropertyChanged(nameof(MassParameter));
            }
        }

        private double initialConditionX;

        public double InitialConditionX
        {
            get { return initialConditionX; }
            set
            {
                initialConditionX = value;
                OnPropertyChanged(nameof(InitialConditionX));
            }
        }

        private double initialConditionY;

        public double InitialConditionY
        {
            get { return initialConditionY; }
            set
            {
                initialConditionY = value;
                OnPropertyChanged(nameof(InitialConditionY));
            }
        }

        private double initialConditionU;

        public double InitialConditionU
        {
            get { return initialConditionU; }
            set
            {
                initialConditionU = value;
                OnPropertyChanged(nameof(InitialConditionU));
            }
        }

        private double initialConditionV;

        public double InitialConditionV
        {
            get { return initialConditionV; }
            set
            {
                initialConditionV = value;
                OnPropertyChanged(nameof(InitialConditionV));
            }
        }

        private double computeStep;

        public double ComputeStep
        {
            get { return computeStep; }
            set
            {
                computeStep = value;
                OnPropertyChanged(nameof(ComputeStep));
            }
        }

        private uint iterationNumber;

        public uint IterationNumber
        {
            get { return iterationNumber; }
            set
            {
                iterationNumber = value;
                OnPropertyChanged(nameof(IterationNumber));
            }
        }

        private bool isComputationEnabled;

        public bool IsComputationEnabled
        {
            get { return isComputationEnabled; }
            set
            {
                isComputationEnabled = value;
                OnPropertyChanged(nameof(IsComputationEnabled));
            }
        }

        // Settings
        int time = 10;
        private double scale;
        private double centerX;
        private double centerY;
        private AstrophysicsAlgorithms.RestrictedThreeBodyProblem.RestrictedThreeBodyProblem problem;
        private Canvas canvas;
        private double x;
        private double y;

        private BackgroundWorker parallelAnimationComputation;
        private BackgroundWorker parallelStaticComputation;

        // EVENEMENT
        public event PropertyChangedEventHandler PropertyChanged;

        // CONSTRUCTEUR
        public MainWindowViewModel(Canvas a_canvas)
        {
            canvas = a_canvas;
            ProblemSelections = new string[] { "Trojan asteroid A", "Trojan asteroid B", "Trojan asteroid C", "Trojan asteroid D", "Hilda without libration", "Hilda with libration", "Thule without libration", "Pluto/Neptune one orbit", "Pluto/Neptune libration" };
            ProblemSelectedIndex = 0;
            IsComputationEnabled = true;

            parallelAnimationComputation = new BackgroundWorker();
            parallelAnimationComputation.WorkerReportsProgress = true;
            parallelAnimationComputation.DoWork += ParallelAnimationComputation_DoWork;
            parallelAnimationComputation.ProgressChanged += ParallelAnimationComputation_ProgressChanged;
            parallelAnimationComputation.RunWorkerCompleted += ParallelAnimationComputation_RunWorkerCompleted;

            parallelStaticComputation = new BackgroundWorker();
            parallelStaticComputation.DoWork += ParallelStaticComputation_DoWork;
            parallelStaticComputation.RunWorkerCompleted += ParallelStaticComputation_RunWorkerCompleted;
        }

        // METHODES
        public void UpdateInitialValues(int a_index)
        {
            switch (a_index)
            {
                case 1: // Trojan asteroid B
                    MassParameter = 0.000953875;
                    InitialConditionX = -0.524046125;
                    InitialConditionY = 0.909326674;
                    InitialConditionU = 0.0646761399;
                    InitialConditionV = 0.0367068277;
                    ComputeStep = 0.1;
                    IterationNumber = 1200;
                    break;
                case 2: // Trojan asteroid C
                    MassParameter = 0.0121396054;
                    InitialConditionX = -0.4978603946;
                    InitialConditionY = 0.8833459119;
                    InitialConditionU = 0.0265752203;
                    InitialConditionV = 0.0146709149;
                    ComputeStep = 0.1;
                    IterationNumber = 220;
                    break;
                case 3: // Trojan asteroid D
                    MassParameter = 0.0121396054;
                    InitialConditionX = -0.5128603946;
                    InitialConditionY = 0.9093266740;
                    InitialConditionU = 0.0682722747;
                    InitialConditionV = 0.0334034039;
                    ComputeStep = 0.1;
                    IterationNumber = 220;
                    break;
                case 4: // Hilda without libration
                    MassParameter = 0.000953875;
                    InitialConditionX = -0.647717531;
                    InitialConditionY = 0.0;
                    InitialConditionU = 0.0;
                    InitialConditionV = -0.6828143998;
                    ComputeStep = 0.02;
                    IterationNumber = 630;
                    break;
                case 5: // Hilda with libration
                    MassParameter = 0.000953875;
                    InitialConditionX = -0.4952265404;
                    InitialConditionY = -0.4163448036;
                    InitialConditionU = 0.4389046359;
                    InitialConditionV = -0.5230661767;
                    ComputeStep = 0.05;
                    IterationNumber = 3000;
                    break;
                case 6: // Thule without libration
                    MassParameter = 0.000953875;
                    InitialConditionX = -0.7997634829;
                    InitialConditionY = 0.0;
                    InitialConditionU = 0.0;
                    InitialConditionV = -0.3334548184;
                    ComputeStep = 0.05;
                    IterationNumber = 344;
                    break;
                case 7: // Pluto/Neptune one orbit
                    MassParameter = 0.0000525;
                    InitialConditionX = -0.6073955952;
                    InitialConditionY = -0.7774968265;
                    InitialConditionU = 0.1083342234;
                    InitialConditionV = -0.08463997159;
                    ComputeStep = 0.05;
                    IterationNumber = 375;
                    break;
                case 8: // Pluto/Neptune libration
                    MassParameter = 0.0000525;
                    InitialConditionX = -0.6073955952;
                    InitialConditionY = -0.7774968265;
                    InitialConditionU = 0.1083342234;
                    InitialConditionV = -0.08463997159;
                    ComputeStep = 0.05;
                    IterationNumber = 3935;
                    break;
                default: // Trojan asteroid A
                    MassParameter = 0.000953875;
                    InitialConditionX = -0.509046125;
                    InitialConditionY = 0.883345912;
                    InitialConditionU = 0.0258975212;
                    InitialConditionV = 0.0149272418;
                    ComputeStep = 0.4;
                    IterationNumber = 210;
                    break;
            }
        }

        #region Animation display processing
        public void ComputeAnimationDisplay()
        {
            if (!parallelStaticComputation.IsBusy && !parallelAnimationComputation.IsBusy && IsComputationEnabled)
            {
                IsComputationEnabled = false;
                problem = new AstrophysicsAlgorithms.RestrictedThreeBodyProblem.RestrictedThreeBodyProblem(massParameter, InitialConditionX, InitialConditionY, InitialConditionU, InitialConditionV);
                parallelAnimationComputation.RunWorkerAsync();
            }
        }

        private void ParallelAnimationComputation_DoWork(object sender, DoWorkEventArgs e)
        {
            // Calcul de l'orbite
            for(uint i = 0; i < 1000; i++)
            {
                double[] results = problem.ComputeNextStep(ComputeStep);
                x = results[1];
                y = results[2];
                parallelAnimationComputation.ReportProgress(0);
                System.Threading.Thread.Sleep(time);
            }
        }

        private void ParallelAnimationComputation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            canvas.Children.Clear();
            SetScaleAndOrigin();
            DrawOrbitForAnimation(x, y);
            canvas.UpdateLayout();
        }

        private void ParallelAnimationComputation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            canvas.Children.Clear();
            IsComputationEnabled = true;
        }
        #endregion


        #region Static display processing
        public void ComputeStaticDisplay()
        {
            if (!parallelStaticComputation.IsBusy && !parallelAnimationComputation.IsBusy && IsComputationEnabled)
            {
                IsComputationEnabled = false;
                problem = new AstrophysicsAlgorithms.RestrictedThreeBodyProblem.RestrictedThreeBodyProblem(massParameter, InitialConditionX, InitialConditionY, InitialConditionU, InitialConditionV);
                System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                parallelStaticComputation.RunWorkerAsync();
            }
        }

        private void ParallelStaticComputation_DoWork(object sender, DoWorkEventArgs e)
        {
            // Calcul de l'orbite
            problem.Compute(ComputeStep, IterationNumber);
        }

        private void ParallelStaticComputation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DrawStaticDisplay();
            System.Windows.Input.Mouse.OverrideCursor = null;
            IsComputationEnabled = true;

        }
        #endregion


        #region Utility methods
        public void DrawStaticDisplay()
        {
            canvas.Children.Clear();
            SetScaleAndOrigin();
            DrawStaticOrbit();
        }

        private void DrawTwoBodies()
        {
            // Dessin du 1er corps
            Ellipse firstBody = new Ellipse();
            firstBody.Width = 10;
            firstBody.Height = 10;
            firstBody.Fill = Brushes.Yellow;
            firstBody.Stroke = Brushes.Yellow;
            canvas.Children.Add(firstBody);
            Canvas.SetLeft(firstBody, centerX);
            Canvas.SetTop(firstBody, centerY - 5);

            // Dessin du 2ème corps
            Ellipse secondBody = new Ellipse();
            secondBody.Width = 5;
            secondBody.Height = 5;
            secondBody.Fill = Brushes.Red;
            secondBody.Stroke = Brushes.Red;
            canvas.Children.Add(secondBody);
            Canvas.SetLeft(secondBody, centerX - scale - 2.5);
            Canvas.SetTop(secondBody, centerY - 2.5);

            // Dessin d'une ligne reliant les 2 corps
            Line line = new Line();
            line.Fill = Brushes.Transparent;
            line.Stroke = Brushes.Black;
            line.StrokeThickness = 0.25;
            line.X1 = centerX;
            line.Y1 = centerY;
            line.X2 = centerX - scale;
            line.Y2 = centerY;
            canvas.Children.Add(line);
        }

        private void DrawScale()
        {
            // Dessin de l'échelle
            // Axe X
            Line axeX = new Line();
            axeX.X1 = centerX + 10;
            axeX.X2 = centerX + 10 + 50;
            axeX.Y1 = centerY;
            axeX.Y2 = centerY;
            axeX.Fill = Brushes.Black;
            axeX.Stroke = Brushes.Black;
            axeX.StrokeThickness = 0.5;
            canvas.Children.Add(axeX);

            // Légende X
            TextBlock textBlockX = new TextBlock();
            textBlockX.Text = "X";
            textBlockX.Foreground = new SolidColorBrush(Colors.Black);
            canvas.Children.Add(textBlockX);
            Canvas.SetLeft(textBlockX, centerX + 10 + 50 + 10);
            Canvas.SetBottom(textBlockX, centerY - 7);

            // Axe Y
            Line axeY = new Line();
            axeY.X1 = centerX + 5;
            axeY.X2 = centerX + 5;
            axeY.Y1 = centerY - 5;
            axeY.Y2 = centerY - 5 - 50;
            axeY.Fill = Brushes.Black;
            axeY.Stroke = Brushes.Black;
            axeY.StrokeThickness = 0.5;
            canvas.Children.Add(axeY);

            // Légende Y
            TextBlock textBlockY = new TextBlock();
            textBlockY.Text = "Y";
            textBlockY.Foreground = new SolidColorBrush(Colors.Black);
            canvas.Children.Add(textBlockY);
            Canvas.SetLeft(textBlockY, centerX + 2);
            Canvas.SetBottom(textBlockY, centerY + 5 + 50 + 10);
        }

        private void SetScaleAndOrigin()
        {
            // Détermination de l'échelle pour le dessin
            double factor = 2;
            if(ProblemSelectedIndex == 7 || ProblemSelectedIndex == 8)
            {
                factor = 4;
            }
            double scaleX = canvas.ActualWidth / factor;
            double scaleY = canvas.ActualHeight / factor;
            scale = Math.Min(scaleX, scaleY);

            // Détermination du point d'origine pour le dessin sur le canvas
            centerX = canvas.ActualWidth / 2;
            centerY = canvas.ActualHeight / 2;
        }

        private void DrawStaticOrbit()
        {
            if(problem != null && problem.Results != null)
            {
                DrawScale();
                DrawTwoBodies();

                // Dessin de l'orbite
                Polyline orbit = new Polyline();
                orbit.Fill = Brushes.Transparent;
                orbit.Stroke = Brushes.Black;
                orbit.StrokeThickness = 0.5;
                //orbit.StrokeDashArray = new DoubleCollection { 10, 10 };
                foreach (var point in problem.Results)
                {
                    orbit.Points.Add(new System.Windows.Point(point[1] * scale, point[2] * scale));
                }
                canvas.Children.Add(orbit);
                Canvas.SetLeft(orbit, centerX);
                Canvas.SetTop(orbit, centerY);
            }
        }

        private void DrawOrbitForAnimation(double a_x, double a_y)
        {
            DrawScale();
            DrawTwoBodies();

            // Dessin du 3ème corps
            Ellipse thirdBody = new Ellipse();
            thirdBody.Width = 10;
            thirdBody.Height = 10;
            thirdBody.Fill = Brushes.Black;
            thirdBody.Stroke = Brushes.Black;
            canvas.Children.Add(thirdBody);
            Canvas.SetLeft(thirdBody, centerX + a_x * scale);
            Canvas.SetTop(thirdBody, centerY + a_y * scale);
        }

        protected void OnPropertyChanged(string a_propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(a_propertyName));
        }
        #endregion
    }
}
