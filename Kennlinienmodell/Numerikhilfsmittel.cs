using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windkraftanlage.Mathematikwerkzeuge.Integration;
using Windkraftanlage.Mathematikwerkzeuge.Interpolation;
using Windkraftanlage.Mathematikwerkzeuge.Nullstelle;

namespace Windkraftanlage.Kennlinienmodell
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

        public Numerikhilfsmittel()
        {
        }


        public Numerikhilfsmittel(double genauigkeit)
        {
            this.genauigkeit = genauigkeit;

            approxNst = new Bisektionsverfahren(genauigkeit);
            integrator = new SimpsonRegel(genauigkeit);


            cWFunktion = new KubischeInterpolation(Parameter.winkelwerte, Parameter.cWwerte);
            cW = InitialisiereCW();

            cAFunktion = new KubischeInterpolation(Parameter.winkelwerte, Parameter.cAwerte);
            cA = InitialisiereCA();
        }

        private Func<double, double> InitialisiereCW()
        {
            return t =>
            {
                if (t <= Math.PI / 2)
                    return cWFunktion.Interpoliere(t);
                else
                    return cWFunktion.Interpoliere(Math.PI - t);
            };
        }

        private Func<double, double> InitialisiereCA()
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
