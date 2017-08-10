
using FlatBuffers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        //LoadFlatProfile();
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

    public void LoadFlatProfile()
    {
        ByteBuffer bb = new ByteBuffer(File.ReadAllBytes(XmlSaver.PersistentDataPath() + "/SAVEDATA"));
        PlayerProfileFlat flat = PlayerProfileFlat.GetRootAsPlayerProfileFlat(bb);
        Debug.Log("Printing  " + flat.Characters(0).Value.Name);
        Debug.Log("Printing  " + flat.Characters(1).Value.Name);
    }

    //TODO Once finished use flatbuffer to save player game data
    public void SaveProfileToFlat(PlayerProfileModel prof)
    {
        List<Offset<CharacterModelFlat>> charList = new List<Offset<CharacterModelFlat>>();

        FlatBufferBuilder fbb = new FlatBufferBuilder(1);
        StringOffset item = fbb.CreateString(prof.itemString);
        foreach(CharacterModel model in prof.characters)
        {
            charList.Add(AddCharacterToFlat(fbb, model));
        }
        VectorOffset charVector = fbb.CreateVectorOfTables<CharacterModelFlat>(charList.ToArray());
        PlayerProfileFlat.StartPlayerProfileFlat(fbb);
        PlayerProfileFlat.AddGold(fbb, prof.Gold);
        PlayerProfileFlat.AddItems(fbb, item);
        PlayerProfileFlat.AddCharacters(fbb, charVector);
        Offset<PlayerProfileFlat> offset  = PlayerProfileFlat.EndPlayerProfileFlat(fbb);
        PlayerProfileFlat.FinishPlayerProfileFlatBuffer(fbb, offset);
        MemoryStream ms = new MemoryStream(fbb.DataBuffer.Data, fbb.DataBuffer.Position, fbb.Offset);
        File.WriteAllBytes(XmlSaver.PersistentDataPath() + "/SAVEDATA", ms.ToArray());
    }

    Offset<CharacterModelFlat> GetCMFlat1(FlatBufferBuilder fbb, CharacterModel model, StringOffset nameOffset)
    {
        CharacterModelFlat.StartCharacterModelFlat(fbb);
        CharacterModelFlat.AddName(fbb, nameOffset);
        CharacterModelFlat.AddLevel(fbb, model.level);
        CharacterModelFlat.AddExp(fbb, model.exp);
        return CharacterModelFlat.EndCharacterModelFlat(fbb);
    }

    Offset<CharacterModelFlat> AddCharacterToFlat(FlatBufferBuilder fbb, CharacterModel model)
    {
        StringOffset nameOffset = fbb.CreateString(model.name);
        CharacterModelFlat.StartCharacterModelFlat(fbb);
        CharacterModelFlat.AddName(fbb, nameOffset);
        CharacterModelFlat.AddLevel(fbb, model.level);
        CharacterModelFlat.AddExp(fbb, model.exp);
        return CharacterModelFlat.EndCharacterModelFlat(fbb);
    }
}
