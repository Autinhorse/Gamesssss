using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using DG.Tweening;

public class GameLogicMemoryPair : GameLogic {
    const int Status_Remebering = 3;
    const int Status_Deciding = 4;

    int _secondStatus;

    const int MapBlockDelta = 24;
    int MapBlockSize;

    //float _timer;
    int _blockNumber;
    int[,] _mapData;
    Image[,] _mapBoard;
    Image[,] _mapShape;
    bool[,] _mapDirection;

    int _mapWidth;
    int _mapHeight;

    int _lastTappedX;
    int _lastTappedY;

    public GameLogicMemoryPair( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {
        
    }

    // 关于这个游戏的难度
    // 难度支持0-15:
    // 记忆时间从2秒开始，每增加一级多0.5秒。方块从6个（三对）开始，每增加两级增加一对，最多到16个。
    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetButtonMode( GameController.Button_None );

        MapBlockSize = (int) _gameController.boardHeight/8;

        _gameController.SetGameDescription1( 7, "Remember the pairs."  );
        _gameController.SetGameDescription2( 4, "Tap screen when you are ready." );


        _gameController.SetColorIndex( 3 );

        int difficulty = _difficulty;
        if(difficulty>2){
            difficulty=2;
        }
        _timer = 5+difficulty/1.5f;
        _totalGameTime = _timer;

        int[] blockNumber = { 4, 6, 8, 6, 6, 8, 8, 8, 8, 10, 10, 10, 10, 10, 10, 10 };

        _blockNumber = blockNumber[difficulty];

        switch(_blockNumber){
        case 4:
            _mapWidth=2;
            _mapHeight=2;
            break;
        case 6:
            _mapWidth=3;
            _mapHeight=2;
            break;
        case 8:
            _mapWidth=3;
            _mapHeight=3;
            break;
        case 10:
        case 12:
            _mapWidth=4;
            _mapHeight=3;
            break;
        case 14:
        case 16:
            _mapWidth=4;
            _mapHeight=4;
            break;
        case 18:
        case 20:
            _mapWidth=5;
            _mapHeight=4;
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
        _mapShape = new Image[_mapWidth,_mapHeight];
        int[] shapes = new int[_blockNumber/2];
        for(int m = 0; m<_blockNumber/2; m++ ) {
            shapes[m] = KWUtility.Random(0, MainPage.instance.SptShapes.Length);
            bool same = false;
            for( int n=0; n<m; n++ ) {
                if(shapes[m]==shapes[n]){
                    same=true;
                    break;
                }
            }
            if(same==true){
                m--;
                continue;
            }

            CreateShape( shapes[m] );
            CreateShape( shapes[m] );
        }
    }

    void CreateShape( int shape ) {
        int posX, posY;
        do {
            posX = KWUtility.Random( 0, _mapWidth );
            posY = KWUtility.Random( 0, _mapHeight );
            Debug.Log( "Pos:"+posX+"-"+posY );
        }while( _mapData[posX, posY]!=-1);
        _mapData[posX,posY]= shape;

        Vector2 pos = GetPosition( posX, posY );
        Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgBoard.gameObject.SetActive( true );
        imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
        imgBoard.color = new Color( 0, 0, 0, 0.2f );
            
        imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
        imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
        imgBoard.rectTransform.localScale = Vector3.one;
        _mapBoard[posX,posY]=imgBoard;
        _goList.Add( imgBoard.gameObject );

        Image imgShape = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgShape.gameObject.SetActive( true );
        imgShape.transform.SetParent( imgBoard.gameObject.transform );
        imgShape.rectTransform.localPosition = Vector3.zero;
        imgShape.rectTransform.localScale = Vector3.one;
        imgShape.color = Color.white;
        imgShape.sprite = MainPage.instance.SptShapes[shape];
        _mapShape[posX,posY]=imgShape;

        imgShape.rectTransform.sizeDelta = new Vector2( MapBlockSize*0.75f, MapBlockSize*0.75f );

    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta)-MapBlockSize/2 );
    }

    void TurnCard( int x, int y, int delay ) {
        if(_mapDirection[x,y]==true) {
            _mapDirection[x,y]=false;

            DOTween.Play( _mapShape[x,y].rectTransform.DOScale( Vector3.zero, 0.25f ).SetEase( Ease.InBack ).SetDelay( delay ) );
        }
        else {
            _mapDirection[x,y]=true;

            DOTween.Play( _mapShape[x,y].rectTransform.DOScale( Vector3.one, 0.25f ).SetEase( Ease.OutBack ).SetDelay( delay ) );
        }
    }

    void HideCard( int x, int y ) {
        DOTween.Play( _mapBoard[x,y].rectTransform.DOScale( Vector3.zero, 0.25f ).SetEase( Ease.InBack ).OnComplete( ()=> {
            _blockNumber--;
            if(_blockNumber==0) {
                _status = Status_Gameover;
                _gameController.SendGameResult( true );
            }
        } ) );
        _mapData[x,y]=-1;
    }


    public override void StartGame() {
        _status = Status_Playing;
        _secondStatus = Status_Remebering;
    }

    public override void Update() {
    }

    public override void OnBoardTapped( Vector3 pos ) {
        if(_status!=Status_Playing) {
            return;
        }

        if(_secondStatus==Status_Remebering) {
            _status=Status_Playing;
            _lastTappedX=-1;
            _lastTappedY=-1;

            _gameController.SetGameNameAndDescription( "PAIRS", "Tap the pairs.", null );

            for(int m=0; m<_mapWidth; m++ ) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if(_mapData[m,n]!=-1) {
                        TurnCard( m, n, 0 );
                    }
                }
            }

            _secondStatus=Status_Deciding;
            _timer = 5+_blockNumber*0.5f;
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

            if(_lastTappedX==-1) {
                if((tapX>=0)&&(tapX<_mapWidth)&&(tapY>=0)&&(tapY<_mapHeight)) {
                    TurnCard( tapX, tapY, 0 );
                    MainPage.instance.PlaySound( MainPage.Sound_Tap );
                    _lastTappedX=tapX;
                    _lastTappedY=tapY;
                }
            }
            else {
                if((tapX>=0)&&(tapX<_mapWidth)&&(tapY>=0)&&(tapY<_mapHeight)) {
                    if((tapX==_lastTappedX)&&(tapY==_lastTappedY)) {
                        TurnCard( tapX, tapY, 0 );
                    }
                    else {
                        _mapDirection[tapX,tapY]=true;
                        bool isSame = (_mapData[_lastTappedX,_lastTappedY]==_mapData[tapX,tapY]);

                        int tapX1 = tapX;
                        int tapY1 = tapY;
                        int tapX2 = _lastTappedX;
                        int tapY2 = _lastTappedY;

                        DOTween.Play( _mapShape[tapX,tapY].rectTransform.DOScale( Vector3.one, 0.5f ).SetEase( Ease.OutBack ).OnComplete( ()=>{
                            if(isSame) {
                                _mapData[tapX1,tapY1]=-1;
                                _mapData[tapX2,tapY2]=-1;

                                HideCard( tapX1, tapY1 );
                                HideCard( tapX2, tapY2 );

                                MainPage.instance.PlaySound( MainPage.Sound_Tap );
                            }
                            else {
                                MainPage.instance.PlaySound( MainPage.Sound_Wrong );
                                TurnCard( tapX1, tapY1, 0 );
                                TurnCard( tapX2, tapY2, 0 );
                            }

                        } ));

                    }
                }
                _lastTappedX = -1;
                _lastTappedY = -1;
            }
        }
    }

}
