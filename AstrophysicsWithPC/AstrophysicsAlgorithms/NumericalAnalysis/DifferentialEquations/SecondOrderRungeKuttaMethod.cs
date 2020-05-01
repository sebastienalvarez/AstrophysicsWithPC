/****************************************************************************************************************************************
 * 
 * Classe SecondOrderRungeKuttaMethod
 * Auteur : S. ALVAREZ
 * Date : 29-04-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de calculer la solution d'un équation différentielle du 2ème ordre décomposée en 2 équations
 *         différentielles du 1er ordre chainées par la méthode de Runge-Kutta au 4ème ordre.
 * 
 ****************************************************************************************************************************************/

using System;

namespace AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations
{
    public class SecondOrderRungeKuttaMethod : SecondOrderDifferentialEquation
    {
        // CONSTRUCTEUR
        /// <summary>
        /// Création d'une instance avec un tableau de 2 delegates représentant les équations différentielles du 1er ordre chainées
        /// </summary>
        /// <param name="a_equations">Tableau de 2 delegates représentant les équations différentielles du 1er ordre chainées</param>
        public SecondOrderRungeKuttaMethod(Func<double, double, double, double>[] a_equations) : base(a_equations)
        {
        }

        // METHODE
        /// <summary>
        /// Calcule la valeur des fonctions y et z pour la valeur de x spécifiée
        /// </summary>
        /// <param name="a_x">Valeur de x pour laquelle les fonctions doivent être calculées</param>
        /// <param name="a_isIterationDetailsAsked">Flag pour spécifier la sortie des itérations des calculs dans la propriété ComputationDetails</param>
        /// <returns>Valeurs des fonctions y et z ou null si le calcul n'est pas possible avec les paramètres définis</returns>
        public override double[] ComputeForGivenX(double a_x, bool a_isIterationDetailsAsked = false)
        {
            if (CheckIfComputationPossible(a_x))
            {
                ComputationDetails = null;
                int index = 0;
                if (a_isIterationDetailsAsked)
                { 
                    ComputationDetails = new double[IterationNumber + 1, 3];
                }
                double[] values = new double[3];
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = startingPoints[i];
                }
                double k10, k20, k30, k40;
                double k11, k21, k31, k41;
                if (a_isIterationDetailsAsked)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        ComputationDetails[index, i] = values[i];
                    }
                    index++;
                }
                while (values[0] < a_x)
                {
                    double increment = ComputeStep;
                    if (a_x - values[0] < ComputeStep)
                    {
                        increment = a_x - values[0];
                    }
                    k10 = increment * Equations[0](values[0], values[1], values[2]);
                    k20 = increment * Equations[0](values[0] + 0.5 * increment, values[1] + 0.5 * k10, values[2] + 0.5 * k10);
                    k30 = increment * Equations[0](values[0] + 0.5 * increment, values[1] + 0.5 * k20, values[2] + 0.5 * k20);
                    k40 = increment * Equations[0](values[0] + increment, values[1] + k30, values[2] + k30);
                    k11 = increment * Equations[1](values[0], values[1], values[2]);
                    k21 = increment * Equations[1](values[0] + 0.5 * increment, values[1] + 0.5 * k11, values[2] + 0.5 * k11);
                    k31 = increment * Equations[1](values[0] + 0.5 * increment, values[1] + 0.5 * k21, values[2] + 0.5 * k21);
                    k41 = increment * Equations[1](values[0] + increment, values[1] + k31, values[2] + k31);
                    values[0] += increment;
                    values[1] += (k10 + 2 * k20 + 2 * k30 + k40) / 6;
                    values[2] += (k11 + 2 * k21 + 2 * k31 + k41) / 6;

                    if (a_isIterationDetailsAsked && index <= IterationNumber)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            ComputationDetails[index, i] = values[i];
                        }
                        index++;
                    }
                }
                return values;
            }
            return null;

        }
    }
}
