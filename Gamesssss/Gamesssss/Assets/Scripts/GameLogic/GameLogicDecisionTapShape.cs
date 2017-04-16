using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using DG.Tweening;

public class GameLogicDecisionTapShape : GameLogic {

    const int MapBlockDelta = 24;
    int MapBlockSize;

    float _timer;
    int _blockNumber;
    int[,] _mapData;
    Image[,] _mapBoard;
    Image[,] _mapShape;
    bool[,] _mapDirection;

    int _mapWidth;
    int _mapHeight;

    int _lastTappedX;
    int _lastTappedY;

    int _shapeNumber;
    int _targetShape;

    public GameLogicDecisionTapShape( int difficulty ) : base(difficulty) {

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

        _gameController.SetGameName( "TAP!" );

        _gameController.SetColorIndex( 4 );

        int shapeType;
        int shapePosY;

        switch(_difficulty) {
        case 0:
            _mapWidth=2;
            _mapHeight=3;
            shapeType=3;
            break;
        case 1:
            _mapWidth=3;
            _mapHeight=3;
            shapeType=3;
            break;
        case 2:
        case 3:
            _mapWidth=3;
            _mapHeight=4;
            shapeType=3;
            break;
        case 4:
        case 5:
            _mapWidth=4;
            _mapHeight=4;
            shapeType=4;
            break;
        case 6:
        case 7:
        case 8:
            _mapWidth=4;
            _mapHeight=5;
            shapeType=4;
            break;
        case 9:
        case 10:
            _mapWidth=5;
            _mapHeight=5;
            shapeType=5;
            break;
        case 11:
        case 12:
            _mapWidth=5;
            _mapHeight=6;
            shapeType=5;
            break;
        case 13:
        case 14:
            _mapWidth=6;
            _mapHeight=6;
            shapeType=5;
            break;
        default:
            _mapWidth=6;
            _mapHeight=6;
            shapeType=5;
            break;
        }
        if(_mapWidth>2) {
            MapBlockSize = (int) _gameController.boardWidth/(_mapWidth+3);
        }
        else {
            MapBlockSize = (int) _gameController.boardWidth/6;
        }

        if( _mapHeight<4 ) {
            _gameController.SetGameDescription1( 2, "Tap all          ." );
            shapePosY = 378;
        }
        else if( _mapHeight<5 ) {
            _gameController.SetGameDescription1( 3, "Tap all          ." );
            shapePosY = 412;
        }
        else {
            _gameController.SetGameDescription1( 4, "Tap all          ." );
            shapePosY = 476;
        }


        int[] shapes = new int[shapeType];
        for(int m=0;m<shapeType; m++ ) {
            bool same;
            do {
                shapes[m]=KWUtility.Random( 0, shapeType );
                same = false;
                for(int n=0;n<m;n++) {
                    if(shapes[m]==shapes[n]) {
                        same=true;
                        break;
                    }
                }
            }while(same==true);
            Debug.Log( "Shape["+m+"]"+shapes[m]);
        }
        _targetShape=shapes[0];

        _mapData = new int[_mapWidth, _mapHeight];
        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
                _mapData[m,n]=-1;
            }
        }

        _shapeNumber = KWUtility.Random( _mapWidth*_mapHeight/3,_mapWidth*_mapHeight/2+1);
        int x, y;
        for(int m=0;m<_shapeNumber;m++ ) {
            do{
                x=KWUtility.Random( 0, _mapWidth );
                y=KWUtility.Random( 0, _mapHeight );
            }while( _mapData[x,y]!=-1);
            _mapData[x,y]=shapes[0];
            Debug.Log( "Pos:"+x+","+y);
        }

        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
                if(_mapData[m,n]==-1) {
                    _mapData[m,n]=shapes[KWUtility.Random( 1, shapeType )];
                }
            }
        }

        _mapBoard = new Image[_mapWidth, _mapHeight];
        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
                CreateShape( _mapData[m,n], m, n );
            }
        }

        Image go1 = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        _goList.Add( go1.gameObject );
        go1.gameObject.SetActive( true );

        go1.gameObject.transform.SetParent( _gameController.goBoardArea.transform );
        go1.rectTransform.localPosition = new Vector3( 72, shapePosY, 0 ); 
        go1.rectTransform.localScale = Vector3.one;

        go1.rectTransform.sizeDelta = new Vector2( 64, 64 );

        go1.color = Color.white;

        go1.sprite = MainPage.instance.SptShapes[_targetShape];
    }

    void CreateShape( int shape, int posX, int posY ) {
        
        Vector2 pos = GetPosition( posX, posY );
        Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgBoard.gameObject.SetActive( true );
        imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
        imgBoard.color = new Color( 0, 0, 0, 0.2f);
            
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

        imgShape.color = Color.white;//MainPage.instance.GameBoardColor[4];
        imgShape.sprite = MainPage.instance.SptShapes[shape];

        imgShape.rectTransform.sizeDelta = new Vector2( MapBlockSize*0.75f, MapBlockSize*0.75f );

    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta)-MapBlockSize/2 );
    }

    void HideCard( int x, int y ) {
        DOTween.Play( _mapBoard[x,y].rectTransform.DOScale( Vector3.zero, 0.5f ).SetEase( Ease.InBack ).OnComplete( ()=> {
            _blockNumber--;
            if(_blockNumber==0) {
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
            if(_mapData[tapX,tapY]==_targetShape) {
                HideCard( tapX, tapY );
                _shapeNumber--;
                if(_shapeNumber==0) {
                    _gameController.SendGameResult( true );
                }
            }
            else {
                _gameController.SendGameResult( false );
            }
        }
    }
}
