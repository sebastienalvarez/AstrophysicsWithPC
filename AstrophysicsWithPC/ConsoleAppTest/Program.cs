using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Définition de l'équation différentielle à tester
            Func<double, double, double> equation = (x, y) => -2 * y * x;
            bool isIterationDetailsAsked = true;
            IFirstOrderDifferentialEquation equationToSolve = new EulerMethod(equation);

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
                    if(details != null)
                    {
                        Console.WriteLine(details);
                    }
                }
            }
            else
            {
                Console.WriteLine("Calcul impossible");
            }
            Console.ReadKey();
        }
    }
}
