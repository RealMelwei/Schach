using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Die AIs, vererbte Klassen dürfen bearbeitet werden
/// </summary>
public abstract class AI : MonoBehaviour { 
    /// <summary>
    /// Lässt die AI einen Zug berechnen
    /// </summary>
    /// <param name="sit">Die momentane Situation</param>
    /// <returns>Gibt einen Zug zurück</returns>
    public abstract Zug ziehe(Situation sit);
    /// <summary>
    /// Gibt einen ZufälligenZug zurück
    /// </summary>
    /// <param name="sit">Die Situation für die ein Zufälliger Zug zurückgegeben werden soll</param>
    /// <returns>Gibt einen Zufälligen gültigen Zug zurück</returns>
    public Zug ZufälligerZug(Situation sit)
    {
        Comfort C = new Comfort();
        ArrayList Möglich = new ArrayList();
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
                            Möglich.Add(check);
                        }
                    }
                }
            }
        }
        int rand = Random.Range(0, Möglich.Count - 1);
        foreach (Zug i in Möglich)
        {
            if (rand == 0)
            {
                return i;
            }
            rand--;
        }
        throw new System.ArgumentException();
    }
}
