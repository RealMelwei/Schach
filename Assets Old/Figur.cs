using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Enthält alle Figuren daten
/// </summary>
public class Figur {
    /// <summary>
    /// Gibt an um welche Figur es sich handelt("D","K","B","L","S","T") wenn auf einem Feld keine Figur ist, ist dieser Parameter ""
    /// </summary>
    public string Typ = "";
    /// <summary>
    /// Gibt an ob die Figur bereits bewegt wurde
    /// </summary>
    public bool Bewegt = false;
    /// <summary>
    /// Gibt den Besitzer der Figur an
    /// </summary>
    public int SpielerID = 0;
    /// <summary>
    /// Erstellt eine Figur
    /// </summary>
    /// <param name="pTyp">Die Figurenart(falls keine Figurexistent, ist dies "")</param>
    /// <param name="pSpielerID">Der Besitzer der Figur</param>
    /// <param name="pBewegt">Wurde die Figur schon bewegt</param>
    public Figur(string pTyp, int pSpielerID, bool pBewegt)
    {
        Typ = pTyp;
        Bewegt = pBewegt;
        SpielerID = pSpielerID;
    }
    /// <summary>
    /// Erstellt eine neue Identische Figur
    /// </summary>
    /// <returns>Gibt eine neue Identische Figur zurück</returns>
    public Figur Clone()
    {
        return new Figur(Typ, SpielerID, Bewegt);
    }
}
