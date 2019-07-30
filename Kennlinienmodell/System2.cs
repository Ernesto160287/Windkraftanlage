using MathematikWerkzeuge;
using MathematikWerkzeuge.Integration;
using System;

namespace Kennlinienmodell
{
    class System2 : System
    {
        Vektor2 FSchub;
        double MSchub;

        Vektor2 FLager;
        double MLager;

        internal System2(Func<double, double> cW, Func<double, double> cA) : base()
        {
            bauteile = new Bauteil[5];
            bauteile[0] = new Querfahne(cW, cA);
            bauteile[1] = new LinkeRotorgondelseite(cW, cA);
            bauteile[2] = new Traeger1(cW, cA);
            bauteile[3] = new Traeger2(cW, cA);
            bauteile[4] = new Traeger3(cW, cA);
        }

        protected override Vektor2 BestimmeVerschiebung(double beta)
        {
            Vektor2 verschiebung = new Vektor2();
            verschiebungUrsprung.x = -Parameter.l3 * Math.Cos(Punkte.Phi2(beta));
            verschiebungUrsprung.y = -Parameter.l3 * Math.Sin(Punkte.Phi2(beta));

            return verschiebung;
        }

        internal override void WerteAus(double v, double alpha, double beta, Integration integrator)
        {
            BerechneKraefte(v, alpha, beta, integrator);
            BerechneDrehmomente(v, alpha, beta, integrator);
        }

        protected override void BerechneKraefte(double v, double alpha, double beta, Integration integrator)
        {
            gesamtkraftBauteile = BerechneGesamtkraftBauteile(v, alpha, beta, integrator);

            // FSeil und FGelenk wurden bereits aus System 1 übertragen

            FSchub.x = Parameter.rhoL / 2 * Parameter.cSchub * Math.Pow(v, 2) * Math.PI * Math.Pow(Parameter.r, 2) * Math.Sin(beta);
            FSchub.y = 0.0;

            FLager = -(gesamtkraftBauteile + FSeil + FGelenk + FLager);
        }

        protected override void BerechneDrehmomente(double v, double alpha, double beta, Integration integrator)
        {
            gesamtdrehmomentBauteile = BerechneGesamtdrehmomentBauteile(v, alpha, beta, integrator);

            MSeil = punkte.P5.x * FSeil.y - punkte.P5.y * FSeil.x;
            MGelenk = punkte.P12.x * FGelenk.y - punkte.P12.y * FGelenk.x;

            MSchub = punkte.P7.x * FSchub.y - punkte.P7.y * FSchub.x;

            MLager = 0.0;
        }

        internal double BerechneGesamtdrehmoment()
        {
            return gesamtdrehmomentBauteile + MSchub + MSeil + MGelenk + MLager;
        }

        internal override double[] GebeOptionaleKraefteAus(double v, double alpha, double beta, Integration integrator)
        {
            double[] optionaleKraefte = new double[8];

            optionaleKraefte[0] = FLager.Norm();
            optionaleKraefte[1] = FSchub.Norm();
            optionaleKraefte[2] = bauteile[0].BerechneWiderstandskraft(v, alpha, beta, integrator).Norm();
            optionaleKraefte[3] = bauteile[0].BerechneAuftriebskraft(v, alpha, beta, integrator).Norm();
            optionaleKraefte[4] = bauteile[1].BerechneWiderstandskraft(v, alpha, beta, integrator).Norm();
            optionaleKraefte[5] = bauteile[1].BerechneAuftriebskraft(v, alpha, beta, integrator).Norm();
            optionaleKraefte[6] = bauteile[2].BerechneWiderstandskraft(v, alpha, beta, integrator).Norm()
                                  + bauteile[3].BerechneWiderstandskraft(v, alpha, beta, integrator).Norm()
                                  + bauteile[4].BerechneWiderstandskraft(v, alpha, beta, integrator).Norm();
            optionaleKraefte[7] = bauteile[2].BerechneAuftriebskraft(v, alpha, beta, integrator).Norm()
                                  + bauteile[3].BerechneAuftriebskraft(v, alpha, beta, integrator).Norm()
                                  + bauteile[4].BerechneAuftriebskraft(v, alpha, beta, integrator).Norm();

            return optionaleKraefte;
        }
    }
}
