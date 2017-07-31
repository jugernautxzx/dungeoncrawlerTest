using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface TooltipInterface
{
    void ShowTooltip(string text);
    void HideTooltip();
}

public class MainMenuUI : MonoBehaviour, CharCreationInterface, RecruitmentInterface, EquipmentUIInterface, EquipmentUILevelUpInterface, InventoryItemShowInterface, TooltipInterface, ConsumableInterface
{

    public CharCreationUI charCreate;
    public PartyManager partyManager;
    public RecruitmentManager recruitmentManager;
    public EquipmentUI equipmentUI;
    public InventoryUI inventoryUI;
    public ConsumableItemUI consumableUI;
    public RectTransform equipmentShowPopUp, tooltip;

    public void OnCreateNewChar()
    {
        partyManager.RequestUpdateMember();
        ShowPartyWindow();
    }

    // Use this for initialization
    void Start()
    {
        charCreate.SetListener(this);
        recruitmentManager.SetListener(this);
        partyManager.SetEquipmentImpl(this);
        inventoryUI.SetItemShowImpl(this);
        equipmentUI.SetEquipmentShowImpl(this);
        equipmentUI.SetLevelUpInterface(this);
        consumableUI.SetListener(this);
        ItemManager.GetInstance();
        LoadGameSession();

    }

    void LoadGameSession()
    {
        if (PlayerSession.GetInstance().IsSessionAvailable())
        {
            ContinueGameSession();
        }
        else
        {
            if (PlayerSession.GetInstance().LoadSession())
            {
                CreateNewSession();
            }
            else
            {
                ContinueGameSession();
            }
        }
    }

    void CreateNewSession()
    {
        charCreate.gameObject.SetActive(true);
        charCreate.SetForNewGame();
    }

    void ContinueGameSession()
    {

        //ADD on dungeon clear condition
        partyManager.RequestUpdateMember();
        ShowPartyWindow();
    }

    void Update()
    {
        if (equipmentShowPopUp.gameObject.activeInHierarchy)
        {
            UpdateEquipmentShowPos();
        }
        if (tooltip.gameObject.activeInHierarchy)
        {
            UpdateTooltipShowPos();
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
        if (newPos.x + width > Screen.width)
        {
            newPos.x -= (newPos.x + width) - Screen.width;
        }
        equipmentShowPopUp.transform.position = newPos;
    }

    void UpdateTooltipShowPos()
    {
        float height = tooltip.rect.height;
        float width = tooltip.rect.width;
        Vector3 newPos = Input.mousePosition;
        if (newPos.y - height < 0)
        {
            newPos.y += height - newPos.y;
        }
        if (newPos.x + width > Screen.width)
        {
            newPos.x -= (newPos.x + width) - Screen.width;
        }
        tooltip.transform.position = newPos;
    }

    public void StartMockupBattle()
    {
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

    public void ShowOwnedItems()
    {
        HideAllWindow();
        consumableUI.gameObject.SetActive(true);
        consumableUI.ChangeFilter(consumableUI.filter.value);
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
        consumableUI.gameObject.SetActive(false);
    }

    void LevelUpCharacter(CharacterModel model)
    {
        charCreate.gameObject.SetActive(true);
        charCreate.SetForLevelUp(model);
    }

    public void OnCharLevelUp()
    {
        partyManager.RequestUpdateMember();
        if (equipmentUI.gameObject.activeInHierarchy)
            equipmentUI.LoadAllAttributes();
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

    public void RequestLevelUpMenu(CharacterModel model)
    {
        LevelUpCharacter(model);
    }

    public void ShowTooltip(string text)
    {
        tooltip.GetComponent<TooltipUI>().SetText(text);
        tooltip.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltip.gameObject.SetActive(false);
    }

}
