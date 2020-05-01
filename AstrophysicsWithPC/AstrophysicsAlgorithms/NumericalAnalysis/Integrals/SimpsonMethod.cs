/****************************************************************************************************************************************
 * 
 * Classe SimpsonMethod
 * Auteur : S. ALVAREZ
 * Date : 01-05-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de calculer une intégrale par la méthode de Simpson.
 * 
 ****************************************************************************************************************************************/

using System;

namespace AstrophysicsAlgorithms.NumericalAnalysis.Integrals
{
    public class SimpsonMethod
    {
        // PROPRIETES
        /// <summary>
        /// Equation f(x) à intégrer
        /// </summary>
        public Func<double, double> Equation { get; }

        /// <summary>
        /// Nombre d'intervalles pour le calcul
        /// </summary>
        public uint IntervalNumber { get; private set; }

        // CONSTRUCTEUR
        /// <summary>
        /// Création d'une instance avec un delegate représentant l'équation à intégrer
        /// </summary>
        /// <param name="a_equation">Equation à intégrer</param>
        public SimpsonMethod(Func<double, double> a_equation)
        {
            Equation = a_equation;
            IntervalNumber = Utilities.DefaultIntervalNumber;
        }

        // METHODES
        /// <summary>
        /// Définit le nombre d'itérations pour le calcul
        /// </summary>
        /// <param name="a_intervalNumber"></param>
        public void SetIterationNumber(uint a_intervalNumber)
        {
            if(a_intervalNumber > 0)
            {
                IntervalNumber = a_intervalNumber % 2 == 0 ? a_intervalNumber : a_intervalNumber - 1;
            }
        }
        
        /// <summary>
        /// Intègre la fonction entre les bornes a et b
        /// </summary>
        /// <param name="a">Borne inférieure</param>
        /// <param name="b">Borne supérieure</param>
        /// <returns>Intégrale entre les bornes a et b si le calcul est possible</returns>
        public double? ComputeIntegral(double a, double b)
        {
            if(a != b)
            {
                double min = Math.Min(a, b);
                double max = Math.Max(a, b);
                double h = (max - min) / IntervalNumber;
                double result = 0;
                double xlow = min;
                double flow = Equation(min);
                double xmid, fmid, xup, fup;
                for(int i = 1; i <= IntervalNumber / 2; i++)
                {
                    xmid = xlow + h;
                    xup = xlow + 2 * h;
                    fmid = Equation(xmid);
                    fup = Equation(xup);
                    result += h * (flow + 4 * fmid + fup) / 3;
                    xlow = xup;
                    flow = fup;
                }
                return result;
            }
            return null;
        }

    }
}
