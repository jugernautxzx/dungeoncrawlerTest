using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{

    public Text nameText;
    public RectTransform healthBar;
    public Text rowText;
    public Image rowColor;
    public GameObject statusEffect;
    public Button selectionButton;

    void Start()
    {
        EnableSelection(false);
    }

    public void EnableSelection(bool enable)
    {
        selectionButton.interactable = enable;
    }

    public void SetHealth(float percentage)
    {
        healthBar.localScale = new Vector2(percentage, 1);
    }

    public void AddClickListener(UnityEngine.Events.UnityAction call)
    {
        selectionButton.onClick.AddListener(call);
    }

    public void UpdateStatusEffect(CharacterModel model)
    {
        for (int i = 0; i < 8; i++)
        {
            statusEffect.transform.GetChild(i).gameObject.SetActive(false);
        }
        int j = 0;
        foreach(BattleBuff buff in model.battleAttribute.buffs)
        {
            statusEffect.transform.GetChild(j).gameObject.SetActive(true);
            statusEffect.transform.GetChild(j).GetComponent<StatusEffectUI>().SetInfoId(buff.nameInfo, buff.infoId);
            j++;
        }
    }

    public void UpdateCharacter(CharacterModel model)
    {
        nameText.text = model.name;
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
        SetHealth(model.battleAttribute.currHp / (float)model.battleAttribute.hp);
        UpdateStatusEffect(model);
    }
}
