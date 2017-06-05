using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public abstract class ListWithDictionary
{
    protected Dictionary<string, int> dict;

    public abstract void GenerateDictionary();

    public bool DictContains(string id)
    {
        return dict.ContainsKey(id);
    }
}

public class XmlLoader
{
    public static string PersistentDataPath()
    {
        return Application.dataPath.Replace("/Assets", "");
    }

    public static T LoadFromXml<T>(string path) where T : ListWithDictionary
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