using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using System.Collections.Generic;

using DG.Tweening;

public class GameLogicMemoryOrder : GameLogic {

    const int Status_Remebering = 3;

    const int Status_Deciding = 4;

    int _secondStatus;

    const int MapBlockDelta = 24;
    int MapBlockSize;

    //float _timer;
    int _blockNumber;
    int[,] _mapData;
    Image[,] _mapBoard;
    Text[,] _mapChar;
    bool[,] _mapDirection;

    int _mapWidth;
    int _mapHeight;

    int _tapIndex;

    public GameLogicMemoryOrder( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {

    }

    // 关于这个游戏的难度
    // 难度支持0-15:
    // 记忆时间从2秒开始，每增加两级多0.5秒。方块从5个开始，每增加两级增加一个，最多到13个。
    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetButtonMode( GameController.Button_None );

        MapBlockSize = (int) _gameController.boardHeight/8;

        _gameController.SetGameDescription1( 7, "Remember the numbers on tiles."  );
        _gameController.SetGameDescription2( 4, "Tap screen when you are ready." );

        _gameController.SetColorIndex( 3 );

        int difficulty = _difficulty;
        if(difficulty>2){
            difficulty=2;
        }

        _blockNumber = 4+difficulty;

        if(_blockNumber>7) {
            _blockNumber=7;
        }

        switch(_blockNumber){
        case 4:
            _mapWidth=2;
            _mapHeight=2;
            break;
        case 5:
        case 6:
            _mapWidth=3;
            _mapHeight=2;
            break;
        case 7:
        case 8:
        case 9:
            _mapWidth=3;
            _mapHeight=3;
            break;
        default:
            _mapWidth=4;
            _mapHeight=3;
            break;
        }

        _mapData = new int[_mapWidth,_mapHeight];
        _mapDirection  = new bool[_mapWidth,_mapHeight];
        for(int m=0;m<_mapWidth;m++ ) {
            for(int n=0;n<_mapHeight;n++ ) {
                _mapData[m,n]=-1;
                _mapDirection[m,n] = true;
            }
        }
        _mapBoard = new Image[_mapWidth,_mapHeight];
        _mapChar = new Text[_mapWidth,_mapHeight];

        for(int m = 0; m<_blockNumber; m++ ) {
            CreateShape( m );
        }
    }

    void CreateShape( int index ) {
        int posX, posY;
        do {
            posX = KWUtility.Random( 0, _mapWidth );
            posY = KWUtility.Random( 0, _mapHeight );
        }while( _mapData[posX, posY]!=-1);
        _mapData[posX,posY]= index;

        Vector2 pos = GetPosition( posX, posY );
        Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgBoard.gameObject.SetActive( true );
        imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
        imgBoard.color = new Color(0,0,0,0.2f);

        imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
        imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
        _mapBoard[posX,posY]=imgBoard;
        _goList.Add( imgBoard.gameObject );

        Text txtChar = (Text) GameObject.Instantiate( _gameController.goBoardChar );
        txtChar.gameObject.SetActive( true );
        txtChar.transform.SetParent( imgBoard.gameObject.transform );
        txtChar.rectTransform.localPosition = Vector3.zero;
        txtChar.color = Color.white;
        txtChar.text = (index+1).ToString();
        _mapChar[posX,posY]=txtChar;

        //imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize-32, MapBlockSize-32 );

        imgBoard.rectTransform.localScale = Vector3.one;
        //DOTween.Play( imgShape.rectTransform.DOScale( Vector3.one, 0.75f).SetEase( Ease.OutBack ) );



    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta)-MapBlockSize/2 );
    }

    void TurnCard( int x, int y, int delay ) {
        
        if(_mapDirection[x,y]==true) {
            _mapDirection[x,y]=false;

            Sequence seq = DOTween.Sequence();
            seq.Append( _mapBoard[x,y].rectTransform.DOScaleX( 0, 0.2f ).SetEase( Ease.InBack ).SetDelay( delay ) );
            seq.Append( _mapBoard[x,y].rectTransform.DOScaleX( 1, 0.2f ).SetEase( Ease.OutBack ));

            DOTween.Play( seq );
            DOTween.Play( _mapChar[x,y].rectTransform.DOScaleX( 0, 0.2f ).SetEase( Ease.InBack ).SetDelay( delay ) );
        }
        else {
            _mapDirection[x,y]=true;

            Sequence seq = DOTween.Sequence();
            seq.Append( _mapBoard[x,y].rectTransform.DOScaleX( 0, 0.2f ).SetEase( Ease.InBack ).SetDelay( delay ) );
            seq.Append( _mapBoard[x,y].rectTransform.DOScaleX( 1, 0.2f ).SetEase( Ease.OutBack ));

            DOTween.Play( _mapChar[x,y].rectTransform.DOScaleX( 1, 0.2f ).SetEase( Ease.OutBack ).SetDelay( delay+0.2f ) );
        }
    }

    public override void StartGame() {
        _status = Status_Playing;
        _secondStatus = Status_Remebering;
    }

    public override void FixedUpdate() {
        if(_status==Status_Gameover) {
            return;
        }

        base.FixedUpdate();


    }

    public override void OnBoardTapped( Vector3 pos ) {
        if(_status!=Status_Playing) {
            return;
        }

        if(_secondStatus==Status_Remebering) {
            _secondStatus = Status_Deciding;

            _gameController.SetGameDescription1( 7, "Tap tiles in order." );

            for(int m=0; m<_mapWidth; m++ ) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if(_mapData[m,n]!=-1) {
                        TurnCard( m, n, 0 );
                    }
                }
            }

            _tapIndex = 0;

            _timer = 2+_blockNumber*0.5f;
            //_timer=100;
            _totalGameTime = _timer;
        }
        else if(_secondStatus==Status_Deciding) {
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
                Debug.Log( "Tap:"+tapX+"---"+tapY);
                if(_mapData[tapX,tapY]==_tapIndex) {
                    TurnCard( tapX, tapY, 0 );

                    _tapIndex++;


                    if(_tapIndex==_blockNumber) {
                        _status = Status_Gameover;
                        _gameController.SendGameResult( true );
                    }
                    else {
                        MainPage.instance.PlaySound( MainPage.Sound_Tap );
                    }
                }
                else if(_mapData[tapX,tapY]<_tapIndex) {
                    
                }
                else {
                    _status = Status_Gameover;
                   _gameController.SendGameResult( false );
                }
            }  
        }
    }
}
