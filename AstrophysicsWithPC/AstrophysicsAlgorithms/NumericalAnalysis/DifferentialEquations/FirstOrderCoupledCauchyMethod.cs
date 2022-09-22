/****************************************************************************************************************************************
 * 
 * Classe FirstOrderCoupledCauchyMethod
 * Auteur : S. ALVAREZ
 * Date : 22-09-2022
 * Statut : En Cours
 * Version : 1
 * Revisions : 1 - 22-09-2022 : 1ère version
 * 
 * Objet : Classe permettant de calculer la solution d'un système couplé d'équations différentielles du 1er ordre par la méthode
 *         de Cauchy.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;

namespace AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations
{
    public class FirstOrderCoupledCauchyMethod : FirstOrderCoupledDifferentialEquations
    {
        // CONSTRUCTEUR
        /// <summary>
        /// Crée une instance
        /// </summary>
        /// <param name="a_equations">Collection d'équation différentielles du 1er ordre : y' = f(x,y)</param>
        /// <param name="a_initialConditions">Collection des conditions initiales pour chacune des équations</param>
        /// <param name="a_variablesInEquations">Collection de références vers les variables utilisées dans le calcul des équations</param>

        public FirstOrderCoupledCauchyMethod(List<Func<double>> a_equations,
                                             List<double> a_initialConditions,
                                             List<double> a_variablesInEquations) : base(a_equations, a_initialConditions, a_variablesInEquations)
        {
        }

        /// <summary>
        ///  Calcule le système couplé d'équations avec le pas et le nombre d'itérations en argument
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        /// <param name="a_iterationNumber">Nombre d'itérations</param>
        public override void Compute(double a_computeStep, uint a_iterationNumber)
        {
            if (isValid)
            {
                if(a_computeStep > 0 && a_iterationNumber > 0)
                {
                    double t = 0;
                    double[] values = new double[EquationNumber + 1];
                    // Utilisation des conditions initiales comme valeurs de départ
                    values[0] = t;
                    for(int i = 0; i < EquationNumber; i++)
                    {
                        values[i + 1] = InitialConditions[i];
                    }
                    double[] intermediaryValues = new double[EquationNumber];
                    ComputationDetails = new List<double[]>();
                    ComputationDetails.Add((double[])(values.Clone()));

                    for (int i = 0; i < a_iterationNumber; i++)
                    {
                        t += a_computeStep;
                        values[0] = t;

                        for (int n = 0; n < EquationNumber; n++)
                        {
                            intermediaryValues[n] = values[n + 1] + 0.5 * a_computeStep * Equations[n].Invoke();
                        }

                        for (int n = 0; n < EquationNumber; n++)
                        {
                            VariablesInEquations[n] = intermediaryValues[n];
                        }

                        for (int n = 0; n < EquationNumber; n++)
                        {
                            values[n + 1] = values[n + 1] + a_computeStep * Equations[n].Invoke();
                        }

                        for (int n = 0; n < EquationNumber; n++)
                        {
                            VariablesInEquations[n] = values[n + 1];
                        }
                        ComputationDetails.Add((double[])(values.Clone()));
                    }
                }
                else
                {
                    throw new ArgumentException("Arguments incorrects : ils doivent être supérieurs à 0");
                }
            }
        }

        /// <summary>
        /// Calcule le système couplé d'équations au prochain pas de calcul,
        /// cette méthode permet de faire varier le pas de calcul dynamiquement.
        /// La méthode renvoit les valeurs des équations pour le contrôle du pas de calcul à utiliser 
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        /// <returns>Valeurs des équations pour le pas de calcul</returns>
        public override double[] ComputeForNextStep(double a_computeStep)
        {
            double[] values = null;
            if (isValid)
            {
                if (a_computeStep > 0)
                {
                    double t = 0;
                    values = new double[EquationNumber + 1];

                    // Initialisation des valeurs initiales
                    if (ComputationDetails != null && ComputationDetails.Count > 0)
                    {
                        // Récupération des dernières valeurs calculées
                        t = ComputationDetails[ComputationDetails.Count - 1][0];
                        values[0] = t;
                        for (int n = 0; n < EquationNumber; n++)
                        {
                            values[n + 1] = ComputationDetails[ComputationDetails.Count - 1][n + 1];
                            VariablesInEquations[n] = values[n + 1];
                        }
                    }
                    else
                    {
                        ComputationDetails = new List<double[]>();
                        // Utilisation des conditions initiales comme valeurs de départ
                        values[0] = t;
                        for (int i = 0; i < EquationNumber; i++)
                        {
                            values[i + 1] = InitialConditions[i];
                        }
                        ComputationDetails.Add((double[])(values.Clone()));
                    }

                    // Calcul du pas suivant
                    double[] intermediaryValues = new double[EquationNumber];
                    t += a_computeStep;
                    values[0] = t;

                    for (int n = 0; n < EquationNumber; n++)
                    {
                        intermediaryValues[n] = values[n + 1] + 0.5 * a_computeStep * Equations[n].Invoke();
                    }

                    for (int n = 0; n < EquationNumber; n++)
                    {
                        VariablesInEquations[n] = intermediaryValues[n];
                    }

                    for (int n = 0; n < EquationNumber; n++)
                    {
                        values[n + 1] = values[n + 1] + a_computeStep * Equations[n].Invoke();
                    }

                    for (int n = 0; n < EquationNumber; n++)
                    {
                        VariablesInEquations[n] = values[n + 1];
                    }
                    ComputationDetails.Add((double[])(values.Clone()));
                }
                else
                {
                    throw new ArgumentException("Argument incorrect : il doit être supérieur à 0");
                }
            }
            return values;
        }

    }
}
