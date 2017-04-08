using UnityEngine;
using System.Collections;

public class GameLogicThreeButtons : GameLogic {

    protected int _rightButtonIndex;

    public GameLogicThreeButtons( int difficulty ) : base (difficulty) {
        
    }

    protected void SetButtonsRandom( string rightResult, string wrongResult1, string wrongResult2 ) {
        int[] results = new int[3];
        for( int i=0; i<3; i++ ) {
            results[i] = i;
        }

        int temp;
        int j, k;
        for( int i=0; i<9; i++ ) {
            j=KWUtility.Random( 0, 3 );
            k=KWUtility.Random( 0, 3 );
            temp=results[j];
            results[j]=results[k];
            results[k]=temp;
        }

        _gameController.SetButtonMode( GameController.Button_Three );

        string result="";
        for( int i=0; i<3; i++ ) {
            switch(results[i]) {
            case 0:
                _rightButtonIndex = i;
                result = rightResult;
                break;
            case 1:
                result = wrongResult1;
                break;
            case 2:
                result = wrongResult2;
                break;
            }
            _gameController.SetButtons( i, result, Color.clear );
        }
    }

    protected void SetButtonsRandom( Sprite rightResult, Sprite wrongResult1, Sprite wrongResult2 ) {
        int[] results = new int[3];
        for( int i=0; i<3; i++ ) {
            results[i] = i;
        }

        int temp;
        int j, k;
        for( int i=0; i<9; i++ ) {
            j=KWUtility.Random( 0, 3 );
            k=KWUtility.Random( 0, 3 );
            temp=results[j];
            results[j]=results[k];
            results[k]=temp;
        }

        _gameController.SetButtonMode( GameController.Button_Three );

        Sprite result=rightResult;
        for( int i=0; i<3; i++ ) {
            switch(results[i]) {
            case 0:
                _rightButtonIndex = i;
                result = rightResult;
                break;
            case 1:
                result = wrongResult1;
                break;
            case 2:
                result = wrongResult2;
                break;
            }
            _gameController.SetButtons( i, result, Color.clear );
        }
    }

    protected void SetButtons( string text1, string text2, string text3 ) {
        _gameController.SetButtonMode( GameController.Button_Three );

        _gameController.SetButtons( 0, text1, Color.clear );
        _gameController.SetButtons( 1, text2, Color.clear );
        _gameController.SetButtons( 2, text3, Color.clear );
    }

    protected void SetButtons( int index, string text, Color color  ) {

        _gameController.SetButtons( index, text, color );
    }

    protected void SetButtons( Sprite shape1, Sprite shape2, Sprite shape3 ) {
        _gameController.SetButtonMode( GameController.Button_Three );

        _gameController.SetButtons( 0, shape1, Color.clear );
        _gameController.SetButtons( 1, shape2, Color.clear );
        _gameController.SetButtons( 2, shape3, Color.clear );
    }

    protected void SetButtons( int index, Sprite shape, Color color ) {
        _gameController.SetButtons( index, shape, color );
    }

    public override void OnButtonPressed( int buttonIndex ) {
        if(buttonIndex==_rightButtonIndex){
            _gameController.SendGameResult( true );
        }
        else {
            _gameController.SendGameResult( false );
        }
    }


}
