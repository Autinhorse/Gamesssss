using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using DG.Tweening;

public class GameLogicResolveTapLetter : GameLogic {

    const int MapBlockDelta = 32;
    int MapBlockSize;

    public GameLogicResolveTapLetter( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    int _mapWidth;
    int _mapHeight;

    int[] _mapData;

    Text[,] _mapChar;

    Image[,] _mapImage;

    int _targetX, _targetY;

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );



        _gameController.SetButtonMode( GameController.Button_None );


        _gameController.SetGameDescription1( 6, "Tap the special one" );

        _gameController.SetColorIndex( 1 );

        int charNumber;
        switch( _difficulty ) {
        case 0:
            _mapWidth=2;
            _mapHeight=2;
            break;
        case 1:
            _mapWidth=3;
            _mapHeight=3;
            break;
        case 2:
            _mapWidth=4;
            _mapHeight=4;
            break;
        default:
            _mapWidth=4;
            _mapHeight=5;
            break;
        }

        MapBlockSize = (int) _gameController.boardHeight/(_mapHeight+5);

        int temp;
        int i1,i2;

        string[] letterPairs = { "I", "1", "M", "N", "O", "Q", "O", "0"};
        int pairsIndex = KWUtility.Random( 0, letterPairs.Length/2 )*2;

        _targetX = KWUtility.Random( 0, _mapWidth );
        _targetY = KWUtility.Random( 0, _mapHeight );
            
        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
            
                Text textObject = (Text) GameObject.Instantiate( _gameController.goBoardTapLetterChar );
                _goList.Add( textObject.gameObject );
                textObject.gameObject.SetActive( true );



                textObject.transform.SetParent( _gameController.goBoardArea.transform );
                Vector2 pos = GetPosition( m, n );

                textObject.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );

                if((m==_targetX)&&(n==_targetY)) {
                    textObject.text = letterPairs[pairsIndex+1];
                }
                else {
                    textObject.text = letterPairs[pairsIndex];
                }
                textObject.color = Color.white;

                textObject.rectTransform.localScale = Vector3.one;
            }
        }
    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta)-MapBlockSize );
    }

    public override void OnBoardTapped( Vector3 pos ) {
        if(_status!=Status_Playing) {
            return;
        }


        int tapX = -100;
        int tapY = -100;
        for(int m=0;m<_mapWidth;m++) {
            for(int n=0; n<_mapHeight;n++ ) {
                Vector2 center = GetPosition( m, n );
                if((pos.x>center.x-(MapBlockSize+MapBlockDelta)/2)&&(pos.x<center.x+(MapBlockSize+MapBlockDelta)/2)&&(pos.y>center.y-(MapBlockSize+MapBlockDelta)/2)&&(pos.y<center.y+(MapBlockSize+MapBlockDelta)/2)) {
                    tapX=m;
                    tapY=n;
                }
            }
        }

        if((tapX>=0)&&(tapX<_mapWidth)&&(tapY>=0)&&(tapY<_mapHeight)) {
            if((tapX==_targetX)&&(tapY==_targetY)) {
                _status = Status_Gameover;
                _gameController.SendGameResult( true );
            }
            else {
                _status = Status_Gameover;
                _gameController.SendGameResult( false );
            }
        }

    }
}
