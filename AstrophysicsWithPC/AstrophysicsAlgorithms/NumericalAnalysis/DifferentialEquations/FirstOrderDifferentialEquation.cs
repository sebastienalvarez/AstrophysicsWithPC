/****************************************************************************************************************************************
 * 
 * Classe FirstOrderDifferentialEquation
 * Auteur : S. ALVAREZ
 * Date : 28-04-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe abstraite permettant de calculer la solution d'un équation différentielle du 1er ordre.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Text;

namespace AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations
{
    public abstract class FirstOrderDifferentialEquation : IFirstOrderDifferentialEquation
    {
        // PROPRIETES
        /// <summary>
        /// Equation différentielle du 1er ordre : y' = f(x,y)
        /// </summary>
        public Func<double, double, double> Equation { get; }

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
        /// x0 du point de départ
        /// </summary>
        protected double? xStartingPoint;

        /// <summary>
        /// y0 du point de départ
        /// </summary>
        protected double? yStartingPoint;
        
        private bool isComputeStepSet;
        private bool isIterationNumberSet;

        // CONSTRUCTEUR
        /// <summary>
        /// Création d'une instance avec un delegate représentant l'équation différentielle
        /// </summary>
        /// <param name="a_equation">Delegate représentant l'équation différentielle</param>
        public FirstOrderDifferentialEquation(Func<double, double, double> a_equation)
        {
            Equation = a_equation;
        }

        // METHODES
        /// <summary>
        /// Calcule la valeur de la fonction y pour la valeur de x spécifiée
        /// </summary>
        /// <param name="a_x">Valeur de x pour laquelle la fonction doit être calculée</param>
        /// <param name="a_isIterationDetailsAsked">Flag pour spécifier la sortie des itérations des calculs dans la propriété ComputationDetails</param>
        /// <returns>Valeur de la fonction y ou null si le calcul n'est pas possible avec les paramètres définis</returns>
        public abstract double? ComputeForGivenX(double a_x, bool a_isIterationDetailsAsked = false);

        /// <summary>
        /// Calcul la faisabilité du calcul en fonction des paramètres définis
        /// </summary>
        /// <param name="a_x">Valeur de x pour laquelle la fonction doit être calculée</param>
        /// <returns>Flag indiquant la faisabilité du calcul</returns>
        protected bool CheckIfComputationPossible(double a_x)
        {
            // Vérification de la définition du point de départ
            if (!xStartingPoint.HasValue && xStartingPoint > a_x || !yStartingPoint.HasValue)
            {
                return false;
            }

            // Calcul ComputeStep et IterationNumber
            ComputeIterationNumberAndComputeStep(a_x);

            // Vérification que le nombre d'itération est supérieur à 1
            if(IterationNumber <= 1)
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
                IterationNumber = (uint)((a_x - xStartingPoint.Value) / ComputeStep);
            }
            // Cas où le nombre d'itération a été défini
            else if (isIterationNumberSet)
            {
                ComputeStep = (a_x - xStartingPoint.Value) / IterationNumber;
            }
            // Cas où ni le nombre d'itération ni le pas de calcul a été défini
            else
            {
                uint iterationNumber = (uint)((a_x - xStartingPoint.Value) / Utilities.DefaultComputeStep);
                double computeStep = (a_x - xStartingPoint.Value) / Utilities.DefaultIterationNumber;
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
                    IterationNumber = (uint)((a_x - xStartingPoint.Value) / ComputeStep);
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
        /// Définit le point (x0, y0) de départ pour le calcul
        /// </summary>
        /// <param name="a_x">x0</param>
        /// <param name="a_y">y0</param>
        public void SetStartingPoint(double a_x, double a_y)
        {
            xStartingPoint = a_x;
            yStartingPoint = a_y;
        }

        /// <summary>
        /// Génère un string formatés avec les résultats contenus dans la propriété ComputationDetails
        /// </summary>
        /// <returns>String formatés avec les résultats contenus dans la propriété ComputationDetails</returns>
        public string BuildFormatedComputationDetailsForDisplay()
        {
            if(ComputationDetails != null && ComputationDetails.Length > 0)
            {
                StringBuilder details = new StringBuilder();
                details.AppendLine($"{"x".PadRight(14)}{"y".PadRight(14)}");
                for (int i = 0; i < ComputationDetails.GetLength(0); i++)
                {
                    details.AppendLine($"{ComputationDetails[i, 0].ToString("f8").PadRight(14)}{ComputationDetails[i, 1].ToString("f8").PadRight(14)}");
                }
                return details.ToString();
            }
            return null;
        }

    }
}
