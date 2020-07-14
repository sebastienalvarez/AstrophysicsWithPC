/****************************************************************************************************************************************
 * 
 * Classe Point
 * Auteur : S. ALVAREZ
 * Date : 03-05-2020
 * Statut : En Test
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter un point cartésien (x,y).
 * 
 ****************************************************************************************************************************************/

namespace AstrophysicsAlgorithms.CometTailModeling
{
    public class Point
    {
        // PROPRIETES
        /// <summary>
        /// Coordonnée x
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Coordonnée y
        /// </summary>
        public double Y { get; }

        // CONSTRUCTEUR
        /// <summary>
        /// Création d'une instance
        /// </summary>
        /// <param name="a_x">Coordonnée x</param>
        /// <param name="a_y">Coordonnée y</param>
        public Point(double a_x, double a_y)
        {
            X = a_x;
            Y = a_y;
        }

        // METHODES
        // Pas de méthodes

    }
}
