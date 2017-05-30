using UnityEngine;
using System.Collections;

public class GameLogicAction01 :   GameLogicTwoButtons {

    public GameLogicAction01(int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    byte[] resultData;
    char[] resultChar;
    int _inputIndex;

    // 难度0-7，只有加减法
    // 难度8-15，两步计算
    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetColorIndex( 2 );

        int charNumber=0;

        switch(_difficulty) {
        case 0:
            charNumber = 6;
            break;
        case 1:
            charNumber = 8;
            break;
        case 2:
            charNumber = 10;
            break;
        default:
            charNumber = 12;
            break;
        }

        resultData = new byte[charNumber];
        byte temp;
        for( int m=0;m<charNumber; m++ ) {
            resultData[m]=(byte)KWUtility.Random(0,2);
        }

        resultChar = new char[2];

        switch(KWUtility.Random(0,3)) {
        case 0: {
                resultChar[0]='A';
                resultChar[1]='B';
                break;
            }
        case 1:
            resultChar[0]='0';
            resultChar[1]='1';
            break;
        case 2:
            resultChar[0]='X';
            resultChar[1]='Y';
            break;
        }

        string result="";

        for(int m=0;m<charNumber;m++) {
            result+= ((char)( (int)resultChar[resultData[m]])).ToString();
        }

        _gameController.SetGameDescription1( 0, "Input the text" );
        _gameController.SetMainText( result, Color.clear );

        SetButtons( resultChar[0].ToString(), resultChar[1].ToString() );

        _inputIndex = 0;
    }

    public override void OnButtonPressed( int buttonIndex ) {
        if(_status!=Status_Playing) {
            return;
        }

        if(buttonIndex!=resultData[_inputIndex]) {
            _status = Status_Gameover;
            _gameController.SendGameResult( false );
        }
        /*
        string result="";

        for(int m=1;m<resultData.Length;m++) {
            result+= ((char)( (int)resultChar[resultData[m]])).ToString();
        }
        _gameController.SetMainText( result, Color.clear );
*/
        _inputIndex++;
        if(_inputIndex==resultData.Length) {
            _status = Status_Gameover;
            _gameController.SendGameResult( true );
        }
    }
}
