using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Spielzugkontrollsystem, nicht ändern
/// </summary>
public class Master : MonoBehaviour
{
    public bool TakeTurnManual = false;
    public bool CheckTurnManual = false;
    public string CheckTurnManualString;
    public string ManualLastTurn;
    public int PlayerID = 0;

    public bool ReadSituation = false;
    public bool WriteSituation = false;

    public string[] ManualSituation1 = new string[8];
    public string[] ManualSituation2 = new string[8];
    public string[] ManualSituation3 = new string[8];
    public string[] ManualSituation4 = new string[8];
    public string[] ManualSituation5 = new string[8];
    public string[] ManualSituation6 = new string[8];
    public string[] ManualSituation7 = new string[8];
    public string[] ManualSituation8 = new string[8];
    

    public bool LogAllPossibleTurns = false;
    /// <summary>
    /// Situation des Originalen Spielbrettes
    /// </summary>
    Situation Situ;

    /// <summary>
    /// Script von AI1, nicht verändern innerhalb von Programmcode
    /// </summary>
    public AI AI1;
    /// <summary>
    /// Script von AI1, nicht verändern innerhalb von Programmcode
    /// </summary>
    public AI AI2;
    /// <summary>
    /// Muss auf true gesetzt werden, wenn Weiß vom Computer gespielt werden soll
    /// </summary>
    public bool AI1IsPC = true;
    /// <summary>
    /// Muss auf true gesetzt werden, wenn Schwarz vom Computer gespielt werden soll
    /// </summary>
    public bool AI2IsPC = true;
    /// <summary>
    /// Muss auf true gesetzt werden, damit <seealso cref="ManualTurn"/> als Spielerzug ausgelesen wird und gezogen wird
    /// </summary>
    public bool TakeTurn = false;
    /// <summary>
    /// Spielerzug
    /// </summary>
    public string ManualTurn;

    Comfort C = new Comfort();

    /// <summary>
    /// Setzt die Startstellung
    /// </summary>
    private void Start()
    {
        /*
        ManualSituation1 = new string[8];
        ManualSituation2 = new string[8];
        ManualSituation3 = new string[8];
        ManualSituation4 = new string[8];
        ManualSituation5 = new string[8];
        ManualSituation6 = new string[8];
        ManualSituation7 = new string[8];
        ManualSituation8 = new string[8];*/
        Figur[,] figs = new Figur[8, 8];
        Situ = new Situation(figs, 0, 0);
        for (int i = 0; i < 8; i++)
        {
            Situ.Stellung[i, 1] = new Figur("B", 0, false);
            Situ.Stellung[i, 2] = new Figur("", 0, false);
            Situ.Stellung[i, 3] = new Figur("", 0, false);
            Situ.Stellung[i, 4] = new Figur("", 0, false);
            Situ.Stellung[i, 5] = new Figur("", 0, false);
            Situ.Stellung[i, 6] = new Figur("B", 1, false);
        }
        Situ.Stellung[0, 0] = new Figur("T", 0, false);
        Situ.Stellung[1, 0] = new Figur("S", 0, false);
        Situ.Stellung[2, 0] = new Figur("L", 0, false);
        Situ.Stellung[3, 0] = new Figur("D", 0, false);
        Situ.Stellung[4, 0] = new Figur("K", 0, false);
        Situ.Stellung[5, 0] = new Figur("L", 0, false);
        Situ.Stellung[6, 0] = new Figur("S", 0, false);
        Situ.Stellung[7, 0] = new Figur("T", 0, false);

        Situ.Stellung[0, 7] = new Figur("T", 1, false);
        Situ.Stellung[1, 7] = new Figur("S", 1, false);
        Situ.Stellung[2, 7] = new Figur("L", 1, false);
        Situ.Stellung[3, 7] = new Figur("D", 1, false);
        Situ.Stellung[4, 7] = new Figur("K", 1, false);
        Situ.Stellung[5, 7] = new Figur("L", 1, false);
        Situ.Stellung[6, 7] = new Figur("S", 1, false);
        Situ.Stellung[7, 7] = new Figur("T", 1, false);

        Situ.LetzterZug = new Zug(new int[] { 0, 0 }, new int[] { 1, 1 }, "D");
    }
    /// <summary>
    /// Kümmert sich um den Zugablauf
    /// </summary>
    private void Update()
    {
        if (CheckTurnManual)
        {
            CheckTurnManual = false;
            if(C.ZugLegal(Situ, C.StringZuZug(CheckTurnManualString),true, true))
            {
                Debug.Log("Zug Legal");
            } else
            {
                Debug.Log("Zug Illegal");
            }
        }
        if (LogAllPossibleTurns)
        {
            LogAllPossibleTurns = false;
            Debug.Log("Starte Testlog");
            /*for (int i1 = 0; i1 < 8; i1++)
            {
                for (int i2 = 0; i2 < 8; i2++)
                {
                    for (int i3 = 0; i3 < 8; i3++)
                    {
                        for (int i4 = 0; i4 < 8; i4++)
                        {
                            Zug check = new Zug(new int[] { i1, i2 }, new int[] { i3, i4 }, "D");
                            if (C.ZugLegal(Situ, check, true, false))
                            {
                                Debug.Log(check.ZugToString());
                            }
                        }
                    }
                }
            }*/
            Zug[] LogZüge = C.AlleMöglichenZüge(Situ, true);
            for(int i=0; i < LogZüge.Length; i++)
            {
                Debug.Log(LogZüge[i].ZugToString());
            }
        }
        if (ReadSituation)
        {
            ReadSituation = false;
            for(int i = 0; i < 8; i++)
            {
                ManualSituation1[i] = Situ.Stellung[i, 0].Typ;
                ManualSituation2[i] = Situ.Stellung[i, 1].Typ;
                ManualSituation3[i] = Situ.Stellung[i, 2].Typ;
                ManualSituation4[i] = Situ.Stellung[i, 3].Typ;
                ManualSituation5[i] = Situ.Stellung[i, 4].Typ;
                ManualSituation6[i] = Situ.Stellung[i, 5].Typ;
                ManualSituation7[i] = Situ.Stellung[i, 6].Typ;
                ManualSituation8[i] = Situ.Stellung[i, 7].Typ;
            }
        }
        if (WriteSituation)
        {
            WriteSituation = false;
            for (int i = 0; i < 8; i++)
            {
                Situ.Stellung[i, 0].Typ = ManualSituation1[i];
                Situ.Stellung[i, 1].Typ = ManualSituation2[i];
                Situ.Stellung[i, 2].Typ = ManualSituation3[i];
                Situ.Stellung[i, 3].Typ = ManualSituation4[i];
                Situ.Stellung[i, 4].Typ = ManualSituation5[i];
                Situ.Stellung[i, 5].Typ = ManualSituation6[i];
                Situ.Stellung[i, 6].Typ = ManualSituation7[i];
                Situ.Stellung[i, 7].Typ = ManualSituation8[i];
            }
        }
        if (TakeTurnManual)
        {
            TakeTurnManual = false;
            Situ.Forward(C.StringZuZug(CheckTurnManualString));
        }
        
        if (Situ.SpielerID == 0)
        {
            if (AI1IsPC)
            {
                double timeNow = System.DateTime.Now.Millisecond + System.DateTime.Now.Second * 1000 + System.DateTime.Now.Minute * 60000 + System.DateTime.Now.Hour * 60000 * 24;
                Zug zug = AI1.ziehe(Situ.Clone()).Clone();
                double timeNowNew = System.DateTime.Now.Millisecond + System.DateTime.Now.Second * 1000 + System.DateTime.Now.Minute * 60000 + System.DateTime.Now.Hour * 60000 * 24; Debug.Log("Time needed: " + (timeNowNew-timeNow));
                if (!C.ZugLegal(Situ, zug, true, false))
                {
                    Debug.LogError("Falscher Zug von Weiß");
                    C.ZugLegal(Situ, zug, true, true);
                    return;
                }
                else
                {
                    Situ.Forward(zug, false);
                }
                Debug.LogWarning(zug.ZugToString() + ", " + Situ.Remiszähler);
            } else
            {
                if (TakeTurn)
                {
                    TakeTurn = false;
                    Zug zug = C.StringZuZug(ManualTurn);
                    if (!C.ZugLegal(Situ, zug, true, false))
                    {
                        Debug.LogError("Falscher Zug von Weiß");
                        C.ZugLegal(Situ, zug, true, true);
                    }
                    else
                    {
                        Situ.Forward(zug, false);
                    }
                    Debug.LogWarning(zug.ZugToString());
                }
            }
            if (!C.IstEinZugMöglich(Situ, Situ.SpielerID, true))
            {
                if (C.IstImSchach(Situ, Situ.SpielerID, false))
                {
                    Debug.LogError("Weiß hat gewonnen.");
                    return;
                }
                else
                {
                    Debug.LogError("Remis durch Patt.");
                    return;
                }
            }
        } else
        {
            if (AI2IsPC)
            {
                double timeNow = System.DateTime.Now.Millisecond + System.DateTime.Now.Second * 1000 + System.DateTime.Now.Minute * 60000 + System.DateTime.Now.Hour * 60000 * 24;
                Zug zug = AI2.ziehe(Situ.Clone()).Clone();
                double timeNowNew = System.DateTime.Now.Millisecond + System.DateTime.Now.Second * 1000 + System.DateTime.Now.Minute * 60000 + System.DateTime.Now.Hour * 60000 * 24;
                Debug.Log("Time needed: " + (timeNowNew-timeNow));
                if (!C.ZugLegal(Situ, zug, true, false))
                {
                    Debug.LogError("Falscher Zug von Schwarz");
                    C.ZugLegal(Situ, zug, true, true);
                    return;
                }
                else
                {
                    Situ.Forward(zug, false);
                }
                Debug.LogWarning(zug.ZugToString());
            }
            else
            {
                if (TakeTurn)
                {
                    TakeTurn = false;
                    Zug zug = C.StringZuZug(ManualTurn);
                    if (!C.ZugLegal(Situ, zug, true, false))
                    {
                        Debug.LogError("Falscher Zug von Schwarz");
                        C.ZugLegal(Situ, zug, true, true);
                    }
                    else
                    {
                        Situ.Forward(zug, false);
                    }
                    Debug.LogWarning(zug.ZugToString()+", "+Situ.Remiszähler);
                }
            }
            if (!C.IstEinZugMöglich(Situ, Situ.SpielerID, true))
            {
                if (C.IstImSchach(Situ, Situ.SpielerID, false))
                {
                    Debug.LogError("Schwarz hat gewonnen.");
                    return;
                }
                else
                {
                    Debug.LogError("Remis durch Patt.");
                    return;
                }
            }
        }

        if (Situ.Remiszähler >= 100)
        {
            Debug.LogError("Remis durch 50 Züge.");
        }
    }
}
