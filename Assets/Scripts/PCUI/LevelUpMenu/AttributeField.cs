using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface AttributeFieldInterface
{
    void OnValueIncrease();
    void OnValueDecrease();
}

public class AttributeField : MonoBehaviour {

    int minimal = 0, maximal = 30;
    int currentVal;

    Button increase, decrease;
    InputField field;
    AttributeFieldInterface afInterface;

	// Use this for initialization
	void Start () {
        decrease = transform.GetChild(2).GetComponent<Button>();
        decrease.onClick.AddListener(OnButtonDecreasePressed);
        increase = transform.GetChild(3).GetComponent<Button>();
        increase.onClick.AddListener(OnButtonIncreasePressed);
        field = GetComponent<InputField>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetInterface(AttributeFieldInterface interf)
    {
        afInterface = interf;
    }

    void OnButtonIncreasePressed()
    {
        if(!(currentVal + 1 > maximal))
        {
            SetValue(currentVal + 1);
            afInterface.OnValueIncrease();
        }
    }

    void OnButtonDecreasePressed()
    {
        if (!(currentVal - 1 < minimal))
        {
            SetValue(currentVal - 1);
            afInterface.OnValueDecrease();
        }
    }

    public void SetIncrease(bool interactable)
    {
        increase.interactable = interactable;
    }

    public void SetDecrease(bool interactable)
    {
        decrease.interactable = interactable;
    }

    public void SetValue(int val, bool minimal)
    {
        this.minimal = val;
        SetValue(val);
    }

    public void SetValue(int val)
    {
        currentVal = val;
        field.text = currentVal.ToString();
    }

    public int GetValue()
    {
        return currentVal;
    }
}
