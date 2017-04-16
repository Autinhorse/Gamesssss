using UnityEngine;
using System.Collections;

public class GameLogicTwoButtons : GameLogic {

    protected int _rightButtonIndex;

    public GameLogicTwoButtons( int difficulty ) : base (difficulty) {

    }

    protected void SetButtonsRandom( string rightResult, string wrongResult ) {
        int[] results = new int[2];
        for( int i=0; i<2; i++ ) {
            results[i] = i;
        }

        int temp;
        int j, k;
        for( int i=0; i<9; i++ ) {
            j=KWUtility.Random( 0, 2 );
            k=KWUtility.Random( 0, 2 );
            temp=results[j];
            results[j]=results[k];
            results[k]=temp;
        }

        _gameController.SetButtonMode( GameController.Button_Two );

        string result="";
        for( int i=0; i<2; i++ ) {
            switch(results[i]) {
            case 0:
                _rightButtonIndex = i;
                result = rightResult;
                break;
            case 1:
                result = wrongResult;
                break;
            }
            _gameController.SetButtons( i, result, Color.clear );
        }
    }

    protected void SetButtons( string leftText, string rightText ) {
        _gameController.SetButtonMode( GameController.Button_Two );

        _gameController.SetButtons( 0, leftText, Color.clear );
        _gameController.SetButtons( 1, rightText, Color.clear );
    }

    public override void OnButtonPressed( int buttonIndex ) {
        _status = Status_Gameover;

        if(buttonIndex==_rightButtonIndex){
            _gameController.SendGameResult( true );
        }
        else {
            _gameController.SendGameResult( false );
        }
    }
}
