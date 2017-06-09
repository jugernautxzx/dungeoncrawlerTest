using System.Xml.Serialization;

[XmlRoot("Texts")]
public class TextModelList:RealListWithDictionary<TextModel>
{
}

public class TextModel : ClassWithId
{
    [XmlAttribute("Text")]
    public string text;
}

public class TextManager
{
    TextModelList list;

    public TextManager()
    {
        list = XmlLoader.LoadFromXmlResource<TextModelList>("Xml/TextEN");
        list.GenerateDictionary();
    }

    public string GetText(string id)
    {
        return list.Get(id).text;
    }
}
