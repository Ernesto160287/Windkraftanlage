namespace Windkraftanlage.Kennlinienmodell
{
    class Modellwerte
    {
        double[] v_Werte;
        double[] alpha_Werte;
        double[] beta_Werte;
        double[] seillaenge_Werte;
        double[] seilkraft_Werte;

        internal Modellwerte()
        {
        }

        internal Modellwerte(int arrayGroesse)
        {
            v_Werte = new double[arrayGroesse];
            alpha_Werte = new double[arrayGroesse];
            beta_Werte = new double[arrayGroesse];
            seillaenge_Werte = new double[arrayGroesse];
            seilkraft_Werte = new double[arrayGroesse];
        }

        internal void SpeichereAktuelleWerte(int i, double v, double alpha, double beta, double seillaenge, double seilkraft)
        {
            v_Werte[i] = v;
            alpha_Werte[i] = alpha;
            beta_Werte[i] = beta;
            seillaenge_Werte[i] = seillaenge;
            seilkraft_Werte[i] = seilkraft;
        }
    }
}
