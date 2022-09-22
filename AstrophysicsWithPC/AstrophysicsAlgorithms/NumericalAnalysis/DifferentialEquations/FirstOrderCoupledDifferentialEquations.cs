/****************************************************************************************************************************************
 * 
 * Classe FirstOrderCoupledDifferentialEquations
 * Auteur : S. ALVAREZ
 * Date : 22-09-2022
 * Statut : En Cours
 * Version : 2
 * Revisions : 1 - 22-09-2022 : 1ère version
 * 
 * Objet : Classe abstraite permettant de calculer la solution d'un système couplé d'équations différentielles du 1er ordre.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations
{
    public abstract class FirstOrderCoupledDifferentialEquations : IFirstOrderCoupledDifferentialEquations
    {
        // PROPRIETES
        /// <summary>
        /// Collection d'équation différentielles du 1er ordre : y' = f(x,y)
        /// </summary>
        public List<Func<double>> Equations { get; }

        /// <summary>
        /// Collection des conditions initiales pour chacune des équations
        /// </summary>
        public List<double> InitialConditions { get; }

        /// <summary>
        /// Collection de références vers les variables utilisées dans le calcul des équations
        /// </summary>
        public List<double> VariablesInEquations { get; set; }

        /// <summary>
        /// Nombre d'équations différentielles couplées
        /// </summary>
        public uint EquationNumber
        {
            get
            {
                if (Equations != null && Equations.Count > 1)
                {
                    return (uint)Equations.Count;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Détails des itérations du du calcul
        /// </summary>
        public List<double[]> ComputationDetails { get; protected set; }

        /// <summary>
        /// Pas de calcul
        /// </summary>
        protected double ComputeStep { get; set; }

        /// <summary>
        /// Nombre d'itérations
        /// </summary>
        protected uint IterationNumber { get; set; }

        /// <summary>
        /// Indique si le système est correctement paramétré pour pouvoir réaliser un calcul
        /// </summary>
        protected bool isValid = false;

        // CONSTRUCTEUR
        /// <summary>
        /// Crée une instance
        /// </summary>
        /// <param name="a_equations">Collection d'équation différentielles du 1er ordre : y' = f(x,y)</param>
        /// <param name="a_initialConditions">Collection des conditions initiales pour chacune des équations</param>
        /// <param name="a_variablesInEquations">Collection de références vers les variables utilisées dans le calcul des équations</param>
        public FirstOrderCoupledDifferentialEquations(List<Func<double>> a_equations, 
                                                      List<double> a_initialConditions,
                                                      List<double> a_variablesInEquations)
        {
            if(a_equations != null && a_equations.Count > 1 && 
               a_initialConditions != null && a_initialConditions.Count > 1 &&
               a_variablesInEquations != null && a_variablesInEquations.Count > 1 &&
               a_equations.Count == a_initialConditions.Count &&
               a_equations.Count == a_variablesInEquations.Count &&
               a_initialConditions.Count == a_variablesInEquations.Count)
            {
                Equations = a_equations;
                InitialConditions = a_initialConditions;
                VariablesInEquations = a_variablesInEquations;
                isValid = true;
            }
            else
            {
                throw new ArgumentException("Arguments incorrects (les collections doivent avoir le même nombre d'éléments et être de taille > 1");
            }
        }

        // METHODES
        /// <summary>
        ///  Calcule le système couplé d'équations avec le pas et le nombre d'itérations en argument
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        /// <param name="a_iterationNumber">Nombre d'itérations</param>
        public abstract void Compute(double a_computeStep, uint a_iterationNumber);

        /// <summary>
        /// Calcule le système couplé d'équations au prochain pas de calcul,
        /// cette méthode permet de faire varier le pas de calcul dynamiquement.
        /// La méthode renvoit les valeurs des équations pour le contrôle du pas de calcul à utiliser 
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        /// <returns>Valeurs des équations pour le pas de calcul</returns>
        public abstract double[] ComputeForNextStep(double a_computeStep);

        /// <summary>
        /// Génère un string formatés avec les résultats contenus dans la propriété ComputationDetails
        /// </summary>
        /// <returns>String formatés avec les résultats contenus dans la propriété ComputationDetails</returns>
        public string BuildFormatedComputationDetailsForDisplay()
        {
            if (ComputationDetails != null && ComputationDetails.Count > 0)
            {
                StringBuilder details = new StringBuilder();
                for (int i = 0; i < ComputationDetails.Count; i++)
                {
                    for (int j = 0; j < ComputationDetails[i].Length; j++)
                    {
                        details.Append($"{ComputationDetails[i][j].ToString("f10").PadRight(14)}");
                    }
                    details.AppendLine();
                }
                return details.ToString();
            }
            return null;
        }

    }
}
