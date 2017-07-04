using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public Dropdown slotFilter;
    public Dropdown sortingFilter;
    public GameObject viewPort;

    public GameObject prefab;

    List<int> sorting;

    // Use this for initialization
    void Start()
    {
        slotFilter.onValueChanged.AddListener(ChangeFilter);
        sortingFilter.onValueChanged.AddListener(ChangeSorting);
        sorting = new List<int>();
        FilterMainHand();
        LoadAllEquipment();
    }

    // Update is called once per frame
    void Update()
    {

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
                FilterMainHand();
                break;
            case 1:
                FilterOffHand();
                break;
        }
        LoadAllEquipment();
    }

    void FilterMainHand()
    {
        int i = 0;
        foreach(Equipment model in PlayerSession.GetInventory().list)
        {
            if (model.slot == EqSlot.MainHand && !model.isUsed)
                sorting.Add(i);
            i++;
        }
    }

    void FilterOffHand()
    {
        int i = 0;
        foreach (Equipment model in PlayerSession.GetInventory().list)
        {
            if (model.slot == EqSlot.OffHand && !model.isUsed)
                sorting.Add(i);
            i++;
        }
    }

    void FilterHead()
    {

    }

    void FilterBody()
    {

    }

    void FilterAcc()
    {

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
    }
}
