using System.ComponentModel;
using System.Xml.Serialization;
using UnityEngine;

[XmlRoot("Items")]
public class ItemModelList : RealListWithDictionary<ItemModel>
{
}

public class ItemModel : ClassWithId{
    [XmlAttribute("Name")]
    public string name;
    [DefaultValue(ItemType.Treasure)]
    [XmlAttribute("Type")]
    public ItemType item;
    [XmlAttribute("Value")]
    public string value;
    [XmlAttribute("Description")]
    public string description;
    [XmlAttribute("Weight")]
    public int weight;
}

public class ItemManager
{
    static ItemManager instance;

    ItemModelList itemList;
    
    public ItemManager()
    {
        itemList = XmlLoader.LoadFromXmlResource<ItemModelList>("Xml/Item");
        itemList.GenerateDictionary();
        Debug.Log("Successfully loaded items: " + itemList.list.Count);
    }

    public static ItemManager GetInstance()
    {
        if (instance == null)
            instance = new ItemManager();
        return instance;
    }

    public ItemModel GetItem(string id)
    {
        return itemList.Get(id);
    }
}