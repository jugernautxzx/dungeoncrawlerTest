
using FlatBuffers;
using System.IO;

public class ProfileManager{

	public PlayerProfileModel CreateNewProfile(CharacterModel mainChara)
    {
        PlayerProfileModel profile = new PlayerProfileModel();
        profile.itemsId = new System.Collections.Generic.List<string>();
        profile.itemsOwned = new System.Collections.Generic.List<int>();
        mainChara.isMainCharacter = true;
        profile.mainChara = new MainCharaModel();
        profile.mainChara.fame = 10;
        profile.mainChara.leadership = 10;
        profile.Gold = 100;
        profile.characters = new System.Collections.Generic.List<CharacterModel>();
        profile.characters.Add(mainChara);
        return profile;
    }

    public void SaveProfile(PlayerProfileModel model)
    {
        XmlSaver.SaveXmlToFile<PlayerProfileModel>("/MainSave.xml", model);
        //SaveProfileToFlat(model);
    }

    public PlayerProfileModel LoadProfile()
    {
        return XmlLoader.LoadFromXmlSave<PlayerProfileModel>("/MainSave.xml");
    }

    public void SaveEquipments(PlayerEquipments model)
    {
        XmlSaver.SaveXmlToFile<PlayerEquipments>("/MainEquips.xml", model);
    }

    public PlayerEquipments LoadEquipments()
    {
        return XmlLoader.LoadFromXmlSave<PlayerEquipments>("/MainEquips.xml");
    }

    //TODO Once finished use flatbuffer to save player game data
    public void SaveProfileToFlat(PlayerProfileModel prof)
    {
        FlatBufferBuilder fbb = new FlatBufferBuilder(1);
        StringOffset item = fbb.CreateString(prof.itemString);
        PlayerProfileFlat.StartPlayerProfileFlat(fbb);
        PlayerProfileFlat.AddGold(fbb, prof.Gold);
        PlayerProfileFlat.AddItems(fbb, item);
        Offset<PlayerProfileFlat> offset  = PlayerProfileFlat.EndPlayerProfileFlat(fbb);
        PlayerProfileFlat.FinishPlayerProfileFlatBuffer(fbb, offset);
        MemoryStream ms = new MemoryStream(fbb.DataBuffer.Data, fbb.DataBuffer.Position, fbb.Offset);
        File.WriteAllBytes(XmlSaver.PersistentDataPath() + "/SAVEDATA", ms.ToArray());
    }
}
