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

    // 关于这个游戏的难度
    // 难度支持0-11:
    // 0-2:有4个字母，其中分别有2，3，4个需要旋转
    // 3-6:有6个字母，其中分别有3，4，5，6个需要旋转
    // 7-11:有9个字母，其中分别有5，6，7，8，9个需要旋转

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        MapBlockSize = (int) _gameController.boardWidth/4;

        _gameController.SetButtonMode( GameController.Button_None );

        _gameController.SetGameNameAndDescription( "Headup", "Tap the charactors.", "Make them headup." );

        _gameController.SetColorIndex( 1 );

        int charNumber;
        switch( _difficulty ) {
        case 0:
            _mapWidth=2;
            _mapHeight=2;
            charNumber=2;
            break;
        case 1:
            _mapWidth=2;
            _mapHeight=2;
            charNumber=3;
            break;
        case 2:
            _mapWidth=2;
            _mapHeight=2;
            charNumber=4;
            break;
        case 3:
            _mapWidth=3;
            _mapHeight=2;
            charNumber=3;
            break;
        case 4:
            _mapWidth=3;
            _mapHeight=2;
            charNumber=4;
            break;
        case 5:
            _mapWidth=3;
            _mapHeight=2;
            charNumber=5;
            break;
        case 6:
            _mapWidth=3;
            _mapHeight=3;
            charNumber=6;
            break;
        case 7:
            _mapWidth=3;
            _mapHeight=3;
            charNumber=7;
            break;
        case 8:
            _mapWidth=3;
            _mapHeight=3;
            charNumber=8;
            break;
        case 9:
            _mapWidth=3;
            _mapHeight=3;
            charNumber=8;
            break;
        case 10:
            _mapWidth=4;
            _mapHeight=3;
            charNumber=9;
            break;
        case 11:
            _mapWidth=4;
            _mapHeight=3;
            charNumber=10;
            break;
        case 12:
            _mapWidth=4;
            _mapHeight=3;
            charNumber=11;
            break;
        case 13:
            _mapWidth=4;
            _mapHeight=4;
            charNumber=11;
            break;
        case 14:
            _mapWidth=4;
            _mapHeight=4;
            charNumber=12;
            break;
        case 15:
            _mapWidth=4;
            _mapHeight=4;
            charNumber=13;
            break;
        default:
            _mapWidth=3;
            _mapHeight=3;
            charNumber=9;
            break;
        }

        _mapData = new int[_mapWidth*_mapHeight];
        _mapChar = new Text[_mapWidth, _mapHeight];

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
        }


    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta)-MapBlockSize/2 );
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
