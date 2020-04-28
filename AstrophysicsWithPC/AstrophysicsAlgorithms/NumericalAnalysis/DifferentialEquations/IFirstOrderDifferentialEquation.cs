/****************************************************************************************************************************************
 * 
 * Interface IFirstOrderDifferentialEquation
 * Auteur : S. ALVAREZ
 * Date : 28-04-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Interface pour les différentes méthode de calcul numérique des équations différentielles du 1er ordre.
 * 
 ****************************************************************************************************************************************/

using System;

namespace AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations
{
    public interface IFirstOrderDifferentialEquation
    {
        // POPRIETES
        /// <summary>
        /// Equation différentielle du 1er ordre : y' = f(x,y)
        /// </summary>
        Func<double, double, double> Equation { get; }

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
        /// Définit le point (x0, y0) de départ pour le calcul
        /// </summary>
        /// <param name="a_x">x0</param>
        /// <param name="a_y">y0</param>
        void SetStartingPoint(double a_x, double a_y);

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
        /// Calcule la valeur de la fonction y pour la valeur de x spécifiée
        /// </summary>
        /// <param name="a_x">Valeur de x pour laquelle la fonction doit être calculée</param>
        /// <param name="a_isIterationDetailsAsked">Flag pour spécifier la sortie des itérations des calculs dans la propriété ComputationDetails</param>
        /// <returns>Valeur de la fonction y ou null si le calcul n'est pas possible avec les paramètres définis</returns>
        double? ComputeForGivenX(double a_x, bool a_isIterationDetailsAsked = false);

        /// <summary>
        /// Génère un string formatés avec les résultats contenus dans la propriété ComputationDetails
        /// </summary>
        /// <returns>String formatés avec les résultats contenus dans la propriété ComputationDetails</returns>
        string BuildFormatedComputationDetailsForDisplay();
    }
}
