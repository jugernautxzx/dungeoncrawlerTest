using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RecruitmentInterface
{
    void RecruitNewCharacter(CharacterModel model);
}

public class RecruitmentManager : MonoBehaviour {

    RecruitmentInterface listener;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetListener(RecruitmentInterface listener)
    {
        this.listener = listener;
    }

    public void RecruitNewMember()
    {
        CharacterModel newModel = Debugger.GenerateCharacterModel("NewMan");
        PlayerSession.GetProfile().characters.Add(newModel);
        listener.RecruitNewCharacter(newModel);
    }

    public void Debug1()
    {
        Debugger.GenerateMainHand();
    }

    public void Debug2()
    {
        Debugger.GenerateOffHand();
    }
}
