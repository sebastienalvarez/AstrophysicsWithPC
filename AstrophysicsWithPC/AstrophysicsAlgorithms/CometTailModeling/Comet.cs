/****************************************************************************************************************************************
 * 
 * Classe Comet
 * Auteur : S. ALVAREZ
 * Date : 03-05-2020
 * Statut : En Cours
 * Version : 1
 * Revisions : NA
 * 
 * Objet : Classe permettant de représenter une comète pour la modélisation de sa trainée.
 * 
 ****************************************************************************************************************************************/

using AstrophysicsAlgorithms.NumericalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstrophysicsAlgorithms.CometTailModeling
{
    public class Comet
    {
        // PROPRIETES
        private double ap;

        public double Perihelion
        {
            get { return ap; }
            set { ap = value; }
        }

        private double e;

        public double Eccentricity
        {
            get { return e; }
            set { e = value; }
        }

        private double mu;

        public double ForceParameter
        {
            get { return mu; }
            set { mu = value; }
        }

        private double v;

        public double OutflowVelocity
        {
            get { return v; }
            set { v = value; }
        }

        private double p;
        public double OrbitalPeriod { get; }
        double nu, r, x, y, A1, A2, A3;

        // CONSTRUCTEUR
        public Comet(double a_perihelion, double a_eccentricity, double a_forceParameter, double a_outflowVelocity)
        {
            Perihelion = a_perihelion;
            Eccentricity = a_eccentricity;
            ForceParameter = a_forceParameter;
            OutflowVelocity = a_outflowVelocity;
            p = ap * (1 + e);
            OrbitalPeriod = Math.Pow(p / (1 - e * e), 1.5);
        }

        // METHODES
        public TailModel ComputeSyndynams(double a_trueAnomalyInDegree)
        {
            nu = Utilities.DegreeToRadian(a_trueAnomalyInDegree);
            r = p / (1 + e * Math.Cos(nu));
            x = r * Math.Cos(nu);
            y = r * Math.Sin(nu);
            A1 = Math.Sqrt(2 / mu) * r;
            A2 = 4 * e * r * Math.Sin(nu) / (3 * mu * Math.Sqrt(p));
            A3 = Math.Sqrt(8 * p / mu) / (3 * r);
            return new TailModel(a_trueAnomalyInDegree, new Point(x, y), ComputeSyndam(-Math.PI / 2), ComputeSyndam(Math.PI / 2));
        }


        private List<Point> ComputeSyndam(double ea)
        {
            List<Point> points = new List<Point>();
            for(byte i = 0; i <= 10; i++)
            {
                points.Add(ComputePoint(ea, 0.05 * i)); 
            }
            return points;
        }

        private Point ComputePoint(double ea, double s)
        {
            double t = v * Math.Sin(ea) * (A1 * Math.Sqrt(s) - A2 * s) + A3 * s * Math.Sqrt(s);
            double xp = (s * x + t * y + r * x) / r;
            double yp = (s * y - t * x + r * y) / r;
            return new Point(xp, yp);
        }

    }
}
