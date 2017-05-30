using UnityEngine;
using System.Collections;

public class GameLogicDecisionYes :  GameLogicTwoButtons {

    public GameLogicDecisionYes(int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    // 难度0-7，只有加减法
    // 难度8-15，两步计算
    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetColorIndex( 4 );

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

        byte[] resultData = new byte[charNumber];
        byte temp;
        for( int m=0;m<charNumber; m++ ) {
            resultData[m]=(byte)KWUtility.Random(0,3);
            if((m>=2)&&(resultData[m]==2)) {
                if((resultData[m-1]==1)&&(resultData[m-2]==0)) {
                    switch(KWUtility.Random(0,2)) {
                    case 0:
                        resultData[m-1]=(byte)(KWUtility.Random(0,2)*2);
                        break;
                    case 1:
                        resultData[m]=(byte)KWUtility.Random(0,2);
                        break;
                    }
                }
            }
        }

        if(KWUtility.Random(0,2)==0) {
            _rightButtonIndex = 0;

            int pos = KWUtility.Random( 1, charNumber-2);
            resultData[pos]=0;
            resultData[pos+1]=1;
            resultData[pos+2]=2;
        }
        else {
            _rightButtonIndex = 1;
        }

        char[] resultChar = new char[3];

        switch(KWUtility.Random(0,4)) {
        case 0: {
                char startChar =(char)( KWUtility.Random(0,6)+49);
            resultChar[0]=(char)startChar;
            resultChar[1]=(char)(startChar+1);
            resultChar[2]=(char)(startChar+2);
            break;
            }
        case 1:
            resultChar[0]='Y';
            resultChar[1]='E';
            resultChar[2]='S';
            break;
        case 2:
            resultChar[0]='A';
            resultChar[1]='B';
            resultChar[2]='C';
            break;
        case 3:
            resultChar[0]='T';
            resultChar[1]='A';
            resultChar[2]='P';
            break;
        }

        string result="";

        for(int m=0;m<charNumber;m++) {
            result+= ((char)( (int)resultChar[resultData[m]])).ToString();
        }

        _gameController.SetGameDescription1( 0, "Do you see '"+resultChar[0].ToString()+resultChar[1].ToString()+resultChar[2].ToString()+"'?" );



        _gameController.SetMainText( result, Color.clear );

        SetButtons( "YES", "NO" );

    }
}
