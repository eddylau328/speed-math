using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonForInstructions : MonoBehaviour {

	public GameObject[] text;
	public Animator instructionsPanel;
	public GameObject Left;
	public GameObject Right;

	public Button instruction;
	public Button setting;

	int count;

	bool isOpen;

	// Use this for initialization
	void Start () {
		count = 1;
		isOpen = false;
	}

	// Update is called once per frame
	void Update () {
		if (count == 8)
			Left.SetActive (false);
		else if (!Left.activeSelf)
			Left.SetActive (true);
		if (count == 1)
			Right.SetActive (false);
		else if (!Right.activeSelf)
			Right.SetActive (true);
	}
		
	public void clickLeft(){
		if (count < 9){
			string name;
			text [count - 1].SetActive (false);
			count++;
			text [count - 1].SetActive (true);
			name = "page" + count;
			instructionsPanel.CrossFade (name, 0);
		}
	}

	public void clickRight(){
		if (count > 1 ){
			string name;
			text [count - 1].SetActive (false);
			count--;
			text [count - 1].SetActive (true);
			name = "page" + count;
			instructionsPanel.CrossFade (name, 0);
		}
	}

	public void clickSettingInMenu(){
		isOpen = !isOpen;
		instruction.interactable = !isOpen;
	}

	public void clickInstructionInMenu(){
		isOpen = !isOpen;
		setting.interactable = !isOpen;
	}
}
