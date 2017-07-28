using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ConsumableInterface
{

}

public class ConsumableItemUI : MonoBehaviour
{
    public const int FILTER_ALL = 0;
    public const int FILTER_CONSUMABLE = 1;
    public const int FILTER_SKILL = 2;
    public const int FILTER_TREASURE = 3;
    public const int FILTER_KEY = 4;

    public Dropdown filter;
    public GameObject viewPort;
    public GameObject prefab;

    TooltipInterface tooltip;

    // Use this for initialization
    void Start()
    {
        filter.onValueChanged.AddListener(ChangeFilter);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeFilter(int code)
    {
        switch (code)
        {
            case FILTER_ALL:
                break;
            case FILTER_CONSUMABLE:
                break;
            case FILTER_KEY:
                break;
            case FILTER_SKILL:
                break;
            case FILTER_TREASURE:
                break;
        }
    }
}
