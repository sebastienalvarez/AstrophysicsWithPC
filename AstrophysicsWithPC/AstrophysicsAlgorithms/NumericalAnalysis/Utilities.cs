/****************************************************************************************************************************************
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
    }
}
