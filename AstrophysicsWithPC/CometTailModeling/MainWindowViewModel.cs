using AstrophysicsAlgorithms.CometTailModeling;
using AstrophysicsAlgorithms.NumericalAnalysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CometTailModeling
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // PROPRIETES
        private double perihelion;

        public double Perihelion
        {
            get { return perihelion; }
            set 
            { 
                perihelion = value;
                OnPropertyChanged(nameof(Perihelion));
            }
        }

        private double eccentricity;

        public double Eccentricity
        {
            get { return eccentricity; }
            set
            {
                eccentricity = value;
                OnPropertyChanged(nameof(Eccentricity));
            }
        }

        private double forceParameter;

        public double ForceParameter
        {
            get { return forceParameter; }
            set
            {
                forceParameter = value;
                OnPropertyChanged(nameof(ForceParameter));
            }
        }

        private double outFlowVelocity;

        public double OutFlowVelocity
        {
            get { return outFlowVelocity; }
            set
            {
                outFlowVelocity = value;
                OnPropertyChanged(nameof(OutFlowVelocity));
            }
        }

        public bool IsStaticComputationExists { get; private set; }
        public bool IsAnimationComputationInProgress { get; private set; }

        public bool IsPerihelionInputValid { get; set; }
        public bool IsEccentricityInputValid { get; set; }
        public bool IsForceParameterInputValid { get; set; }
        public bool IsOutFlowVelocityInputValid { get; set; }

        private bool isInputValid;

        public bool IsInputValid
        {
            get { return isInputValid; }
            set
            {
                isInputValid = IsPerihelionInputValid && IsEccentricityInputValid && IsForceParameterInputValid && IsOutFlowVelocityInputValid;
                OnPropertyChanged(nameof(IsInputValid));
            }
        }

        private BackgroundWorker parallelAnimationComputation;
        private BackgroundWorker parallelStaticComputation;

        // Settings
        private double trueAnomalyThreshold = 120;
        int time = 10;
        private Comet comet;
        private double scale;
        private double centerX;
        private double centerY;
        private List<Point> orbitalPoints;
        private List<TailModel> tailModels;
        private Canvas canvas;

        // EVENEMENT
        public event PropertyChangedEventHandler PropertyChanged;

        // CONSTRUCTEUR
        public MainWindowViewModel(Canvas a_canvas)
        {
            canvas = a_canvas;
            Perihelion = 0.5;
            Eccentricity = 0.95;
            ForceParameter = 1.0;
            OutFlowVelocity = 0.03;
            IsPerihelionInputValid = true;
            IsEccentricityInputValid = true;
            IsForceParameterInputValid = true;
            IsOutFlowVelocityInputValid = true;

            IsStaticComputationExists = false;
            IsAnimationComputationInProgress = false;

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
        #region Animation display processing
        public void ComputeAnimationDisplay()
        {
            IsStaticComputationExists = false;
            if (!parallelStaticComputation.IsBusy && !parallelAnimationComputation.IsBusy && !IsAnimationComputationInProgress)
            {
                comet = new Comet(Perihelion, Eccentricity, ForceParameter, OutFlowVelocity);
                tailModels = new List<TailModel>();
                IsAnimationComputationInProgress = true;
                parallelAnimationComputation.RunWorkerAsync();
            }
        }

        private void ParallelAnimationComputation_DoWork(object sender, DoWorkEventArgs e)
        {
            // Calcul de l'orbite de la comète
            orbitalPoints = ComputeOrbit();

            // Calcul de la forme de la trainée de la comète
            for (double nu = -trueAnomalyThreshold; nu <= trueAnomalyThreshold; nu += 0.5)
            {
                TailModel tailModel = comet.ComputeSyndynams(nu);
                tailModels.Add(tailModel);
                parallelAnimationComputation.ReportProgress(0);
                System.Threading.Thread.Sleep(time);
            }
        }

        private void ParallelAnimationComputation_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            canvas.Children.Clear();
            SetScaleAndOrigin();
            DrawOrbit();
            DrawCometTails(tailModels[tailModels.Count - 1]);
            DrawScale();
            canvas.UpdateLayout();
        }

        private void ParallelAnimationComputation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            canvas.Children.Clear();
            IsAnimationComputationInProgress = false;
        }
        #endregion

        #region Static display processing
        public void ComputeStaticDisplay()
        {
            IsStaticComputationExists = false;
            if (!parallelStaticComputation.IsBusy && !parallelAnimationComputation.IsBusy && !IsAnimationComputationInProgress)
            {
                comet = new Comet(Perihelion, Eccentricity, ForceParameter, OutFlowVelocity);
                tailModels = new List<TailModel>();
                System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                parallelStaticComputation.RunWorkerAsync();
            }
        }

        private void ParallelStaticComputation_DoWork(object sender, DoWorkEventArgs e)
        {
            // Calcul de l'orbite de la comète
            orbitalPoints = ComputeOrbit();

            // Calcul de la forme de la trainée de la comète
            for (double nu = -120; nu <= 120; nu += 30)
            {
                TailModel tailModel = comet.ComputeSyndynams(nu);
                tailModels.Add(tailModel);
            }
        }

        private void ParallelStaticComputation_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DrawStaticDisplay();
            IsStaticComputationExists = true;
            System.Windows.Input.Mouse.OverrideCursor = null;
        }
        #endregion

        #region Utility methods
        public void DrawStaticDisplay()
        {
            canvas.Children.Clear();
            SetScaleAndOrigin();
            DrawOrbit();
            foreach (var tailModel in tailModels)
            {
                DrawCometTails(tailModel);
            }
            DrawScale();
        }

        private void SetScaleAndOrigin()
        {
            // Détermination de l'échelle pour le dessin en fonction de l'orbite de la comète
            double scaleX = canvas.ActualWidth / (ComputeDistance(trueAnomalyThreshold) * 1.65);
            double scaleY = canvas.ActualHeight / (ComputeDistance(trueAnomalyThreshold) * 1.65);
            scale = Math.Min(scaleX, scaleY);

            // Détermination du point d'origine pour le dessin sur le canvas
            centerX = canvas.ActualWidth / 2;
            centerY = canvas.ActualHeight / 2;
        }

        private void DrawOrbit()
        {
            // Dessin du Soleil
            Ellipse sun = new Ellipse();
            sun.Width = 10;
            sun.Height = 10;
            sun.Fill = Brushes.Yellow;
            sun.Stroke = Brushes.Yellow;
            canvas.Children.Add(sun);
            Canvas.SetLeft(sun, centerX);
            Canvas.SetTop(sun, centerY);

            // Dessin de l'orbite
            Polyline orbit = new Polyline();
            orbit.Fill = Brushes.Transparent;
            orbit.Stroke = Brushes.Black;
            orbit.StrokeThickness = 0.5;
            orbit.StrokeDashArray = new DoubleCollection { 10, 10 };
            foreach (var point in orbitalPoints)
            {
                orbit.Points.Add(new System.Windows.Point(point.X * scale, point.Y * scale));
            }
            canvas.Children.Add(orbit);
            Canvas.SetLeft(orbit, centerX);
            Canvas.SetTop(orbit, centerY);
        }

        private void DrawCometTails(TailModel a_tailModel)
        {
            // Dessin de la comète
            Ellipse nucleus = new Ellipse();
            nucleus.Width = 3;
            nucleus.Height = 3;
            nucleus.Fill = Brushes.OrangeRed;
            nucleus.Stroke = Brushes.OrangeRed;
            canvas.Children.Add(nucleus);
            Canvas.SetLeft(nucleus, centerX + -a_tailModel.NucleusPosition.X * scale - 1.65);
            Canvas.SetTop(nucleus, centerY + a_tailModel.NucleusPosition.Y * scale - 1.65);

            Polyline cometShape = new Polyline();
            cometShape.Fill = Brushes.Transparent;
            cometShape.Stroke = Brushes.OrangeRed;
            cometShape.StrokeThickness = 1;
            foreach (var point in a_tailModel.SyndamA)
            {
                cometShape.Points.Add(new System.Windows.Point(-point.X * scale, point.Y * scale));
            }
            canvas.Children.Add(cometShape);
            Canvas.SetLeft(cometShape, centerX);
            Canvas.SetTop(cometShape, centerY);
            cometShape = new Polyline();
            cometShape.Fill = Brushes.Transparent;
            cometShape.Stroke = Brushes.OrangeRed;
            cometShape.StrokeThickness = 1;
            foreach (var point in a_tailModel.SyndamB)
            {
                cometShape.Points.Add(new System.Windows.Point(-point.X * scale, point.Y * scale));
            }
            canvas.Children.Add(cometShape);
            Canvas.SetLeft(cometShape, centerX);
            Canvas.SetTop(cometShape, centerY);
        }

        private void DrawScale()
        {
            // Affichage de la légende
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "1 AU";
            textBlock.Foreground = new SolidColorBrush(Colors.Black);
            canvas.Children.Add(textBlock);
            Canvas.SetLeft(textBlock, 0);
            Canvas.SetBottom(textBlock, 0);

            // Dessin de l'échelle
            Line axeX = new Line();
            axeX.X1 = 0;
            axeX.X2 = scale;
            axeX.Y1 = 0;
            axeX.Y2 = 0;
            axeX.Fill = Brushes.Black;
            axeX.Stroke = Brushes.Black;
            axeX.StrokeThickness = 2;
            canvas.Children.Add(axeX);
            Canvas.SetLeft(axeX, 30);
            Canvas.SetBottom(axeX, 6);
        }

        private double ComputeDistance(double a_trueAnomaly)
        {
            return Perihelion * (1 + Eccentricity) / (1 + Eccentricity * Math.Cos(Utilities.DegreeToRadian(a_trueAnomaly)));
        }

        private List<Point> ComputeOrbit()
        {
            List<Point> points = new List<Point>();
            double r = 0;
            for (double nu = -trueAnomalyThreshold; nu <= trueAnomalyThreshold; nu += 2)
            {
                r = Perihelion * (1 + Eccentricity) / (1 + Eccentricity * Math.Cos(Utilities.DegreeToRadian(nu)));
                Point p = new Point(-r * Math.Cos(Utilities.DegreeToRadian(nu)), r * Math.Sin(Utilities.DegreeToRadian(nu)));
                points.Add(p);
            }
            return points;
        }

        protected void OnPropertyChanged(string a_propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(a_propertyName));
        }
        #endregion

    }
}