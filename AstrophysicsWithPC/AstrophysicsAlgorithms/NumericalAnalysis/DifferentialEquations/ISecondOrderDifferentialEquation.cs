/****************************************************************************************************************************************
 * 
 * Interface ISecondOrderDifferentialEquation
 * Auteur : S. ALVAREZ
 * Date : 29-04-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Interface pour les différentes méthodes de calcul numérique des équations différentielles du 2ème ordre. Une équation
 *         différentielle du 2èmeordre se décompose en 2 équations différentielles du 1er ordre chainées.
 * 
 ****************************************************************************************************************************************/

using System;

namespace AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations
{
    public interface ISecondOrderDifferentialEquation
    {
        // POPRIETES
        /// <summary>
        /// Equations différentielles du 1er ordre : y' = f(x,y,z) et z' = g(x,y,z)
        /// </summary>
        Func<double, double, double, double>[] Equations { get; }

        /// <summary>
        /// Pas de calcul
        /// </summary>
        double ComputeStep { get; }

        /// <summary>
        /// Nombre d'itérations
        /// </summary>
        uint IterationNumber { get; }

        /// <summary>
        /// Détails des itérations du du calcul
        /// </summary>
        double[,] ComputationDetails { get; }

        // METHODES
        /// <summary>
        /// Définit les points (x0, y0) et (x0, z0) de départ pour le calcul (conditions initiales)
        /// </summary>
        /// <param name="a_startingPoints">Points de départ pour le calcul</param>
        void SetStartingPoints(double[] a_startingPoints);

        /// <summary>
        /// définit le pas de calcul à utiliser, le nombre d'itérations utilisé est issu du pas de calcul défini
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        void SetComputeStep(double a_computeStep);

        /// <summary>
        /// Définit le nombre d'itérations à utiliser pour le calcul, le pas de calcul utilisé est issu du nombre d'itérations issu
        /// </summary>
        /// <param name="a_iterationNumber">Nombre d'itérations</param>
        void SetIterationNumber(uint a_iterationNumber);

        /// <summary>
        /// Calcule la valeur des fonctions y et z pour la valeur de x spécifiée
        /// </summary>
        /// <param name="a_x">Valeur de x pour laquelle les fonctions doivent être calculées</param>
        /// <param name="a_isIterationDetailsAsked">Flag pour spécifier la sortie des itérations des calculs dans la propriété ComputationDetails</param>
        /// <returns>Valeurs des fonctions y et z ou null si le calcul n'est pas possible avec les paramètres définis</returns>
        double[] ComputeForGivenX(double a_x, bool a_isIterationDetailsAsked = false);

        /// <summary>
        /// Génère un string formatés avec les résultats contenus dans la propriété ComputationDetails
        /// </summary>
        /// <returns>String formatés avec les résultats contenus dans la propriété ComputationDetails</returns>
        string BuildFormatedComputationDetailsForDisplay();
    }
}
