using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RecruitmentInterface
{
    void RecruitNewCharacter(CharacterModel model);
}

public class RecruitmentManager : MonoBehaviour
{

    RecruitmentInterface listener;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetListener(RecruitmentInterface listener)
    {
        this.listener = listener;
    }

    public void RecruitNewMember()
    {
        CharacterModel newModel = GenerateCharacter();
        PlayerSession.GetProfile().characters.Add(newModel);
        listener.RecruitNewCharacter(newModel);
    }

    CharacterModel GenerateCharacter()
    {
        CharacterModel model = new CharacterModel();
        model.name = "";
        model.attribute = new Attribute();
        model.attribute.agi = 1;
        model.attribute.cons = 1;
        model.attribute.endurance = 1;
        model.attribute.intel = 1;
        model.attribute.wisdom = 1;
        model.attribute.str = 1;
        model.attribute.speed = 1;
        model.battleSetting = new BattleSetting();
        model.elemental = new ElementAttribute();
        model.learnActive = new List<string>();
        model.actives = new List<string>();
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.passives = new List<string>();
        return model;
    }

    public void Debug1()
    {
        Debugger.GenerateMainHand();
        //GeneratePotion();
    }

    public void Debug2()
    {
        //Debugger.GenerateOffHand();
        GenerateSkillBook();
    }

    void GeneratePotion()
    {
        PlayerSession.GetProfile().AddItem("OldPot", 1);
    }

    void GenerateSkillBook()
    {
        PlayerSession.GetProfile().AddItem("Skill1", 1);
    }
}
