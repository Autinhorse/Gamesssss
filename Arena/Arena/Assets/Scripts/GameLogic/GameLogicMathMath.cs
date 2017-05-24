using UnityEngine;
using System.Collections;

public class GameLogicMathMath : GameLogicTwoButtons {

    public GameLogicMathMath(int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    // 难度0-7，只有加减法
    // 难度8-15，两步计算
    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameName( "MATH" );
        _gameController.SetGameDescription1( 0, "Right or wrong?" );

        _gameController.SetColorIndex( 0 );

        if(_difficulty<3) {
            int maxNumber = _difficulty*3+4;
            int number1 = UnityEngine.Random.Range( _difficulty, maxNumber )+1;
            int number2 = UnityEngine.Random.Range( _difficulty, maxNumber )+1;

            int result = number1+number2;

            bool right;
            if(KWUtility.Random(0,2)==0){
                _rightButtonIndex = 0;
            }
            else {
                _rightButtonIndex = 1;
                if(KWUtility.Random(0,2)==0) {
                    result+=KWUtility.Random(1,4);
                }
                else {
                    result-=KWUtility.Random(1,4);
                }
            }

            if(KWUtility.Random(0,2)==0) {
                // 加法
                _gameController.SetMainText( number1.ToString()+" + "+number2.ToString()+" = "+result.ToString(), Color.clear );
            }
            else {
                _gameController.SetMainText( result.ToString()+" - "+number1.ToString()+" = "+number2.ToString(), Color.clear );
            }
        }
        else {
            int maxNumber = _difficulty*2-10;
            if(maxNumber>19) {
                maxNumber=19;
            }
            int minNumber = _difficulty-7;
            if(minNumber>5) {
                minNumber=5;
            }
            int number1 = UnityEngine.Random.Range( minNumber, maxNumber )+1;
            int number2 = UnityEngine.Random.Range( minNumber, maxNumber )+1;
            int number3 = UnityEngine.Random.Range( minNumber, maxNumber )+1;

            string question;

            bool sign1, sign2;
            sign1=(KWUtility.Random( 0, 2)==0);
            int result1 = number1+number2;

            if(sign1==true) {
                question = number1.ToString()+" + "+number2.ToString();
            }
            else {
                question = result1.ToString()+" - "+number1.ToString();
                result1 = number2;
            }

            sign2=(KWUtility.Random( 0, 2)==0);
            int result2 = 0;

            if(sign2==true) {
                question = question + " + " + number3;
                result2 = result1+number3;
            }
            else {
                question = question + " - " + number3;
                result2 = result1-number3;
            }

            bool right;
            if(KWUtility.Random(0,2)==0){
                _rightButtonIndex = 0;
            }
            else {
                _rightButtonIndex = 1;
                if(KWUtility.Random(0,2)==0) {
                    result2+=KWUtility.Random(1,4);
                }
                else {
                    result2-=KWUtility.Random(1,4);
                }
            }

            _gameController.SetMainText( question+" = "+result2.ToString(), Color.clear );
        }
        SetButtons( "RIGHT", "WRONG" );

    }
}
