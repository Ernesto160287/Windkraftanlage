﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windkraftanlage.Mathematikwerkzeuge;
using Windkraftanlage.Mathematikwerkzeuge.Integration;

namespace Windkraftanlage.Kennlinienmodell
{
    class System1 : System
    {
        internal double Seillaenge { get; set; } // Seillänge
        internal double Seilkraft { get; set; }  // Seilkraft

        Vektor2 P2P5;  // Verbindungsvektor Steuerfahne und Trägerkonstruktion


        public System1(Integration integrator, Func<double, double> cW, Func<double, double> cA) : base()
        {
            bauteile = new Bauteil[1];
            bauteile[0] = new Steuerfahne(integrator, cW, cA);
        }

        internal override void WerteAus(double v, double alpha, double beta)
        {
            BerechneDrehmomente(v, alpha, beta);

            Seillaenge = BerechneSeillaenge();
            Seilkraft = BerechneSeilkraft();

            BerechneKraefte(v, alpha, beta);
        }

        private protected override void AktualisierePunkte(double alpha, double beta)
        {
            base.Aktualisiere(alpha, beta);
            AktualisiereP2P5();
        }

        private protected override Vektor2 BestimmeVerschiebung(double beta)
        {
            return Vektor2.Zero();
        }

        private void AktualisiereP2P5()
        {
            P2P5 = new Vektor2(punkte.P2, punkte.P5);
        }

        private protected override void BerechneDrehmomente(double v, double alpha, double beta)
        {
            gesamtdrehmomentBauteile = BerechneGesamtdrehmomentBauteile(v, alpha, beta);

            MSeil = -gesamtdrehmomentBauteile;
            MGelenk = 0.0;
        }

        private protected override void BerechneKraefte(double v, double alpha, double beta)
        {
            gesamtkraftBauteile = BerechneGesamtkraftBauteile(v, alpha, beta);

            FSeil = Seilkraft / Seillaenge * P2P5;
            FGelenk = - (gesamtkraftBauteile + FSeil);
        }

        internal double BerechneSeillaenge()
        {
            return P2P5.Norm();
        }

        internal double BerechneSeilkraft()
        {
            return Seillaenge * gesamtdrehmomentBauteile / (punkte.P2.y * P2P5.x - punkte.P2.x * P2P5.y);
        }
    }
}
