using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

	public static class Const {
		public const int XaxisSize = 4;
		public const int YaxisSize = 4;
	}

	public TileObject[] tileScript;

	public GameManager gameManagerScript;

	int pastNum;
	int pastX,pastY;

	int hitTargetX,hitTargetY;
	bool firstClickForTarget = false;												//used for target check after disapper tile

	bool[,] coorIsNumCell = new bool[Const.YaxisSize,Const.XaxisSize];
	bool[,] coorIsDeadCell = new bool[Const.YaxisSize, Const.XaxisSize];

	int[,] coorOnOffEmptyCell = new int[Const.YaxisSize + Const.XaxisSize - 2,2];
	int countOnOffEmpty;

	public void selectedTile(int x,int y,ref bool firstTime){
		if (firstTime) {
			tileScript [y * Const.YaxisSize + x].tellTileChoose (true);				//light up selected tile

			pastX = x;
			pastY = y;
			pastNum = tileScript [y * Const.YaxisSize + x].CellNum;

			OnOffEmptyTile (pastX,pastY,true);

			firstTime = !firstTime;

			firstClickForTarget = true;
		} else {
			if (x == pastX && y == pastY) {																//click the original clicked tile
				tileScript [pastY * Const.YaxisSize + pastX].tellTileChoose (false);
				OnOffEmptyTile (pastX,pastY,false);

				firstTime = true;

				firstClickForTarget = false;
			}else if ((x == pastX || y == pastY) && coorIsNumCell [y, x] && checkCanCombine(x,y)){		//click the tile that he wanted to combine
				gameManagerScript.hasMoveBeforeTime = true;

				tileScript [pastY * Const.YaxisSize + pastX].tellTileChoose (false);
				OnOffEmptyTile (pastX,pastY,false);
				combineTile (x, y);
				disableFirstSelectedTile ();
				//FindEmptyTile ();									<-- too slow
				updateEmptyTile(x,y);
				firstTime = !firstTime;

				gameManagerScript.isAMove = true;

				firstClickForTarget = false;

				gameManagerScript.addScore ("combine");
			}else if ((x == pastX || y == pastY) && !coorIsNumCell [y,x]){								// the tile to empty						//click the tile that he wanted to move				
				tileScript [pastY * Const.YaxisSize + pastX].tellTileChoose (false);
				OnOffEmptyTile (pastX,pastY,false);
				moveTileToEmpty(x,y);
				disableFirstSelectedTile ();
			//	FindEmptyTile ();								<-- too slow
				updateEmptyTile(x,y);
				firstTime = !firstTime;
				gameManagerScript.isAMove = true;

				firstClickForTarget = false;

				gameManagerScript.addScore ("move");
			}else{																						//click other tile
				tileScript [pastY * Const.YaxisSize + pastX].tellTileChoose (false);
				OnOffEmptyTile (pastX,pastY,false);
				tileScript [y * Const.YaxisSize + x].tellTileChoose (true);

				pastX = x;
				pastY = y;
				pastNum = tileScript [y * Const.YaxisSize + x].CellNum;

				OnOffEmptyTile (pastX, pastY, true);

				firstTime = false;

				firstClickForTarget = true;
			}
		}
		//FindEmptyTile ();
	}

	public void spawnNumberTile(){
		int x, y;
		FindEmptyTile ();
		do {
			x = Random.Range(0,Const.XaxisSize);
			y = Random.Range(0,Const.YaxisSize);
		} while(coorIsNumCell [y, x]);
		tileScript [y * Const.YaxisSize + x].enableNumberCell (true);
		FindEmptyTile ();
	}

	public void ResetDeadCell(){
		for (int y = 0; y < Const.YaxisSize; y++)
			for (int x = 0; x < Const.XaxisSize; x++)
				coorIsDeadCell [y, x] = false;
	}

	public void UpdateDeadCell(int x,int y){
		coorIsDeadCell [y, x] = true;
	}

	public bool CheckGameOver(){
		for (int y = 0; y < Const.YaxisSize; y++) {
			for (int x = 0; x < Const.XaxisSize - 1; x++)
				if (!coorIsDeadCell [y, x] && !coorIsDeadCell [y, x + 1])
					return false;
		}
		for (int x = 0; x < Const.XaxisSize; x++) {
			for (int y = 0; y < Const.YaxisSize - 1; y++)
				if (!coorIsDeadCell [y, x] && !coorIsDeadCell [y + 1, x ])
					return false;
		}
		return true;
	}

	public bool CheckNumberIsTarget(int checkNum){
		for (int y = 0; y < Const.YaxisSize; y++)
			for (int x = 0; x < Const.XaxisSize; x++)
				if (coorIsNumCell [y, x] && !coorIsDeadCell [y, x] && tileScript [y * Const.YaxisSize + x].CellNum == checkNum) {
					tileScript [y * Const.YaxisSize + x].hitTarget ();
					hitTargetX = x;
					hitTargetY = y;
					gameManagerScript.addScore ("hitTarget");
					return true;
				}
		return false;
	}

	public void RecheckSelectedTileAfterHitTarget(){
		FindEmptyTile ();
		if (firstClickForTarget) {
			if (pastX == hitTargetX || pastY == hitTargetY) {
				OnOffEmptyTile (pastX, pastY, true);
			}
		}
	}

	public int CountNumberCell(){
		int counter = 0;
		for (int y = 0; y < Const.YaxisSize; y++)
			for (int x = 0; x < Const.XaxisSize; x++)
				if (coorIsNumCell [y, x] && !coorIsDeadCell[y,x])
					counter++;
		return counter;
	}

	private void combineTile(int x,int y){
		int result = 0;
		switch(GameManager.NumericSign){
		case ('+'):
			result = pastNum + tileScript [y * Const.YaxisSize + x].CellNum;
			break;
		case ('-'):
			if (SettingPanel.isNoMinusReverse)
				result = tileScript [y * Const.YaxisSize + x].CellNum - pastNum;
			else
				result = pastNum - tileScript [y * Const.YaxisSize + x].CellNum;
			break;
		case ('*'):
			result = pastNum * tileScript [y * Const.YaxisSize + x].CellNum;
			break;
		case ('/'):
			if (SettingPanel.isNoDivideReverse)
				result = tileScript [y * Const.YaxisSize + x].CellNum / pastNum;
			else
				result = pastNum / tileScript [y * Const.YaxisSize + x].CellNum;
			break;
		}
		tileScript [y * Const.YaxisSize + x].updateCellNum (result);
	}

	private void moveTileToEmpty (int x,int y){
		tileScript [y * Const.YaxisSize + x].enableNumberCell (false);
		tileScript [y * Const.YaxisSize + x].updateCellNum (pastNum);
	}

	private void disableFirstSelectedTile(){
		tileScript [pastY * Const.YaxisSize + pastX].disableNumberCell ();
	}

	private void FindEmptyTile(){
		for (int y = 0; y < Const.YaxisSize; y++)
			for (int x = 0; x < Const.XaxisSize; x++)
				coorIsNumCell [y, x] = tileScript [y * Const.YaxisSize + x].isNumberCell;
	}
		
	// focus on selected tile -> move (faster, no loop)
	private void updateEmptyTile(int x, int y){
		coorIsNumCell [pastY, pastX] = tileScript [pastY * Const.YaxisSize + pastX].isNumberCell;
		coorIsNumCell [y, x] = tileScript [y * Const.YaxisSize + x].isNumberCell;
	}

	private void OnOffEmptyTile(int x,int y,bool switchOnOff){
		int i;
		if (switchOnOff) {
			checkObstacle (x, y);
		}
		for (i = 0; i < countOnOffEmpty; i++) 
			tileScript [coorOnOffEmptyCell [i, 1] * Const.YaxisSize + coorOnOffEmptyCell [i, 0]].isEnableEmptyCell (switchOnOff);
	}

	private bool checkCanCombine(int x,int y){
		if (x == pastX - 1 || x == pastX + 1) 
			return true;
		if (y == pastY - 1 || y == pastY + 1) 
			return true;
		if (y == pastY) {
			if (x > pastX) {
				for (int i = x - 1; i > pastX; i--)
					if (coorIsNumCell [y, i])
						return false;
				return true;
			} else {
				for (int i = x + 1; i < pastX; i++)
					if (coorIsNumCell [y, i])
						return false;
				return true;
			}
		}
		if (x == pastX) {
			if (y > pastY) {
				for (int i = y - 1; i > pastY; i--)
					if (coorIsNumCell [i, x]) 
						return false;
				return true;
			} else {
				for (int i = y + 1; i < pastY; i++) 
					if (coorIsNumCell [i, x]) 
						return false;
				return true;
			}
		}
		return false;
	}

	private void checkObstacle(int x,int y){
		int i;
		bool[] tempX = new bool[Const.XaxisSize];
		bool[] tempY = new bool[Const.YaxisSize];

		for (i = 0; i < Const.XaxisSize; i++)
			tempX [i] = false;
		for (i = 0; i < Const.YaxisSize; i++)
			tempY [i] = false;
		if (x == 0) {
			for (i = 1; i < Const.XaxisSize; i++)
				if (coorIsNumCell [y, i] == true)
					break;
				else
					tempX [i] = true;
		} else if (x == Const.XaxisSize-1) {
			for (i = 2; i >= 0; i--)
				if (coorIsNumCell [y, i] == true)
					break;
				else
					tempX [i] = true;
		} else {
			for (i = x-1; i >= 0; i--)
				if (coorIsNumCell [y, i] == true)
					break;
				else
					tempX [i] = true;
			for (i = x+1; i < Const.XaxisSize; i++)
				if (coorIsNumCell [y, i] == true)
					break;
				else
					tempX [i] = true;
		}
			
		if (y == 0) {
			for (i = 1; i < Const.YaxisSize; i++)
				if (coorIsNumCell [i, x] == true)
					break;
				else
					tempY [i] = true;
		} else if (y == Const.YaxisSize-1) {
			for (i = 2; i >= 0; i--)
				if (coorIsNumCell [i, x] == true)
					break;
				else
					tempY [i] = true;
		} else {
			for (i = y-1; i >= 0; i--)
				if (coorIsNumCell [i, x] == true)
					break;
				else
					tempY [i] = true;
			for (i = y+1; i < Const.YaxisSize; i++)
				if (coorIsNumCell [i, x] == true)
					break;
				else
					tempY [i] = true;
		}

		countOnOffEmpty = 0;
		for (i = 0; i < Const.XaxisSize; i++)
			if (tempX [i] == true) {
				coorOnOffEmptyCell [countOnOffEmpty,0] = i;
				coorOnOffEmptyCell [countOnOffEmpty, 1] = y;
				countOnOffEmpty++;
			} 
		for (i = 0; i < Const.YaxisSize; i++)
			if (tempY [i] == true) {
				coorOnOffEmptyCell [countOnOffEmpty,0] = x;
				coorOnOffEmptyCell [countOnOffEmpty, 1] = i;
				countOnOffEmpty++;
			}
	}

}
