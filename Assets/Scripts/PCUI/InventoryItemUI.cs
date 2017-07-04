using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface InventoryItemInterface
{
    void OnItemClicked(Equipment model);
}

public class InventoryItemUI : MonoBehaviour {

    public Text eqName;
    public Text eqType;
    public Text eqAttack;

    Equipment model;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetModel(Equipment model)
    {
        this.model = model;
        eqName.text = model.name;
        eqAttack.text = "Attack " + model.bonus.attack;
        eqType.text = model.weapon.ToString();
    }
}
