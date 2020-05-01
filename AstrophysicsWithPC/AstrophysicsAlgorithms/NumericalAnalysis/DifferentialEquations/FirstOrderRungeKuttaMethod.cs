﻿/****************************************************************************************************************************************
 * 
 * Classe FirstOrderRungeKuttaMethod
 * Auteur : S. ALVAREZ
 * Date : 28-04-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de calculer la solution d'un équation différentielle du 1er ordre par la méthode de Runge-Kutta 
 *         au 4ème ordre.
 * 
 ****************************************************************************************************************************************/

using System;

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

        // METHODE
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
                int index = 0;
                if (a_isIterationDetailsAsked)
                {
                    ComputationDetails = new double[IterationNumber + 1, 2];
                }
                double x = xStartingPoint.Value;
                double y = yStartingPoint.Value;
                double k1, k2, k3, k4;
                if (a_isIterationDetailsAsked)
                {
                    ComputationDetails[index, 0] = x;
                    ComputationDetails[index, 1] = y;
                    index++;
                }
                while (x < a_x)
                {
                    double increment = ComputeStep;
                    if (a_x - x < ComputeStep)
                    {
                        increment = a_x - x;
                    }
                    k1 = increment * Equation(x, y);
                    k2 = increment * Equation(x + 0.5 * increment, y + 0.5 * k1);
                    k3 = increment * Equation(x + 0.5 * increment, y + 0.5 * k2);
                    k4 = increment * Equation(x + increment, y + k3);
                    y += (k1 + 2 * k2 + 2 * k3 + k4) / 6;
                    x += increment;
                    if (a_isIterationDetailsAsked)
                    {
                        ComputationDetails[index, 0] = x;
                        ComputationDetails[index, 1] = y;
                        index++;
                    }
                }
                return y;
            }
            return null;
        }

    }
}
