using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentShowUI : MonoBehaviour {

    public Text eqName, eqType;
    public Text[] bonusTxt;
    public Text[] bonusDigit;

    public ContentSizeFitter fitter;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setModel(Equipment eq)
    {
        eqName.text = eq.name;
        if (eq.slot == EqSlot.MainHand)
            eqType.text = eq.weapon.ToString();
        else
            eqType.text = eq.slot.ToString();
        UpdateEquipmentBonus(eq.battle.basePAtk, bonusTxt[0], bonusDigit[0]);
        UpdateEquipmentBonus(eq.battle.baseMatk, bonusTxt[1], bonusDigit[1]);
        UpdateEquipmentBonus(eq.battle.basePDef, bonusTxt[2], bonusDigit[2]);
        UpdateEquipmentBonus(eq.battle.baseMDef, bonusTxt[3], bonusDigit[3]);
        UpdateEquipmentBonus(eq.battle.hp, bonusTxt[4], bonusDigit[4]);
        UpdateEquipmentBonus(eq.battle.mp, bonusTxt[5], bonusDigit[5]);
        UpdateEquipmentBonus(eq.battle.stamina, bonusTxt[6], bonusDigit[6]);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    void UpdateEquipmentBonus(int value, Text txt, Text digit)
    {
        if(value == 0)
        {
            txt.gameObject.SetActive(false);
            digit.gameObject.SetActive(false);
        }
        else
        {
            txt.gameObject.SetActive(true);
            digit.gameObject.SetActive(true);
            digit.text = value.ToString();
        }
    }
}
