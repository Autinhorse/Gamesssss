using UnityEngine;
using System.Collections;

public class GameLogicMathSum : GameLogicThreeButtons {

    public GameLogicMathSum( int difficulty ) : base(difficulty) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameName( "Sum" );
        _gameController.SetGameDescription1( 0, "Which is the right answer?" );

        _gameController.SetColorIndex( 0 );

        int maxNumber = _difficulty*5+4;
        int number1 = UnityEngine.Random.Range( 0, maxNumber )+1;
        int number2 = UnityEngine.Random.Range( 0, maxNumber )+1;

        int result = number1+number2;

        _gameController.SetMainText( number1.ToString()+" + "+number2.ToString(), Color.clear );

        switch(UnityEngine.Random.Range(0,3)){
        case 0:
            SetButtonsRandom( result.ToString(), (result-2).ToString(), (result-1).ToString() );
            break;
        case 1:
            SetButtonsRandom( result.ToString(), (result+1).ToString(), (result-1).ToString() );
            break;
        case 2:
            SetButtonsRandom( result.ToString(), (result+2).ToString(), (result+1).ToString() );
            break;
        }
    }

}
