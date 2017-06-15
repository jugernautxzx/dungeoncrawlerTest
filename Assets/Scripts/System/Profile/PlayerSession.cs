using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSession  {

    PlayerProfileModel player;
    ProfileManager manager;

    static PlayerSession instance;

    public static PlayerSession GetInstance()
    {
        if (instance == null)
            instance = new PlayerSession();
        return instance;
    }

    public PlayerSession()
    {
        manager = new ProfileManager();
    }

    public void CreateNewSession(CharacterModel model)
    {
        player = manager.CreateNewProfile(model);
    }

    public bool LoadSession()
    {
        return false;
    }

    public bool SaveSession()
    {
        player = new PlayerProfileModel();
        player.characters = new List<CharacterModel>();
        player.mainChara = new MainCharaModel();
        player.Gold = 5555;
        player.characters.Add(Debugger.GenerateCharacterModel("Johnny McHammer"));
        manager.SaveProfile(player);
        return true;
    }

}
