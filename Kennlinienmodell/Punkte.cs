using MathematikWerkzeuge;
using System;

namespace Kennlinienmodell
{
    class Punkte
    {
        // Holepunkt der Steuerfahne
        internal Vektor2 P2;
        // Mastlager (Ursprung on System 2)
        internal Vektor2 P4;
        // Holepunkt der Trägerkonstruktion
        internal Vektor2 P5;
        // Kreuzungspunkt von Träger 1 und Träger 3
        internal Vektor2 P5a;
        // Endpunkt von Träger 1, Startpunkt von Träger 2
        internal Vektor2 P5b;
        // Endpunkt von Träger 2 und Träger 3
        internal Vektor2 P5c;
        // Rotornabe
        internal Vektor2 P7;
        // Startpunkt des Generators auf der linken Rotorgondelseite
        internal Vektor2 P8;
        // Eckpunkt der Generatorplate (vorne rechts)
        internal Vektor2 P9;
        // Endpunkt der Bahnkurenplatten auf der linken Rotorgondelseite
        internal Vektor2 P11;
        // Gelenk (Ursprung von System 1)
        internal Vektor2 P12;
        // Endpunkt des Rotors (links)
        internal Vektor2 PR1;
        // Endpunkt des Rotors (rechts)
        internal Vektor2 PR2;

        internal void Aktualisiere(double alpha, double beta, Vektor2 verschiebung)
        {
            P2.x = Parameter.l2 * Math.Cos(alpha);
            P2.y = Parameter.l2 * Math.Sin(alpha);
            P2 += verschiebung;

            P4.x = Parameter.l4 * Math.Cos(Phi1(beta));
            P4.y = Parameter.l4 * Math.Sin(Phi1(beta));
            P4 += verschiebung;

            P5a.x = P4.x + (Parameter.l5b - Parameter.l5a) * Math.Cos(Phi1(beta));
            P5a.y = P4.y + (Parameter.l5b - Parameter.l5a) * Math.Sin(Phi1(beta));

            P5b.x = P4.x + Parameter.l5b * Math.Cos(Phi1(beta));
            P5b.y = P4.y + Parameter.l5b * Math.Sin(Phi1(beta));

            P5c.x = P4.x + Parameter.l5b * Math.Cos(Phi1(beta)) + Parameter.l5c * Math.Cos(Phi5(beta));
            P5c.y = P4.y + Parameter.l5b * Math.Sin(Phi1(beta)) + Parameter.l5c * Math.Sin(Phi5(beta));

            P5.x = P5a.x + Parameter.l5 * Math.Cos(Phi4(beta));
            P5.y = P5a.y + Parameter.l5 * Math.Sin(Phi4(beta));

            P7.x = (Parameter.l3 + Parameter.l7) * Math.Cos(Phi2(beta));
            P7.y = (Parameter.l3 + Parameter.l7) * Math.Sin(Phi2(beta));
            P7 += verschiebung;

            P8.x = P4.x + Parameter.l8 * Math.Cos(Phi2(beta));
            P8.y = P4.y + Parameter.l8 * Math.Sin(Phi2(beta));

            P9.x = (Parameter.l9 - Parameter.l3) * Math.Cos(Phi2(beta)) + Parameter.l4a * Math.Cos(Phi3(beta));
            P9.y = (Parameter.l9 - Parameter.l3) * Math.Sin(Phi2(beta)) + Parameter.l4a * Math.Sin(Phi3(beta));
            P9 += verschiebung;

            P11.x = P4.x + Parameter.l11 * Math.Cos(Phi2(beta));
            P11.y = P4.y + Parameter.l11 * Math.Sin(Phi2(beta));

            P12.x = -Parameter.l3 * Math.Cos(Phi2(beta));
            P12.y = -Parameter.l3 * Math.Sin(Phi2(beta));
            P12 += verschiebung;

            PR1.x = P7.x + Parameter.r * Math.Cos(Phi1(beta));
            PR1.y = P7.y + Parameter.r * Math.Sin(Phi1(beta));

            PR2.x = P7.x + Parameter.r * Math.Cos(Phi3(beta));
            PR2.y = P7.y + Parameter.r * Math.Sin(Phi3(beta));
        }

        internal static double Phi1(double angle)
        {
            return Math.PI - angle;
        }

        internal static double Phi2(double angle)
        {
            return 3 * Math.PI / 2 - angle;
        }

        internal static double Phi3(double angle)
        {
            return 2 * Math.PI - angle;
        }

        internal static double Phi4(double angle)
        {
            return Math.PI - angle - Parameter.gamma;
        }

        internal static double Phi5(double angle)
        {
            return Math.PI / 2 - angle + 0.001;
        }
    }
}
