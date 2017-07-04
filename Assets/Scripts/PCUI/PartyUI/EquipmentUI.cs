using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour {

    public Text[] actives;
    public Text[] pasives;
    public Text[] traits;
    public Text cname, clevel, str, agi, con, wis, intl, end;
    public Text eqMain, eqOff, eqHead, eqBody, eqAcc1, eqAcc2;

    CharacterModel model;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCharacterModel(CharacterModel model)
    {
        this.model = model;
        SetAttributeText(str, model.attribute.str, model.eqAttribute.str);
        SetAttributeText(agi, model.attribute.agi, model.eqAttribute.agi);
        SetAttributeText(con, model.attribute.cons, model.eqAttribute.cons);
        SetAttributeText(wis, model.attribute.wisdom, model.eqAttribute.wisdom);
        SetAttributeText(intl, model.attribute.intel, model.eqAttribute.intel);
        SetAttributeText(end, model.attribute.endurance, model.eqAttribute.endurance);
    }

    void SetAttributeText(Text uiText, int baseStat, int eqStat)
    {
        uiText.text = baseStat.ToString() + " + " + eqStat.ToString();
    }
}
