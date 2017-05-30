using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using DG.Tweening;

public class GameLogicDecisionTapNumber : GameLogic {

    const int MapBlockDelta = 24;
    int MapBlockSize;

    int _blockNumber;
    int[,] _mapData;
    Text[,] _mapBoard;
    bool[,] _mapDirection;

    int _mapWidth;
    int _mapHeight;

    int _lastTappedX;
    int _lastTappedY;

    int _numberCount;
    int _targetNumber;

    public GameLogicDecisionTapNumber( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {

    }

    // 关于这个游戏的难度
    // 难度支持0-11:
    // 难度0: 2x2
    // 难度1-2: 2x3
    // 难度3-5: 3x3
    // 难度6-8: 3x4
    // 难度9-12: 4x4
    // 难度13-16: 4x5


    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetButtonMode( GameController.Button_None );

        _gameController.SetColorIndex( 4 );

        int shapePosY;

        switch(_difficulty) {
        case 0:
            _mapWidth=2;
            _mapHeight=2;
            break;
        case 1:
            _mapWidth=3;
            _mapHeight=2;
            break;
        case 2:
            _mapWidth=3;
            _mapHeight=3;
            break;
        default:
            _mapWidth=4;
            _mapHeight=3;
            break;
        }
        if(_mapWidth>2) {
            MapBlockSize = (int) _gameController.boardHeight/(_mapWidth+5);
        }
        else {
            MapBlockSize = (int) _gameController.boardHeight/7;
        }

        _targetNumber = KWUtility.Random( 0, 2 );

        string title = "";
        if(_targetNumber==0) {
            title = "Tap all even numbers";
        }
        else {
            title = "Tap all odd numbers";
        }

        if( _mapHeight<4 ) {
            _gameController.SetGameDescription1( 4, title );
            shapePosY = _gameController.boardHeight*25/80;
        }
        else if( _mapHeight<5 ) {
            _gameController.SetGameDescription1( 4, title);
            shapePosY = _gameController.boardHeight*25/80;
        }
        else {
            _gameController.SetGameDescription1( 5, title );
            shapePosY = _gameController.boardHeight*33/80;
        }

        _mapData = new int[_mapWidth, _mapHeight];
        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
                _mapData[m,n]=KWUtility.Random( 6, 20 );
            }
        }

        if(_targetNumber==0){
            _mapData[KWUtility.Random(0,_mapWidth),KWUtility.Random(0,_mapHeight)]=KWUtility.Random(3,8)*2;
        }
        else {
            _mapData[KWUtility.Random(0,_mapWidth),KWUtility.Random(0,_mapHeight)]=KWUtility.Random(3,8)*2+1;
        }

        _numberCount = 0;
        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
                if((_targetNumber==0)&&(_mapData[m,n]%2==0)) {
                    _numberCount++;
                }
                if((_targetNumber==1)&&(_mapData[m,n]%2==1)) {
                    _numberCount++;
                }
            }
        }

       

        _mapBoard = new Text[_mapWidth, _mapHeight];
        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
                CreateText( _mapData[m,n], m, n );
            }
        }

       
    }

    void CreateText( int number, int posX, int posY ) {

        Vector2 pos = GetPosition( posX, posY );
        Text txtBoard = (Text) GameObject.Instantiate( _gameController.goBoardChar );
        txtBoard.gameObject.SetActive( true );
        txtBoard.transform.SetParent( _gameController.goBoardArea.transform );
        txtBoard.color = Color.white;

        txtBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
        txtBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
        txtBoard.rectTransform.localScale = Vector3.one;

        _mapBoard[posX,posY]=txtBoard;
        _goList.Add( txtBoard.gameObject );

        txtBoard.text = number.ToString();

    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta)-MapBlockSize/2 );
    }

    void HideCard( int x, int y ) {
        DOTween.Play( _mapBoard[x,y].rectTransform.DOScale( Vector3.zero, 0.5f ).SetEase( Ease.InBack ).OnComplete( ()=> {
            _blockNumber--;
            if(_blockNumber==0) {
                _status = Status_Gameover;
                _gameController.SendGameResult( true );
            }
        } ) );
        _mapData[x,y]=-1;
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
            if(_mapData[tapX,tapY]%2==_targetNumber) {
                HideCard( tapX, tapY );
                _numberCount--;

                if(_numberCount==0) {
                    _status = Status_Gameover;
                    _gameController.SendGameResult( true );
                }
                else {
                    MainPage.instance.PlaySound( MainPage.Sound_Tap );
                }
            }
            else {
                _gameController.SendGameResult( false );
            }
        }
    }
}
