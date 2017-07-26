using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour {

    public Text textUI;

	public void SetText(string text)
    {
        textUI.text = text;
    }
}
