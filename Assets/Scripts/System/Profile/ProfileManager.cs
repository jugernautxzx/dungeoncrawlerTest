
using FlatBuffers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProfileManager
{

    public PlayerProfileModel CreateNewProfile(CharacterModel mainChara)
    {
        PlayerProfileModel profile = new PlayerProfileModel();
        profile.itemsId = new List<string>();
        profile.itemsOwned = new List<int>();
        mainChara.isMainCharacter = true;
        profile.mainChara = new MainCharaModel();
        profile.mainChara.fame = 10;
        profile.mainChara.leadership = 10;
        profile.Gold = 100;
        profile.characters = new List<CharacterModel>();
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
        LoadFlatProfile();
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
    {//TODO Finish this in final version, convert to useable ProfileManager
        float start = Time.realtimeSinceStartup;
        if (!File.Exists(XmlSaver.PersistentDataPath() + "/SAVEDATA"))
            return;
        ByteBuffer bb = new ByteBuffer(File.ReadAllBytes(XmlSaver.PersistentDataPath() + "/SAVEDATA"));
        if (!PlayerProfileFlat.PlayerProfileFlatBufferHasIdentifier(bb))
            return;
        PlayerProfileFlat flat = PlayerProfileFlat.GetRootAsPlayerProfileFlat(bb);
        Debug.Log("Flat Loading time  " + (Time.realtimeSinceStartup - start) + " " + flat.Characters(0).Value.Name);
    }

    //TODO Once finished use flatbuffer to save player game data
    public void SaveProfileToFlat(PlayerProfileModel prof)
    {
        float start = Time.realtimeSinceStartup;
        List<Offset<CharacterModelFlat>> charList = new List<Offset<CharacterModelFlat>>();
        FlatBufferBuilder fbb = new FlatBufferBuilder(1);
        StringOffset item = fbb.CreateString(prof.itemString);
        foreach (CharacterModel model in prof.characters)
        {
            charList.Add(AddCharacterToFlat(fbb, model));
        }
        VectorOffset charVector = fbb.CreateVectorOfTables<CharacterModelFlat>(charList.ToArray());
        PlayerProfileFlat.StartPlayerProfileFlat(fbb);
        PlayerProfileFlat.AddGold(fbb, prof.Gold);
        PlayerProfileFlat.AddItems(fbb, item);
        PlayerProfileFlat.AddCharacters(fbb, charVector);
        Offset<PlayerProfileFlat> offset = PlayerProfileFlat.EndPlayerProfileFlat(fbb);
        PlayerProfileFlat.FinishPlayerProfileFlatBuffer(fbb, offset);
        MemoryStream ms = new MemoryStream(fbb.DataBuffer.Data, fbb.DataBuffer.Position, fbb.Offset);
        File.WriteAllBytes(XmlSaver.PersistentDataPath() + "/SAVEDATA", ms.ToArray());
        Debug.Log("Flat Saving time  " + (Time.realtimeSinceStartup - start));
    }

    Offset<CharacterModelFlat> AddCharacterToFlat(FlatBufferBuilder fbb, CharacterModel model)
    {
        StringOffset nameOffset = fbb.CreateString(model.name);
        StringOffset learnedSkill = fbb.CreateString(JoinAllStrings(model.learnActive));
        StringOffset activeSkill = fbb.CreateString(JoinAllStrings(model.actives));
        Offset<AttributeFlat> attrib = GetAttribute(fbb, model.attribute);
        CharacterModelFlat.StartCharacterModelFlat(fbb);
        CharacterModelFlat.AddName(fbb, nameOffset);
        CharacterModelFlat.AddLevel(fbb, model.level);
        CharacterModelFlat.AddExp(fbb, model.exp);
        CharacterModelFlat.AddLearnActive(fbb, learnedSkill);
        CharacterModelFlat.AddLearnActive(fbb, activeSkill);
        CharacterModelFlat.AddAtt(fbb, attrib);
        return CharacterModelFlat.EndCharacterModelFlat(fbb);
    }

    Offset<AttributeFlat> GetAttribute(FlatBufferBuilder fbb, Attribute att)
    {
        AttributeFlat.StartAttributeFlat(fbb);
        AttributeFlat.AddAgi(fbb, att.agi);
        AttributeFlat.AddSpeed(fbb, att.speed);
        AttributeFlat.AddEnd(fbb, att.endurance);
        AttributeFlat.AddCons(fbb, att.cons);
        AttributeFlat.AddIntel(fbb, att.intel);
        AttributeFlat.AddStr(fbb, att.str);
        return AttributeFlat.EndAttributeFlat(fbb);
    }

    string JoinAllStrings(List<string> list)
    {
        if (list == null)
            return "";
        string joined = "";
        foreach (string text in list)
            joined += "|" + text;
        return joined;
    }
}
