using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class createTargetNumber : MonoBehaviour {

	Text targetNum;
	int num;
	Animator anim;
	bool flag = false;

	GameObject panel;
	CreateTarget script;

	// Use this for initialization

	public void MessageFromTargetManager(){
		targetNum = gameObject.GetComponent<Text> ();
		panel = GameObject.FindGameObjectWithTag ("TargetPanel");
		script = panel.GetComponent<CreateTarget> ();
		while (!flag) {
			flag = true;
			num = SpawnNumberSetting ();
			for (int i = 0; i < CreateTarget.NumOfTarget; i++)
				if (num == CreateTarget.targetNum [i])
					flag = false;
		}
		targetNum.text = num.ToString();
		script.sendTargetNum(num);
	}

	public void shine(){
		anim = transform.parent.GetComponent<Animator> ();
		anim.SetTrigger ("correct");
	}

	private int SpawnNumberSetting(){
		if (script.GetTargetCounter < 2) {
			bool flag = false;
			int temp = 1;
			while (!flag) {
				temp = Random.Range (GameManager.maxSpawnNumRange + 1, GameManager.maxNumRange + 1);
				if (temp % 4 == 0 || temp % 5 == 0)
					flag = true;
			}
			return temp;
		} else if (script.GetTargetCounter < 6){
			bool flag = false;
			int temp = 1;
			while (!flag) {
				temp = Random.Range (GameManager.maxSpawnNumRange + 1, GameManager.maxNumRange + 1);
				if (temp % 5 == 0|| temp % 2 == 0)
					flag = true;
			}
			return temp;
		}else {
			return Random.Range (GameManager.maxSpawnNumRange + 1, GameManager.maxNumRange + 1);
		}
	}

}
