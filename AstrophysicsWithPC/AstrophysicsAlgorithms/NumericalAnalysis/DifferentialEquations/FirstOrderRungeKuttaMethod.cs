/****************************************************************************************************************************************
 * 
 * Classe FirstOrderRungeKuttaMethod
 * Auteur : S. ALVAREZ
 * Date : 04-09-2022
 * Statut : En Cours
 * Version : 2
 * Revisions : 1 - 28-04-2020 : 1ère version
 *             2 - 04-09-2022 : modification pour permettre le couplage de n équations différentielles du 1er ordre
 * 
 * Objet : Classe permettant de calculer la solution d'un équation différentielle du 1er ordre par la méthode de Runge-Kutta 
 *         au 4ème ordre.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;

namespace AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations
{
    public class FirstOrderRungeKuttaMethod : FirstOrderDifferentialEquation
    {
        // CONSTRUCTEUR
        /// <summary>
        /// Création d'une instance avec un delegate représentant l'équation différentielle
        /// </summary>
        /// <param name="a_equation">Delegate représentant l'équation différentielle</param>
        public FirstOrderRungeKuttaMethod(Func<double, double, double> a_equation) : base(a_equation)
        {
        }

        // METHODES
        /// <summary>
        /// Calcule la valeur de la fonction y au prochain pas de calcul
        /// </summary>
        /// <param name="a_x">Valeur de x au pas précédent</param>
        /// <param name="a_y">Valeur de y au pas précédent</param>
        /// <param name="a_increment">Valeur de l'incrément</param>
        /// <param name="a_isIterationDetailsAsked">Flag pour spécifier la sortie des itérations des calculs dans la propriété ComputationDetails</param>
        /// <returns>Valeurs de x et de la fonction y du pas de calcul</returns>
        public override double[] ComputeForNextStep(double a_x, double a_y, double a_increment, bool a_isIterationDetailsAsked = false)
        {
            double k1 = a_increment * Equation(a_x, a_y);
            double k2 = a_increment * Equation(a_x + 0.5 * a_increment, a_y + 0.5 * k1);
            double k3 = a_increment * Equation(a_x + 0.5 * a_increment, a_y + 0.5 * k2);
            double k4 = a_increment * Equation(a_x + a_increment, a_y + k3);
            double y = a_y + (k1 + 2 * k2 + 2 * k3 + k4) / 6;
            double x = a_x + a_increment;
            double[] computedValues = new double[] { x, y };
            if (a_isIterationDetailsAsked)
            {
                if (ComputationDetails == null)
                {
                    ComputationDetails = new List<double[]>();
                }
                ComputationDetails.Add(computedValues);
            }
            return computedValues;
        }

        /// <summary>
        /// Calcule la valeur de la fonction y pour la valeur de x spécifiée
        /// </summary>
        /// <param name="a_x">Valeur de x pour laquelle la fonction doit être calculée</param>
        /// <param name="a_isIterationDetailsAsked">Flag pour spécifier la sortie des itérations des calculs dans la propriété ComputationDetails</param>
        /// <returns>Valeur de la fonction y ou null si le calcul n'est pas possible avec les paramètres définis</returns>
        public override double? ComputeForGivenX(double a_x, bool a_isIterationDetailsAsked = false)
        {
            if (CheckIfComputationPossible(a_x))
            {
                ComputationDetails = null;
                if (a_isIterationDetailsAsked)
                {
                    ComputationDetails = new List<double[]>();
                }
                double x = xStartingPoint.Value;
                double y = yStartingPoint.Value;
                double[] currentValues = new double[] { x, y };
                if (a_isIterationDetailsAsked)
                {
                    ComputationDetails.Add(currentValues);
                }
                while (currentValues[0] < a_x)
                {
                    double increment = ComputeStep;
                    if (a_x - currentValues[0] < ComputeStep)
                    {
                        increment = a_x - currentValues[0];
                    }
                    currentValues = ComputeForNextStep(currentValues[0], currentValues[1], increment, a_isIterationDetailsAsked);
                }
                return y;
            }
            return null;
        }

    }
}
