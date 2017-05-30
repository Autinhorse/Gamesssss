using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using DG.Tweening;

public class GameLogicResolveHeadup : GameLogic {

    const int MapBlockDelta = 32;
    int MapBlockSize;

    public GameLogicResolveHeadup( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    int _mapWidth;
    int _mapHeight;

    int[] _mapData;

    Text[,] _mapChar;

    Image[,] _mapImage;

    int _targetType;

    // 关于这个游戏的难度
    // 难度支持0-11:
    // 0-2:有4个字母，其中分别有2，3，4个需要旋转
    // 3-6:有6个字母，其中分别有3，4，5，6个需要旋转
    // 7-11:有9个字母，其中分别有5，6，7，8，9个需要旋转

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        MapBlockSize = (int) _gameController.boardHeight/8;

        _gameController.SetButtonMode( GameController.Button_None );

        ;
        if(_difficulty>1) {
            _targetType=0;
        }
        else {
            _targetType=1;
        }

        if(_targetType==0) {
           _gameController.SetGameDescription1( 4, "Tap the charactors" );
        }
        else {
            _gameController.SetGameDescription1( 4, "Tap the aliens" );
        }

        _gameController.SetGameDescription2( 5, "Make them headup" );

        _gameController.SetColorIndex( 1 );

        int charNumber;
        switch( _difficulty ) {
        case 0:
            _mapWidth=2;
            _mapHeight=2;
            charNumber=3;
            break;
        case 1:
            _mapWidth=3;
            _mapHeight=2;
            charNumber=3;
            break;
        case 2:
            _mapWidth=3;
            _mapHeight=2;
            charNumber=4;
            break;
        default:
            _mapWidth=3;
            _mapHeight=2;
            charNumber=5;
            break;
        }

        _mapData = new int[_mapWidth*_mapHeight];
        if(_targetType==0){
            _mapChar = new Text[_mapWidth, _mapHeight];
        }
        else {
            _mapImage= new Image[_mapWidth, _mapHeight];
        }

        for( int m=0; m<_mapHeight*_mapWidth; m++ ) {
            if(m<charNumber){
                _mapData[m]=KWUtility.Random( 1, 4 );
            }
            else {
                _mapData[m]=0;
            }
        }

        int temp;
        int i1,i2;

        for( int m=0; m<20; m++ ) {
            i1 = KWUtility.Random( 0, _mapHeight*_mapWidth );
            i2 = KWUtility.Random( 0, _mapHeight*_mapWidth );

            temp = _mapData[i1];
            _mapData[i1]=_mapData[i2];
            _mapData[i2]=temp;
        }
            
        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {

                if(_targetType==0) {
                    Text textObject = (Text) GameObject.Instantiate( _gameController.goBoardChar );
                    _goList.Add( textObject.gameObject );
                    textObject.gameObject.SetActive( true );

                    textObject.transform.SetParent( _gameController.goBoardArea.transform );
                    Vector2 pos = GetPosition( m, n );

                    textObject.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );

                    int unicode;
                    do {
                        unicode = 65+ KWUtility.Random( 0, 26 );
                    } while((unicode==72)||(unicode==73)||(unicode==78)||(unicode==79)||(unicode==83)||(unicode==88)||(unicode==90));

                    char character = (char) unicode;
                    string text = character.ToString();

                    textObject.text = text;
                    textObject.color = Color.white;

                    _mapChar[m,n] = textObject;

                    switch( _mapData[m*_mapHeight+n] ) {
                    case 1:
                    case 2:
                    case 3:
                        _mapData[m*_mapHeight+n]  = 1;
                        textObject.rectTransform.localEulerAngles = new Vector3( 0, 0, 90 );
                        break;
                    /*case 2:
                        textObject.rectTransform.localEulerAngles = new Vector3( 0, 0, 180 );
                        break;
                    case 3:
                        textObject.rectTransform.localEulerAngles = new Vector3( 0, 0, 270 );
                        break;*/
                    }
                    textObject.rectTransform.localScale = Vector3.one;
                }
                else {
                    Image imageObject = (Image) GameObject.Instantiate( _gameController.goBoardImage );
                    _goList.Add( imageObject.gameObject );
                    imageObject.gameObject.SetActive( true );

                    imageObject.transform.SetParent( _gameController.goBoardArea.transform );
                    Vector2 pos = GetPosition( m, n );

                    imageObject.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );

                    imageObject.sprite = MainPage.instance.SptAlien;
                    imageObject.color = Color.white;

                    _mapImage[m,n] = imageObject;

                    switch( _mapData[m*_mapHeight+n] ) {
                    case 1:
                    case 2:
                    case 3:
                        _mapData[m*_mapHeight+n]  = 1;
                        imageObject.rectTransform.localEulerAngles = new Vector3( 0, 0, 90 );
                        break;
                    }
                
                    imageObject.rectTransform.localScale = Vector3.one;
                }
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

        if(tapX>=0) {
            

            _mapData[tapX*_mapHeight+tapY]--;
            if(_mapData[tapX*_mapHeight+tapY]<0) {
                _mapData[tapX*_mapHeight+tapY]=3;
            }
            if(_targetType==0) {
                switch(_mapData[tapX*_mapHeight+tapY]) {
                case 0:
                    _mapChar[tapX,tapY].rectTransform.localEulerAngles = Vector3.zero;
                    break;
                case 1:
                    _mapChar[tapX,tapY].rectTransform.localEulerAngles = new Vector3( 0, 0, 90 );
                    break;
                case 2:
                    _mapChar[tapX,tapY].rectTransform.localEulerAngles = new Vector3( 0, 0, 180 );
                    break;
                case 3:
                    _mapChar[tapX,tapY].rectTransform.localEulerAngles = new Vector3( 0, 0, 270 );
                    break;
                }
            }
            else {
                switch(_mapData[tapX*_mapHeight+tapY]) {
                case 0:
                    _mapImage[tapX,tapY].rectTransform.localEulerAngles = Vector3.zero;
                    break;
                case 1:
                    _mapImage[tapX,tapY].rectTransform.localEulerAngles = new Vector3( 0, 0, 90 );
                    break;
                case 2:
                    _mapImage[tapX,tapY].rectTransform.localEulerAngles = new Vector3( 0, 0, 180 );
                    break;
                case 3:
                    _mapImage[tapX,tapY].rectTransform.localEulerAngles = new Vector3( 0, 0, 270 );
                    break;
                }
            }

            int count = 0;
            for(int m=0;m<_mapWidth*_mapHeight;m++ ) {
                count+=_mapData[m];
            }

            if(count==0) {
                _status = Status_Gameover;
                _gameController.SendGameResult( true );
            }
            else {
                MainPage.instance.PlaySound( MainPage.Sound_Tap );
            }
        }

    }
}
