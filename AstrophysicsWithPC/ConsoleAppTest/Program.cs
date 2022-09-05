using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations;
using AstrophysicsAlgorithms.NumericalAnalysis.ZerosOfFunctions;
using AstrophysicsAlgorithms.NumericalAnalysis.Integrals;
using AstrophysicsAlgorithms.CometTailModeling;
using AstrophysicsAlgorithms.MeteorDynamics;
using System.IO;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Définition de l'équation différentielle à tester
            Func<double, double, double> equation = (x, y) => -2 * y * x;
            bool isIterationDetailsAsked = true;
            IFirstOrderDifferentialEquation equationToSolve = new FirstOrderEulerMethod(equation);

            // Test de la méthode d'Euler
            Console.WriteLine("Test méthode Euler :");
            equationToSolve.SetStartingPoint(0, 1);
            equationToSolve.SetComputeStep(0.1);
            double? result = equationToSolve.ComputeForGivenX(1.8, isIterationDetailsAsked);
            if (result.HasValue)
            {
                Console.WriteLine("Résultat du calcul : " + result);
                if (isIterationDetailsAsked)
                {
                    string details = equationToSolve.BuildFormatedComputationDetailsForDisplay();
                    if (details != null)
                    {
                        Console.WriteLine(details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }

            // Test de la méthode de Cauchy
            Console.WriteLine("Test méthode Cauchy :");
            equationToSolve = new FirstOrderCauchyMethod(equation);
            equationToSolve.SetStartingPoint(0, 1);
            equationToSolve.SetComputeStep(0.1);
            result = equationToSolve.ComputeForGivenX(1.8, isIterationDetailsAsked);
            if (result.HasValue)
            {
                Console.WriteLine("Résultat du calcul : " + result);
                if (isIterationDetailsAsked)
                {
                    string details = equationToSolve.BuildFormatedComputationDetailsForDisplay();
                    if (details != null)
                    {
                        Console.WriteLine(details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }

            // Test de la méthode de Heun
            Console.WriteLine("Test méthode Heun :");
            equationToSolve = new FirstOrderHeunMethod(equation);
            equationToSolve.SetStartingPoint(0, 1);
            equationToSolve.SetComputeStep(0.1);
            result = equationToSolve.ComputeForGivenX(1.8, isIterationDetailsAsked);
            if (result.HasValue)
            {
                Console.WriteLine("Résultat du calcul : " + result);
                if (isIterationDetailsAsked)
                {
                    string details = equationToSolve.BuildFormatedComputationDetailsForDisplay();
                    if (details != null)
                    {
                        Console.WriteLine(details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }

            // Test de la méthode Runge-Kutta au 4ème ordre
            Console.WriteLine("Test méthode Runge-Kutta au 4ème ordre :");
            equationToSolve = new FirstOrderRungeKuttaMethod(equation);
            equationToSolve.SetStartingPoint(0, 1);
            equationToSolve.SetComputeStep(0.1);
            result = equationToSolve.ComputeForGivenX(1.8, isIterationDetailsAsked);
            if (result.HasValue)
            {
                Console.WriteLine("Résultat du calcul : " + result);
                if (isIterationDetailsAsked)
                {
                    string details = equationToSolve.BuildFormatedComputationDetailsForDisplay();
                    if (details != null)
                    {
                        Console.WriteLine(details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }



            // Définition de l'équation différentielle à tester
            Func<double, double, double, double>[] equations = new Func<double, double, double, double>[]
            {
                (x,y,z) => -2 * z,
                (x,y,z) => y - Math.Sin(x) * Math.Sin(x)
            };
            isIterationDetailsAsked = true;
            ISecondOrderDifferentialEquation equations2Solve = new SecondOrderEulerMethod(equations);

            // Test de la méthode de Euler 2ème ordre
            Console.WriteLine("Test méthode Euler 2ème ordre :");
            equations2Solve.SetStartingPoints(new double[3] { 0, 1, 0 });
            equations2Solve.SetComputeStep(0.05);
            double[] results = equations2Solve.ComputeForGivenX(0.5, isIterationDetailsAsked);
            if (results != null && results.Length > 0)
            {
                Console.WriteLine($"Résultat du calcul : y = {results[1]}, z = {results[2]}");
                if (isIterationDetailsAsked)
                {
                    string details = equations2Solve.BuildFormatedComputationDetailsForDisplay();
                    if (details != null)
                    {
                        Console.WriteLine(details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }


            // Test de la méthode de Cauchy 2ème ordre
            Console.WriteLine("Test méthode Cauchy 2ème ordre :");
            equations2Solve = new SecondOrderCauchyMethod(equations);
            equations2Solve.SetStartingPoints(new double[3] { 0, 1, 0});
            equations2Solve.SetComputeStep(0.05);
            results = equations2Solve.ComputeForGivenX(0.5, isIterationDetailsAsked);
            if (results != null && results.Length > 0)
            {
                Console.WriteLine($"Résultat du calcul : y = {results[1]}, z = {results[2]}");
                if (isIterationDetailsAsked)
                {
                    string details = equations2Solve.BuildFormatedComputationDetailsForDisplay();
                    if (details != null)
                    {
                        Console.WriteLine(details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }

            // Test de la méthode de Heun 2ème ordre
            Console.WriteLine("Test méthode Heun 2ème ordre :");
            equations2Solve = new SecondOrderHeunMethod(equations);
            equations2Solve.SetStartingPoints(new double[3] { 0, 1, 0 });
            equations2Solve.SetComputeStep(0.05);
            results = equations2Solve.ComputeForGivenX(0.5, isIterationDetailsAsked);
            if (results != null && results.Length > 0)
            {
                Console.WriteLine($"Résultat du calcul : y = {results[1]}, z = {results[2]}");
                if (isIterationDetailsAsked)
                {
                    string details = equations2Solve.BuildFormatedComputationDetailsForDisplay();
                    if (details != null)
                    {
                        Console.WriteLine(details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }

            // Test de la méthode de Runge-Kutta au 4ème ordre
            Console.WriteLine("Test méthode Runge-Kutta au 4ème ordre :");
            equations2Solve = new SecondOrderRungeKuttaMethod(equations);
            equations2Solve.SetStartingPoints(new double[3] { 0, 1, 0 });
            equations2Solve.SetComputeStep(0.05);
            results = equations2Solve.ComputeForGivenX(0.5, isIterationDetailsAsked);
            if (results != null && results.Length > 0)
            {
                Console.WriteLine($"Résultat du calcul : y = {results[1]}, z = {results[2]}");
                if (isIterationDetailsAsked)
                {
                    string details = equations2Solve.BuildFormatedComputationDetailsForDisplay();
                    if (details != null)
                    {
                        Console.WriteLine(details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }

            // Calcul du zéro d'une fonction
            Func<double, double> equationToComputeZero = (x) => Math.Exp(x) - 6 * x;
            Func<double, double> derivedEquationToComputeZero = (x) => Math.Exp(x) - 6;
            NewtonRaphsonMethod zeroFinder = new NewtonRaphsonMethod(equationToComputeZero, derivedEquationToComputeZero);
            double? zero = zeroFinder.ComputeForZero(3, true);
            if (zero.HasValue)
            {
                Console.WriteLine($"Zéro trouvé : {zero.Value} en {zeroFinder.IterationNumberReached} iterations");
                Console.WriteLine(zeroFinder.BuildFormatedComputationDetailsForDisplay());
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }

            // Calcul d'une intégrale
            Func<double, double> equationToInteger = (x) => Math.Exp(-x) + 2 * x;
            SimpsonMethod integral = new SimpsonMethod(equationToInteger);
            integral.SetIterationNumber(100);
            double? computedIntegral = integral.ComputeIntegral(2, 6);
            if (computedIntegral.HasValue)
            {
                Console.WriteLine($"Intégrale : {computedIntegral.Value}");
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }

            // Calcul d'une comète
            Comet comet = new Comet(0.5, 0.95, 1.0, 0.03);
            var tailResult = comet.ComputeSyndynams(-114.5916);

            // TEST CALCUL METEORE
            Meteor meteor = new Meteor(0.01, 160, 20, 40, 1, 1e-11, 0.02);
            //Meteor meteor = new Meteor(5000, 160, 20, 60, 1, 1e-11, 0.02);
            meteor.ComputeDynamics();
            meteor.OutputDataToConsole();

            meteor.ComputeDynamicsWithDefaultMethod();
            meteor.OutputDataToConsole();

            //string meteorFilePath = Path.Combine(Environment.CurrentDirectory, "meteor_output.csv");
            //meteor.OutputDataToCsv(meteorFilePath);

            Console.ReadKey();
        }
    }
}
