using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour, CharCreationInterface, RecruitmentInterface, EquipmentUIInterface, InventoryItemShowInterface
{

    public CharCreationUI charCreate;
    public PartyManager partyManager;
    public RecruitmentManager recruitmentManager;
    public EquipmentUI equipmentUI;
    public InventoryUI inventoryUI;
    public RectTransform equipmentShowPopUp;

    public void OnCreateNewChar()
    {
        partyManager.UpdatePartyMembersInformation();
    }

    // Use this for initialization
    void Start()
    {
        charCreate.SetListener(this);
        recruitmentManager.SetListener(this);
        partyManager.SetEquipmentImpl(this);
        inventoryUI.SetItemShowImpl(this);
        equipmentUI.SetEquipmentShowImpl(this);
        if (PlayerSession.GetInstance().LoadSession())
        {
            charCreate.gameObject.SetActive(true);
            charCreate.SetForNewGame();
        }
        else
        {
            partyManager.RequestUpdateMember();
            ShowPartyWindow();
        }

    }

    void Update()
    {
        if (equipmentShowPopUp.gameObject.activeInHierarchy)
        {
            UpdateEquipmentShowPos();
        }
    }

    void UpdateEquipmentShowPos()
    {
        float height = equipmentShowPopUp.rect.height;
        float width = equipmentShowPopUp.rect.width;
        Vector3 newPos = Input.mousePosition;
        if (newPos.y - height < 0)
        {
            newPos.y += height - newPos.y;
        }
        if(newPos.x + width > Screen.width)
        {
            newPos.x -= (newPos.x + width) - Screen.width;
        }
        equipmentShowPopUp.transform.position = newPos;
    }

    public void StartMockupBattle()
    {
        //TODO Move to dungeon generation
        SceneManager.LoadScene(1);

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
        inventoryUI.ChangeFilter(inventoryUI.slotFilter.value);
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
        partyManager.RequestUpdateMember();
    }

    public void RecruitNewCharacter(CharacterModel model)
    {
        LevelUpCharacter(model);
    }

    public void LoadEquipmentUI(CharacterModel model)
    {
        equipmentUI.SetCharacterModel(model);
        HideAllWindow();
        equipmentUI.gameObject.SetActive(true);
    }

    public void OnShowEquipmentStatus(Equipment model, Vector2 pos)
    {
        equipmentShowPopUp.gameObject.SetActive(true);
        equipmentShowPopUp.GetComponent<EquipmentShowUI>().setModel(model);
    }

    public void OnCloseEquipmentStatus()
    {
        equipmentShowPopUp.gameObject.SetActive(false);
    }
}
