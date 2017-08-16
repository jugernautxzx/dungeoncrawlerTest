using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour, PartySelectionInterface
{

    public CharacterUI[] activeMember;
    public GameObject reservedMember;

    public GameObject memberPrefab;

    int[] partyIndex = new int[3];

    EquipmentUIInterface eqImpl;
    bool requireUpdate;

    // Use this for initialization
    void Start()
    {
        requireUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnEnable()
    {
        if (PlayerSession.GetProfile() != null && requireUpdate)
            UpdatePartyMembersInformation();
    }

    public void RequestUpdateMember()
    {
        requireUpdate = true;
    }

    public void SetEquipmentImpl(EquipmentUIInterface eqImpl)
    {
        this.eqImpl = eqImpl;
    }

    public void RemoveFromParty(int index)
    {
        if (partyIndex[index] < 0)
            return;
        reservedMember.transform.GetChild(partyIndex[index]).GetComponent<CharacterUI>().SetInParty(false);
        partyIndex[index] = -1;
        UpdatePartyMembersLineUp();
    }

    public void UpdatePartyMembersInformation()
    {
        partyIndex[0] = PlayerSession.GetProfile().party.member1;
        partyIndex[1] = PlayerSession.GetProfile().party.member2;
        partyIndex[2] = PlayerSession.GetProfile().party.member3;
        StartCoroutine(UpdateAllMembersRoutine());
        UpdateActiveMemberInfo();
    }

    public void UpdatePartyMembersLineUp()
    {
        PlayerSession.GetProfile().party.member1 = partyIndex[0];
        PlayerSession.GetProfile().party.member2 = partyIndex[1];
        PlayerSession.GetProfile().party.member3 = partyIndex[2];
        UpdateActiveMemberInfo();
    }

    void UpdateActiveMemberInfo()
    {
        activeMember[0].LoadInformation(PlayerSession.GetProfile().GetCharacter(0));
        activeMember[1].LoadInformation(PlayerSession.GetProfile().GetCharacter(partyIndex[0]));
        activeMember[2].LoadInformation(PlayerSession.GetProfile().GetCharacter(partyIndex[1]));
        activeMember[3].LoadInformation(PlayerSession.GetProfile().GetCharacter(partyIndex[2]));
    }

    void UpdateAllMembers()
    {
        foreach (Transform child in reservedMember.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (CharacterModel model in PlayerSession.GetProfile().characters)
        {
            GameObject member = Instantiate(memberPrefab, reservedMember.transform, false);
            member.GetComponent<CharacterUI>().LoadInformation(model);
            member.GetComponent<CharacterUI>().SetListener(this);
            member.GetComponent<CharacterUI>().SetEquipmentUI(eqImpl);
        }
    }

    void UpdateSelectedMembers()
    {
        reservedMember.transform.GetChild(0).GetComponent<CharacterUI>().SetInParty(true);
        for (int i = 0; i < 3; i++)
        {
            if (partyIndex[i] > -1)
                reservedMember.transform.GetChild(partyIndex[i]).GetComponent<CharacterUI>().SetInParty(true);
        }
    }

    IEnumerator UpdateAllMembersRoutine()
    {
        UpdateAllMembers();
        yield return new WaitForEndOfFrame();
        UpdateSelectedMembers();
    }

    public bool OnAddToParty(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            if (partyIndex[i] == -1)
            {
                partyIndex[i] = index;
                UpdatePartyMembersLineUp();
                return true;
            }
        }
        return false;
    }

    void PartySelectionInterface.RemoveFromParty(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            if (partyIndex[i] == index)
            {
                RemoveFromParty(i);
                break;
            }
        }
    }
}
