using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static int minNumRange = 1;
	public static int maxNumRange = 100;

	public static int minSpawnNumRange = 1;
	public static int maxSpawnNumRange = 10;

	public static char NumericSign;						//Sign that contains +,-,*,/

	public static bool isPause = false;
	public static bool isGameOver;

	public TileManager tilemanagerScript;
	public SignManager signmanagerScript;
	public CreateTarget createTargetScript;

	bool isFirstClickTile;								//it means is it your first time click the tile (true when start)

	bool isFirstPass;									//using to check the generating is just start
	public static bool isFinishAnimation;				//using to check the animation is finish or not
	public static bool isFinishGenerate;
	[HideInInspector]
	public bool hasMoveBeforeTime;

	[HideInInspector]
	public bool isAMove;
	public int ProbabilityGenerate = 80;
	public float TimeForGenerationSign = 5f;

	public GameObject GameOverPanel;
	public Slider timeSlider;
	public Animator timeSliderAnimator;
	public GameObject StopFromClickingPanel;

	public Text GameScoreText;
	public Text MaxScoreText;
	public Text GameOverGameScoreText;
	public Text GameOverMaxScoreText;
	public Text HitTargetScoreText;
	int maxScore;
	int gameScore;
	public int ScoreForHitTarget = 2000;
	public int ScoreForMove = 5;
	public int ScoreForCombine = 20;

	float timeleft;

    char tempSign; // used to save the sign instead of changing the sign immediately

	void Start(){
		isGameOver = false;
		isFirstClickTile = true;
		isAMove = false;
		isFirstPass = true;
		isFinishAnimation = false;
		isFinishGenerate = false;
		hasMoveBeforeTime = false;

		resetAllScore ();

		signmanagerScript.GameStartSetSign ();

		tilemanagerScript.ResetDeadCell ();

		spawnWhenNoTile ();

		timeleft = TimeForGenerationSign;
	}

	void Update(){
		spawnAfterMove ();
		if (!isGameOver)
			isGameOver = tilemanagerScript.CheckGameOver ();
		else
			gameOver ();
		changeTimeSliderColor ();
		timeCount ();
		checkNumberIsTarget ();
		spawnWhenNoTile ();
	}

	// Tile function //

	public void ClickATileInGameField(int x,int y){
		tilemanagerScript.selectedTile (x, y,ref isFirstClickTile);
	}

	public void RecieveDeadMessage(int x,int y){
		tilemanagerScript.UpdateDeadCell (x, y);
	}

	private void numOfTileSpawn(int times){
		for (int i = 0; i < times ; i++)
			tilemanagerScript.spawnNumberTile ();
	}

	private void spawnAfterMove(){
		if (isAMove && probabilityGenerate ()) {
			numOfTileSpawn (1);
			isAMove = false;
		} else {
			isAMove = false;
		}
	}

	private bool probabilityGenerate(){
		/*
		int usedProb = 0;
		if (tilemanagerScript.CountNumberCell () <= 4)
			usedProb = 100;
		else
			usedProb = ProbabilityGenerate;
		*/
		if (Random.Range (0, 100) < ProbabilityGenerate)
			return true;
		else
			return false;
	}

	private void spawnWhenNoTile(){
		int num = tilemanagerScript.CountNumberCell ();
		if (num < 1)
			numOfTileSpawn (4);
		else if ( num < 3 )
			numOfTileSpawn (2);
	}

	// GameOver function //

	private void gameOver(){
		GameOverPanel.SetActive (true);
		GameOverGameScoreText.text = gameScore.ToString ();
		if (gameScore < maxScore)
			GameOverMaxScoreText.text = maxScore.ToString ();
		else {
			GameOverMaxScoreText.text = gameScore.ToString ();
			PlayerPrefs.SetInt ("BestScore", gameScore);
		}
		HitTargetScoreText.text = createTargetScript.GetTargetCounter.ToString ();
	}

	// Time Slider Function //

	private void timeCount(){
		if (!isPause){
			if (timeleft > 0)
				timeleft -= Time.deltaTime;
			else {
				if (isFirstPass) {
			//		StopFromClickingPanel.SetActive (true);
					isFirstPass = false;
					if (!hasMoveBeforeTime) 								//It checks whether player moves before time
						isGameOver = true;
					else
           //			NumericSign = signmanagerScript.GenerateSign ();
                        tempSign = signmanagerScript.GenerateSign();
                }
				if (isFinishGenerate) {
					NumericSign = tempSign;             // new
					isFinishGenerate = false;
				}
				if (isFinishAnimation) {
					isFinishAnimation = false;
					isFirstPass = true;
            //        NumericSign = tempSign;             // new
					timeleft = TimeForGenerationSign;
			//		StopFromClickingPanel.SetActive (false);
					hasMoveBeforeTime = false;
				}
			}
			changeSlider();
		}
	}

	private void changeSlider(){
		timeSlider.value = timeleft/TimeForGenerationSign;
	}

	private void changeTimeSliderColor(){
		timeSliderAnimator.SetBool ("pass", hasMoveBeforeTime);
	} 

	// Target Function //

	private void checkNumberIsTarget(){
		int i;
		bool flag = false;
		for (i = 0 ; i < CreateTarget.NumOfTarget; i++) {
			flag = tilemanagerScript.CheckNumberIsTarget (CreateTarget.targetNum [i]);
			if (flag) {
				createTargetScript.correctTile (i);
				break;
			}
		}
	}

	// Score Counting Function //

	private void resetAllScore(){
		gameScore = 0;
		maxScore = PlayerPrefs.GetInt("BestScore",0);
		MaxScoreText.text = maxScore.ToString ();
		GameScoreText.text = gameScore.ToString ();
	}

	public void addScore(string name){
		if (string.Compare (name, "combine") == 0) {
			gameScore += ScoreForCombine;
		} else if (string.Compare (name, "move") == 0) {
			gameScore += ScoreForMove;
		} else {
			gameScore += ScoreForHitTarget;
		}
		updateScore ();
	}

	private void updateScore(){
		GameScoreText.text = gameScore.ToString ();
		if (gameScore > maxScore)
			MaxScoreText.text = gameScore.ToString ();
	}

} 
