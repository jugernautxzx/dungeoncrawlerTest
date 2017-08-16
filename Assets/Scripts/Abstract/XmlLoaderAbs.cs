using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public abstract class RealListWithDictionary<T> where T : ClassWithId
{
    [XmlElement("Item")]
    public List<T> list;
    [XmlIgnore]
    protected Dictionary<string, int> dict;

    public void GenerateDictionary()
    {
        dict = new Dictionary<string, int>();
        for (int i = 0; i < list.Count; i++)
        {
            if (!DictContains(list[i].id))
            {
                dict.Add(list[i].id, i);
            }
            else
            {
                Debug.Log("Duplicate id: " + list[i].id);
            }
        }
    }

    public T Get(string id)
    {
        if (DictContains(id))
            return list[dict[id]];
        else
            return null;
    }

    public bool DictContains(string id)
    {
        return dict.ContainsKey(id);
    }
}

public class ClassWithId
{
    [XmlAttribute("Id")]
    public string id;
}

public abstract class ListWithDictionary<T>
{
    protected Dictionary<string, int> dict;

    public abstract void GenerateDictionary();

    public abstract T Get(string id);

    public bool DictContains(string id)
    {
        return dict.ContainsKey(id);
    }

}

public static class XmlLoader
{
    public static string PersistentDataPath()
    {
        return Application.dataPath.Replace("/Assets", "");
    }

    public static T LoadFromXml<T>(string path) where T : ListWithDictionary<T>
    {
        TextAsset asset = Resources.Load<TextAsset>(path);
        XmlSerializer serial = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(asset.text);
        T obj = serial.Deserialize(reader) as T;
        obj.GenerateDictionary();
        reader.Close();
        return obj;
    }

    public static T LoadFromXmlResource<T>(string path) where T : class
    {
        TextAsset asset = Resources.Load<TextAsset>(path);
        XmlSerializer serial = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(asset.text);
        T obj = serial.Deserialize(reader) as T;
        reader.Close();
        return obj;
    }

    public static T LoadFromXmlSave<T>(string path) where T : class
    {
        if (!File.Exists(PersistentDataPath() + path))
            return null;
        XmlSerializer serial = new XmlSerializer(typeof(T));
        FileStream stream = new FileStream(PersistentDataPath() + path, FileMode.Open);
        T obj = serial.Deserialize(stream) as T;
        stream.Close();
        return obj;
    }
    
    public static bool IsFileExist(string path)
    {
        return File.Exists(PersistentDataPath() + path);
    }
}

public class XmlSaver
{
    public static string PersistentDataPath()
    {
        return Application.dataPath.Replace("/Assets", "");
    }

    public static void SaveXmlToFile<T>(string path, T file)
    {
        XmlSerializer serial = new XmlSerializer(typeof(T));
        FileStream stream = new FileStream(PersistentDataPath() + path, FileMode.Create);
        serial.Serialize(stream, file);
        stream.Close();
    }

}