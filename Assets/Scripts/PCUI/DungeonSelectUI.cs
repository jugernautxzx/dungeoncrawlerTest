using UnityEngine;

public class DungeonSelectUI : MonoBehaviour, IDungeonSelect
{
    public GameObject dungeonList;
    public GameObject levelList;
    public GameObject dungeonPrefab;
    public GameObject levelPrefab;

    // Use this for initialization
    void Start()
    {
        DungeonSelectManager.GetInstance();
        InitializeListener();
        UpdateDungeonList();
    }

    void InitializeListener()
    {
        foreach(Transform dungeon in dungeonList.transform)
        {
            dungeon.GetComponent<DungeonSelectItemUI>().SetDungeonSelectListener(this);
        }
    }

    public void ShowDungeonList()
    {
        dungeonList.SetActive(true);
        levelList.SetActive(false);
    }

    public void ShowLevelList(UIDungeonInfo info)
    {
        dungeonList.SetActive(false);
        levelList.SetActive(true);
        UpdateLevelList(info);
    }

    public void OnDungeonSelected(int index)
    {
        ShowLevelList(DungeonSelectManager.GetInstance().GetModel().stage[index]);
    }

    public void UpdateDungeonList()
    {
        int totalItem = DungeonSelectManager.GetInstance().GetModel().stage.Count;
        for (int i = 0; i < totalItem; i++)
        {
            if (i < dungeonList.transform.childCount)
                UpdateDungeonItem(i, DungeonSelectManager.GetInstance().GetModel().stage[i]);
            else
                CreateDungeonItem(DungeonSelectManager.GetInstance().GetModel().stage[i]);
        }
    }

    void HideAllDungeonItem()
    {
        foreach (Transform child in dungeonList.transform)
            child.gameObject.SetActive(false);
    }

    void CreateDungeonItem(UIDungeonInfo info)
    {
        GameObject dungeon = Instantiate(dungeonPrefab, dungeonList.transform, false);
        dungeon.GetComponent<DungeonSelectItemUI>().SetDungeonInfo(info);
        dungeon.GetComponent<DungeonSelectItemUI>().SetDungeonSelectListener(this);
    }

    void UpdateDungeonItem(int index, UIDungeonInfo info)
    {
        GameObject dungeon = dungeonList.transform.GetChild(index).gameObject;
        dungeon.GetComponent<DungeonSelectItemUI>().SetDungeonInfo(info);
        dungeon.SetActive(true);
    }

    void UpdateLevelList(UIDungeonInfo info)
    {

    }

    void HideAllLevelItem()
    {
        foreach (Transform child in levelList.transform)
            child.gameObject.SetActive(false);
    }

    void CreateLevelItem(UILevelInfo info)
    {

    }

    void UpdateLevelItem(int index, UILevelInfo info)
    {

    }

    public void OnDungeonSelectClicked(int index)
    {
        OnDungeonSelected(index);
    }
}
