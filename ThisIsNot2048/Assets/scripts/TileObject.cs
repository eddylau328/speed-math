using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileObject : MonoBehaviour {

	public int Xcoor;
	public int Ycoor;
	public GameManager scriptGameManager;
	public TileManager scriptTileManager;
	public EasyTween scriptEasyTween;
	public GameObject NumberCell;
	public GameObject EmptyCell;
	public GameObject BlockPanel;

	public Text CellText;
	public Button CellButton;
	public Animator CellAnim;

	[HideInInspector]
	public bool isNumberCell = true;
	[HideInInspector]
	public int CellNum = 1;

	bool deadTile = false;
	bool isChoosing = false;

	bool isHitTarget = false;

	void Update() {
		if (!deadTile)
			checkDeadCell ();
	}

	private void spawnNumber(){
		CellNum = Random.Range (GameManager.minSpawnNumRange, GameManager.maxSpawnNumRange + 1);
		CellText.text = CellNum.ToString ();
	}

	public void clickCell(){
		if (Input.touchCount == 1)
			scriptGameManager.ClickATileInGameField (Xcoor, Ycoor);
	}

	public void tellTileChoose(bool isClick){
		isChoosing = isClick;
		playAnimation ();
	}

	private void playAnimation(){
		CellAnim.SetBool ("isChose", isChoosing);
	}

	public void updateCellNum(int newNum){
		CellNum = newNum;
		CellText.text = CellNum.ToString ();
	}
		
	private void checkDeadCell(){
		if ((CellNum > GameManager.maxNumRange || CellNum < GameManager.minNumRange) && isNumberCell) {
			CellButton.interactable = false;
			CellAnim.SetBool ("isDead", true);
			scriptGameManager.RecieveDeadMessage (Xcoor, Ycoor);
			deadTile = true;
		}
	}

	public void disableNumberCell(){
		isNumberCell = false;
//		NumberCell.SetActive (false);

		callEasyTweenAnimation();

	}

	public void hitTarget(){
		BlockPanel.SetActive (true);
		if (CellAnim.GetBool ("isChose"))
			CellAnim.SetBool ("isChose", false);
		CellAnim.SetBool ("isHitTarget", true);
		Invoke ("disableNumberCell", 2f);
		Invoke ("tellTargetDisable", 2f);
		isHitTarget = true;
	}

	private void tellTargetDisable(){
		if (isHitTarget) {
			BlockPanel.SetActive (false);
			isHitTarget = false;
			scriptTileManager.RecheckSelectedTileAfterHitTarget ();
		}
	}

	public void enableNumberCell(bool needSpawn){
//		NumberCell.SetActive (true);

		callEasyTweenAnimation();

		CellButton.interactable = true;
		if (needSpawn)
			spawnNumber ();
		isNumberCell = true;
	}

	public void isEnableEmptyCell(bool isEnable){
		EmptyCell.SetActive (isEnable);
	}

	private void callEasyTweenAnimation(){
		scriptEasyTween.OpenCloseObjectAnimation ();
	}

}
