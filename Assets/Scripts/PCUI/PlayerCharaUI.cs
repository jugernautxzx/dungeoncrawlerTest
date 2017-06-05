using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharaUI : MonoBehaviour
{

    public Text charName;
    public Text charHp;
    public Text charMorale;
    public Text charMana;
    public Text charStamina;
    public Text rowText;
    public Image rowColor;
    public GameObject statusEffect;
    public Button selectionButton;
    public RectTransform turnBar;

    void Start()
    {
        EnableSelection(false);
    }

    public void EnableSelection(bool enable)
    {
        selectionButton.interactable = enable;
    }

    public void UpdateTurnBar(float percentage)
    {
        turnBar.localScale = new Vector2(percentage, 1);
    }

    public void AddClickListener(UnityEngine.Events.UnityAction call)
    {
        selectionButton.onClick.AddListener(call);
    }

    public void UpdateCharacter(CharacterModel model)
    {
        charName.text = model.name;
        charHp.text = model.battleAttribute.currHp + "/" + model.battleAttribute.hp;
        charMana.text = model.battleAttribute.currMp + "/" + model.battleAttribute.mp;
        charStamina.text = model.battleAttribute.stamina + "/100";
        charMorale.text = model.battleAttribute.morale + "";
        if (model.battleAttribute.backRow)
        {
            rowColor.color = Color.blue;
            rowText.text = "Back";
        }
        else
        {
            rowColor.color = Color.red;
            rowText.text = "Front";
        }
    }
}
