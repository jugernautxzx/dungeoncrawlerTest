using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSession  {

    PlayerProfileModel player;
    ProfileManager manager;
    PlayerEquipments equipments;

    static PlayerSession instance;

    public static PlayerSession GetInstance()
    {
        if (instance == null)
            instance = new PlayerSession();
        return instance;
    }

    public static PlayerEquipments GetInventory()
    {
        return instance.equipments;
    }

    public static Equipment GetEquipment(int index)
    {
        if (index < 0)
            return null;
        else
            return instance.equipments.list[index];
    }

    public PlayerSession()
    {
        manager = new ProfileManager();
    }

    public void CreateNewSession(CharacterModel model)
    {
        player = manager.CreateNewProfile(model);
        equipments = new PlayerEquipments();
        equipments.list = new List<Equipment>();
    }

    public bool LoadSession()
    {
        player = manager.LoadProfile();
        equipments = manager.LoadEquipments();
        return player == null;
    }

    public bool IsSessionAvailable()
    {
        return player != null;
    }

    public bool SaveSession()
    {
        manager.SaveProfile(player);
        manager.SaveEquipments(equipments);
        return true;
    }

    public static PlayerProfileModel GetProfile()
    {
        return GetInstance().player;
    }
}
