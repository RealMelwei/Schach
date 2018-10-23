using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI2 : AI
{
    Comfort C = new Comfort();

    public int Berechnungstiefe = 3;

    //Wirkungsstärken der verschiedenen Subfunktionen der Stellungsbewertung
    const float kMaterial = 1; //Wie stark wird der Materialvorteil in die Stellung eingebunden?
    const float kLeichtfigurenfreiheit = 0.015f;
    const float kSchwerfigurenfreiheit = 0.01f;

    public override Zug ziehe(Situation sit)
    {

        return BesterZugBruteForce(Berechnungstiefe, sit);
    }

    Zug BesterZugBruteForce(int Iterationen, Situation sit)
    {
        if (Iterationen <= 1)
        {
            Zug[] Machbar = C.AlleMöglichenZüge(sit,true);
            if (Machbar.Length == 0)
            {
                return null;
            }
            Zug Bester=Machbar[0];
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
            Zug[] Machbar = C.AlleMöglichenZüge(sit,true);
            Zug Bester = Machbar[0];
            float BesterZugBewertung = float.MinValue;
            for (int i = 0; i < Machbar.Length; i++)
            {
                Situation sit2 = sit.Clone();
                sit2.Forward(Machbar[i]);
                float Quali = float.MinValue;
                Zug BesterInSit2 = BesterZugBruteForce(Iterationen - 1, sit2);
                if (BesterInSit2 == null)
                {
                    if(C.IstImSchach(sit2, sit2.SpielerID))
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
                } else
                {
                    sit2.Forward(BesterInSit2);
                    Quali = Stellungsbewertung(sit2);
                    if(Quali > BesterZugBewertung)
                    {
                        BesterZugBewertung = Quali;
                        Bester = Machbar[i];
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
                        ret -= kLeichtfigurenfreiheit * C.AlleZügeVomFeld(sit, new int[] { i, j }, false).Length;
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
                        ret -= kSchwerfigurenfreiheit * C.AlleZügeVomFeld(sit, new int[] { i, j }, false).Length;
                    }
                }
            }
        }


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
}
