using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Situation
{
    /// <summary>
    /// Die Stellung
    /// </summary>
    public Figur[,] Stellung = new Figur[8, 8];
    /// <summary>
    /// Wie viele Einzelzüge ist kein Bauer gezogen und keine Figur geschlagen worden
    /// </summary>
    public int Remiszähler = 0;
    /// <summary>
    /// Die SpielerID des Spielers am Zug, Weiß ist 0, Schwarz ist 1
    /// </summary>
    public int SpielerID = 0;
    /// <summary>
    /// Enthält den Letzten Zug
    /// </summary>
    public Zug LetzterZug;
    /// <summary>
    /// Erstellt eine neue Situation
    /// </summary>
    /// <param name="pStellung">Die Stellung der Figuren</param>
    /// <param name="pRemiszähler">Höhe des Remiszählers</param>
    /// <param name="pSpielerID">SpielerID des spielers am Zug</param>
    public Situation(Figur[,] pStellung, int pRemiszähler, int pSpielerID)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Stellung[i, j] = pStellung[i, j];
            }
        }
        Remiszähler = pRemiszähler;
        SpielerID = pSpielerID;
    }
    /// <summary>
    /// Erstellt eine Identische Situation
    /// </summary>
    /// <returns>Gibt eine Identische Situation zurück</returns>
    public Situation Clone()
    {
        Figur[,] StellungClone = new Figur[8, 8];
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                StellungClone[i, j] = Stellung[i, j].Clone();
            }
        }
        Situation ret = new Situation(StellungClone, Remiszähler, SpielerID);
        ret.LetzterZug = LetzterZug;
        return ret;
    }
    /// <summary>
    /// Zieht einen Zug in der gegebenen Stellung
    /// </summary>
    /// <param name="zug">Der Zug der gezogen werden soll</param>
    /// <param name="DebugMode">Sollen Debug informationen gegeben werden</param>
    public void Forward(Zug zug, bool DebugMode)
    {

        if (DebugMode) Debug.Log("Zug durchgeführt");
        Figur startFigur = Stellung[zug.Start[0], zug.Start[1]].Clone();
        if (startFigur.Typ == "B" || Stellung[zug.Ziel[0], zug.Ziel[1]].Typ != "")
        {
            Remiszähler = 0;
        }
        else
        {
            Remiszähler++;
        }
        if (startFigur.Typ == "B" && Mathf.Abs(zug.Start[0] - zug.Ziel[0]) == 1 && Stellung[zug.Ziel[0], zug.Ziel[1]].Typ == "")
        {
            Stellung[zug.Ziel[0], zug.Start[1]].Typ = "";
            if (DebugMode) Debug.Log("Em Passant");
        }
        Stellung[zug.Start[0], zug.Start[1]].Typ = "";
        Stellung[zug.Ziel[0], zug.Ziel[1]] = startFigur;
        Stellung[zug.Ziel[0], zug.Ziel[1]].Bewegt = true;

        if (startFigur.Typ == "K" && zug.Start[0] == 4 && zug.Ziel[0] == 2)
        {
            Stellung[0, zug.Ziel[1]].Typ = "";
            Stellung[3, zug.Ziel[1]] = new Figur("T", SpielerID, true);
            if (DebugMode) Debug.Log("Rochade");
        }
        if (startFigur.Typ == "K" && zug.Start[0] == 4 && zug.Ziel[0] == 6)
        {
            Stellung[7, zug.Ziel[1]].Typ = "";
            Stellung[5, zug.Ziel[1]] = new Figur("T", SpielerID, true);
            if (DebugMode) Debug.Log("Rochade");
        }
        if (startFigur.Typ == "B" && zug.Ziel[1] == -7 * (SpielerID - 1))
        {
            Stellung[zug.Ziel[0], zug.Ziel[1]].Typ = zug.Umwandlungsfigur;
        }
        LetzterZug = zug;
        SpielerID = (SpielerID + 1) % 2;
    }
    /// <summary>
    /// Zieht einen Zug in der gegebenen Stellung
    /// </summary>
    /// <param name="zug">Der Zug der gezogen werden soll</param>
    public void Forward(Zug zug)
    {
        Forward(zug, false);
    }
}
