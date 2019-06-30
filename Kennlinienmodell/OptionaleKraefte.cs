namespace Windkraftanlage.Kennlinienmodell
{
    class OptionaleKraefte
    {
        double[] FGelenk { get; set; }
        double[] FLager { get; set; }
        double[] FSchub { get; set; }
        double[] FWS { get; set; }
        double[] FAS { get; set; }
        double[] FWQ { get; set; }
        double[] FAQ { get; set; }
        double[] FWR { get; set; }
        double[] FAR { get; set; }
        double[] FWT { get; set; }
        double[] FAT { get; set; }


        public OptionaleKraefte()
        {

        }

        public OptionaleKraefte(int arrayGroesse)
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
