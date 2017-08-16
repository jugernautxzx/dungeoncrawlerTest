using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DungeonCharPanel : MonoBehaviour
{

    public PlayerCharaUI[] cUI;

    // Use this for initialization
    void Start()
    {
        UpdateAllCharacterInfo();
        SceneManager.sceneUnloaded += OnActiveSceneChanged;
        cUI[0].AddClickListener(delegate { OnCharacterClicked(0); });
        cUI[1].AddClickListener(delegate { OnCharacterClicked(1); });
        cUI[2].AddClickListener(delegate { OnCharacterClicked(2); });
        cUI[3].AddClickListener(delegate { OnCharacterClicked(3); });
    }

    void UpdateAllCharacterInfo()
    {
        cUI[0].UpdateCharacter(PlayerSession.GetProfile().characters[0]);
        cUI[1].UpdateCharacter(PlayerSession.GetProfile().GetCharacter(PlayerSession.GetProfile().party.member1));
        cUI[2].UpdateCharacter(PlayerSession.GetProfile().GetCharacter(PlayerSession.GetProfile().party.member2));
        cUI[3].UpdateCharacter(PlayerSession.GetProfile().GetCharacter(PlayerSession.GetProfile().party.member3));
    }

    void EnableSelection(bool enable)
    {
        cUI[0].EnableSelection(enable);
        cUI[1].EnableSelection(enable);
        cUI[2].EnableSelection(enable);
        cUI[3].EnableSelection(enable);
    }

    void OnCharacterClicked(int index)
    {
    }

    void OnActiveSceneChanged(Scene arg0)
    {
        if (arg0.buildIndex == BuildIndex.BATTLE_SCENE)
            UpdateAllCharacterInfo();
    }

    public void SelectCharacterToUseItem(ItemModel item)
    {
        EnableSelection(true);
    }
}
