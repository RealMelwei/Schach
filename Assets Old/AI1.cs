using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI1 : AI {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override Zug ziehe(Situation sit)
    {
        return ZufälligerZug(sit);
    }

}
