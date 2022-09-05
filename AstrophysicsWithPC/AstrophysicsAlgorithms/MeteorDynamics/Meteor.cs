/****************************************************************************************************************************************
 * 
 * Classe Meteor
 * Auteur : S. ALVAREZ
 * Date : 05-09-2022
 * Statut : En Cours
 * Version : 1
 * Revisions : 1 - 05-09-2022 : 1ère version
 * 
 * Objet : Classe permettant de représenter un météore pour la modélisation de sa dynamique lors de son entrée dans son atmosphère.
 *         La méthode ComputeDynamics() utilise l'algorithme du livre.
 *         La méthode ComputeDynamicsWithDefaultMethod() utilise une méthode de couplage des équations différentielles définie dans
 *         pour la bibliothèque NumericalAnalysis.DifferentialEquations. La précision est moins bonne et nécessite un pas de calcul
 *         plus petit.
 * 
 ****************************************************************************************************************************************/

using AstrophysicsAlgorithms.NumericalAnalysis.DifferentialEquations;
using System;
using System.Collections.Generic;
using System.IO;

namespace AstrophysicsAlgorithms.MeteorDynamics
{
    public class Meteor
    {
        // PROPRIETES
        /// <summary>
        /// Masse initiale en gramme
        /// </summary>
        public double InitialMass { get; }

        /// <summary>
        /// Altitude d'entrée dans l'atmosphère (100 à 160 km)
        /// </summary>
        public double AtmosphereEntryAltitude { get; }

        /// <summary>
        /// Vitesse horizontale en km/s (vitesse totale d'un météore compris entre 11 km/s et 72 km/s)
        /// </summary>
        public double InitialHorizontalSpeed { get; }

        /// <summary>
        /// Vitesse verticale en km/s (vitesse totale d'un météore compris entre 11 km/s et 72 km/s)
        /// </summary>
        public double InitialVerticalSpeed { get; }

        /// <summary>
        /// Paramètre K1 (mettre 1 par défaut)
        /// </summary>
        public double K1Parameter { get; }

        /// <summary>
        /// Paramètre K2 (mettre 1e-11 par défaut)
        /// </summary>
        public double K2Parameter { get; }

        /// <summary>
        /// Paramètre tau (mettre 0,02 par défaut)
        /// </summary>
        public double TauParameter { get; }

        /// <summary>
        /// Collection des résultats (une ligne de résultat indique t, x, y, u, v, m et M)
        /// </summary>
        public List<double[]> Results { get; private set; }

        // CONSTRUCTEUR
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a_mass">Masse initiale en gramme</param>
        /// <param name="a_atmosphereEntryAltitude">Altitude d'entrée dans l'atmosphère (100 à 160 km)</param>
        /// <param name="a_initialHorizontalSpeed">Vitesse horizontale en km/s (vitesse totale d'un météore compris entre 11 km/s et 72 km/s)</param>
        /// <param name="a_initialVerticalSpeed">Vitesse verticale en km/s (vitesse totale d'un météore compris entre 11 km/s et 72 km/s)</param>
        /// <param name="a_k1Parameter">Paramètre K1 (mettre 1 par défaut)</param>
        /// <param name="a_k2Parameter">Paramètre K2 (mettre 1e-11 par défaut)</param>
        /// <param name="a_tauParameter">Paramètre tau (mettre 0,02 par défaut)</param>
        public Meteor(double a_mass,
                      double a_atmosphereEntryAltitude,
                      double a_initialHorizontalSpeed,
                      double a_initialVerticalSpeed,
                      double a_k1Parameter,
                      double a_k2Parameter,
                      double a_tauParameter)
        {
            InitialMass = a_mass;           
            AtmosphereEntryAltitude = a_atmosphereEntryAltitude;
            InitialHorizontalSpeed = a_initialHorizontalSpeed;
            InitialVerticalSpeed = a_initialVerticalSpeed;
            K1Parameter = a_k1Parameter;
            K2Parameter = a_k2Parameter;
            TauParameter = a_tauParameter;
            Results = new List<double[]>();
        }

        // METHODES
        /// <summary>
        /// Calcule la dynamique d'un météore selon l'algorithme du livre
        /// </summary>
        public void ComputeDynamics()
        {
            // Initialisation des variables du calcul
            double t = 0;
            double m = InitialMass;
            double x = 0;
            double y = AtmosphereEntryAltitude * 100000; // Conversion en cm
            double u = InitialHorizontalSpeed * 100000; // Conversion en cm
            double v = -Math.Abs(InitialVerticalSpeed) * 100000; // Conversion en cm/s
            double dt, e, mag;
            double fx, fy, fu, fv, fm;
            double x1, y1, u1, v1, m1;
            double fx1, fy1, fu1, fv1, fm1;

            Results.Add(new double[] { t, x, y, u, v, m, 0 });

            do
            {
                // Ajustement du pas de calcul
                if (m > 0.8 * InitialMass)
                {
                    dt = 0.1;
                }
                else if (m > 0.5 * InitialMass && m <= InitialMass)
                {
                    dt = 0.05;
                }
                else if (m > 0.35 * InitialMass && m <= 0.5 * InitialMass)
                {
                    dt = 0.02;
                }
                else
                {
                    dt = 0.01;
                }
                t += dt;

                ComputeDerivatives(x, y, u, v, m, out fx, out fy, out fu, out fv, out fm);
                x1 = x + dt * fx;
                y1 = y + dt * fy;
                u1 = u + dt * fu;
                v1 = v + dt * fv;
                m1 = m + dt * fm;

                ComputeDerivatives(x1, y1, u1, v1, m1, out fx1, out fy1, out fu1, out fv1, out fm1);
                x = x + 0.5 * dt * (fx + fx1);
                y = y + 0.5 * dt * (fy + fy1);
                u = u + 0.5 * dt * (fu + fu1);
                v = v + 0.5 * dt * (fv + fv1);
                m = m + 0.5 * dt * (fm + fm1);

                e = -0.5 * TauParameter * fm1 * (u1 * u1 + v1 * v1);
                mag = 5 * Math.Log10(y) - 2.5 * Math.Log10(e) - 8.795;

                if (m > 0 && y > 0)
                {
                    Results.Add(new double[] { t, x, y, u, v, m, mag });
                }

            } while (m > 0 && y > 0);
        }

        /// <summary>
        /// Calcule la dynamique d'un météore avec la méthode de couplage par défaut de la bibliothèque
        /// </summary>
        public void ComputeDynamicsWithDefaultMethod()
        {
            // Initialisation des variables du calcul
            double m = InitialMass;
            double x = 0;
            double y = AtmosphereEntryAltitude * 100000; // Conversion en cm
            double u = InitialHorizontalSpeed * 100000; // Conversion en cm
            double v = -Math.Abs(InitialVerticalSpeed) * 100000; // Conversion en cm/s
            double dt;
            double[] currentX = new double[] { 0, x };
            double[] currentY = new double[] { 0, y };
            double[] currentU = new double[] { 0, u };
            double[] currentV = new double[] { 0, v };
            double[] currentM = new double[] { 0, m };
            double energy, magnitude;

            // Définitions des fonctions dérivées
            Func<double, double, double> equationX = (p1, p2) => currentU[1];
            Func<double, double, double> equationY = (p1, p2) => currentV[1];
            Func<double, double, double> equationU = (p1, p2) => -K1Parameter * ComputeAtmosphereDensity(currentY[1]) * Math.Sqrt(currentU[1] * currentU[1] + currentV[1] * currentV[1]) * currentU[1] * Math.Pow(currentM[1], -1.0/3.0);
            Func<double, double, double> equationV = (p1, p2) => -K1Parameter * ComputeAtmosphereDensity(currentY[1]) * Math.Sqrt(currentU[1] * currentU[1] + currentV[1] * currentV[1]) * currentV[1] * Math.Pow(currentM[1], -1.0 / 3.0) - 980;
            Func<double, double, double> equationM = (p1, p2) => -K2Parameter * ComputeAtmosphereDensity(currentY[1]) * (currentU[1] * currentU[1] + currentV[1] * currentV[1]) * Math.Sqrt(currentU[1] * currentU[1] + currentV[1] * currentV[1]) * Math.Pow(currentM[1], 2.0/3.0);

            IFirstOrderDifferentialEquation fx = new FirstOrderRungeKuttaMethod(equationX);
            fx.SetStartingPoint(0, x);
            IFirstOrderDifferentialEquation fy = new FirstOrderRungeKuttaMethod(equationY);
            fy.SetStartingPoint(0, y);
            IFirstOrderDifferentialEquation fu = new FirstOrderRungeKuttaMethod(equationU);
            fu.SetStartingPoint(0, u);
            IFirstOrderDifferentialEquation fv = new FirstOrderRungeKuttaMethod(equationV);
            fv.SetStartingPoint(0, v);
            IFirstOrderDifferentialEquation fm = new FirstOrderRungeKuttaMethod(equationM);
            fm.SetStartingPoint(0, m);
            energy = 0;
            magnitude = 0;

            Results.Add(new double[] { currentX[0], currentX[1], currentY[1], currentU[1], currentV[1], currentM[1], magnitude });

            do
            {
                // Ajustement du pas de calcul
                if (currentM[1] > 0.8 * InitialMass)
                {
                    dt = 0.05;
                }
                else if (currentM[1] > 0.5 * InitialMass && currentM[1] <= InitialMass)
                {
                    dt = 0.01;
                }
                else if (currentM[1] > 0.35 * InitialMass && currentM[1] <= 0.5 * InitialMass)
                {
                    dt = 0.005;
                }
                else
                {
                    dt = 0.001;
                }

                double[] current;
                current = fx.ComputeForNextStep(currentX[0], currentX[1], dt, true);
                currentX[0] = current[0];
                currentX[1] = current[1];
                current = fy.ComputeForNextStep(currentY[0], currentY[1], dt, true);
                currentY[0] = current[0];
                currentY[1] = current[1];
                current = fu.ComputeForNextStep(currentU[0], currentU[1], dt, true);
                currentU[0] = current[0];
                currentU[1] = current[1];
                current = fv.ComputeForNextStep(currentV[0], currentV[1], dt, true);
                currentV[0] = current[0];
                currentV[1] = current[1];
                current = fm.ComputeForNextStep(currentM[0], currentM[1], dt, true);
                currentM[0] = current[0];
                currentM[1] = current[1];
                energy = ComputeEnergy(currentU[1], currentV[1], fm.Equation.Invoke(0, 0));
                magnitude = ComputeMagnitude(currentY[1], energy);

                if (currentM[1] > 0 && currentY[1] > 0.0)
                {
                    Results.Add(new double[] { currentX[0], currentX[1], currentY[1], currentU[1], currentV[1], currentM[1], magnitude });
                }
            } while (currentM[1] > 0 && currentY[1] > 0.0);
        }

        /// <summary>
        /// Affiche le résultat formaté sur la console
        /// </summary>
        public void OutputDataToConsole()
        {
            if (Results != null && Results.Count > 0)
            {
                Console.WriteLine("t\tx\ty\t\tu\t\tv\t\tm\t\tM");
                foreach (var values in Results)
                {
                    string t = values[0].ToString("f4");
                    string x = (values[1] / 100000).ToString("f4");
                    string y = (values[2] / 100000).ToString("f4");
                    string u = (values[3] / 100000).ToString("f5");
                    string v = (values[4] / 100000).ToString("f5");
                    string m = values[5].ToString("f7");
                    string mag = values[6].ToString("f2");
                    Console.WriteLine($"{t}\t{x}\t{y}\t\t{u}\t{v}\t{m}\t{mag}");
                }
            }
        }

        /// <summary>
        /// Génère un fichier CSV avec le chemin spécifié avec le résultat du calcul
        /// </summary>
        /// <param name="a_filePath">Chemin du fichier CSV</param>
        public void OutputDataToCsv(string a_filePath)
        {
            if (!string.IsNullOrEmpty(a_filePath) && Results != null && Results.Count > 0)
            {
                StreamWriter writer = new StreamWriter(a_filePath);
                writer.WriteLine("Time;Horizontal position;Altitude;Horizontal speed;Vertical speed;m;Magnitude");
                foreach (var values in Results)
                {
                    string t = values[0].ToString("f4");
                    string x = (values[1] / 100000).ToString("f4");
                    string y = (values[2] / 100000).ToString("f4");
                    string u = (values[3] / 100000).ToString("f5");
                    string v = (values[4] / 100000).ToString("f5");
                    string m = values[5].ToString("f7");
                    string mag = values[6].ToString("f2");
                    writer.WriteLine($"{t};{x};{y};{u};{v};{m};{mag}");
                }
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Calcule les dérivées des équations couplées selon l'algorithme du livre
        /// </summary>
        /// <param name="a_x">Valeur x au pas de calcul i</param>
        /// <param name="a_y">Valeur y au pas de calcul i</param>
        /// <param name="a_u">Valeur u au pas de calcul i</param>
        /// <param name="a_v">Valeur v au pas de calcul i</param>
        /// <param name="a_m">Valeur m au pas de calcul i</param>
        /// <param name="a_fx">Valeur de la dérivée fx calculée au pas de calcul i</param>
        /// <param name="a_fy">Valeur de la dérivée fy calculée au pas de calcul i</param>
        /// <param name="a_fu">Valeur de la dérivée fu calculée au pas de calcul i</param>
        /// <param name="a_fv">Valeur de la dérivée fv calculée au pas de calcul i</param>
        /// <param name="a_fm">Valeur de la dérivée fm calculée au pas de calcul i</param>
        private void ComputeDerivatives(double a_x,
                                        double a_y,
                                        double a_u,
                                        double a_v,
                                        double a_m,
                                        out double a_fx,
                                        out double a_fy,
                                        out double a_fu,
                                        out double a_fv,
                                        out double a_fm)
        {
            a_fx = a_u;
            a_fy = a_v;
            double s = Math.Sqrt(a_u * a_u + a_v * a_v);
            double d = ComputeAtmosphereDensity(a_y);
            a_fu = -K1Parameter * d * s * a_u * Math.Exp(-Math.Log(a_m) / 3);
            a_fv = -K1Parameter * d * s * a_v * Math.Exp(-Math.Log(a_m) / 3) - 980;
            a_fm = -K2Parameter * d * s * s * s * Math.Exp(2 * Math.Log(a_m) / 3);
        }

        /// <summary>
        /// Calcule la densité atmosphérique à partir d el'altitude en cm
        /// </summary>
        /// <param name="a_y">Altitude en cm</param>
        /// <returns>Densité de l'atmosphère</returns>
        private double ComputeAtmosphereDensity(double a_y)
        {
            return Math.Exp(-6.65125 - 1.39813e-6 * a_y);
        }

        /// <summary>
        /// Calcul l'énergie dégagée pendant le pas de calcul
        /// </summary>
        /// <param name="a_u">Vitesse horizontale en cm/s au pas de calcul i</param>
        /// <param name="a_v">Vitesse verticale en cm/s au pas de calcul i</param>
        /// <param name="a_fm">Valeur de la dérivée de m au pas de calcul i</param>
        /// <returns>Energie dégagée au pas de calcul i</returns>
        private double ComputeEnergy(double a_u, double a_v, double a_fm)
        {
            return -0.5 * TauParameter * a_fm * (a_u * a_u + a_v * a_v);
        }

        /// <summary>
        /// Calcule la magnitude du météore au pas de calcul i
        /// </summary>
        /// <param name="a_y">Altitude en cm</param>
        /// <param name="a_e">Energie dégagée au pas de calcul i</param>
        /// <returns>Magnitude au pas de calcul i</returns>
        private double ComputeMagnitude(double a_y, double a_e)
        {
            return 5 * Math.Log10(a_y) - 2.5 * Math.Log10(a_e) - 8.795;
        }

    }
}
