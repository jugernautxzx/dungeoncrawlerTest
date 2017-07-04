using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour, CharCreationInterface, RecruitmentInterface {

    public CharCreationUI charCreate;
    public PartyManager partyManager;
    public RecruitmentManager recruitmentManager;
    public EquipmentUI equipmentUI;
    public InventoryUI inventoryUI;

    public void OnCreateNewChar()
    {
        partyManager.UpdatePartyMembersInformation();
    }

    // Use this for initialization
    void Start () {
        charCreate.SetListener(this);
        recruitmentManager.SetListener(this);
        if (PlayerSession.GetInstance().LoadSession())
        {
            charCreate.gameObject.SetActive(true);
            charCreate.SetForNewGame();
        }
        else
        {
            partyManager.UpdatePartyMembersInformation();
        }
    }

    public void StartMockupBattle()
    {
        SceneManager.LoadScene(2);
    }
	
    public void ShowRecruitWindow()
    {
        HideAllWindow();
        recruitmentManager.gameObject.SetActive(true);
    }

    public void ShowPartyWindow()
    {
        HideAllWindow();
        partyManager.gameObject.SetActive(true);
        PlayerSession.GetInstance().SaveSession();
    }


    public void ShowInventory()
    {
        HideAllWindow();
        inventoryUI.gameObject.SetActive(true);
    }

    void HideAllWindow()
    {
        recruitmentManager.gameObject.SetActive(false);
        charCreate.gameObject.SetActive(false);
        partyManager.gameObject.SetActive(false);
        equipmentUI.gameObject.SetActive(false);
        inventoryUI.gameObject.SetActive(false);
    }

    void LevelUpCharacter(CharacterModel model)
    {
        charCreate.gameObject.SetActive(true);
        charCreate.SetForLevelUp(model);
    }

    public void OnCharLevelUp()
    {
        partyManager.UpdatePartyMembersInformation();
    }

    public void RecruitNewCharacter(CharacterModel model)
    {
        LevelUpCharacter(model);
    }
}
