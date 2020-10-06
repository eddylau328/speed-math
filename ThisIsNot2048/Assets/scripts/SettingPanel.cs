using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour {

	public static bool isNoMinusReverse;
	public static bool isNoDivideReverse;

	public static bool firstOperating;

	public Switch MinusReverse;
	public Switch DivideReverse;

	void Awake (){
		firstOperating = true;
	}

	// Use this for initialization
	void Start () {
		int temp;
		temp = PlayerPrefs.GetInt ("NoMinusReverse",0);
		if (temp == 1)
			isNoMinusReverse = true;
		else
			isNoMinusReverse = false;
		temp = PlayerPrefs.GetInt ("NoDivideReverse",0);
		if (temp == 1)
			isNoDivideReverse = true;
		else
			isNoDivideReverse = false;

		MinusReverse.isOn = isNoMinusReverse;
		DivideReverse.isOn = isNoDivideReverse;

		firstOperating = false;
	}

}
