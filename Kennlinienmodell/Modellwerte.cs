using System.Collections.Generic;

namespace Windkraftanlage.Kennlinienmodell
{
    class Modellwerte
    {
        List<double> v_Werte;
        List<double> alpha_Werte;
        List<double> beta_Werte;
        List<double> seillaenge_Werte;
        List<double> seilkraft_Werte;

        internal Modellwerte()
        {
            v_Werte = new List<double>();
            alpha_Werte = new List<double>();
            beta_Werte = new List<double>();
            seillaenge_Werte = new List<double>();
            seilkraft_Werte = new List<double>();
        }

        internal void SpeichereAktuelleWerte(double v, double alpha, double beta, double seillaenge, double seilkraft)
        {
            v_Werte.Add(v);
            alpha_Werte.Add(alpha);
            beta_Werte.Add(beta);
            seillaenge_Werte.Add(seillaenge);
            seilkraft_Werte.Add(seilkraft);
        }
    }
}
