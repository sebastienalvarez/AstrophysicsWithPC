/****************************************************************************************************************************************
 * 
 * Interface IFirstOrderCoupledDifferentialEquations
 * Auteur : S. ALVAREZ
 * Date : 22-09-2022
 * Statut : En Cours
 * Version : 1
 * Revisions : 1 - 22-09-2022 : 1ère version
 * 
 * Objet : Interface pour les différentes méthodes de calcul numérique pour un système d'équations différentielles couplées du 1er ordre.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;

namespace AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations
{
    public interface IFirstOrderCoupledDifferentialEquations
    {
        // POPRIETES
        /// <summary>
        /// Collection d'équation différentielles du 1er ordre : y' = f(x,y)
        /// </summary>
        List<Func<double>> Equations { get; }

        /// <summary>
        /// Collection des conditions initiales pour chacune des équations
        /// </summary>
        List<double> InitialConditions { get; }

        /// <summary>
        /// Collection de références vers les variables utilisées dans le calcul des équations
        /// </summary>
        List<double> VariablesInEquations { get; set; }

        /// <summary>
        /// Détails des itérations du du calcul
        /// </summary>
        List<double[]> ComputationDetails { get; }

        /// <summary>
        /// Nombre d'équations différentielles couplées
        /// </summary>
        uint EquationNumber { get; }

        // METHODES
        /// <summary>
        ///  Calcule le système couplé d'équations avec le pas et le nombre d'itérations en argument
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        /// <param name="a_iterationNumber">Nombre d'itérations</param>
        void Compute(double a_computeStep, uint a_iterationNumber);

        /// <summary>
        /// Calcule le système couplé d'équations au prochain pas de calcul,
        /// cette méthode permet de faire varier le pas de calcul dynamiquement.
        /// La méthode renvoit les valeurs des équations pour le contrôle du pas de calcul à utiliser 
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        /// <returns>Valeurs des équations pour le pas de calcul</returns>
        double[] ComputeForNextStep(double a_computeStep);

        /// <summary>
        /// Génère un string formatés avec les résultats contenus dans la propriété ComputationDetails
        /// </summary>
        /// <returns>String formatés avec les résultats contenus dans la propriété ComputationDetails</returns>
        string BuildFormatedComputationDetailsForDisplay();

    }
}
