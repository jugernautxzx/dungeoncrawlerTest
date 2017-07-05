using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface EquipmentUIInterface
{
    void LoadEquipmentUI(CharacterModel model);
}

public class EquipmentUI : MonoBehaviour, EquipInterface {

    public Text[] actives;
    public Text[] pasives;
    public Text[] traits;
    public Text cname, clevel, str, agi, con, wis, intl, end;
    public Text eqMain, eqOff, eqHead, eqBody, eqAcc1, eqAcc2;
    public InventoryUI invUI;

    CharacterModel model;
    EqSlot selectedSlot;

	// Use this for initialization
	void Start () {
        eqMain.GetComponent<Button>().onClick.AddListener(delegate { OnEquipmentClicked(EqSlot.MainHand); });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetCharacterModel(CharacterModel model)
    {
        this.model = model;
        cname.text = model.name;
        LoadAllAttributes();
        LoadAllActives();
        LoadAllPassives();
    }

    void LoadAllAttributes()
    {
        model.CalculateEqAttribute();
        SetAttributeText(str, model.attribute.str, model.eqAttribute.str);
        SetAttributeText(agi, model.attribute.agi, model.eqAttribute.agi);
        SetAttributeText(con, model.attribute.cons, model.eqAttribute.cons);
        SetAttributeText(wis, model.attribute.wisdom, model.eqAttribute.wisdom);
        SetAttributeText(intl, model.attribute.intel, model.eqAttribute.intel);
        SetAttributeText(end, model.attribute.endurance, model.eqAttribute.endurance);
    }

    void LoadAllActives()
    {
        int i = 0;
        foreach (string actId in model.actives)
        {
            SetActives(actives[i], actId);
            i++;
        }
        i = 0;
    }

    void LoadAllPassives()
    {

    }

    void SetAttributeText(Text uiText, int baseStat, int eqStat)
    {
        uiText.text = baseStat.ToString() + " + " + eqStat.ToString();
    }

    void SetActives(Text act, string activeName)
    {
        if (activeName == null || activeName.Length == 0)
        {
            act.text = "None";
        }
        else
        {
            act.text = ActiveSkillManager.GetInstance().GetActive(activeName).name;
        }
    }

    void OnEquipmentClicked(EqSlot slot)
    {
        if (invUI.isActiveAndEnabled)
            invUI.gameObject.SetActive(false);
        else
            invUI.gameObject.SetActive(true);
    }

    public void OnItemEquiped(int index)
    {
        
    }

    void ChangeEquipment(EqSlot slot, int index)
    {
        selectedSlot = slot;
        model.battleSetting.mainHand = index;
    }
}
