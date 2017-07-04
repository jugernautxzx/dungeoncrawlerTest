using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface PartySelectionInterface
{
    bool OnAddToParty(int index);
    void RemoveFromParty(int index);
}

public class CharacterUI : MonoBehaviour
{
    public Text cName, cLevel;
    public GameObject inPartyNotice;

    PartySelectionInterface listener;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetListener(PartySelectionInterface listener)
    {
        this.listener = listener;
        GetComponent<Button>().onClick.AddListener(AddIntoParty);
    }

    public void LoadInformation(CharacterModel model)
    {
        if (model == null)
        {
            cName.text = "NONE";
            cLevel.text = "";
        }
        else
        {
            cName.text = model.name;
            cLevel.text = "Level " + model.level;
        }
    }

    public void SetInParty(bool inParty)
    {
        inPartyNotice.SetActive(inParty);
    }

    public void AddIntoParty()
    {
        if (inPartyNotice.activeInHierarchy)
        {
            listener.RemoveFromParty(transform.GetSiblingIndex());
            return;
        }
        if (listener.OnAddToParty(transform.GetSiblingIndex()))
            SetInParty(true);
        else
            SetInParty(false);
    }
}
