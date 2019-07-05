using System;
using Windkraftanlage.Mathematikwerkzeuge;
using Windkraftanlage.Mathematikwerkzeuge.Integration;

namespace Windkraftanlage.Kennlinienmodell
{
    class System2 : System
    {
        Vektor2 FSchub;
        double MSchub;

        Vektor2 FLager;
        double MLager;

        internal System2(Integration integrator, Func<double, double> cW, Func<double, double> cA) : base()
        {
            bauteile = new Bauteil[5];
            bauteile[0] = new Querfahne(integrator, cW, cA);
            bauteile[1] = new LinkeRotorgondelseite(integrator, cW, cA);
            bauteile[2] = new Traeger1(integrator, cW, cA);
            bauteile[3] = new Traeger2(integrator, cW, cA);
            bauteile[4] = new Traeger3(integrator, cW, cA);
        }

        private protected override Vektor2 BestimmeVerschiebung(double beta)
        {
            Vektor2 verschiebung = new Vektor2();
            verschiebungUrsprung.x = -Parameter.l3 * Math.Cos(Punkte.Phi2(beta));
            verschiebungUrsprung.y = -Parameter.l3 * Math.Sin(Punkte.Phi2(beta));

            return verschiebung;
        }

        internal override void WerteAus(double v, double alpha, double beta)
        {
            BerechneKraefte(v, alpha, beta);
            BerechneDrehmomente(v, alpha, beta);
        }

        private protected override void BerechneKraefte(double v, double alpha, double beta)
        {
            gesamtkraftBauteile = BerechneGesamtkraftBauteile(v, alpha, beta);

            // FSeil und FGelenk wurden bereits aus System 1 übertragen

            FSchub.x = Parameter.rhoL / 2 * Parameter.cSchub * Math.Pow(v, 2) * Math.PI * Math.Pow(Parameter.r, 2) * Math.Sin(beta);
            FSchub.y = 0.0;

            FLager = -(gesamtkraftBauteile + FSeil + FGelenk + FLager);
        }

        private protected override void BerechneDrehmomente(double v, double alpha, double beta)
        {
            gesamtdrehmomentBauteile = BerechneGesamtdrehmomentBauteile(v, alpha, beta);

            MSeil = punkte.P5.x * FSeil.y - punkte.P5.y * FSeil.x;
            MGelenk = punkte.P12.x * FGelenk.y - punkte.P12.y * FGelenk.x;

            MSchub = punkte.P7.x * FSchub.y - punkte.P7.y * FSchub.x;

            MLager = 0.0;
        }

        internal double BerechneGesamtdrehmoment()
        {
            return gesamtdrehmomentBauteile + MSchub + MSeil + MGelenk + MLager;
        }
    }
}
