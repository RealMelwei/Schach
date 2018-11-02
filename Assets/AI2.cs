using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI2 : AI
{
    Comfort C = new Comfort();

    public int Berechnungstiefe = 3;
    public int Tiefensuchentiefe = 3;

    //Wirkungsstärken der verschiedenen Subfunktionen der Stellungsbewertung
    const float kMaterial = 1; //Wie stark wird der Materialvorteil in die Stellung eingebunden? Basis: 1
    const float kLeichtfigurenfreiheit = 0.015f; //Wie stark wird Leichtfigurenentwicklung eingebunden? Basis: 0.015
    const float kSchwerfigurenfreiheit = 0.007f; //Wie stark wird Schwerfigurenentwicklung eingebunden? Basis: 0.007
    const float kZentrum = 0.02f; //Wie stark wird Zentrumskontrolle eingebunden? Basis: 0.02

    public override Zug ziehe(Situation sit)
    {

        return BesterZugBruteForce(Berechnungstiefe, sit, Tiefensuchentiefe);
    }

    Zug BesterZugBruteForce(int Iterationen, Situation sit, int Tiefeniteration)
    {
        if (Iterationen <= 1)
        {
            float BewertungStart = Stellungsbewertung(sit);
            Zug[] Machbar = C.AlleMöglichenZüge(sit, true);
            if (Machbar.Length == 0)
            {
                return null;
            }
            Zug Bester = Machbar[0];
            float BesterZugBewertung = float.MinValue;

            for (int i = 0; i < Machbar.Length; i++)
            {
                Situation sit2 = sit.Clone();
                sit2.Forward(Machbar[i]);
                float Quali = -Stellungsbewertung(sit2);
                if (Quali > BesterZugBewertung)
                {
                    Bester = Machbar[i];
                    BesterZugBewertung = Quali;
                }
            }
            return Bester;
        }
        else
        {
            float BewertungStart = Stellungsbewertung(sit);
            Zug[] Machbar = C.AlleMöglichenZüge(sit, true);

            if (Machbar.Length == 0)
            {
                return null;
            }
            Zug Bester = Machbar[0];
            float BesterZugBewertung = float.MinValue;
            for (int i = 0; i < Machbar.Length; i++)
            {
                Situation sit2 = sit.Clone();
                sit2.Forward(Machbar[i]);
                float Quali = float.MinValue;
                bool Schachzug = C.IstImSchach(sit2, sit2.SpielerID);
                if ((!Schachzug&&sit.Stellung[Machbar[i].Ziel[0],Machbar[i].Ziel[1]].Typ!="")||Tiefeniteration<=0)
                {
                    Zug BesterInSit2 = BesterZugBruteForce(Iterationen - 1, sit2, Tiefeniteration);
                    if (BesterInSit2 == null)
                    {
                        if (C.IstImSchach(sit2, sit2.SpielerID))
                        {
                            return Machbar[i];
                        }
                        else
                        {
                            if (BesterZugBewertung < 0)
                            {
                                BesterZugBewertung = 0;
                                Bester = Machbar[i];
                            }
                        }
                    }
                    else
                    {
                        sit2.Forward(BesterInSit2);
                        Quali = Stellungsbewertung(sit2);
                        if (Quali > BesterZugBewertung)
                        {
                            BesterZugBewertung = Quali;
                            Bester = Machbar[i];
                        }
                    }
                }
                else if (Schachzug)
                {
                    Zug BesterInSit2 = BesterZugBruteForce(Iterationen+1, sit2, Tiefeniteration-1);
                    if (BesterInSit2 == null)
                    {
                        if (C.IstImSchach(sit2, sit2.SpielerID))
                        {
                            return Machbar[i];
                        }
                        else
                        {
                            if (BesterZugBewertung < 0)
                            {
                                BesterZugBewertung = 0;
                                Bester = Machbar[i];
                            }
                        }
                    }
                    else
                    {
                        sit2.Forward(BesterInSit2);
                        Quali = Stellungsbewertung(sit2);
                        if (Quali > BesterZugBewertung)
                        {
                            BesterZugBewertung = Quali;
                            Bester = Machbar[i];
                        }
                    }
                }
                else
                {
                    Zug BesterInSit2 = BesterZugBruteForce(Iterationen, sit2, Tiefeniteration - 2);
                    if (BesterInSit2 == null)
                    {
                        if (C.IstImSchach(sit2, sit2.SpielerID))
                        {
                            return Machbar[i];
                        }
                        else
                        {
                            if (BesterZugBewertung < 0)
                            {
                                BesterZugBewertung = 0;
                                Bester = Machbar[i];
                            }
                        }
                    }
                    else
                    {
                        sit2.Forward(BesterInSit2);
                        Quali = Stellungsbewertung(sit2);
                        if (Quali > BesterZugBewertung)
                        {
                            BesterZugBewertung = Quali;
                            Bester = Machbar[i];
                        }
                    }
                }

            }
            return Bester;
        }
    }


    float Stellungsbewertung(Situation sit)
    {
        float ret = 0;

        //Matt oder Patt?
        if (!C.IstEinZugMöglich(sit, sit.SpielerID, true))
        {
            if (C.IstImSchach(sit, sit.SpielerID))
            {
                return float.MinValue;
            }
            else
            {
                return 0;
            }
        }

        //Figurenvorteil?
        ret += Materialvorteil(sit);

        //Figurenfreiheit?
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (sit.Stellung[i, j].Typ == "S" || sit.Stellung[i, j].Typ == "L")
                {
                    if (sit.Stellung[i, j].SpielerID == sit.SpielerID)
                    {
                        ret += kLeichtfigurenfreiheit * C.AlleZügeVomFeld(sit, new int[] { i, j }, false).Length;
                    }
                    else
                    {
                        sit.SpielerID = -sit.SpielerID + 1;
                        ret -= kLeichtfigurenfreiheit * C.AlleZügeVomFeld(sit, new int[] { i, j }, false).Length;
                        sit.SpielerID = -sit.SpielerID + 1;
                    }
                }
                if (sit.Stellung[i, j].Typ == "T" || sit.Stellung[i, j].Typ == "D")
                {
                    if (sit.Stellung[i, j].SpielerID == sit.SpielerID)
                    {
                        ret += kSchwerfigurenfreiheit * C.AlleZügeVomFeld(sit, new int[] { i, j }, false).Length;
                    }
                    else
                    {
                        sit.SpielerID = -sit.SpielerID + 1;
                        ret -= kSchwerfigurenfreiheit * C.AlleZügeVomFeld(sit, new int[] { i, j }, false).Length;
                        sit.SpielerID = -sit.SpielerID + 1;
                    }
                }
            }
        }
        //Zentrumskontrolle?
        if (sit.Stellung[3, 3].Typ != "")
        {
            if (sit.Stellung[3, 3].SpielerID == sit.SpielerID)
            {
                ret += kZentrum;
            }
            else
            {
                ret -= kZentrum;
            }
        }
        if (sit.Stellung[3, 4].Typ != "")
        {
            if (sit.Stellung[3, 4].SpielerID == sit.SpielerID)
            {
                ret += kZentrum;
            }
            else
            {
                ret -= kZentrum;
            }
        }
        if (sit.Stellung[4, 3].Typ != "")
        {
            if (sit.Stellung[4, 3].SpielerID == sit.SpielerID)
            {
                ret += kZentrum;
            }
            else
            {
                ret -= kZentrum;
            }
        }
        if (sit.Stellung[4, 4].Typ != "")
        {
            if (sit.Stellung[4, 4].SpielerID == sit.SpielerID)
            {
                ret += kZentrum;
            }
            else
            {
                ret -= kZentrum;
            }
        }
        ret += kZentrum * AnzahlAngegriffenOderGedeckt(sit, sit.SpielerID, new int[] { 3, 3 }, false);
        ret += kZentrum * AnzahlAngegriffenOderGedeckt(sit, sit.SpielerID, new int[] { 3, 4 }, false);
        ret += kZentrum * AnzahlAngegriffenOderGedeckt(sit, sit.SpielerID, new int[] { 4, 3 }, false);
        ret += kZentrum * AnzahlAngegriffenOderGedeckt(sit, sit.SpielerID, new int[] { 4, 4 }, false);

        ret -= kZentrum * AnzahlAngegriffenOderGedeckt(sit, -sit.SpielerID + 1, new int[] { 3, 3 }, false);
        ret -= kZentrum * AnzahlAngegriffenOderGedeckt(sit, -sit.SpielerID + 1, new int[] { 3, 4 }, false);
        ret -= kZentrum * AnzahlAngegriffenOderGedeckt(sit, -sit.SpielerID + 1, new int[] { 4, 3 }, false);
        ret -= kZentrum * AnzahlAngegriffenOderGedeckt(sit, -sit.SpielerID + 1, new int[] { 4, 4 }, false);
        return ret;
    }

    float Materialvorteil(Situation sit)
    {
        int ret = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                switch (sit.Stellung[i, j].Typ)
                {
                    case "B":
                        if (sit.Stellung[i, j].SpielerID == sit.SpielerID)
                        {
                            ret += 1;
                        }
                        else
                        {
                            ret -= 1;
                        }
                        break;
                    case "S":
                        if (sit.Stellung[i, j].SpielerID == sit.SpielerID)
                        {
                            ret += 3;
                        }
                        else
                        {
                            ret -= 3;
                        }
                        break;
                    case "L":
                        if (sit.Stellung[i, j].SpielerID == sit.SpielerID)
                        {
                            ret += 3;
                        }
                        else
                        {
                            ret -= 3;
                        }
                        break;
                    case "T":
                        if (sit.Stellung[i, j].SpielerID == sit.SpielerID)
                        {
                            ret += 5;
                        }
                        else
                        {
                            ret -= 5;
                        }
                        break;
                    case "D":
                        if (sit.Stellung[i, j].SpielerID == sit.SpielerID)
                        {
                            ret += 9;
                        }
                        else
                        {
                            ret -= 9;
                        }
                        break;

                }
            }
        }
        return ret;
    }
    int AnzahlAngegriffenOderGedeckt(Situation sit, int VonSpieler, int[] Feld, bool Schachprüfen)
    {
        Situation sit2 = sit.Clone();
        sit2.Stellung[Feld[0], Feld[1]].SpielerID = -VonSpieler + 1;
        sit2.Stellung[Feld[0], Feld[1]].Typ = "D";
        return C.AnzahlAngegriffen(sit2, Feld, VonSpieler, Schachprüfen);
    }
}
