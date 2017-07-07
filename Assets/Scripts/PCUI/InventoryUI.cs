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

    public Dropdown slotFilter;
    public Dropdown sortingFilter;
    public GameObject viewPort;

    public GameObject prefab;

    List<int> sorting = new List<int>();
    EquipInterface eqImpl;

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
        sorting.Clear();
        foreach (Transform child in viewPort.transform)
        {
            Destroy(child.gameObject);
        }
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
                FilterEquipmentSlot(EqSlot.Acc);
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
            Destroy(child.gameObject);
        }
        foreach (int index in sorting)
        {
            LoadEquipment(PlayerSession.GetInventory().list[index]);
        }
    }

    void LoadEquipment(Equipment equipment)
    {
        GameObject eqItem = Instantiate(prefab, viewPort.transform, false);
        eqItem.GetComponent<InventoryItemUI>().SetModel(equipment);
        if (eqImpl != null)
            eqItem.GetComponent<InventoryItemUI>().SetInterface(this);
    }

  
    public void OnItemClicked(int index)
    {
        eqImpl.OnItemEquiped(sorting[index]);
    }
}
