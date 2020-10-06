using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateTarget : MonoBehaviour {

	public GameObject targetTile;

	public static int[] targetNum = new int[Mathf.Max(TileManager.Const.XaxisSize,TileManager.Const.YaxisSize)] ;
	public static int NumOfTarget = 0;

	GameObject[] tile = new GameObject[Mathf.Max(TileManager.Const.XaxisSize,TileManager.Const.YaxisSize)];

	bool isDestroy = false;

	public int GetTargetCounter = 0;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < Mathf.Max(TileManager.Const.XaxisSize,TileManager.Const.YaxisSize); i++) {	
			spawnTarget (i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (NumOfTarget < Mathf.Max(TileManager.Const.XaxisSize,TileManager.Const.YaxisSize) && isDestroy) {
			isDestroy = false;
			spawnTarget (NumOfTarget);
		}
	}

	public void sendTargetNum(int num){
		targetNum [NumOfTarget] = num;
		NumOfTarget++;
	}

	public void correctTile(int counter){
		GetTargetCounter++;
		createTargetNumber script = tile [counter].GetComponentInChildren<createTargetNumber> ();
		script.shine ();
		Destroy(tile[counter],2);
		Invoke ("passIsDestroy", 3);
		for (int j = counter; j < Mathf.Max(TileManager.Const.XaxisSize,TileManager.Const.YaxisSize)-1; j++) {
			targetNum [j] = targetNum [j + 1];
			tile [j] = tile [j + 1];
		}
		NumOfTarget--;
	}

	private void passIsDestroy(){
		isDestroy = true;
	}

	private void spawnTarget(int count){
		tile[count] = Instantiate(targetTile);
		tile[count].transform.SetParent(gameObject.transform,false);
		tile[count].name = "TargetTile" + count;
		createTargetNumber Script = tile [count].GetComponentInChildren<createTargetNumber> ();
		Script.MessageFromTargetManager ();
	}
}
