namespace Windkraftanlage.Kennlinienmodell
{
    class OptionaleKraefte
    {
        double[] FGelenk;
        double[] FLager;
        double[] FSchub;
        double[] FWS;
        double[] FAS;
        double[] FWQ;
        double[] FAQ;
        double[] FWR;
        double[] FAR;
        double[] FWT;
        double[] FAT;


        internal OptionaleKraefte()
        {
        }

        internal OptionaleKraefte(int arrayGroesse)
        {
            FGelenk = new double[arrayGroesse];
            FLager = new double[arrayGroesse];
            FSchub = new double[arrayGroesse];
            FWS = new double[arrayGroesse];
            FAS = new double[arrayGroesse];
            FWQ = new double[arrayGroesse];
            FAQ = new double[arrayGroesse];
            FWR = new double[arrayGroesse];
            FAR = new double[arrayGroesse];
            FWT = new double[arrayGroesse];
            FAT = new double[arrayGroesse];
        }
    }
}
