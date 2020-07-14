/*************************** *************************************************************************************************************
 * 
 * Classe Utilities
 * Auteur : S. ALVAREZ
 * Date : 28-04-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe static avec les constantes et les méthodes helper.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstrophysicsAlgorithms.NumericalAnalysis
{
    public static class Utilities
    {
        // Constantes pour le calcul des équations différentielles
        public static readonly double DefaultComputeStep = 0.0001;
        public static readonly uint DefaultIterationNumber = 10000;

        // Constantes pour le calcul d'un zéro d'une fonction
        public static readonly double DefaultPrecision = 0.0001;
        public static readonly uint DefaultMaximumIterationNumber = 1000;

        // Constante pour le calcul d'une intégrale
        public static readonly uint DefaultIntervalNumber = 1000;

        public static double DegreeToRadian(double a_degree)
        {
            return a_degree * Math.PI / 180.0;
        }

        public static double RadianToDegree(double a_radian)
        {
            return a_radian * 180.0 / Math.PI;
        }
    }
}
