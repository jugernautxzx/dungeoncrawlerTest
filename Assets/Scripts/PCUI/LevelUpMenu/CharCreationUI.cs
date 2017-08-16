using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface CharCreationInterface
{
    void OnCreateNewChar();
    void OnCharLevelUp();
}

public class CharCreationUI : MonoBehaviour, AttributeFieldInterface
{
    public InputField pName;
    public AttributeField attStr, attAgi, attInt, attCon, attWis, attEnd;
    public Button confirmButton;
    public Text remainingText;
    bool isNewGame;
    public CharCreationInterface listener;

    CharacterModel toLevelUp;
    Attribute initialAttr;

    int startingValue;
    int bonusValue;
    int remainingValue;

    void Start()
    {
        SetListener();
    }

    void SetListener()
    {
        pName.onEndEdit.AddListener(delegate { CheckValidForm(); });
        attStr.SetInterface(this);
        attAgi.SetInterface(this);
        attInt.SetInterface(this);
        attCon.SetInterface(this);
        attWis.SetInterface(this);
        attEnd.SetInterface(this);
    }

    void Update()
    {
    }

    public void SetListener(CharCreationInterface listen)
    {
        listener = listen;
    }

    public void SetForLevelUp(CharacterModel model)
    {
        //TODO Badly need new recalculation on total attributes
        isNewGame = false;
        if (model.exp > 0)
            bonusValue = LevelCalculator.CalculateGainedLevel(model.exp, model.level) * 3;
        else
            bonusValue = 15;
        //pName.interactable = false;
        initialAttr = model.attribute.DeepCopy();
        pName.text = model.name;
        toLevelUp = model;
        LoadModelAttribute(model.attribute);
        startingValue = TotalAttributeValue();
        RecalculateRemainingValue();
    }

    void LoadModelAttribute(Attribute att)
    {
        attStr.SetValue(att.str, true);
        attAgi.SetValue(att.agi, true);
        attEnd.SetValue(att.endurance, true);
        attInt.SetValue(att.intel, true);
        attWis.SetValue(att.wisdom, true);
        attCon.SetValue(att.cons, true);
    }

    public void SetForNewGame()
    {
        isNewGame = true;
        pName.interactable = true;
        bonusValue = 21;
        RecalculateRemainingValue();
    }

    int TotalAttributeValue()
    {
        return attStr.GetValue() + attAgi.GetValue() + attInt.GetValue() + attCon.GetValue() + attWis.GetValue() + attEnd.GetValue();
    }

    void RecalculateRemainingValue()
    {
        remainingValue = bonusValue - (TotalAttributeValue() - startingValue);
        remainingText.text = remainingValue + " points available";
        CheckValidForm();
    }

    public void CheckValidForm()
    {
        confirmButton.interactable = IsFormValid();
    }

    bool IsFormValid()
    {
        return remainingValue == 0 && pName.text.Length > 3;
    }

    public void ConfirmClicked()
    {
        gameObject.SetActive(false);
        if (isNewGame)
        {
            CreateNewProfile();
            listener.OnCreateNewChar();
        }
        else
        {
            UpdateModelAttribute();
            listener.OnCharLevelUp();
        }
    }

    void CreateNewProfile()
    {
        CharacterModel model = new CharacterModel();
        model.attribute = new Attribute();
        model.elemental = new ElementAttribute();
        model.name = pName.text;
        model.level = 1;
        model.attribute.endurance = attEnd.GetValue();
        model.attribute.agi = attAgi.GetValue();
        model.attribute.cons = attCon.GetValue();
        model.attribute.intel = attInt.GetValue();
        model.attribute.wisdom = attWis.GetValue();
        model.attribute.str = attStr.GetValue();
        model.battleSetting = new BattleSetting();
        model.actives = new List<string>();
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.actives.Add("");
        model.passives = new List<string>();
        model.passives.Add("");
        model.passives.Add("");
        model.passives.Add("");
        model.passives.Add("");
        model.passives.Add("");
        PlayerSession.GetInstance().CreateNewSession(model);
        PlayerSession.GetInstance().SaveSession();
    }

    void UpdateModelAttribute()
    {
        toLevelUp.levelUp = false;
        toLevelUp.attribute = new Attribute();
        toLevelUp.elemental = new ElementAttribute();
        toLevelUp.name = pName.text;
        toLevelUp.level += LevelCalculator.CalculateGainedLevel(toLevelUp.exp, toLevelUp.level);
        toLevelUp.attribute.endurance = attEnd.GetValue();
        toLevelUp.attribute.agi = attAgi.GetValue();
        toLevelUp.attribute.cons = attCon.GetValue();
        toLevelUp.attribute.intel = attInt.GetValue();
        toLevelUp.attribute.wisdom = attWis.GetValue();
        toLevelUp.attribute.str = attStr.GetValue();
        PlayerSession.GetInstance().SaveSession();
    }

    public void OnValueIncrease()
    {
        RecalculateRemainingValue();
    }

    public void OnValueDecrease()
    {
        RecalculateRemainingValue();
    }
}
