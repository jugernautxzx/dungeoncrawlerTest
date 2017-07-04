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
        CharacterModel newModel = GenerateNewCharacter();
        PlayerSession.GetProfile().characters.Add(newModel);
        listener.RecruitNewCharacter(newModel);
    }

    CharacterModel GenerateNewCharacter()
    {
        CharacterModel model = new CharacterModel();
        model.attribute = new Attribute();
        model.attribute.agi = 1;
        model.attribute.str = 1;
        model.attribute.endurance = 1;
        model.attribute.cons = 1;
        model.attribute.intel = 1;
        model.attribute.wisdom = 1;
        model.name = "NewCharacter";
        model.battleSetting = new BattleSetting();
        return model;
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
