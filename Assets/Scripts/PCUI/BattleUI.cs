using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour, BattleInterface
{

    const string TURN = "'s turn";

    public static BattleUI instance;

    public Text infoText;
    public Text playerTurnText;
    public Text battleLog;
    public GameObject basicActions;
    public GameObject skillActions;

    public Button attackButton;
    public Button skillBtn;
    public Button cancelSkill;

    public PlayerCharaUI[] playerChar;
    public EnemyUI[] enemyChar;

    BattleManager battleManager;

    public static BattleUI GetInstance()
    {
        return instance;
    }

    public static void UpdateInfo(string text)
    {
        if (instance != null)
            instance.UpdateInfoText(text);
    }

    void Start()
    {
        if (instance != this)
            instance = this;

        skillBtn.onClick.AddListener(OnSkillButtonClick);
        cancelSkill.onClick.AddListener(OnCancelSkillButtonClick);
        attackButton.onClick.AddListener(OnAttackButtonClick);
        for (int i = 0; i < 1; i++)
        {//TODO increase to 4
            int j = i;
            enemyChar[0].AddClickListener(delegate { OnAttackTargetSelected(j, false); });
            playerChar[0].AddClickListener(delegate { OnAttackTargetSelected(j, true); });
        }
        battleManager = new BattleManager(this);
        battleManager.BattleStart();
    }

    void UpdateInfoText(string text)
    {
        infoText.text = text;
    }

    void OnSkillButtonClick()
    {
        basicActions.SetActive(false);
        skillActions.SetActive(true);
        CancelTargetingIndicator();
    }

    void OnCancelSkillButtonClick()
    {
        basicActions.SetActive(true);
        skillActions.SetActive(false);
    }

    void OnAttackButtonClick()
    {
        battleManager.RequestAttackTarget();
    }

    void OnAttackTargetSelected(int index, bool isPlayer)
    {
        battleManager.PlayerActorAttackTarget(index, isPlayer);
        basicActions.SetActive(false);
        skillActions.SetActive(false);
        CancelTargetingIndicator();
    }

    public void UpdateTimer(int p1, int p2, int p3, int p4)
    {
        playerChar[0].UpdateTurnBar(p1 / 2000f);
        //turnBar[1].localScale = new Vector2(p2 / 2000f, 1);
        //turnBar[2].localScale = new Vector2(p3 / 2000f, 1);
        //turnBar[3].localScale = new Vector2(p4 / 2000f, 1);
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
    }

    public void UpdatePlayerTeam(CharacterModel model1, CharacterModel model2, CharacterModel model3, CharacterModel model4)
    {
        if (model1 != null)
            playerChar[0].UpdateCharacter(model1);
    }

    public void UpdateEnemyTeam(CharacterModel model1, CharacterModel model2, CharacterModel model3, CharacterModel model4)
    {
        if (model1 != null)
            enemyChar[0].UpdateCharacter(model1);
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
        for (int i = 0; i < 1; i++)//TODO Increase to 4 once completed
        {
            EnableTargetingIndicator(i, true, false);
            EnableTargetingIndicator(i, false, false);
        }
    }

    public void WriteLog(string log)
    {
        battleLog.text += "\n"+log;
    }

    public void StartEnemyCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
