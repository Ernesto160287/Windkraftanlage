using MathematikWerkzeuge.Integration;
using MathematikWerkzeuge.Interpolation;
using MathematikWerkzeuge.Nullstelle;
using System;

namespace Kennlinienmodell
{
    class Numerikhilfsmittel
    {
        readonly double genauigkeit;

        internal readonly NullstellenApproximationAusIntervall approxNst;
        internal readonly Integration integrator;

        readonly Interpolation cWFunktion;
        internal Func<double, double> cW;

        readonly Interpolation cAFunktion;
        internal Func<double, double> cA;

        internal Numerikhilfsmittel()
        {
        }

        internal Numerikhilfsmittel(double genauigkeit)
        {
            this.genauigkeit = genauigkeit;

            approxNst = new Bisektionsverfahren(genauigkeit);
            integrator = new SimpsonRegel(genauigkeit);


            cWFunktion = new KubischeInterpolation(Parameter.winkelwerte, Parameter.cWwerte);
            cW = InitialisiereCW();

            cAFunktion = new KubischeInterpolation(Parameter.winkelwerte, Parameter.cAwerte);
            cA = InitialisiereCA();
        }

        Func<double, double> InitialisiereCW()
        {
            return t =>
            {
                if (t <= Math.PI / 2)
                    return cWFunktion.Interpoliere(t);
                else
                    return cWFunktion.Interpoliere(Math.PI - t);
            };
        }

        Func<double, double> InitialisiereCA()
        {
            return t =>
            {
                if (t <= Math.PI / 2)
                    return cAFunktion.Interpoliere(t);
                else
                    return -cAFunktion.Interpoliere(Math.PI - t);
            };
        }
    }
}
