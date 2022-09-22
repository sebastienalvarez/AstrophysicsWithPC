/****************************************************************************************************************************************
 * 
 * Classe RestrictedThreeBodyProblem
 * Auteur : S. ALVAREZ
 * Date : 22-09-2022
 * Statut : En Cours
 * Version : 1
 * Revisions : 1 - 22-09-2022 : 1ère version
 * 
 * Objet : Classe permettant de représenter un problème restreint à 3 corps.
 * 
 ****************************************************************************************************************************************/

using AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations;
using System;
using System.Collections.Generic;

namespace AstrophysicsAlgorithms.RestrictedThreeBodyProblem
{
    public class RestrictedThreeBodyProblem
    {
        // PROPRIETES
        /// <summary>
        /// Paramètre de masse mu
        /// </summary>
        public double MassParameter { get; }

        /// <summary>
        /// Condition initiale de position x
        /// </summary>
        public double InitialConditionX { get; }

        /// <summary>
        /// Condition initiale de position y
        /// </summary>
        public double InitialConditionY { get; }

        /// <summary>
        /// Condition initiale de vitesse u
        /// </summary>
        public double InitialConditionU { get; }

        /// <summary>
        /// Condition initiale de vitesse v
        /// </summary>
        public double InitialConditionV { get; }

        /// <summary>
        /// Collection des équations différentielles
        /// </summary>
        private List<Func<double>> equations;

        /// <summary>
        /// Collection des conditions initiales
        /// </summary>
        private List<double> initialConditions;

        /// <summary>
        /// Collection des variables utilisées dans les équations
        /// </summary>
        private List<double> variablesInEquations;

        /// <summary>
        /// Système couplé d'équations différentielles du 1er ordre
        /// </summary>
        private IFirstOrderCoupledDifferentialEquations system;

        public List<double[]> Results;

        // CONSTRUCTEUR
        /// <summary>
        /// Créé une instance
        /// </summary>
        /// <param name="a_massParameter">Paramètre de masse mu</param>
        /// <param name="a_initialConditionX">Condition initiale de position x</param>
        /// <param name="a_initialConditionY">Condition initiale de position y</param>
        /// <param name="a_initialConditionU">Condition initiale de vitesse u</param>
        /// <param name="a_initialConditionV">Condition initiale de vitesse v</param>
        public RestrictedThreeBodyProblem(double a_massParameter,
                                          double a_initialConditionX,
                                          double a_initialConditionY,
                                          double a_initialConditionU,
                                          double a_initialConditionV)
        {
            MassParameter = a_massParameter;
            InitialConditionX = a_initialConditionX;
            InitialConditionY = a_initialConditionY;
            InitialConditionU = a_initialConditionU;
            InitialConditionV = a_initialConditionV;

            equations = new List<Func<double>>(4);
            initialConditions = new List<double>(4) { InitialConditionX, InitialConditionY, InitialConditionU, InitialConditionV };
            variablesInEquations = new List<double>(4) { InitialConditionX, InitialConditionY, InitialConditionU, InitialConditionV };
            equations.Add(() => variablesInEquations[2]);
            equations.Add(() => variablesInEquations[3]);
            equations.Add(() => -((1 - MassParameter) * (variablesInEquations[0] - MassParameter)) / (ComputeR1() * ComputeR1() * ComputeR1()) - (MassParameter * (variablesInEquations[0] + 1 - MassParameter)) / (ComputeR2() * ComputeR2() * ComputeR2()) + (variablesInEquations[0] + 2 * variablesInEquations[3]));
            equations.Add(() => -((1 - MassParameter) * variablesInEquations[1]) / (ComputeR1() * ComputeR1() * ComputeR1()) - (MassParameter * variablesInEquations[1]) / (ComputeR2() * ComputeR2() * ComputeR2()) + (variablesInEquations[1] - 2 * variablesInEquations[2]));
            system = new FirstOrderCoupledCauchyMethod(equations, initialConditions, variablesInEquations);
            Results = new List<double[]>();
        }

        // METHODES
        /// <summary>
        /// Calcule l'orbite du 3ème corps selon le pas de calcul et le nombre d'itérations en argument
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        /// <param name="a_iterationNumber">Nombre d'itérations</param>
        public void Compute(double a_computeStep, uint a_iterationNumber)
        {
            system.Compute(a_computeStep, a_iterationNumber);
            Results.AddRange(system.ComputationDetails);
        }

        /// <summary>
        /// Calcule l'orbite du 3ème corps pour le pas de calcul suivant
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        /// <returns>Résultat du calcul pour le pas de calcul</returns>
        public double[] ComputeNextStep(double a_computeStep)
        {
            double[] results = system.ComputeForNextStep(a_computeStep);
            return results;
        }

        /// <summary>
        /// Affiche les résultats numériques sur la console
        /// </summary>
        public void DisplayResultsOnConsole()
        {
            Console.WriteLine(system.BuildFormatedComputationDetailsForDisplay());
        }

        /// <summary>
        /// Calcule la distance R1
        /// </summary>
        /// <returns>Distance R1</returns>
        private double ComputeR1()
        {
            return Math.Sqrt((variablesInEquations[0] - MassParameter) * (variablesInEquations[0] - MassParameter) + variablesInEquations[1] * variablesInEquations[1]);
        }

        /// <summary>
        /// Calcule la distance R2
        /// </summary>
        /// <returns>Distance R2</returns>
        private double ComputeR2()
        {
            return Math.Sqrt((variablesInEquations[0] + 1 - MassParameter) * (variablesInEquations[0] + 1 - MassParameter) + variablesInEquations[1] * variablesInEquations[1]);
        }

    }
}
