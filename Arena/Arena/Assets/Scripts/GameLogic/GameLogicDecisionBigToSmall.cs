using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using DG.Tweening;

public class GameLogicDecisionBigToSmall : GameLogic {

    const int MapBlockDelta = 24;
    int MapBlockSize;

    int _blockNumber;
    int[,] _mapData;
    Image[,] _mapBoard;
    bool[,] _mapDirection;

    int _mapWidth;
    int _mapHeight;

    int _lastTappedX;
    int _lastTappedY;

    int _numberCount;
    int _targetNumber;

    public GameLogicDecisionBigToSmall( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {

    }



    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetButtonMode( GameController.Button_None );

        _gameController.SetColorIndex( 4 );

       switch(_difficulty) {
        case 0:
            _mapWidth=2;
            _mapHeight=2;
            _numberCount = 4;
            break;
        case 1:
            _mapWidth=3;
            _mapHeight=2;
            _numberCount = 5;
            break;
        case 2:
            _mapWidth=3;
            _mapHeight=2;
            _numberCount = 6;
            break;
        default:
            _mapWidth=3;
            _mapHeight=3;
            _numberCount = 8;
            break;
        }
        if(_mapWidth>2) {
            MapBlockSize = (int) _gameController.boardHeight/(_mapWidth+5);
        }
        else {
            MapBlockSize = (int) _gameController.boardHeight/7;
        }

        int[] data = new int[_numberCount];
        for( int m=0; m<_numberCount; m++ ) {
            bool sameFlag = false;
            do {
                sameFlag = false;
                if(m<_numberCount/2) {
                    data[m]=KWUtility.Random(1,9)*2;
                }
                else {
                    data[m]=KWUtility.Random(1,9)*2+1;
                }
                for(int n=0; n<m; n++ ) {
                    if(data[m]==data[n]){
                        sameFlag=true;
                        break;
                    }
                }
            }while(sameFlag);
        }

        for(int m=0; m<20; m++ ) {
            int n1=KWUtility.Random(0,_numberCount);
            int n2=KWUtility.Random(0,_numberCount);

            int temp = data[n1];
            data[n1]= data[n2];
            data[n2] = temp;
        }

        _targetNumber = KWUtility.Random( 0, 2 );

        string title = "";
        if(_targetNumber==0) {
            title = "Tap from small to big";
        }
        else {
            title = "Tap from big to small";
        }

        _gameController.SetGameDescription1( 4, title );
       
        _mapData = new int[_mapWidth, _mapHeight];
        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
                _mapData[m,n]=-1;
            }
        }

        _mapBoard = new Image[_mapWidth,_mapHeight];

        ShowNumber( data );

    }

    void ShowNumber( int[] data ) {
        int shapeIndex = 0;

        switch( data.Length ) {
        case 4:
            _mapWidth = 2;
            _mapHeight = 2;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateNumber( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 5:
            _mapWidth = 3;
            _mapHeight = 2;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if((n==0)&&(m==1)) {
                        continue;
                    }
                    CreateNumber( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 6:
            _mapWidth = 3;
            _mapHeight = 2;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateNumber( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 8:
            _mapWidth = 3;
            _mapHeight = 3;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if((n==0)&&(m==1)) {
                        continue;
                    }
                    CreateNumber( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        default:
            _mapWidth = 4;
            _mapHeight = 3;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if(((m==0)||(m==3))&&(n==0)) {
                        continue;
                    }
                    CreateNumber(data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        }
    }

    void CreateNumber( int number, int m, int n ) {
        _mapData[m,n]=number;

        Vector2 pos = GetPosition( m, n );
        Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgBoard.gameObject.SetActive( true );
        imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
        imgBoard.color = new Color( 0, 0, 0, 0.2f );

        imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
        imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
        _mapBoard[m,n]=imgBoard;
        _goList.Add( imgBoard.gameObject );

        Text txtNumber = (Text) GameObject.Instantiate( _gameController.goBoardChar );
        txtNumber.gameObject.SetActive( true );
        txtNumber.transform.SetParent( imgBoard.gameObject.transform );
        txtNumber.rectTransform.localPosition = Vector3.zero;
        txtNumber.color = Color.white;
        txtNumber.text = number.ToString();
        //_mapShape[m,n]=imgShape;

        txtNumber.rectTransform.sizeDelta = new Vector2( MapBlockSize*0.75f, MapBlockSize*0.75f );

        imgBoard.rectTransform.localScale = Vector3.zero;
        DOTween.Play( imgBoard.rectTransform.DOScale( Vector3.one, 0.75f).SetEase( Ease.OutBack ) );
    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta)-MapBlockSize/2 );
    }

    void HideCard( int x, int y ) {
        DOTween.Play( _mapBoard[x,y].rectTransform.DOScale( Vector3.zero, 0.5f ).SetEase( Ease.InBack ) );
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
            if(_mapData[tapX,tapY]==-1) {
                return;
            }

            for(int m=0;m<_mapWidth;m++) {
                for( int n=0;n<_mapHeight;n++) {
                    if(_mapData[m,n]==-1) {
                        continue;
                    }
                    if(((_mapData[m,n]<_mapData[tapX,tapY])&&(_targetNumber==0))||((_mapData[m,n]>_mapData[tapX,tapY])&&(_targetNumber==1))) {
                        _status = Status_Gameover;
                        _gameController.SendGameResult( false );
                        return;
                    }
                }
            }

            HideCard( tapX, tapY );
            _mapData[tapX,tapY]=-1;
            _numberCount--;

            if(_numberCount==0) {
                _status = Status_Gameover;
                _gameController.SendGameResult( true );
            }
            else {
                MainPage.instance.PlaySound( MainPage.Sound_Tap );
            }
        }
    }
}
