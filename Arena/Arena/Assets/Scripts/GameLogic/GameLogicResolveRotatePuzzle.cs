using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using UnityEngine.UI;
using DG.Tweening;

public class GameLogicResolveRotatePuzzle : GameLogic {

    const int Status_Remebering = 3;
    const int MapBlockDelta = 16;
    int MapBlockSize;

    int _blockNumber;
    int[,] _mapData;

    Image[,] _mapBoard;

    int _mapWidth;
    int _mapHeight;


    public GameLogicResolveRotatePuzzle(int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {

    }

    // 关于这个游戏的难度
    // 难度支持0-15:
    // 0-2 2x2 格子，分别有1，2，3个需要旋转，总共需要旋转2，3，4下需要旋转
    // 3-8 3x3 格子，分别有3，4，5，5，6，6个需要旋转，总共需要旋转5，6，7，9，10，12下需要旋转
    // 9-15 4x4 格子，分别有5，6，7，7，8，8, 9个需要旋转，总共需要旋转10，11，12，14，15，16, 18下需要旋转

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetButtonMode( GameController.Button_None );

        MapBlockSize = (int) _gameController.boardWidth/6;

        _gameController.SetGameDescription1( 6, "Tap block to rotate" );

        _gameController.SetColorIndex( 1 );

        int boardNumber=0;
        int rotateNumber=0;

        switch( _difficulty ) {
        case 0:
            _mapWidth =2 ;
            _mapHeight = 2;
            boardNumber = 2;
            rotateNumber = 3;
            break;
        case 1:
            _mapWidth =3 ;
            _mapHeight = 3;
            boardNumber = 3;
            rotateNumber = 5;
            break;
        case 2:
            _mapWidth =3 ;
            _mapHeight = 3;
            boardNumber = 4;
            rotateNumber = 6;
            break;
        default:
            _mapWidth =3 ;
            _mapHeight = 3;
            boardNumber = 6;
            rotateNumber = 9;
            break;
        }

        _mapData=new int[_mapWidth,_mapHeight];
        for(int m=0;m<_mapWidth;m++) {
            for(int n=0; n<_mapHeight; n++ ) {
                _mapData[m,n]=0;
            }
        }



        // 计算哪几个需要旋转
        int x, y;
        for(int m=0;m<boardNumber;m++) {
            do{ 
                x=KWUtility.Random( 0, _mapWidth );     
                y=KWUtility.Random( 0, _mapHeight );     
            } while( _mapData[x,y]!=0);

            _mapData[x,y]=1;
        }

        for( int m=0;m<rotateNumber-boardNumber; m++ ) {
            int index = KWUtility.Random( 0, boardNumber );
            x=0;
            y=0;
            for( x=0; x<_mapWidth;x++ ) {
                for( y=0; y<_mapHeight; y++ ) {
                    if(_mapData[x,y]!=0) {
                        index--;
                        if(index<0) {
                            break;
                        }
                    }
                }
                if(index<=0){
                    break;
                }
            }
            if(index<0) {
                _mapData[x,y]++;
                if(_mapData[x,y]==4){
                    _mapData[x,y]=1;
                }
            }
        }

        MapBlockSize = _gameController.boardHeight/(_mapWidth+3);
        int imageSize = 420/_mapWidth;
        _mapBoard=new Image[_mapWidth,_mapHeight];

        Texture2D texture = MainPage.instance.TexPuzzles[KWUtility.Random(0,MainPage.instance.TexPuzzles.Length)];
        
        for(int m=0;m<_mapWidth;m++ ) {
            for( int n=0;n<_mapHeight;n++ ) {
                
                Sprite sprite = Sprite.Create(texture,
                    new Rect(imageSize*m, imageSize*(_mapHeight-1-n), imageSize, imageSize),
                    new Vector2(0.5f,0.5f),
                    64);
                Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
                imgBoard.gameObject.SetActive( true );
                imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
                imgBoard.color = Color.white;
                imgBoard.sprite = sprite;
                _goList.Add( imgBoard.gameObject );

                Vector2 pos = GetPosition( m, n);
                imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
                imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
                imgBoard.rectTransform.localScale = Vector3.one*0.96f;

                _mapBoard[m,n] = imgBoard;

                switch( _mapData[m,n] ) {
                case 1:
                    imgBoard.rectTransform.localEulerAngles = new Vector3( 0, 0, 90 );
                    break;
                case 2:
                    imgBoard.rectTransform.localEulerAngles = new Vector3( 0, 0, 180 );
                    break;
                case 3:
                    imgBoard.rectTransform.localEulerAngles = new Vector3( 0, 0, 270 );
                    break;
                }

                _goList.Add( imgBoard.gameObject );
            }
        }
    }


    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize), ((_mapHeight-1.0f)/2-y)*(MapBlockSize)-MapBlockSize/2 );
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

        int count = 0;

        for(int m=0;m<_mapWidth;m++) {
            for(int n=0; n<_mapHeight;n++ ) {
                Vector2 center = GetPosition( m, n );
                if((pos.x>center.x-(MapBlockSize+MapBlockDelta)/2)&&(pos.x<center.x+(MapBlockSize+MapBlockDelta)/2)&&(pos.y>center.y-(MapBlockSize+MapBlockDelta)/2)&&(pos.y<center.y+(MapBlockSize+MapBlockDelta)/2)) { 
                    _mapData[m,n]--;
                    if(_mapData[m,n]==-1){
                        _mapData[m,n]=3;
                    }
                    switch( _mapData[m,n] ) {
                    case 0:
                        _mapBoard[m,n].rectTransform.localEulerAngles = new Vector3( 0, 0, 0 );
                        break;
                    case 1:
                        _mapBoard[m,n].rectTransform.localEulerAngles = new Vector3( 0, 0, 90 );
                        break;
                    case 2:
                        _mapBoard[m,n].rectTransform.localEulerAngles = new Vector3( 0, 0, 180 );
                        break;
                    case 3:
                        _mapBoard[m,n].rectTransform.localEulerAngles = new Vector3( 0, 0, 270 );
                        break;
                    }
                    MainPage.instance.PlaySound( MainPage.Sound_Tap );
                }

                count += _mapData[m,n];
            }
        }

        if(count==0){
            for(int m=0;m<_mapWidth;m++) {
                for(int n=0; n<_mapHeight;n++ ) {
                    if((m==_mapWidth-1)&&(n==_mapHeight-1)) {
                        DOTween.Play( _mapBoard[m,n].rectTransform.DOScale( Vector3.one, 0.2f).OnComplete( () => {
                            _status=Status_Gameover;
                            _gameController.SendGameResult( true );
                        } ) );
                    }
                    else {
                        DOTween.Play( _mapBoard[m,n].rectTransform.DOScale( Vector3.one, 0.2f) );
                    }
                }
            }
           
        }


    }
}
