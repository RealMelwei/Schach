using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feld : MonoBehaviour {
    public Transform WeißesFeld;
    public Transform SchwarzesFeld;
    public Transform Figurprefab;
    public bool Weiß = true;
    private Comfort C = new Comfort();
	// Use this for initialization
	void Start () {
		for(int A = 7; A >= 0; A--)
        {
            for (int B = 7; B >=0; B--)
            {
                if (Weiß)
                {
                    ErstelleWeißesFeld(A, B);
                }
                else
                {
                    ErstelleSchwarzesFeld(A, B);
                }
                Weiß = !Weiß;
            }
            Weiß = !Weiß;
        }
	}
	void ErstelleWeißesFeld(int Buchstabe,int Zahl)
    {
        GameObject obj = (GameObject)Instantiate(WeißesFeld.gameObject);
        Positioniere(Buchstabe, Zahl, obj);
    }
    void ErstelleSchwarzesFeld(int Buchstabe, int Zahl)
    {
        GameObject obj = (GameObject)Instantiate(SchwarzesFeld.gameObject);
        Positioniere(Buchstabe, Zahl, obj);
    }
    void Positioniere(int Buchstabe, int Zahl, GameObject obj)
    {
        int size = Screen.height / 8;
        obj.transform.SetParent(transform);
        obj.transform.position = new Vector3(Screen.width - size * (Zahl + 1), size * (Buchstabe + 1) - size / 2, 0);
        obj.transform.GetComponent<RectTransform>().sizeDelta = new Vector3(size, size);
        obj.name = C.FeldZuString(new int[] { Buchstabe, Zahl });
    }
    public void SetzeFiguren(Situation sit)
    {
        UnityEngine.UI.Text[] alleFiguren = GameObject.FindObjectsOfType<UnityEngine.UI.Text>();
        foreach(UnityEngine.UI.Text t in alleFiguren)
        {
            GameObject.Destroy(t.gameObject);
        }
        for(int A = 0; A < 8; A++)
        {
            for(int B = 0; B < 8; B++)
            {
                if(sit.Stellung[A,B].Typ != "")
                {
                    SetzeFigur(sit.Stellung[A, B], new int[] { B, 7-A });
                }
            }
        }
    }
    void SetzeFigur(Figur F, int[] pos)
    {
        GameObject obj = (GameObject)Instantiate(Figurprefab.gameObject);
        if (F.SpielerID == 0)
        {
            obj.GetComponent<UnityEngine.UI.Text>().text = F.Typ.ToUpper();
        }
        else { 
            obj.GetComponent<UnityEngine.UI.Text>().text = F.Typ.ToLower();
        }
        Positioniere(pos[0], pos[1], obj);
    }
}
