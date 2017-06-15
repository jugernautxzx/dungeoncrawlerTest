
public class ProfileManager{



	public PlayerProfileModel CreateNewProfile(CharacterModel mainChara)
    {
        return null;
    }

    public void SaveProfile(PlayerProfileModel model)
    {
        XmlSaver.SaveXmlToFile<PlayerProfileModel>("/MainSave.xml", model);
    }

    public void LoadProfile()
    {

    }
}
