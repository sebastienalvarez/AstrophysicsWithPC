/****************************************************************************************************************************************
 * 
 * Classe TailModel
 * Auteur : S. ALVAREZ
 * Date : 03-05-2020
 * Statut : En Test
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter les données nécessaires pour la modélisation d'une trainée à une position donnée sur l'orbite
 *         de la comète.
 * 
 ****************************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstrophysicsAlgorithms.CometTailModeling
{
    public class TailModel
    {
        // PROPRIETES
        public double TrueAnomalie { get; }
        public Point NucleusPosition { get; }
        public List<Point> SyndamA { get; }
        public List<Point> SyndamB { get; }

        // CONSTRUCTEUR
        public TailModel(double a_trueAnomaly, Point a_nucleusPosition, List<Point> a_syndamA, List<Point> a_syndamB)
        {
            TrueAnomalie = a_trueAnomaly;
            NucleusPosition = a_nucleusPosition;
            SyndamA = a_syndamA;
            SyndamB = a_syndamB;
        }

        // METHODES
        // Pas de méthodes

    }
}
