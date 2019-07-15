using System;

namespace Kennlinienmodell
{
    static class Parameter
    {
        internal const double eta = 0.2;  // Wirkungsgrad der Windkraftanlage
        internal const double rhoL = 1.2;  // Luftdichte
        internal const double cSchub = 0.7;  // Schubbeiwert
        internal const double PG = 200;  // Generatorleistung

        internal static readonly double[] winkelwerte = { 0, Math.PI / 18, Math.PI / 9, Math.PI / 6, 2 * Math.PI / 9, 5 * Math.PI / 18,
                                                        Math.PI / 3, 7 * Math.PI / 18, 4 * Math.PI/ 9, Math.PI / 2};

        internal static readonly double[] cWwerte = { 0.0, 0.12, 0.32, 0.53, 0.67, 0.8, 0.93, 1.09, 1.15, 1.2 };
        internal static readonly double[] cAwerte = { 0.0, 0.72, 0.797, 0.88, 0.792, 0.69, 0.55, 0.41, 0.23, 0.0 };

        internal const double r = 0.97;  // Rotorradius

        internal const double l1a = 1.2;
        internal const double l1b = 2.0;
        internal const double l2 = 0.3;
        internal const double l3 = 0.18;
        internal const double l4 = 0.18;
        internal const double l4a = 0.18;
        internal const double l5 = 0.38;
        internal const double l5a = 0.15;
        internal const double l5b = 0.38;
        internal const double l5c = 0.42;
        internal const double l5d = 0.46;
        internal const double l6a = 1.0;
        internal const double l6b = 1.5;
        internal const double l7 = 0.3;
        internal const double l8 = 0.45;
        internal const double l9 = 0.63;
        internal const double l11 = 0.25;

        internal readonly static double gamma = Math.Atan(l5c / l5a);
    }
}
