using System;
using System.Collections.Generic;

namespace Windkraftanlage.Kennlinienmodell
{
    internal class OptionaleKraefte
    {
        List<double> FGelenk_Werte;
        List<double> FWS_Werte;
        List<double> FAS_Werte;
        List<double> FLager_Werte;
        List<double> FSchub_Werte;
        List<double> FWQ_Werte;
        List<double> FAQ_Werte;
        List<double> FWR_Werte;
        List<double> FAR_Werte;
        List<double> FWT_Werte;
        List<double> FAT_Werte;

        internal OptionaleKraefte()
        {
            FGelenk_Werte = new List<double>();
            FWS_Werte = new List<double>();
            FAS_Werte = new List<double>();
            FLager_Werte = new List<double>();
            FSchub_Werte = new List<double>();
            FWQ_Werte = new List<double>();
            FAQ_Werte = new List<double>();
            FWR_Werte = new List<double>();
            FAR_Werte = new List<double>();
            FWT_Werte = new List<double>();
            FAT_Werte = new List<double>();
        }

        internal void SpeichereAktuelleWerteSystem1(double[] kraefteSystem1)
        {
            FGelenk_Werte.Add(kraefteSystem1[0]);
            FWS_Werte.Add(kraefteSystem1[1]);
            FAS_Werte.Add(kraefteSystem1[2]);
        }

        internal void SpeichereAktuelleWerteSystem2(double[] kraefteSystem2)
        {
            FLager_Werte.Add(kraefteSystem2[0]);
            FSchub_Werte.Add(kraefteSystem2[1]);
            FWQ_Werte.Add(kraefteSystem2[2]);
            FAQ_Werte.Add(kraefteSystem2[3]);
            FWR_Werte.Add(kraefteSystem2[4]);
            FAR_Werte.Add(kraefteSystem2[5]);
            FWT_Werte.Add(kraefteSystem2[6]);
            FAT_Werte.Add(kraefteSystem2[7]);
        }
    }

}
