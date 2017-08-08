using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonCharPanel : MonoBehaviour {

    public PlayerCharaUI[] cUI;

	// Use this for initialization
	void Start () {
        UpdateAllCharacterInfo();
        SceneManager.sceneUnloaded += OnActiveSceneChanged;
	}
	
    void UpdateAllCharacterInfo()
    {
        cUI[0].UpdateCharacter(PlayerSession.GetProfile().characters[0]);
        cUI[1].UpdateCharacter(PlayerSession.GetProfile().GetCharacter(PlayerSession.GetProfile().party.member1));
        cUI[2].UpdateCharacter(PlayerSession.GetProfile().GetCharacter(PlayerSession.GetProfile().party.member2));
        cUI[3].UpdateCharacter(PlayerSession.GetProfile().GetCharacter(PlayerSession.GetProfile().party.member3));
    }

    void OnActiveSceneChanged(Scene arg0)
    {
        if (arg0.buildIndex == BuildIndex.BATTLE_SCENE)
            UpdateAllCharacterInfo();
    }
}
