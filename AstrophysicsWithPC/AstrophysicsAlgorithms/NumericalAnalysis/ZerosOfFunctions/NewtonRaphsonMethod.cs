/****************************************************************************************************************************************
 * 
 * Classe NewtonRaphsonMethod
 * Auteur : S. ALVAREZ
 * Date : 01-05-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de calculer le zéro d'une fonction par la méthode de Newton-Raphson.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace AstrophysicsAlgorithms.NumericalAnalysis.ZerosOfFunctions
{
    public class NewtonRaphsonMethod
    {
        // PROPRIETES
        /// <summary>
        /// Equation f(x)
        /// </summary>
        public Func<double, double> Equation { get; }

        /// <summary>
        /// Derivée f'(x)
        /// </summary>
        public Func<double, double> DerivedEquation { get; }

        /// <summary>
        /// Précision
        /// </summary>
        public double Precision { get; private set; }

        /// <summary>
        /// Nombre d'itérations maximum
        /// </summary>
        public uint MaximumIterationNumber { get; private set; }

        /// <summary>
        /// Nombre d'itérations réalisées lors du calcul
        /// </summary>
        public uint IterationNumberReached  { get; private set; }

        /// <summary>
        /// Détails des itérations du calcul
        /// </summary>
        public Dictionary<uint, double> ComputationDetails { get; protected set; }

        /// <summary>
        /// Point de départ
        /// </summary>
        private double? startingPoint;

        // CONSTRUCTEUR
        /// <summary>
        /// Création d'une instance avec un delegate représentant l'équation et une delegate représentant la dérivée
        /// </summary>
        /// <param name="a_equation">Delegate représentant l'équation</param>
        /// <param name="a_derivedEquation">Delegate représentant la dérivée</param>
        public NewtonRaphsonMethod(Func<double, double> a_equation, Func<double, double> a_derivedEquation)
        {
            Equation = a_equation;
            DerivedEquation = a_derivedEquation;
            Precision = Utilities.DefaultPrecision;
            MaximumIterationNumber = Utilities.DefaultMaximumIterationNumber;
        }

        // METHODES
        /// <summary>
        /// Définit le point (x0) de départ pour le calcul
        /// </summary>
        /// <param name="a_startingPoint">x0</param>
        public void SetStartingPoint(double a_startingPoint)
        {
            startingPoint = a_startingPoint;
        }

        /// <summary>
        /// définit la précision à atteindre sur le calcul, le nombre d'itérations maximum est alors le nombre d'itérations maximum par défaut
        /// </summary>
        /// <param name="a_precision">Pas de calcul</param>
        public void SetPrecision(double a_precision)
        {
            if (a_precision > 0)
            {
                Precision = a_precision;
                MaximumIterationNumber = Utilities.DefaultMaximumIterationNumber;
            }
        }

        /// <summary>
        /// Définit le nombre d'itérations maximum pour le calcul, la précision est alors la précision par défaut
        /// </summary>
        /// <param name="a_maximumIterationNumber">Nombre maximum d'itérations</param>
        public void SetMaximumIterationNumber(uint a_maximumIterationNumber)
        {
            if (a_maximumIterationNumber > 1)
            {
                MaximumIterationNumber = a_maximumIterationNumber;
                Precision = Utilities.DefaultPrecision;
            }
        }

        /// <summary>
        /// Calcul du zéro de la fonction
        /// </summary>
        /// <returns>Zéro de la fonction si le calcul est possible</returns>
        public double? ComputeForZero(bool a_isIterationDetailsAsked = false)
        {
            if (startingPoint.HasValue)
            {
                return ComputeForZero(startingPoint.Value, a_isIterationDetailsAsked);
            }
            return null;
        }

        /// <summary>
        /// Calcul du zéro de la fonction
        /// </summary>
        /// <param name="a_startingPoint">Point de départ</param>
        /// <returns>Zéro de la fonction si le calcul est possible</returns>
        public double? ComputeForZero(double a_startingPoint, bool a_isIterationDetailsAsked = false)
        {
            IterationNumberReached = 0;
            uint iterationNumber = 0;
            bool isPrecisionReached = false;
            startingPoint = a_startingPoint;
            double zero = startingPoint.Value;
            double previousZero = startingPoint.Value;
            if (a_isIterationDetailsAsked)
            {
                ComputationDetails = new Dictionary<uint, double>();
            }
            else
            {
                ComputationDetails = null;
            }
            try
            {
                while (!isPrecisionReached && iterationNumber < MaximumIterationNumber)
                {
                    previousZero = zero;
                    zero = zero - Equation(zero) / DerivedEquation(zero);
                    iterationNumber++;
                    if (a_isIterationDetailsAsked)
                    {
                        ComputationDetails.Add(iterationNumber, zero);
                    }
                    isPrecisionReached = (Math.Abs(zero - previousZero)) <= Precision;
                }
                IterationNumberReached = iterationNumber;
                return zero;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Génère un string formatés avec les résultats contenus dans la propriété ComputationDetails
        /// </summary>
        /// <returns>String formatés avec les résultats contenus dans la propriété ComputationDetails</returns>
        public string BuildFormatedComputationDetailsForDisplay()
        {
            if (ComputationDetails != null && ComputationDetails.Count > 0)
            {
                StringBuilder details = new StringBuilder();
                details.AppendLine($"{"Iteration".PadRight(10)}{"y".PadRight(14)}");
                int numberOfDigits = ComputationDetails.Count.ToString().Length;
                string formatToApply = $"d{numberOfDigits.ToString()}";
                for (uint i = 0; i < ComputationDetails.Count; i++)
                {
                    details.AppendLine($"{(i+1).ToString(formatToApply).PadRight(10)}{ComputationDetails[i + 1].ToString("f8").PadRight(14)}");
                }
                return details.ToString();
            }
            return null;
        }

    }
}
