using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showRange : MonoBehaviour {

	Text rangeText;

	// Use this for initialization
	void Start () {
		rangeText = gameObject.GetComponent<Text> ();
		rangeText.text = GameManager.minNumRange + "~" + GameManager.maxNumRange;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
