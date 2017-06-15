using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (PlayerSession.GetInstance().LoadSession())
        {

        }
        else
        {
            PlayerSession.GetInstance().CreateNewSession(null);
            PlayerSession.GetInstance().SaveSession();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
