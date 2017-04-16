using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameLogicDecisionNoExistChar : GameLogicThreeButtons {

    public GameLogicDecisionNoExistChar( int difficulty ) : base(difficulty) {
    }

    // 难度从0到15
    // 字符数从5个开始，每升一级增加一个
    // 包含字符数从2个开始，第1级增加到3个，然后升3级增加一个。
    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameNameAndDescription( "NonExist", "Which character", " is not here?" );

        _gameController.SetColorIndex( 4 );

        int dif = _difficulty;
        if(dif>15){
            dif=15;
        }

        int charNumber = dif+6;
        int candidateNumber = 2+(dif+2)/2;

        string chars="";
        int[] candidates = new int[candidateNumber+1];
        for(int m=0; m<candidateNumber+1; m++ ) {
            bool exist = false;
            do{
                exist = false;
                candidates[m]=KWUtility.Random(0,26)+65;
                for(int n=0; n<m; n++ ) {
                    if(candidates[m]==candidates[n]){
                        exist=true;
                        break;
                    }
                }
            }while( exist==true );
        }

        int[] charData = new int[charNumber];
        for(int m=0; m<charNumber; m++ ) {
            if(m<candidateNumber) {
                charData[m] = candidates[m];
            }
            else {
                charData[m]=candidates[KWUtility.Random(0,candidateNumber)];
            }
        }
        int x, y;
        int temp;
        for(int m=0; m<20; m++ ) {
            x = KWUtility.Random( 0, charNumber);
            y = KWUtility.Random( 0, charNumber);
            temp = charData[x];
            charData[x]=charData[y];
            charData[y]=temp;
        }


        for(int m=0; m<charNumber; m++ ) {
            chars+=((char)charData[m]).ToString();
        }

        Debug.Log( "String:"+chars );

        _gameController.SetMainText( chars, Color.clear );

        Debug.Log( "Buttons:"+ ((char)candidates[candidateNumber]).ToString()+ ((char)candidates[0]).ToString()+ ((char)candidates[1]).ToString() );

        SetButtonsRandom( ((char)candidates[candidateNumber]).ToString(), ((char)candidates[0]).ToString(), ((char)candidates[1]).ToString() );
    }
}
