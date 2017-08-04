using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationDialog : MonoBehaviour
{

    public Text dialog;

    public Button noButton;
    public Button yesButton;

    public void SetText(string text)
    {
        dialog.text = text;
    }

    public void SetYesClicked(UnityAction call)
    {
        yesButton.onClick.RemoveAllListeners();
        if (call != null)
            yesButton.onClick.AddListener(call);
        yesButton.onClick.AddListener(RemoveDialog);
    }

    public void SetNoClicked(UnityAction call)
    {
        noButton.onClick.RemoveAllListeners();
        if (call != null)
            noButton.onClick.AddListener(call);
        noButton.onClick.AddListener(RemoveDialog);
    }

    void RemoveDialog()
    {
        gameObject.SetActive(false);
    }
}
