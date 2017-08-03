using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public interface PartySelectionInterface
{
    bool OnAddToParty(int index);
    void RemoveFromParty(int index);
}

public interface OnCharacterUIClicked
{
    void OnClicked(int index);
}

public class CharacterUI : MonoBehaviour, IPointerClickHandler
{
    public Text cName, cLevel;
    public GameObject inPartyNotice;

    PartySelectionInterface listener;
    EquipmentUIInterface eqListener;
    OnCharacterUIClicked clickListener;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetClickListener(OnCharacterUIClicked listener)
    {
        clickListener = listener;
    }

    public void RemoveEquipmentButton()
    {
        transform.GetChild(3).gameObject.SetActive(false);
    }

    public void SetEquipmentUI(EquipmentUIInterface eqImpl)
    {
        eqListener = eqImpl;
    }

    public void SetListener(PartySelectionInterface listener)
    {
        this.listener = listener;
        GetComponent<Button>().onClick.AddListener(AddIntoParty);
        transform.GetChild(3).GetComponent<Button>().onClick.AddListener(LoadEquipmentInformation);
    }

    public void LoadInformation(CharacterModel model)
    {
        if (model == null)
        {
            cName.text = "NONE";
            cLevel.text = "";
        }
        else
        {
            cName.text = model.name;
            cLevel.text = "Level " + model.level;
        }
    }

    public void SetInParty(bool inParty)
    {
        inPartyNotice.SetActive(inParty);
    }

    public void AddIntoParty()
    {
        if (inPartyNotice.activeInHierarchy)
        {
            listener.RemoveFromParty(transform.GetSiblingIndex());
            return;
        }
        if (listener.OnAddToParty(transform.GetSiblingIndex()))
            SetInParty(true);
        else
            SetInParty(false);
    }

    public void LoadEquipmentInformation()
    {
        eqListener.LoadEquipmentUI(PlayerSession.GetProfile().characters[transform.GetSiblingIndex()]);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickListener != null)
            clickListener.OnClicked(transform.GetSiblingIndex());
    }
}
