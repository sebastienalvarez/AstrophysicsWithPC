/****************************************************************************************************************************************
 * 
 * Classe SecondOrderDifferentialEquation
 * Auteur : S. ALVAREZ
 * Date : 29-04-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe abstraite permettant de calculer la solution d'une équation différentielle du 2ème ordre décomposée en 2 équations
 *         différentielles du 1er ordre chainées.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Text;

namespace AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations
{
    public abstract class SecondOrderDifferentialEquation : ISecondOrderDifferentialEquation
    {
        // PROPRIETES
        /// <summary>
        /// Equations différentielles du 1er ordre : y' = f(x,y,z) et z' = g(x,y,z)
        /// </summary>
        public Func<double, double, double, double>[] Equations { get; }

        /// <summary>
        /// Pas de calcul
        /// </summary>
        public double ComputeStep { get; private set; }

        /// <summary>
        /// Nombre d'itérations
        /// </summary>
        public uint IterationNumber { get; private set; }

        /// <summary>
        /// Détails des itérations du du calcul
        /// </summary>
        public double[,] ComputationDetails { get; protected set; }

        /// <summary>
        /// Points de départ (conditions initiales)
        /// </summary>
        protected double[] startingPoints;

        private bool isComputeStepSet;
        private bool isIterationNumberSet;

        // CONSTRUCTEUR
        /// <summary>
        /// Création d'une instance avec un tableau de 2 delegates représentant les équations différentielles du 1er ordre chainées
        /// </summary>
        /// <param name="a_equations">Tableau de 2 delegates représentant les équations différentielles du 1er ordre chainées</param>
        public SecondOrderDifferentialEquation(Func<double, double, double, double>[] a_equations)
        {
            if(a_equations.Length != 2)
            {
                throw new ArgumentException("Argument 'a_equations' must have a dimension of 2");
            }
            Equations = a_equations;
        }

        // METHODES
        /// <summary>
        /// Calcule la valeur des fonctions y et z pour la valeur de x spécifiée
        /// </summary>
        /// <param name="a_x">Valeur de x pour laquelle les fonctions doivent être calculées</param>
        /// <param name="a_isIterationDetailsAsked">Flag pour spécifier la sortie des itérations des calculs dans la propriété ComputationDetails</param>
        /// <returns>Valeurs des fonctions y et z ou null si le calcul n'est pas possible avec les paramètres définis</returns>
        public abstract double[] ComputeForGivenX(double a_x, bool a_isIterationDetailsAsked = false);

        /// <summary>
        /// Calcul la faisabilité du calcul en fonction des paramètres définis
        /// </summary>
        /// <param name="a_x">Valeur de x pour laquelle la fonction doit être calculée</param>
        /// <returns>Flag indiquant la faisabilité du calcul</returns>
        protected bool CheckIfComputationPossible(double a_x)
        {
            // Vérification de la définition des points de départ
            if (startingPoints == null || startingPoints.Length != 3 || startingPoints[0] > a_x)
            {
                return false;
            }

            // Calcul ComputeStep et IterationNumber
            ComputeIterationNumberAndComputeStep(a_x);

            // Vérification que le nombre d'itération est supérieur à 1
            if (IterationNumber <= 1)
            {
                return false;
            }

            return true;
        }

        // Calcule le pas de calcul et le nombre d'itération en fonction de qui a été défini
        private void ComputeIterationNumberAndComputeStep(double a_x)
        {
            // Cas où le pas de calcul a été défini
            if (isComputeStepSet)
            {
                IterationNumber = (uint)((a_x - startingPoints[0]) / ComputeStep);
            }
            // Cas où le nombre d'itération a été défini
            else if (isIterationNumberSet)
            {
                ComputeStep = (a_x - startingPoints[0]) / IterationNumber;
            }
            // Cas où ni le nombre d'itération ni le pas de calcul a été défini
            else
            {
                uint iterationNumber = (uint)((a_x - startingPoints[0]) / Utilities.DefaultComputeStep);
                double computeStep = (a_x - startingPoints[0]) / Utilities.DefaultIterationNumber;
                if (iterationNumber > Utilities.DefaultIterationNumber)
                {
                    // Le calcul est basé sur le nombre d'itérations par défaut
                    IterationNumber = Utilities.DefaultIterationNumber;
                    ComputeStep = computeStep;
                }
                else
                {
                    // le calcul est basé sur le pas de calcul par défaut
                    ComputeStep = Utilities.DefaultComputeStep;
                    IterationNumber = (uint)((a_x - startingPoints[0]) / ComputeStep);
                }
            }
        }

        /// <summary>
        /// définit le pas de calcul à utiliser, le nombre d'itérations utilisé est issu du pas de calcul défini
        /// </summary>
        /// <param name="a_computeStep">Pas de calcul</param>
        public void SetComputeStep(double a_computeStep)
        {
            if (a_computeStep > 0)
            {
                ComputeStep = a_computeStep;
                isComputeStepSet = true;
                isIterationNumberSet = false;
            }
        }

        /// <summary>
        /// Définit le nombre d'itérations à utiliser pour le calcul, le pas de calcul utilisé est issu du nombre d'itérations issu
        /// </summary>
        /// <param name="a_iterationNumber">Nombre d'itérations</param>
        public void SetIterationNumber(uint a_iterationNumber)
        {
            if (a_iterationNumber > 1)
            {
                IterationNumber = a_iterationNumber;
                isIterationNumberSet = true;
                isComputeStepSet = false;
            }
        }

        /// <summary>
        /// Définit les points (x0, y0) et (x0, z0) de départ pour le calcul (conditions initiales)
        /// </summary>
        /// <param name="a_startingPoints">Points de départ pour le calcul</param>
        public void SetStartingPoints(double[] a_startingPoints)
        {
            if (a_startingPoints.Length == 3)
            {
                startingPoints = a_startingPoints;
            }
        }

        /// <summary>
        /// Génère un string formatés avec les résultats contenus dans la propriété ComputationDetails
        /// </summary>
        /// <returns>String formatés avec les résultats contenus dans la propriété ComputationDetails</returns>
        public string BuildFormatedComputationDetailsForDisplay()
        {
            if (ComputationDetails != null && ComputationDetails.Length > 0)
            {
                StringBuilder details = new StringBuilder();
                details.AppendLine($"{"x".PadRight(14)}{"y".PadRight(14)}{"z".PadRight(14)}");
                for (int i = 0; i < ComputationDetails.GetLength(0); i++)
                {
                    details.AppendLine($"{ComputationDetails[i, 0].ToString("f8").PadRight(14)}{ComputationDetails[i, 1].ToString("f8").PadRight(14)}{ComputationDetails[i, 2].ToString("f8").PadRight(14)}");
                }
                return details.ToString();
            }
            return null;
        }

    }
}
