using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface CharCreationInterface
{
    void OnCreateNewChar();
    void OnCharLevelUp();
}

public class CharCreationUI : MonoBehaviour
{
    public InputField pName;
    public InputField str, agi, intl, con, wis, end;
    public Button confirmButton;
    public Text remainingText;
    bool isNewGame;
    public CharCreationInterface listener;

    CharacterModel toLevelUp;

    int startingValue = 20;

    void Start()
    {
        str.onEndEdit.AddListener(delegate { OnValueChanged(str); });
        agi.onEndEdit.AddListener(delegate { OnValueChanged(agi); });
        intl.onEndEdit.AddListener(delegate { OnValueChanged(intl); });
        con.onEndEdit.AddListener(delegate { OnValueChanged(con); });
        wis.onEndEdit.AddListener(delegate { OnValueChanged(wis); });
        end.onEndEdit.AddListener(delegate { OnValueChanged(end); });
        pName.onEndEdit.AddListener(delegate { CheckValidForm(); });
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
        //pName.interactable = false;
        startingValue = 20;
        pName.text = model.name;
        toLevelUp = model;
        LoadModelAttribute(model.attribute);
        RecalculateRemainingValue();
    }

    void LoadModelAttribute(Attribute att)
    {
        str.text = att.str.ToString();
        intl.text = att.intel.ToString();
        agi.text = att.agi.ToString();
        wis.text = att.wisdom.ToString();
        con.text = att.cons.ToString();
        end.text = att.endurance.ToString();
    }

    public void SetForNewGame()
    {
        isNewGame = true;
        pName.interactable = true;
    }

    public void OnValueChanged(InputField field)
    {
        if (field.text.Length == 0)
        {
            field.text = "0";
            return;
        }
        field.text = Mathf.Clamp(int.Parse(field.text), 1, 15).ToString();
        RecalculateRemainingValue();
    }

    void RecalculateRemainingValue()
    {
        int strInt = int.Parse(str.text);
        int agiInt = int.Parse(agi.text);
        int intInt = int.Parse(intl.text);
        int conInt = int.Parse(con.text);
        int wisInt = int.Parse(wis.text);
        int endInt = int.Parse(end.text);
        startingValue = 20 - strInt - agiInt - intInt - conInt - wisInt - endInt;
        remainingText.text = startingValue + " points available";
        CheckValidForm();
    }

    public void CheckValidForm()
    {
        if (IsFormValid())
            confirmButton.interactable = true;
    }

    bool IsFormValid()
    {
        return startingValue == 0 && pName.text.Length > 3;
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
        model.attribute.endurance = int.Parse(end.text);
        model.attribute.agi = int.Parse(agi.text);
        model.attribute.cons = int.Parse(con.text);
        model.attribute.intel = int.Parse(intl.text);
        model.attribute.wisdom = int.Parse(wis.text);
        model.attribute.str = int.Parse(str.text);
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
        toLevelUp.attribute = new Attribute();
        toLevelUp.elemental = new ElementAttribute();
        toLevelUp.name = pName.text;
        toLevelUp.level += 1;
        toLevelUp.attribute.endurance = int.Parse(end.text);
        toLevelUp.attribute.agi = int.Parse(agi.text);
        toLevelUp.attribute.cons = int.Parse(con.text);
        toLevelUp.attribute.intel = int.Parse(intl.text);
        toLevelUp.attribute.wisdom = int.Parse(wis.text);
        toLevelUp.attribute.str = int.Parse(str.text);
        PlayerSession.GetInstance().SaveSession();
    }
}
