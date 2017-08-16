using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour, BattleInterface
{
    const string TURN = "'s turn";

    public Text infoText;
    public Text playerTurnText;
    public Text battleLog;
    public GameObject basicActions;
    public GameObject skillActions;

    public Toggle attackButton;
    public Button skillBtn;
    public Button cancelSkill;
    public Toggle[] activeBtns;

    public PlayerCharaUI[] playerChar;
    public EnemyUI[] enemyChar;

    BattleManager battleManager;
    TextManager textManager;

    int selectedSkillIndex;

    void Start()
    {
        skillBtn.onClick.AddListener(OnSkillButtonClick);
        cancelSkill.onClick.AddListener(OnCancelSkillButtonClick);
        attackButton.onValueChanged.AddListener(OnAttackButtonClick);
        for (int i = 0; i < 4; i++)
        {
            int j = i;
            enemyChar[i].AddClickListener(delegate { OnAttackTargetSelected(j, false); });
            playerChar[i].AddClickListener(delegate { OnAttackTargetSelected(j, true); });
        }
        for (int i = 0; i < 5; i++)
        {
            int j = i;
            activeBtns[i].onValueChanged.AddListener(delegate { OnActiveSkillButtonClicked(j); });
        }
        textManager = TextManager.GetInstance();
        battleManager = new BattleManager(this);
        battleManager.BattleStart();
    }

    public void UpdateSkillInfo(int index)
    {
        UpdateInfoText(battleManager.GetSkillNote(index));
    }

    public void UpdateInfo(string text)
    {
        UpdateInfoText(text);
    }

    public void UpdateInfoId(string id)
    {
        UpdateInfoText(textManager.GetText(id));
    }

    void UpdateInfoText(string text)
    {
        infoText.text = text;
    }

    void OnSkillButtonClick()
    {
        basicActions.SetActive(false);
        skillActions.SetActive(true);
        ResetToggleButton();
        CancelTargetingIndicator();
    }

    void OnActiveSkillButtonClicked(int i)
    {
        CancelTargetingIndicator();
        if (activeBtns[i].isOn)
        {
            ResetOtherButton(i);
            activeBtns[i].targetGraphic.color = Color.grey;
            selectedSkillIndex = i;
            battleManager.RequestSkillTarget(i);
        }
        else
        {
            activeBtns[i].targetGraphic.color = Color.white;
        }
    }

    void OnCancelSkillButtonClick()
    {
        basicActions.SetActive(true);
        skillActions.SetActive(false);
        ResetToggleButton();
        CancelTargetingIndicator();
    }

    void OnAttackButtonClick(bool onToggle)
    {
        if (onToggle)
        {
            battleManager.RequestAttackTarget();
            attackButton.targetGraphic.color = Color.grey;
        }
        else
            CancelTargetingIndicator();
    }

    void OnAttackTargetSelected(int index, bool isPlayer)
    {
        if (attackButton.isOn)
        {
            battleManager.PlayerActorAttackTarget(index, isPlayer);
        }
        else if (selectedSkillIndex != -1)
        {
            battleManager.PlayerActorSkillTarget(index, isPlayer, selectedSkillIndex);
        }
        basicActions.SetActive(false);
        skillActions.SetActive(false);
        CancelTargetingIndicator();
    }

    public void UpdateTimer(int p1, int p2, int p3, int p4)
    {
        playerChar[0].UpdateTurnBar(p1 / 2000f);
        playerChar[1].UpdateTurnBar(p2 / 2000f);
        playerChar[2].UpdateTurnBar(p3 / 2000f);
        playerChar[3].UpdateTurnBar(p4 / 2000f);
    }

    public void StartBattleTimer(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void StopBattleTimer()
    {
        StopAllCoroutines();
    }

    public void PlayerTakeTurn(CharacterModel player)
    {
        playerTurnText.text = player.name + TURN;
        OnCancelSkillButtonClick();
        SetPlayerActiveSkills(player);
    }

    void SetPlayerActiveSkills(CharacterModel player)
    {
        for (int x = 0; x < 5; x++)
        {
            activeBtns[x].gameObject.SetActive(false);
        }
        int i = 0;
        foreach (string active in player.actives)
        {
            if (active.Length > 0)
            {
                activeBtns[i].gameObject.SetActive(true);
                activeBtns[i].GetComponentInChildren<Text>().text = ActiveSkillManager.GetInstance().GetActive(active).name;
            }
            i++;
        }
    }

    public void UpdatePlayerTeam(CharacterModel model1, CharacterModel model2, CharacterModel model3, CharacterModel model4)
    {
        playerChar[0].UpdateCharacter(model1);
        playerChar[1].UpdateCharacter(model2);
        playerChar[2].UpdateCharacter(model3);
        playerChar[3].UpdateCharacter(model4);
    }

    public void UpdateEnemyTeam(CharacterModel model1, CharacterModel model2, CharacterModel model3, CharacterModel model4)
    {
        enemyChar[0].UpdateCharacter(model1);
        enemyChar[1].UpdateCharacter(model2);
        enemyChar[2].UpdateCharacter(model3);
        enemyChar[3].UpdateCharacter(model4);
    }

    public void EnableTargetingIndicator(int index, bool isPlayerSide, bool enabled)
    {
        if (isPlayerSide)
            playerChar[index].EnableSelection(enabled);
        else
            enemyChar[index].EnableSelection(enabled);
    }

    void CancelTargetingIndicator()
    {
        attackButton.targetGraphic.color = Color.white;
        for (int i = 0; i < 4; i++)
        {
            EnableTargetingIndicator(i, true, false);
            EnableTargetingIndicator(i, false, false);
        }
    }

    void ResetToggleButton()
    {
        attackButton.targetGraphic.color = Color.white;
        attackButton.isOn = false;
        selectedSkillIndex = -1;
        for (int i = 0; i < 5; i++)
        {
            activeBtns[i].isOn = false;
            activeBtns[i].targetGraphic.color = Color.white;
        }
    }

    void ResetOtherButton(int excluded)
    {
        for (int i = 0; i < 5; i++)
        {
            if (i != excluded)
            {
                activeBtns[i].isOn = false;
                activeBtns[i].targetGraphic.color = Color.white;
            }
        }
    }

    public void WriteLog(string log, bool clear)
    {
        if (clear)
            battleLog.text = "";
        if (log.Length > 0)
        {
            if (battleLog.text.Length > 0)
                battleLog.text += "\n" + log;
            else
                battleLog.text = log;
        }
    }

    public void StartEnemyCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

}
