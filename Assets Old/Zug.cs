using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zug {
    /// <summary>
    /// StartFeld des Zuges
    /// </summary>
    public int[] Start;
    /// <summary>
    /// ZielFeld des Zuges
    /// </summary>
    public int[] Ziel;
    /// <summary>
    /// In was soll die Figur umgewandelt werden, um sicher zu sein, das kein Fehler in der Umwandlung auftritt immer "D" setzen
    /// </summary>
    public string Umwandlungsfigur;
    /// <summary>
    /// Erstellt einen Zug
    /// </summary>
    /// <param name="pStartfeld">Von wo soll gezogen werden</param>
    /// <param name="pZielfeld">Nach wo soll gezogen werden</param>
    /// <param name="pUmwandlungsfigur">in was soll umgewandelt werden</param>
    public Zug(int[] pStartfeld, int[] pZielfeld, string pUmwandlungsfigur)
    {
        Start = new int[2];
        Start[0] = pStartfeld[0];
        Start[1] = pStartfeld[1];
        Ziel = new int[2];
        Ziel[0] = pZielfeld[0];
        Ziel[1] = pZielfeld[1];
        Umwandlungsfigur = pUmwandlungsfigur;
    }
    /// <summary>
    /// Erstellt einen Zug
    /// </summary>
    /// <param name="pStartfeld">Von wo soll gezogen werden</param>
    /// <param name="pZielfeld">Nach wo soll gezogen werden</param>
    /// <param name="pUmwandlungsfigur">in was soll umgewandelt werden</param>
    public Zug(string pStartfeld, string pZielfeld, string pUmwandlungsfigur)
    {
        Comfort C = new Comfort();
        Start = C.StringZuFeld(pStartfeld);        
        Ziel = C.StringZuFeld(pZielfeld);
        Umwandlungsfigur = pUmwandlungsfigur;
    }
    /// <summary>
    /// Erstellt einen Zug(Umwandlungs figur ist "D")
    /// </summary>
    /// <param name="pStartfeld">Von wo soll gezogen werden</param>
    /// <param name="pZielfeld">Nach wo soll gezogen werden</param>
    /// <param name="pUmwandlungsfigur">in was soll umgewandelt werden</param>
    public Zug(int[] pStartfeld, int[] pZielfeld)
    {
        Start = new int[2];
        Start[0] = pStartfeld[0];
        Start[1] = pStartfeld[1];
        Ziel = new int[2];
        Ziel[0] = pZielfeld[0];
        Ziel[1] = pZielfeld[1];
        Umwandlungsfigur = "D";
    }
    /// <summary>
    /// Erstellt einen Zug(Umwandlungs figur ist "D")
    /// </summary>
    /// <param name="pStartfeld">Von wo soll gezogen werden</param>
    /// <param name="pZielfeld">Nach wo soll gezogen werden</param>
    public Zug(string pStartfeld, string pZielfeld)
    {
        Comfort C = new Comfort();
        Start = C.StringZuFeld(pStartfeld);
        Ziel = C.StringZuFeld(pZielfeld);
        Umwandlungsfigur = "D";
    }
    /// <summary>
    /// Wandelt einen Zug in einen String um
    /// </summary>
    /// <returns>gibt einen String zurück, der den Zug representiert</returns>
    public string ZugToString()
    {
        return zuBuchstabe(Start[0]) + (Start[1] + 1).ToString() + zuBuchstabe(Ziel[0]) + (Ziel[1] + 1).ToString();
    }
    /// <summary>
    /// Erstellt einen Identische Zug
    /// </summary>
    /// <returns>Gibt einen Identischen Zug zurück</returns>
    public Zug Clone()
    {
        int[] StartClone = new int[] { Start[0], Start[1] };
        int[] ZielClone = new int[] { Ziel[0], Ziel[1] };
        return new Zug(StartClone, ZielClone, Umwandlungsfigur);
    }
    /// <summary>
    /// Macht einen Integer zu einem Buchstaben
    /// </summary>
    /// <param name="input">Integer der umgewandelt werden soll</param>
    /// <returns>gibt einen Buchstaben zurück, der den Integer representiert</returns>
    private string zuBuchstabe(int input)
    {
        switch (input)
        {
            case 0:
                return "A";
            case 1:
                return "B";
            case 2:
                return "C";
            case 3:
                return "D";
            case 4:
                return "E";
            case 5:
                return "F";
            case 6:
                return "G";
            case 7:
                return "H";
        }
        Debug.LogError("Falsches Zahlenformat");
        return "Spam";
    } 
}
