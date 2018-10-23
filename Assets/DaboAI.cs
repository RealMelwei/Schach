using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaboAI : AI {
    public int Vorausbrechnungsrange = 2;
    public bool DebugModus = false;
    Comfort C = new Comfort();
    /*public Zug[] GetAllPossibleZüge(Situation sit)
    {
        Zug[] Möglich = new Zug[0];
        for (int i1 = 0; i1 < 8; i1++)
        {
            for (int i2 = 0; i2 < 8; i2++)
            {
                for (int i3 = 0; i3 < 8; i3++)
                {
                    for (int i4 = 0; i4 < 8; i4++)
                    {
                        Zug check = new Zug(new int[] { i1, i2 }, new int[] { i3, i4 }, "D");
                        if (C.ZugLegal(sit, check, true, false))
                        {
                            System.Array.Resize(ref Möglich, Möglich.Length + 1);
                            Möglich[Möglich.Length - 1] = check;
                        }
                    }
                }
            }
        }
        return Möglich;
    }
    public Zug[] GetAllPossibleZüge(Situation sit, int[] von)
    {
        Zug[] Möglich = new Zug[0];
        for (int A = 0; A < 8; A++)
        {
            for (int B = 0; B < 8; B++)
            {
                Zug check = new Zug(von, new int[2] { A, B });
                if (C.ZugLegal(sit,check,true))
                {
                    System.Array.Resize(ref Möglich, Möglich.Length + 1);
                    Möglich[Möglich.Length - 1] = check;
                }
            }
        }
        return Möglich;
    }*/
    private int BerechneZentrumsKontrolleBonus(Situation sit, Zug Z)
    {
        if (sit.Stellung[Z.Ziel[0], Z.Ziel[1]].Typ == "B")
        {
            if(Z.Ziel[0] >=2 && Z.Ziel[0] <= 5){
                if (Z.Ziel[1] >= 3 && Z.Ziel[1] <= 4){
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
        }
        return 0;
}
    private bool WarRochhade(Situation sit,Zug Z)
    {
        if (sit.Stellung[Z.Ziel[0], Z.Ziel[1]].Typ == "K")
        {
            int Distanze = Mathf.Abs(Z.Ziel[0] - Z.Start[0]);
            if(Distanze > 1)
            {
                return true;
            }
        }
        return false;
    }
    public int BewerteZug(Situation sit,Zug Z)
    {
        //Multiplikatoren
        int ZugmöglichkeitenMultiplikator = 1;
        int KönigzugmöglichkeitenMultiplikator = -5;
        int RochhadeunmöglichkeitsMultiplikator = -5;
        int RochhadeunwahrscheinlicherdurchTurmZugMultiplikator = -3;
        int FigurenWertMultiplikator = 20;
        int DamenMultiplikator = 9;
        int BauernMultiplikator = 1;
        int LäuferMultiplikator = 3;
        int SpringerMultiplikator = 3;
        int TurmMultipliaktor = 3;
        int EigenFigurenWertMultiplikator = 1;
        int GegnerFigurenWertMultiplikator = 1;
        int RochhadeMultiplikator = 30;
        int ZugmöglichkeitenScore = 0;
        int RochhadeScore = 0;
        int RochhadeunmöglichkeitsScore = 0;
        int FigurenScore= 0;
        bool Rochhadeunmöglich = false;
        for (int A = 0; A < 8; A++)
        {
            for (int B = 0; B < 8; B++)
            {
                if(sit.Stellung[A,B].Typ != "")
                {
                    int BesitzerFaktor = 1*EigenFigurenWertMultiplikator;
                    if(sit.SpielerID != sit.Stellung[A, B].SpielerID)
                    {
                        BesitzerFaktor = -1*GegnerFigurenWertMultiplikator;
                    }
                    switch(sit.Stellung[A, B].Typ)
                    {
                        case "K":
                            //ZugmöglichkeitenScore += GetAllPossibleZüge(sit, new int[2] { A, B }).Length*KönigzugmöglichkeitenMultiplikator;
                            if(sit.SpielerID == sit.Stellung[A, B].SpielerID)
                            {
                                if(sit.Stellung[A, B].Bewegt)
                                {
                                    Rochhadeunmöglich = true;
                                }
                            }
                            break;
                        case "D":
                            FigurenScore += BesitzerFaktor * DamenMultiplikator;
                            //ZugmöglichkeitenScore += GetAllPossibleZüge(sit, new int[2] { A, B }).Length;
                            break;
                        case "T":
                            FigurenScore += BesitzerFaktor * TurmMultipliaktor;
                            //ZugmöglichkeitenScore += GetAllPossibleZüge(sit, new int[2] { A, B }).Length;
                            if (sit.SpielerID == sit.Stellung[A, B].SpielerID)
                            {
                                if (sit.Stellung[A, B].Bewegt)
                                {
                                    RochhadeunmöglichkeitsScore = RochhadeunwahrscheinlicherdurchTurmZugMultiplikator;
                                }
                            }
                            break;
                        case "S":
                            FigurenScore += BesitzerFaktor * SpringerMultiplikator;
                            //ZugmöglichkeitenScore += GetAllPossibleZüge(sit, new int[2] { A, B }).Length;
                            break;
                        case "L":
                            FigurenScore += BesitzerFaktor * LäuferMultiplikator;
                            //ZugmöglichkeitenScore += GetAllPossibleZüge(sit, new int[2] { A, B }).Length;
                            break;
                        case "B":
                            FigurenScore += BesitzerFaktor * BauernMultiplikator;
                            //ZugmöglichkeitenScore += GetAllPossibleZüge(sit, new int[2] { A, B }).Length;
                            break;
                    }
                }
            }
        }
        if (WarRochhade(sit, Z))
        {
            RochhadeScore = RochhadeMultiplikator;
        }
        if (FigurenScore > 0 && C.IstPatt(sit, sit.SpielerID, false))
        {
            return -1000000;
        }
        sit.SpielerID = 1 - sit.SpielerID;
        if (C.IstEinZugMöglich(sit, sit.SpielerID, true) == false)
        {
            //Debug.Log(sit.SpielerID + " ist angeblich matt, letzer Zug war " + sit.LetzterZug.ZugToString());
            return -1000000;
        }
        sit.SpielerID = 1 - sit.SpielerID;
        if (C.IstEinZugMöglich(sit,sit.SpielerID, true) == false)
        {
            /*string F = "";
            for(int A = 0; A < 8; A++)
            {
                string L = "";
                for(int B=0; B < 8; B++)
                {
                    
                    if (sit.Stellung[B, A].Typ == "")
                    {
                        L += "X ";
                    }
                    else
                    {
                        if (sit.Stellung[B, A].SpielerID == 0)
                        {
                            L += sit.Stellung[B, A].Typ + " ";
                        }
                        else
                        {
                            L += sit.Stellung[B, A].Typ.ToLower() + " ";
                        }
                    }
                }
                F += L + "\n";
            }
            System.IO.File.WriteAllText(Application.persistentDataPath + "/Fehler.txt", F);
            Debug.Log(sit.SpielerID + " würde gewinenn. Letzer Zug " + sit.LetzterZug.ZugToString());*/
            return 1000600;
        }
        FigurenScore *= FigurenWertMultiplikator;
        if (RochhadeunwahrscheinlicherdurchTurmZugMultiplikator*2 == RochhadeunmöglichkeitsScore)
        {
            Rochhadeunmöglich = true;
        }
        if (Rochhadeunmöglich)
        {
            RochhadeunmöglichkeitsScore = RochhadeunmöglichkeitsMultiplikator;
        }
        //ZugmöglichkeitenScore *= ZugmöglichkeitenMultiplikator;
        int GesammtScore = FigurenScore + ZugmöglichkeitenScore+RochhadeunmöglichkeitsScore + BerechneZentrumsKontrolleBonus(sit,Z);
        return GesammtScore;
    }
    public int BewerteZugVoraus(Situation sit2,Zug Z2,int Schritte)
    {
        Situation sit = sit2.Clone();
        Zug Z = Z2.Clone();
        sit.Forward(Z2);
        if(Schritte == 0)
        {
            sit.SpielerID = Mathf.Abs(sit.SpielerID - 1);
            return BewerteZug(sit,Z);
        }
        else
        {
            Schritte--;
            Zug[] MöglicheZüge = C.AlleMöglichenZüge(sit);
            if (MöglicheZüge.Length == 0)
            {
                return 1000000;
            }
            int BestScore = BewerteZugVoraus(sit,MöglicheZüge[0],Schritte);
            int[] BesteZüge = new int[1] { 0 };
            for (int A = 1; A < MöglicheZüge.Length; A++)
            {
                int Score = BewerteZugVoraus(sit, MöglicheZüge[A],Schritte);
                if (Score > BestScore)
                {
                    BestScore = Score;
                    BesteZüge = new int[1] { A };
                }
                else
                {
                    if (Score == BestScore && DebugModus)
                    {
                        System.Array.Resize(ref BesteZüge, BesteZüge.Length + 1);
                        BesteZüge[BesteZüge.Length - 1] = A;
                    }
                }
            }
            if (DebugModus)
            {
                if (Schritte == 0)
                {
                    foreach (int i in BesteZüge)
                    {
                        Debug.Log(Z.ZugToString() + " hat " + MöglicheZüge[i].ZugToString() + " als erwiederung " + BestScore + " Schritte übrig " + Schritte);
                    }
                }
            }
            sit.SpielerID = Mathf.Abs(sit.SpielerID - 1);
            int Bewertung = BewerteZug(sit, Z) - BestScore;
            sit.SpielerID = Mathf.Abs(sit.SpielerID - 1);
            return Bewertung;
        }
    }
    public override Zug ziehe(Situation sit)
    {
        Zug[] MöglicheZüge = C.AlleMöglichenZüge(sit);
        int BestScore = BewerteZugVoraus(sit,MöglicheZüge[0], Vorausbrechnungsrange);
        int[] BesteZüge = new int[1] { 0 };
        if (DebugModus)
        {
            Debug.Log("Zug " + MöglicheZüge[0].ZugToString() + " Score : " + BestScore);
        }
        for (int A = 1; A < MöglicheZüge.Length; A++)
        {
            int Score = BewerteZugVoraus(sit,MöglicheZüge[A], Vorausbrechnungsrange);
            if (DebugModus)
            {
                Debug.Log("Zug " + MöglicheZüge[A].ZugToString() + " Score : " + Score);
            }
            if (Score > BestScore)
            {
                BestScore = Score;
                BesteZüge = new int[1] {A};
            }
            else
            {
                if(Score == BestScore)
                {
                    System.Array.Resize(ref BesteZüge, BesteZüge.Length + 1);
                    BesteZüge[BesteZüge.Length - 1] = A;
                }
            }
        }
        return MöglicheZüge[BesteZüge[Random.Range(0, BesteZüge.Length)]];
    }
}
