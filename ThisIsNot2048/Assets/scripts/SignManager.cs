using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignManager : MonoBehaviour {

	public Animator RandomPanel;

	int SignNumber;

	class ConstSign{
		public const char AddSign = '+';
		public const char MinusSign = '-';
		public const char MultipleSign = '*';
		public const char DivideSign = '/';
		public const int Add = 1;
		public const int Minus = 2;
		public const int Multiple = 3;
		public const int Divide = 4;

		public static char findSuitableSign(int temp){
			switch (temp) {
			case (Add):
				return AddSign;
			case (Minus):
				return MinusSign;
			case (Multiple):
				return MultipleSign;
			case (Divide):
				return DivideSign;
			}
			return AddSign;
		}
	}

	class ProbabiltySign{
		public static int AddProb = 25;
		public static int MinusProb = 25;
		public static int MultipleProb = 25;
		public static int DivideProb = 25;

		public static int SameSignCounter = 1;

		const int probalityDecrease = 3;

		public static int probabilityAppearSign(int SignNumber){
			int temp = Random.Range(1,101);
			if (SameSignCounter > 1) {
				switch (SignNumber) {
				case 1:
					AddProb -= probalityDecrease;
					MinusProb++;
					MultipleProb++;
					DivideProb++;
					break;
				case 2:
					MinusProb -= probalityDecrease;
					AddProb++;
					MultipleProb++;
					DivideProb++;
					break;
				case 3:
					MultipleProb -= probalityDecrease;
					AddProb++;
					MinusProb++;
					DivideProb++;
					break;
				case 4:
					DivideProb -= probalityDecrease;
					AddProb++;
					MinusProb++;
					MultipleProb++;
					break;
				}
			}
			if (temp < AddProb) {
				if (SignNumber == ConstSign.Add)
					SameSignCounter++;
				else
					setToDefault ();
				return ConstSign.Add;
			}else if (temp < AddProb + MinusProb){
				if (SignNumber == ConstSign.Minus)
					SameSignCounter++;
				else
					setToDefault ();
				return ConstSign.Minus;
			}else if (temp < AddProb + MinusProb + MultipleProb){
				if (SignNumber == ConstSign.Multiple)
					SameSignCounter++;
				else
					setToDefault ();
				return ConstSign.Multiple;
			}else{
				if (SignNumber == ConstSign.Divide)
					SameSignCounter++;
				else
					setToDefault ();
				return ConstSign.Divide;
			}
		}

		public static void setToDefault(){
			AddProb = 25;
			MinusProb = 25;
			MultipleProb = 25;
			DivideProb = 25;
			SameSignCounter = 1;
		}
	}

	public char GenerateSign(){
		SignNumber = ProbabiltySign.probabilityAppearSign (SignNumber);
		RandomPanel.CrossFade ("randomChoosing", 0f);
		RandomPanel.SetInteger("counter",SignNumber);
		return ConstSign.findSuitableSign (SignNumber);
	}

	public void GameStartSetSign(){
		SignNumber = ConstSign.Add;
		RandomPanel.SetInteger ("counter", SignNumber);
		GameManager.NumericSign = ConstSign.AddSign;
		ProbabiltySign.SameSignCounter = 2;
	}

}
