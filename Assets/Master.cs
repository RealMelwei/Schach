using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Spielzugkontrollsystem, nicht ändern
/// </summary>
public class Master : MonoBehaviour
{
    /*public bool TakeTurnManual = false;
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
    

    public bool LogAllPossibleTurns = false;*/
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
    /// <summary>
    /// Sollen irgendwelche Züge gezogen werden, wird auch verwended vom Feature Replay A Game
    /// </summary>
    public string[] Replay = new string[0];
    /// <summary>
    /// Ist dafür da das Replay weiß bei welchem zug es ist
    /// </summary>
    private int Replaypos = 0;
    /// <summary>
    /// Ist für jedes Spiel eindeutig, startet bei 1, ab dem ersten Zug einer neuen Partie wird eine neue GameNummer erstellt, Zum Laden einer Partie einfach hier die GameNummer der Zuladenen Partie eingeben
    /// </summary>
    public int GameNumber = 0;
    /// <summary>
    /// Gibt an ob die Partie mit <seealso cref="GameNumber"/> geladen werden soll, ist diese Variable an, wird nicht weiter an der Zughistory geschrieben
    /// </summary>
    public bool ReplayAGame = false;
    /// <summary>
    /// Anzahl an Einzel Zügen, die geladen werden sollen
    /// </summary>
    public int ReplayAmountofTurns = 0;
    /// <summary>
    /// Kompfort Klasse
    /// </summary>
    public bool LoadNextTurn = false;
    Comfort C = new Comfort();

    void Awake()
    {
        if (Application.isEditor)
            Application.runInBackground = true;
    }

    /// <summary>
    /// Setzt die Startstellung
    /// </summary>
    private void Start()
    {
        if (ReplayAGame == false)
        {
            GameNumber = System.IO.Directory.GetFiles(Application.persistentDataPath).Length + 1;
        }
        else
        {
            string[] partie = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + GameNumber).Split('\n');
            Replay = new string[ReplayAmountofTurns];
            for (int A = 1; A < ReplayAmountofTurns + 1; A++)
            {
                Replay[A - 1] = partie[A];
            }
        }
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
        Application.runInBackground = true;

    }
    /// <summary>
    /// Kümmert sich um den Zugablauf
    /// </summary>
    //bool haschecked = false;
    private void Update()
    {
        /*if (haschecked == false)
        {
            haschecked = true;
            for (int A = 0; A < 8; A++)
            {
                for (int B = 0; B < 8; B++)
                {
                    for (int D = 0; D < 8; D++)
                    {
                        for (int E = 0; E < 8; E++)
                        {
                            Zug check = new Zug(new int[2] { A, B }, new int[2] { D, E });
                            double timeNow = System.DateTime.Now.Millisecond + System.DateTime.Now.Second * 1000 + System.DateTime.Now.Minute * 60000 + System.DateTime.Now.Hour * 60000 * 24;
                            bool legal = C.ZugLegal(Situ, check, true);
                            double timeNowNew = System.DateTime.Now.Millisecond + System.DateTime.Now.Second * 1000 + System.DateTime.Now.Minute * 60000 + System.DateTime.Now.Hour * 60000 * 24;
                            if (legal)
                            {
                                Debug.Log("Time needed: " + (timeNowNew - timeNow) + " for " + check.ZugToString());
                            }
                        }
                    }
                }
            }
        }*/
        if (Replay.Length !=0 && Replaypos < Replay.Length)
        {
            AI1IsPC = false;
            AI2IsPC = false;
            TakeTurn = true;
            ManualTurn = Replay[Replaypos];
            Replaypos++;
        }
        else
        {
            if (LoadNextTurn)
            {
                LoadNextTurn = false;
                string[] partie = System.IO.File.ReadAllText(Application.persistentDataPath + "/" + GameNumber).Split('\n');
                ReplayAmountofTurns++;
                ManualTurn = partie[ReplayAmountofTurns];
                TakeTurn = true;
            }
        }
        /*
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
            for (int i1 = 0; i1 < 8; i1++)
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
                                Debug.Log(ZugZuString(check));
                            }
                        }
                    }
                }
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
        }*/
        
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
                    GameObject.Find("Canvas").GetComponent<Feld>().SetzeFiguren(Situ);
                }
                if(ReplayAGame == false)
                {
                    if (System.IO.File.Exists(Application.persistentDataPath + "/" + GameNumber))
                    {
                        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GameNumber, System.IO.File.ReadAllText(Application.persistentDataPath + "/" + GameNumber) + "\n" + zug.ZugToString());
                    }
                    else
                    {
                        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GameNumber, "\n" + zug.ZugToString());
                    }
                }
                Debug.LogWarning(zug.ZugToString());
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
                        GameObject.Find("Canvas").GetComponent<Feld>().SetzeFiguren(Situ);
                    }
                    if (ReplayAGame == false)
                    {
                        if (System.IO.File.Exists(Application.persistentDataPath + "/" + GameNumber))
                        {
                            System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GameNumber, System.IO.File.ReadAllText(Application.persistentDataPath + "/" + GameNumber) + "\n" + zug.ZugToString());
                        }
                        else
                        {
                            System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GameNumber, "\n" + zug.ZugToString());
                        }
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
                    GameObject.Find("Canvas").GetComponent<Feld>().SetzeFiguren(Situ);
                }
                if (ReplayAGame == false)
                {
                    if (System.IO.File.Exists(Application.persistentDataPath + "/" + GameNumber))
                    {
                        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GameNumber, System.IO.File.ReadAllText(Application.persistentDataPath + "/" + GameNumber) + "\n" + zug.ZugToString());
                    }
                    else
                    {
                        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GameNumber, "\n" + zug.ZugToString());
                    }
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
                        GameObject.Find("Canvas").GetComponent<Feld>().SetzeFiguren(Situ);
                    }
                    if (ReplayAGame == false)
                    {
                        if (System.IO.File.Exists(Application.persistentDataPath + "/" + GameNumber))
                        {
                            System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GameNumber, System.IO.File.ReadAllText(Application.persistentDataPath + "/" + GameNumber) + "\n" + zug.ZugToString());
                        }
                        else
                        {
                            System.IO.File.WriteAllText(Application.persistentDataPath + "/" + GameNumber, "\n" + zug.ZugToString());
                        }
                    }
                    Debug.LogWarning(zug.ZugToString());
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
