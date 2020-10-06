using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour {
	
	public void clickStart(){
		StartCoroutine (ChooseFunction (1));
	}

	public void clickPause(){
		GameManager.isPause = !GameManager.isPause;
	}

	public void clickQuit(){
		StartCoroutine (ChooseFunction (2));
	}

	public void clickLeft(){
		ButtonForInstructions script = GameObject.FindGameObjectWithTag ("ButtonForInstructions").GetComponent<ButtonForInstructions> ();
		script.clickLeft ();
	}

	public void clickRight(){
		ButtonForInstructions script = GameObject.FindGameObjectWithTag ("ButtonForInstructions").GetComponent<ButtonForInstructions> ();
		script.clickRight ();
	}

	public void clickSettingInMenu(){
		ButtonForInstructions script = GameObject.FindGameObjectWithTag ("ButtonForInstructions").GetComponent<ButtonForInstructions> ();
		script.clickSettingInMenu ();
	}

	public void clickInstructionInMenu(){
		ButtonForInstructions script = GameObject.FindGameObjectWithTag ("ButtonForInstructions").GetComponent<ButtonForInstructions> ();
		script.clickInstructionInMenu ();
	}

	public void clickMinusReverse(){
		if (!SettingPanel.firstOperating) {
			SettingPanel.isNoMinusReverse = !SettingPanel.isNoMinusReverse;
			if (SettingPanel.isNoMinusReverse)
				PlayerPrefs.SetInt ("NoMinusReverse", 1);
			else
				PlayerPrefs.SetInt ("NoMinusReverse", 0);
		}
	}

	public void clickDivideReverse(){
		if (!SettingPanel.firstOperating) {
			SettingPanel.isNoDivideReverse = !SettingPanel.isNoDivideReverse;
			if (SettingPanel.isNoDivideReverse)
				PlayerPrefs.SetInt ("NoDivideReverse", 1);
			else
				PlayerPrefs.SetInt ("NoDivideReverse", 0);
		}
	}

	IEnumerator ChooseFunction(int caseNum){
		float fadeTime = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<Fade>().BeginFade (1);
		switch (caseNum) {
		case 1:
			CreateTarget.NumOfTarget = 0;
			GameManager.isPause = false;
			GameManager.isGameOver = false;

			yield return new WaitForSeconds (fadeTime);

			SceneManager.LoadScene ("game");
			break;
		case 2:
			
			yield return new WaitForSeconds (fadeTime);

			SceneManager.LoadScene ("Opening");

			break;
		}
	}

}
