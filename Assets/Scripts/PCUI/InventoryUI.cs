using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface EquipInterface {

    void OnItemEquiped(int index);

}

public class InventoryUI : MonoBehaviour, InventoryItemInterface
{
    public const int SORT_ACQUIRED = 0;
    public const int SORT_ATTACK = 1;
    public const int SORT_DEFENSE = 2;


    public Dropdown slotFilter;
    public Dropdown sortingFilter;
    public GameObject viewPort;

    public GameObject prefab;

    List<int> sorting = new List<int>();
    EquipInterface eqImpl;
    InventoryItemShowInterface showImpl;

    // Use this for initialization
    void Start()
    {
        if(slotFilter != null)
            slotFilter.onValueChanged.AddListener(ChangeFilter);
        sortingFilter.onValueChanged.AddListener(ChangeSorting);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        
    }

    public void SetItemShowImpl(InventoryItemShowInterface iis)
    {
        showImpl = iis;
    }

    public void SetEquipImpl(EquipInterface ei)
    {
        eqImpl = ei;
    }

    public void ChangeSorting(int sort)//TODO NOT IMPLEMENTED YET
    {
        //sorting.Clear();
        //LoadAllEquipment();
    }

    //0 Main hand, 1 offhand, 2 head, 3 body, 4 acc
    public void ChangeFilter(int code)
    {
        sorting.Clear();
        switch (code)
        {
            case 0:
                FilterEquipmentSlot(EqSlot.MainHand);
                break;
            case 1:
                FilterEquipmentSlot(EqSlot.OffHand);
                break;
            case 2:
                FilterEquipmentSlot(EqSlot.Head);
                break;
            case 3:
                FilterEquipmentSlot(EqSlot.Body);
                break;
            default:
                FilterEquipmentSlot(EqSlot.Accessory);
                break;
        }
        LoadAllEquipment();
    }

    void FilterEquipmentSlot(EqSlot filter)
    {
        int i = 0;
        foreach(Equipment model in PlayerSession.GetInventory().list)
        {
            if (model.slot == filter && !model.isUsed)
                sorting.Add(i);
            i++;
        }
    }

    void LoadAllEquipment()
    {
        StartCoroutine(LoadEquipmentRoutine());
    }

    IEnumerator LoadEquipmentRoutine()
    {
        yield return null;
        foreach (Transform child in viewPort.transform)
        {
            //Destroy(child.gameObject);
            child.gameObject.SetActive(false);
        }
        for(int i=0; i<sorting.Count; i++)
        {
            if (i < viewPort.transform.childCount)
                UpdateEquipmentItem(PlayerSession.GetInventory().list[sorting[i]], viewPort.transform.GetChild(i).GetComponent<InventoryItemUI>());
            else
                CreateNewEquipmentItem(PlayerSession.GetInventory().list[sorting[i]]);
        }
    }

    void UpdateEquipmentItem(Equipment model, InventoryItemUI ui)
    {
        ui.gameObject.SetActive(true);
        ui.SetModel(model, sortingFilter.value);
    }

    void CreateNewEquipmentItem(Equipment equipment)
    {
        GameObject eqItem = Instantiate(prefab, viewPort.transform, false);
        eqItem.GetComponent<InventoryItemUI>().SetModel(equipment, sortingFilter.value);
        if (eqImpl != null)
            eqItem.GetComponent<InventoryItemUI>().SetInterface(this);
        eqItem.GetComponent<InventoryItemUI>().SetShowInterface(showImpl);
    }

  
    public void OnItemClicked(int index, int mouseIndex)
    {
        if(mouseIndex == MouseInput.MOUSE_LEFT)
            eqImpl.OnItemEquiped(sorting[index]);
    }

    public void ClearItemList()
    {
        sorting.Clear();
        foreach (Transform child in viewPort.transform)
        {
            //Destroy(child.gameObject);
            child.gameObject.SetActive(false);
        }
    }
}
