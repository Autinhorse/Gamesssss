using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameLogicMathDice : GameLogicThreeButtons {

    const int MapBlockDelta = 16;
    int MapBlockSize;

    int _mapWidth;
    int _mapHeight;

    public GameLogicMathDice( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    // 难度0-15
    // 难度0-1 2个骰子
    // 难度2-4 3个骰子
    // 难度5-8 4个骰子
    // 难度9-12 5个骰子
    // 难度13-15 6个骰子

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameName( "DICE" );
        _gameController.SetGameDescription1( 0, "How many dots on dices?" );

        _gameController.SetColorIndex( 0 );

        MapBlockSize = _gameController.boardWidth/5;

        int diceNumber=0;
        switch(_difficulty){
        case 0:
            diceNumber=2;
            _mapWidth = 1;
            _mapHeight = 2;
            break;
        case 1:
        case 2:
            diceNumber=3;
            _mapWidth = 3;
            _mapHeight = 1;
            break;
        case 3:
        case 4:
            diceNumber=4;
            _mapWidth = 2;
            _mapHeight = 2;
            break;
        case 5:
            diceNumber=5;
            _mapWidth = 3;
            _mapHeight = 2;
            break;
        default:
            diceNumber=6;
            _mapWidth = 3;
            _mapHeight = 2;
            break;
        }

        int result = 0;
        for(int m=0;m<_mapWidth;m++ ) {
            for(int n=0;n<_mapHeight;n++ ) {
                if((diceNumber==5)&&(m==1)&&(n==0)) {
                    continue;
                }
                int dice = KWUtility.Random( 0, 6 );
                result += dice+1;

                Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
                imgBoard.gameObject.SetActive( true );
                imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
                imgBoard.color = Color.white;
                imgBoard.sprite = MainPage.instance.SptDices[dice];
                _goList.Add( imgBoard.gameObject );

                Vector2 pos = GetPosition( m, n);
                imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
                imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
                imgBoard.rectTransform.localScale = Vector3.one*0.99f;
            }
        }
       
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

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta));
    }
}
