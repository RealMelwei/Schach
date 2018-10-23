using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enthält Kompfort funktionen
/// </summary>
public class Comfort
{
    /// <summary>
    /// Prüft ob ein Zug in der gegebenen Situation legal ist
    /// </summary>
    /// <param name="sitIn">Die gegebene Situation</param>
    /// <param name="zugIn">Der zuprüfende Zug</param>
    /// <param name="Schachprüfen">Soll geprüft werden ob nach dem Zug der König im Schach steht</param>
    /// <returns>Gibt an ob der Zug erlaubt ist</returns>
    public bool ZugLegal(Situation sitIn, Zug zugIn, bool Schachprüfen)
    {
        return ZugLegal(sitIn, zugIn, Schachprüfen, false);
    }
    /// <summary>
    /// Prüft ob ein Zug in der gegebenen Situation legal ist
    /// </summary>
    /// <param name="sitIn">Die gegebene Situation</param>
    /// <param name="zugIn">Der zuprüfende Zug</param>
    /// <param name="Schachprüfen">Soll geprüft werden ob nach dem Zug der König im Schach steht</param>
    /// <param name="DebugMode">Sollen Meldungen warum der Zug verboten ist ausgeben werden</param>
    /// <returns>Gibt an ob der Zug erlaubt ist</returns>
    public bool ZugLegal(Situation sitIn, Zug zugIn, bool Schachprüfen, bool DebugMode)
    {
        Zug zug = zugIn.Clone();
        Situation sit = sitIn.Clone();
        if (zug.Start[0] < 0 || zug.Start[1] < 0 || zug.Start[0] > 7 || zug.Start[1] > 7 || zug.Ziel[0] < 0 || zug.Ziel[1] < 0 || zug.Ziel[0] > 7 || zug.Ziel[1] > 7)
        {
            if (DebugMode) Debug.LogError("Zugformat falsch. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
            return false;
        }
        Figur Fig = sit.Stellung[zug.Start[0], zug.Start[1]].Clone();
        Figur ZielFig = sit.Stellung[zug.Ziel[0], zug.Ziel[1]].Clone();
        int xDist = Mathf.Abs(zug.Start[0] - zug.Ziel[0]);
        int yDist = Mathf.Abs(zug.Start[1] - zug.Ziel[1]);

        if (Fig.SpielerID != sit.SpielerID || Fig.Typ == "")
        {
            if (DebugMode) Debug.LogError("Die Figur gehört dir nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
            return false;
        }
        if (ZielFig.SpielerID == sit.SpielerID && ZielFig.Typ != "")
        {
            if (DebugMode) Debug.LogError("Das Zielfeld ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
            return false;
        }
        switch (Fig.Typ)
        {
            case "B":
                if (xDist > 1 || yDist > 2)
                {
                    if (DebugMode) Debug.LogError("So zieht ein Bauer nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                    return false;
                }
                if (sit.SpielerID == 0)
                {
                    if (zug.Start[1] >= zug.Ziel[1])
                    {
                        if (DebugMode) Debug.LogError("So zieht ein Bauer nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                        return false;
                    }
                    if (xDist == 0)
                    {
                        if (ZielFig.Typ != "")
                        {
                            if (DebugMode) Debug.LogError("Das Zielfeld ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                        if (yDist == 2)
                        {
                            if (zug.Start[1] != 1)
                            {
                                if (DebugMode) Debug.LogError("So zieht ein Bauer nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                                return false;
                            }
                            if (sit.Stellung[zug.Start[0], zug.Start[1] + 1].Typ != "")
                            {
                                if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                                return false;
                            }
                        }

                    }
                    else
                    {
                        if (yDist != 1)
                        {
                            if (DebugMode) Debug.LogError("So zieht ein Bauer nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                        if (ZielFig.Typ == "")
                        {
                            if (sit.LetzterZug.Start[1] != 6 || sit.LetzterZug.Start[0] != zug.Ziel[0] || sit.LetzterZug.Ziel[0] != zug.Ziel[0] || sit.LetzterZug.Ziel[1] != 4 || sit.Stellung[sit.LetzterZug.Ziel[0], sit.LetzterZug.Ziel[1]].Typ != "B" || zug.Start[1] != 4)
                            {
                                if (DebugMode) Debug.LogError("Der Bauer darf nicht diagonal auf ein leeres Feld ziehen. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                                return false;
                            }

                        }
                    }
                    if (zug.Ziel[1] == 7)
                    {
                        if (zug.Umwandlungsfigur != "S" && zug.Umwandlungsfigur != "L" && zug.Umwandlungsfigur != "T" && zug.Umwandlungsfigur != "D")
                        {
                            if (DebugMode) Debug.LogError("Zu dieser Figur kann nicht umgewandelt werden. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                    }
                }
                else
                {
                    if (zug.Start[1] <= zug.Ziel[1])
                    {
                        if (DebugMode) Debug.LogError("So zieht ein Bauer nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                        return false;
                    }
                    if (xDist == 0)
                    {
                        if (ZielFig.Typ != "")
                        {
                            if (DebugMode) Debug.LogError("Das Zielfeld ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                        if (yDist == 2)
                        {
                            if (zug.Start[1] != 6)
                            {
                                if (DebugMode) Debug.LogError("So zieht ein Bauer nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                                return false;
                            }
                            if (sit.Stellung[zug.Start[0], zug.Start[1] - 1].Typ != "")
                            {
                                if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                                return false;
                            }
                        }

                    }
                    else
                    {
                        if (yDist != 1)
                        {
                            if (DebugMode) Debug.LogError("So zieht ein Bauer nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }

                        if (ZielFig.Typ == "")
                        {
                            if (sit.LetzterZug.Start[1] != 1 || sit.LetzterZug.Start[0] != zug.Ziel[0] || sit.LetzterZug.Ziel[0] != zug.Ziel[0] || sit.LetzterZug.Ziel[1] != 3 || sit.Stellung[sit.LetzterZug.Ziel[0], sit.LetzterZug.Ziel[1]].Typ != "B" || zug.Start[1] != 5)
                            {
                                if (DebugMode) Debug.LogError("Der Bauer darf nicht diagonal auf ein leeres Feld ziehen. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                                return false;
                            }
                        }
                    }
                    if (zug.Ziel[1] == 0)
                    {
                        if (zug.Umwandlungsfigur != "S" && zug.Umwandlungsfigur != "L" && zug.Umwandlungsfigur != "T" && zug.Umwandlungsfigur != "D")
                        {
                            if (DebugMode) Debug.LogError("Zu dieser Figur kann nicht umgewandelt werden. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                    }
                }
                break;

            case "S":
                if (!(xDist == 1 && yDist == 2) && !(xDist == 2 && yDist == 1))
                {
                    if (DebugMode) Debug.LogError("So zieht ein Springer nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                    return false;
                }
                break;

            case "L":
                if (xDist != yDist)
                {
                    if (DebugMode) Debug.LogError("So zieht ein Läufer nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                    return false;
                }
                for (int i = 1; i < xDist; i++)
                {
                    int xKoo = zug.Start[0] + i * (int)Mathf.Sign(zug.Ziel[0] - zug.Start[0]);
                    int yKoo = zug.Start[1] + i * (int)Mathf.Sign(zug.Ziel[1] - zug.Start[1]);
                    if (sit.Stellung[xKoo, yKoo].Typ != "")
                    {
                        if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                        return false;
                    }
                }
                break;

            case "T":
                if (xDist != 0 && yDist != 0)
                {
                    if (DebugMode) Debug.LogError("So zieht ein Turm nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                    return false;
                }
                if (xDist == 0)
                {
                    for (int i = 1; i < yDist; i++)
                    {
                        int Richtung = (int)Mathf.Sign(zug.Ziel[1] - zug.Start[1]);
                        if (sit.Stellung[zug.Start[0], zug.Start[1] + Richtung * i].Typ != "")
                        {
                            if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                    }
                }
                else
                {
                    for (int i = 1; i < xDist; i++)
                    {
                        int Richtung = (int)Mathf.Sign(zug.Ziel[0] - zug.Start[0]);
                        if (sit.Stellung[zug.Start[0] + Richtung * i, zug.Start[1]].Typ != "")
                        {
                            if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                    }
                }
                break;

            case "D":
                if (xDist != 0 && yDist != 0 && xDist != yDist)
                {
                    if (DebugMode) Debug.LogError("So zieht eine Dame nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                    return false;
                }
                if (xDist == yDist)
                {
                    for (int i = 1; i < xDist; i++)
                    {
                        int xKoo = zug.Start[0] + i * (int)Mathf.Sign(zug.Ziel[0] - zug.Start[0]);
                        int yKoo = zug.Start[1] + i * (int)Mathf.Sign(zug.Ziel[1] - zug.Start[1]);
                        if (sit.Stellung[xKoo, yKoo].Typ != "")
                        {
                            if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                    }
                    break;
                }
                else
                {
                    if (xDist == 0)
                    {
                        for (int i = 1; i < yDist; i++)
                        {
                            int Richtung = (int)Mathf.Sign(zug.Ziel[1] - zug.Start[1]);
                            if (sit.Stellung[zug.Start[0], zug.Start[1] + Richtung * i].Typ != "")
                            {
                                if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                                return false;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < xDist; i++)
                        {
                            int Richtung = (int)Mathf.Sign(zug.Ziel[0] - zug.Start[0]);
                            if (sit.Stellung[zug.Start[0] + Richtung * i, zug.Start[1]].Typ != "")
                            {
                                if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                                return false;
                            }
                        }
                    }
                }
                break;

            case "K":
                if (xDist > 1 || yDist > 1)
                {
                    if (zug.Start[0] != 4 || zug.Start[1] != sit.SpielerID * 7 || xDist != 2 || yDist != 0)
                    {
                        if (DebugMode) Debug.LogError("So zieht ein König nicht. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                        return false;
                    }
                    if (IstAngegriffen(sit, zug.Start, 1, false, false))
                    {
                        if (DebugMode) Debug.LogError("Aus dem Schach kann nicht rochiert werden. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                        return false;
                    }
                    if (Fig.Bewegt == true)
                    {
                        if (DebugMode) Debug.LogError("Der König hat schon gezogen. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                        return false;
                    }
                    if (zug.Start[0] > zug.Ziel[0])
                    {
                        if (sit.Stellung[0, sit.SpielerID * 7].Typ != "T" || sit.Stellung[1, sit.SpielerID * 7].Typ != "" || sit.Stellung[2, sit.SpielerID * 7].Typ != "" || sit.Stellung[3, sit.SpielerID * 7].Typ != "")
                        {
                            if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                        if (IstAngegriffen(sit, new int[] { 3, sit.SpielerID * 7 }, -sit.SpielerID + 1, false, false))
                        {
                            if (DebugMode) Debug.LogError("Durch das Schach kann nicht rochiert werden. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                        if (sit.Stellung[0, sit.SpielerID * 7].Bewegt)
                        {
                            if (DebugMode) Debug.LogError("Der Turm hat schon gezogen. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                    }
                    else
                    {
                        if (sit.Stellung[5, sit.SpielerID * 7].Typ != "" || sit.Stellung[6, sit.SpielerID * 7].Typ != "" || sit.Stellung[7, sit.SpielerID * 7].Typ != "T")
                        {
                            if (DebugMode) Debug.LogError("Der Weg ist blockiert. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                        if (IstAngegriffen(sit, new int[] { 5, sit.SpielerID * 7 }, -sit.SpielerID + 1, false, false))
                        {
                            if (DebugMode) Debug.LogError("Durch das Schach kann nicht rochiert werden. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                        if (sit.Stellung[7, sit.SpielerID * 7].Bewegt)
                        {
                            if (DebugMode) Debug.LogError("Der Turm hat schon gezogen. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                            return false;
                        }
                    }

                }
                break;
        }
        if (Schachprüfen)
        {
            Situation sit2 = sit.Clone();
            sit2.Forward(zug, false);
            int[] Königsfeld = null;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (sit2.Stellung[i, j].Typ == "K" && sit2.Stellung[i, j].SpielerID == sit.SpielerID)
                    {
                        Königsfeld = new int[] { i, j };
                    }
                }
            }
            if (Königsfeld == null)
            {
                if (DebugMode) Debug.LogError("Kein eigener König nach dem Zug...? Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                return false;
            }
            if (IstAngegriffen(sit2, Königsfeld, sit2.SpielerID, false, false))
            {
                if (DebugMode) Debug.LogError("Der König darf nach dem Zug nicht im Schach stehen. Zug war: " + zug.ZugToString() + ", Spieler: " + sit.SpielerID);
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Prüft ob ein Feld von einem Spieler angegriffen wird
    /// </summary>
    /// <param name="sit">Die Stellung in der geprüft werden soll</param>
    /// <param name="Feld">Das Feld das geprüft werden soll</param>
    /// <param name="VonSpieler">Der Spieler für den geprüft werden soll ob dieser das Feld angreift</param>
    /// <param name="Schachprüfen">Sollen nicht mögliche Angriffe(durch Fesslungen) ignoriert werden</param>
    /// <param name="DebugMode">Sollen Debug Informationen ausgegeben werden</param>
    /// <returns>Gibt an ob ein Feld angegriffen wird</returns>
    public bool IstAngegriffen(Situation sit, int[] Feld, int VonSpieler, bool Schachprüfen, bool DebugMode)
    {
        Situation WorkWithMe = sit.Clone();
        WorkWithMe.SpielerID = VonSpieler;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (ZugLegal(WorkWithMe, new Zug(new int[] { i, j }, Feld, "D"), Schachprüfen, DebugMode))
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Prüft ob ein Zug möglich ist
    /// </summary>
    /// <param name="sit">Die Momentane Stellung</param>
    /// <param name="AlsSpieler">Spieler für den geprüft werden soll ob ein Zug möglich ist</param>
    /// <param name="Schachprüfen">Sollen Züge die Wegen Fesslungen nicht möglich sind als illegal erkannt werden</param>
    /// <returns>Gibt an ob ein Zug möglich ist</returns>
    public bool IstEinZugMöglich(Situation sit, int AlsSpieler, bool Schachprüfen)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (sit.Stellung[i, j].SpielerID == AlsSpieler && sit.Stellung[i, j].Typ != "")
                {
                    switch (sit.Stellung[i, j].Typ)
                    {
                        case "K":
                            for (int k = -1; k < 2; k++)
                            {
                                for (int l = -1; l < 2; l++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + l }, "D"), Schachprüfen, false))
                                    {
                                        return true;
                                    }
                                }
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 2, j }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 2, j }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            break;
                        case "S":
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j + 2 }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j + 2 }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j - 2 }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j - 2 }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 2, j + 1 }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 2, j + 1 }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 2, j - 1 }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 2, j - 1 }, "D"), Schachprüfen, false))
                            {
                                return true;
                            }
                            break;
                        case "B":
                            if (j == AlsSpieler * 5 + 1)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, AlsSpieler + 3 }, "D"), Schachprüfen, false))
                                {
                                    return true;
                                }
                            }
                            if (AlsSpieler == 0)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j + 1 }, "D"), Schachprüfen, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, j + 1 }, "D"), Schachprüfen, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j + 1 }, "D"), Schachprüfen, false))
                                {
                                    return true;
                                }
                            } else
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j - 1 }, "D"), Schachprüfen, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, j - 1 }, "D"), Schachprüfen, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j - 1 }, "D"), Schachprüfen, false))
                                {
                                    return true;
                                }
                            }
                            break;
                        case "T":
                            if (!Schachprüfen)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, j + 1 }, "D"), false, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, j - 1 }, "D"), false, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j }, "D"), false, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j }, "D"), false, false))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                for (int k = i + 1; k < 8; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = i - 1; k >= 0; k--)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = j + 1; k < 8; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = j - 1; k >= 0; k--)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                            }
                            break;
                        case "L":
                            if (!Schachprüfen)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j + 1 }, "D"), false, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j - 1 }, "D"), false, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j + 1 }, "D"), false, false))
                                {
                                    return true;
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j - 1 }, "D"), false, false))
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                for (int k = 1; i + k < 8 && j + k < 8; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = 1; i - k >= 0 && j + k < 8; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = 1; i + k < 8 && j - k >= 0; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = 1; i - k >= 0 && j - k >= 0; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                            }
                            break;
                        case "D":
                            if (!Schachprüfen)
                            {
                                for (int k = -1; k < 2; k++)
                                {
                                    for (int l = -1; l < 2; l++)
                                    {
                                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + l }, "D"), false, false))
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int k = 1; i + k < 8 && j + k < 8; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = 1; i - k >= 0 && j + k < 8; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = 1; i + k < 8 && j - k >= 0; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = 1; i - k >= 0 && j - k >= 0; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = i + 1; k < 8; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = i - 1; k >= 0; k--)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = j + 1; k < 8; k++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                                for (int k = j - 1; k >= 0; k--)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), true, false))
                                    {
                                        return true;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }
        return false;
    }
    /// <summary>
    /// prüft ob ein Spieler im Schach steht
    /// </summary>
    /// <param name="sit">Situation zu prüfen</param>
    /// <param name="AlsSpieler">Spieler für den auf Schach geprüft werden soll</param>
    /// <param name="DebugMode">Sollen Debug Informationen ausgegeben werden</param>
    /// <returns>Gibt an ob ein Spieler im Schach steht</returns>
    public bool IstImSchach(Situation sit, int AlsSpieler, bool DebugMode)
    {
        int[] Königsfeld = null;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (sit.Stellung[i, j].Typ == "K" && sit.Stellung[i, j].SpielerID == AlsSpieler)
                {
                    Königsfeld = new int[] { i, j };
                }
            }
        }
        if (Königsfeld == null)
        {
            if (DebugMode) Debug.LogError("Kein eigener König...?");
            return false;
        }
        if (!IstAngegriffen(sit, Königsfeld, -AlsSpieler + 1, false, false))
        {
            if (DebugMode) Debug.LogError("Nope, kein Schach!");
            return false;
        }
        return true;
    }
    /// <summary>
    /// prüft ob ein Spieler im Schach steht
    /// </summary>
    /// <param name="sit">Situation zu prüfen</param>
    /// <param name="AlsSpieler">Spieler für den auf Schach geprüft werden soll</param>
    /// <returns>Gibt an ob ein Spieler im Schach steht</returns>
    public bool IstImSchach(Situation sit, int AlsSpieler)
    {
        return IstImSchach(sit, AlsSpieler, false);
    }
    /// <summary>
    /// Prüft ob ein Spieler Matt ist
    /// </summary>
    /// <param name="sit">Sitation in der geprüft werden soll</param>
    /// <param name="AlsSpieler">Spieler für den auf Matt geprüft werden soll</param>
    /// <param name="DebugMode">Sollen Debugmeldungen ausgeben werden</param>
    /// <returns>Gibt an ob ein Spieler Matt ist</returns>
    public bool IstMatt(Situation sit, int AlsSpieler, bool DebugMode)
    {
        if (IstEinZugMöglich(sit, AlsSpieler, true))
        {
            if (DebugMode) Debug.LogError("Es sind noch Züge möglich.");
            return false;
        }
        if (!IstImSchach(sit, AlsSpieler, DebugMode))
        {
            if (DebugMode) Debug.LogError("Patt, nicht Matt.");
            return false;
        }
        if (DebugMode) Debug.Log("Matt!");
        return true;
    }
    /// <summary>
    /// Prüft ob ein Spieler Matt ist
    /// </summary>
    /// <param name="sit">Sitation in der geprüft werden soll</param>
    /// <param name="AlsSpieler">Spieler für den auf Matt geprüft werden soll</param>
    /// <returns>Gibt an ob ein Spieler Matt ist</returns>
    public bool IstMatt(Situation sit, int AlsSpieler)
    {
        return IstMatt(sit, AlsSpieler, false);
    }
    /// <summary>
    /// Prüft ob ein Spieler Patt ist
    /// </summary>
    /// <param name="sit">Sitation in der geprüft werden soll</param>
    /// <param name="AlsSpieler">Spieler für den auf Patt geprüft werden soll</param>
    /// <param name="DebugMode">Sollen Debugmeldungen ausgeben werden</param>
    /// <returns>Gibt an ob ein Spieler Patt ist</returns>
    public bool IstPatt(Situation sit, int AlsSpieler, bool DebugMode)
    {
        if (IstEinZugMöglich(sit, AlsSpieler, true))
        {
            if (DebugMode) Debug.LogError("Es sind noch Züge möglich.");
            return false;
        }
        if (IstImSchach(sit, AlsSpieler, DebugMode))
        {
            if (DebugMode) Debug.LogError("Matt, nicht Patt.");
            return false;
        }
        if (DebugMode) Debug.Log("Matt!");
        return true;
    }
    /// <summary>
    /// Erstellt aus einem String einen Zug
    /// </summary>
    /// <param name="ein">Der String aus dem der Zug erstellt werden soll</param>
    /// <returns>Der Zu dem string passende Zug</returns>
    public Zug StringZuZug(string ein)
    {
        Zug ret;
        if (ein.Length == 4)
        {
            ret = new Zug(StringZuFeld(ein[0].ToString() + ein[1].ToString()), StringZuFeld(ein[2].ToString() + ein[3].ToString()), "");

        }
        else
        {
            ret = new Zug(StringZuFeld(ein[0].ToString() + ein[1].ToString()), StringZuFeld(ein[2].ToString() + ein[3].ToString()), ein[4].ToString());
        }
        return ret;
    }
    /// <summary>
    /// Gibt für einen String das Feld zürck
    /// </summary>
    /// <param name="ein">Der String der umgewandelt werden soll</param>
    /// <returns>Das Feld als int[] Array</returns>
    public int[] StringZuFeld(string ein)
    {
        int[] ret = { 0, 0 };
        switch (ein[0])
        {
            case 'A':
                ret[0] = 0;
                break;
            case 'B':
                ret[0] = 1;
                break;
            case 'C':
                ret[0] = 2;
                break;
            case 'D':
                ret[0] = 3;
                break;
            case 'E':
                ret[0] = 4;
                break;
            case 'F':
                ret[0] = 5;
                break;
            case 'G':
                ret[0] = 6;
                break;
            case 'H':
                ret[0] = 7;
                break;
        }
        ret[1] = int.Parse(ein[1].ToString()) - 1;
        return ret;
    }
    /// <summary>
    /// Macht aus einem int[] Feld ein String Feld
    /// </summary>
    /// <param name="Feld">Das Feld das umgewandelt werdne soll</param>
    /// <returns>Der String der das Feld representiert</returns>
    public string FeldZuString(int[] Feld)
    {
        string ret = "";
        switch (Feld[0])
        {
            case 0:
                ret += "A";
                break;
            case 1:
                ret += "B";
                break;
            case 2:
                ret += "C";
                break;
            case 3:
                ret += "D";
                break;
            case 4:
                ret += "E";
                break;
            case 5:
                ret += "F";
                break;
            case 6:
                ret += "G";
                break;
            case 7:
                ret += "H";
                break;
        }
        ret += (Feld[1] + 1).ToString();
        return ret;
    }

    /// <summary>
    /// Gibt ein Array aller in der Situation möglichen Züge zurück.
    /// </summary>
    /// <param name="sit">Die Situation, von der die möglichen Züge gesucht werden.</param>
    /// <returns>Ein Array der möglichen Züge.</returns>
    public Zug[] AlleMöglichenZüge(Situation sit, bool Schachprüfen)
    {
        ArrayList Möglich = new ArrayList();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (sit.Stellung[i, j].SpielerID == sit.SpielerID && sit.Stellung[i, j].Typ != "")
                {
                    switch (sit.Stellung[i, j].Typ)
                    {
                        case "K":
                            for (int k = -1; k < 2; k++)
                            {
                                for (int l = -1; l < 2; l++)
                                {
                                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + l }, "D"), Schachprüfen, false))
                                    {
                                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j + l }, "D"));
                                    }
                                }
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 2, j }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 2, j }, "D"));
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 2, j }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 2, j }, "D"));
                            }
                            break;
                        case "S":
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j + 2 }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 1, j + 2 }, "D"));
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j + 2 }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 1, j + 2 }, "D"));
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j - 2 }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 1, j - 2 }, "D"));
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j - 2 }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 1, j - 2 }, "D"));
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 2, j + 1 }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 2, j + 1 }, "D"));
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 2, j + 1 }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 2, j + 1 }, "D"));
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 2, j - 1 }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 2, j - 1 }, "D"));
                            }
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 2, j - 1 }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 2, j - 1 }, "D"));
                            }
                            break;
                        case "B":
                            if (j == sit.SpielerID * 5 + 1)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, sit.SpielerID + 3 }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, sit.SpielerID + 3 }, "D"));
                                }
                            }
                            if (sit.SpielerID==0)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j + 1 }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 1, j + 1 }, "D"));
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, j + 1 }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, j + 1 }, "D"));
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j + 1 }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 1, j + 1 }, "D"));
                                }
                            } else
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j - 1 }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 1, j - 1 }, "D"));
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, j - 1 }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, j - 1 }, "D"));
                                }
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j - 1 }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 1, j - 1 }, "D"));
                                }
                            }
                            break;
                        case "T":

                            for (int k = i + 1; k < 8; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { k, j }, "D"));
                                }
                            }
                            for (int k = i - 1; k >= 0; k--)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { k, j }, "D"));
                                }
                            }
                            for (int k = j + 1; k < 8; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, k }, "D"));
                                }
                            }
                            for (int k = j - 1; k >= 0; k--)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, k }, "D"));
                                }
                            }
                            break;
                        case "L":
                            for (int k = 1; i + k < 8 && j + k < 8; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"));
                                }
                            }
                            for (int k = 1; i - k >= 0 && j + k < 8; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"));
                                }
                            }
                            for (int k = 1; i + k < 8 && j - k >= 0; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"));
                                }
                            }
                            for (int k = 1; i - k >= 0 && j - k >= 0; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"));
                                }
                            }

                            break;
                        case "D":
                            for (int k = 1; i + k < 8 && j + k < 8; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"));
                                }
                            }
                            for (int k = 1; i - k >= 0 && j + k < 8; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"));
                                }
                            }
                            for (int k = 1; i + k < 8 && j - k >= 0; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"));
                                }
                            }
                            for (int k = 1; i - k >= 0 && j - k >= 0; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"));
                                }
                            }
                            for (int k = i + 1; k < 8; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { k, j }, "D"));
                                }
                            }
                            for (int k = i - 1; k >= 0; k--)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { k, j }, "D"));
                                }
                            }
                            for (int k = j + 1; k < 8; k++)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, k }, "D"));
                                }
                            }
                            for (int k = j - 1; k >= 0; k--)
                            {
                                if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), Schachprüfen, false))
                                {
                                    Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, k }, "D"));
                                }
                            }

                            break;
                    }
                }
            }
        }
        object[] MöglichArray = Möglich.ToArray();
        Zug[] ZügeMöglich = new Zug[MöglichArray.Length];
        for (int i = 0; i < MöglichArray.Length; i++)
        {
            ZügeMöglich[i] = (Zug)MöglichArray[i];
        }
        return ZügeMöglich;
    }
    /// <summary>
    /// Gibt ein Array aller in der Situation möglichen Züge, deren Startfeld mit dem der Eingabe übereinstimmt, zurück.
    /// </summary>
    /// <param name="sit">Die Situation, von der die möglichen Züge gesucht werden.</param>
    /// <param name="feld">Das Feld, von dem die Züge ausgehen müssen.</param>
    /// <returns>Ein Array der möglichen Züge.</returns>
    public Zug[] AlleZügeVomFeld(Situation sit, int[] Feld, bool Schachprüfen)
    {
        int i = Feld[0];
        int j = Feld[1];
        ArrayList Möglich = new ArrayList();
        if (sit.Stellung[i, j].SpielerID == sit.SpielerID && sit.Stellung[i, j].Typ != "")
        {
            switch (sit.Stellung[i, j].Typ)
            {
                case "K":
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + l }, "D"), Schachprüfen, false))
                            {
                                Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j + l }, "D"));
                            }
                        }
                    }
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 2, j }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 2, j }, "D"));
                    }
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 2, j }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 2, j }, "D"));
                    }
                    break;
                case "S":
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j + 2 }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 1, j + 2 }, "D"));
                    }
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j + 2 }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 1, j + 2 }, "D"));
                    }
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j - 2 }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 1, j - 2 }, "D"));
                    }
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j - 2 }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 1, j - 2 }, "D"));
                    }
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 2, j + 1 }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 2, j + 1 }, "D"));
                    }
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 2, j + 1 }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 2, j + 1 }, "D"));
                    }
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 2, j - 1 }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 2, j - 1 }, "D"));
                    }
                    if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 2, j - 1 }, "D"), Schachprüfen, false))
                    {
                        Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 2, j - 1 }, "D"));
                    }
                    break;
                case "B":
                    if (j == sit.SpielerID * 5 + 1)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, sit.SpielerID + 3 }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, sit.SpielerID + 3 }, "D"));
                        }
                    }
                    if (sit.SpielerID == 0)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j + 1 }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 1, j + 1 }, "D"));
                        }
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, j + 1 }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, j + 1 }, "D"));
                        }
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j + 1 }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 1, j + 1 }, "D"));
                        }
                    } else
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - 1, j - 1 }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - 1, j - 1 }, "D"));
                        }
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, j - 1 }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, j - 1 }, "D"));
                        }
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + 1, j - 1 }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + 1, j - 1 }, "D"));
                        }
                    }
                    break;
                case "T":

                    for (int k = i + 1; k < 8; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { k, j }, "D"));
                        }
                    }
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { k, j }, "D"));
                        }
                    }
                    for (int k = j + 1; k < 8; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, k }, "D"));
                        }
                    }
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, k }, "D"));
                        }
                    }
                    break;
                case "L":
                    for (int k = 1; i + k < 8 && j + k < 8; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"));
                        }
                    }
                    for (int k = 1; i - k >= 0 && j + k < 8; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"));
                        }
                    }
                    for (int k = 1; i + k < 8 && j - k >= 0; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"));
                        }
                    }
                    for (int k = 1; i - k >= 0 && j - k >= 0; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"));
                        }
                    }

                    break;
                case "D":
                    for (int k = 1; i + k < 8 && j + k < 8; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j + k }, "D"));
                        }
                    }
                    for (int k = 1; i - k >= 0 && j + k < 8; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - k, j + k }, "D"));
                        }
                    }
                    for (int k = 1; i + k < 8 && j - k >= 0; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i + k, j - k }, "D"));
                        }
                    }
                    for (int k = 1; i - k >= 0 && j - k >= 0; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i - k, j - k }, "D"));
                        }
                    }
                    for (int k = i + 1; k < 8; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { k, j }, "D"));
                        }
                    }
                    for (int k = i - 1; k >= 0; k--)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { k, j }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { k, j }, "D"));
                        }
                    }
                    for (int k = j + 1; k < 8; k++)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, k }, "D"));
                        }
                    }
                    for (int k = j - 1; k >= 0; k--)
                    {
                        if (ZugLegal(sit, new Zug(new int[] { i, j }, new int[] { i, k }, "D"), Schachprüfen, false))
                        {
                            Möglich.Add(new Zug(new int[] { i, j }, new int[] { i, k }, "D"));
                        }
                    }

                    break;
            }
        }
        object[] MöglichArray = Möglich.ToArray();
        Zug[] ZügeMöglich = new Zug[MöglichArray.Length];
        for (int k = 0; k < MöglichArray.Length; k++)
        {
            ZügeMöglich[k] = (Zug)MöglichArray[k];
        }
        return ZügeMöglich;
    }


}