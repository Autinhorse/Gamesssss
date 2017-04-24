using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

using DG.Tweening;

public class GameLogicResolveMaze : GameLogic {

    const int Status_Remebering = 3;
    const int MapBlockDelta = 16;
    int MapBlockSize;

    float _timer;
    int _blockNumber;
    int[,] _mapData;
    Image[,] _mapBoard;
    Image _imgActor;
    Text _imgExit;
    bool[,] _mapDirection;

    int _mapWidth;
    int _mapHeight;

    int _lastTappedX;
    int _lastTappedY;

    int _actorX;
    int _actorY;

    public GameLogicResolveMaze( int difficulty ) : base(difficulty) {

    }

    // 关于这个游戏的难度
    // 难度支持0-12:
    // 0-1: 2x3
    // 2-5: 3x3
    // 6-10: 3x4
    // 11-16: 4x4

    int[,] Map1 = {
        { 2, 1, 1 },
        { 0, 0, 3 }
    };

     int[,] Map2 = {
        { 2, 1, 0 },
        { 0, 1, 3 }
    };

     int[,] Map3 = {
        { 1, 1, 1 },
        { 2, 0, 3 }
    };

     int[,] Map4 = {
        { 2, 1, 1 },
        { 0, 0, 1 },
        { 0, 0, 3 }
    };
     int[,] Map5 = {
        { 1, 1, 1 },
        { 2, 0, 1 },
        { 0, 0, 3 }
    };

     int[,] Map6 = {
        { 2, 0, 0 },
        { 1, 1, 1 },
        { 0, 0, 3 }
    };

     int[,] Map7 = {
        { 1, 1, 1 },
        { 1, 0, 1 },
        { 2, 0, 3 }
    };

     int[,] Map8 = {
        { 2, 1, 0 },
        { 0, 1, 1 },
        { 0, 0, 3 }
    };

     int[,] Map9 = {
        { 1, 1, 1 },
        { 1, 0, 3 },
        { 1, 2, 0 }
    };

     int[,] Map10 = {
        { 1, 1, 3 },
        { 1, 0, 0 },
        { 1, 0, 0 },
        { 2, 0, 0 }
    };

     int[,] Map11 = {
        { 1, 1, 1 },
        { 1, 0, 3 },
        { 2, 0, 0 },
        { 0, 0, 0 }
    };

     int[,] Map12 = {
        { 1, 1, 1 },
        { 1, 0, 1 },
        { 1, 0, 3 },
        { 2, 0, 0 }
    };

     int[,] Map13 = {
        { 1, 1, 3 },
        { 1, 0, 0 },
        { 1, 0, 0 },
        { 1, 2, 0 }
    };

     int[,] Map14 = {
        { 0, 1, 3 },
        { 0, 1, 0 },
        { 1, 1, 0 },
        { 2, 0, 0 }
    };

     int[,] Map15 = {
        { 0, 1, 3 },
        { 1, 1, 0 },
        { 1, 0, 0 },
        { 2, 0, 0 }
    };

     int[,] Map16 = {
        { 1, 1, 3 },
        { 1, 0, 0 },
        { 1, 1, 0 },
        { 0, 2, 0 }
    };

     int[,] Map17 = {
        { 1, 1, 1, 3 },
        { 1, 0, 0, 0 },
        { 1, 0, 0, 0 },
        { 1, 2, 0, 0 }
    };

     int[,] Map18= {
        { 1, 1, 1, 3 },
        { 1, 0, 0, 0 },
        { 1, 1, 1, 1 },
        { 0, 0, 0, 2 }
    };

     int[,] Map19= {
        { 1, 1, 1, 0 },
        { 1, 0, 1, 3 },
        { 1, 0, 0, 0 },
        { 1, 1, 2, 0 }
    };

     int[,] Map20= {
        { 1, 1, 1, 0 },
        { 1, 0, 1, 3 },
        { 1, 1, 0, 0 },
        { 0, 1, 1, 2 }
    };

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetButtonMode( GameController.Button_None );



        _gameController.SetGameNameAndDescription( "MAZE", "Help me to exit.", null );

        _gameController.SetColorIndex( 1 );

        int mapType;
        int[,] mapData=null;

        switch(_difficulty) {
        case 0:
            mapType = 0;
            switch(KWUtility.Random(0,3)) {
            case 0:
                mapData = Map1;
                break;
            case 1:
                mapData = Map2;
                break;
            case 2:
                mapData = Map3;
                break;
            }
            break;
        case 1:
            mapType = 1;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map4;
                break;
            case 1:
                mapData = Map5;
                break;
            }
            break;
        case 2:
            mapType = 1;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map6;
                break;
            case 1:
                mapData = Map7;
                break;
            }
            break;
        case 3:
            mapType = 1;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map8;
                break;
            case 1:
                mapData = Map9;
                break;
            }
            break;
        case 4:
            mapType = 2;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map10;
                break;
            case 1:
                mapData = Map11;
                break;
            }
            break;
        case 5:
            mapType = 2;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map11;
                break;
            case 1:
                mapData = Map12;
                break;
            }
            break;
        case 6:
            mapType = 2;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map12;
                break;
            case 1:
                mapData = Map13;
                break;
            }
            break;
        case 7:
            mapType = 2;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map13;
                break;
            case 1:
                mapData = Map14;
                break;
            }
            break;
        case 8:
            mapType = 2;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map14;
                break;
            case 1:
                mapData = Map15;
                break;
            }
            break;
        case 9:
            mapType = 2;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map15;
                break;
            case 1:
                mapData = Map16;
                break;
            }
            break;
        case 10:
            mapType = 3;
            mapData = Map17;
            break;
        case 11:
            mapType = 3;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map17;
                break;
            case 1:
                mapData = Map18;
                break;
            }
            break;
        case 12:
            mapType = 3;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map18;
                break;
            case 1:
                mapData = Map19;
                break;
            }
            break;
        default:// 13 and above
            mapType = 3;
            switch(KWUtility.Random(0,2)) {
            case 0:
                mapData = Map19;
                break;
            case 1:
                mapData = Map20;
                break;
            }
            break;
        
        }

        switch( mapType ) {
        case 0:
            if(KWUtility.Random( 0, 2)==0 ) {
                _mapWidth=2;
                _mapHeight=3;
                _mapData = new int[_mapWidth,_mapHeight];
                for(int m=0; m<_mapWidth; m++ ) {
                    for(int n=0; n<_mapHeight; n++ ) {
                        _mapData[m,n]=mapData[m,n];
                    }
                };
            }
            else {
                _mapWidth=3;
                _mapHeight=2;
                _mapData = new int[_mapWidth,_mapHeight];
                for(int m=0; m<_mapWidth; m++ ) {
                    for(int n=0; n<_mapHeight; n++ ) {
                        _mapData[m,n]=mapData[n,m];
                    }
                }
            }
            break;
        case 1:
            _mapWidth=3;
            _mapHeight=3;
            _mapData = new int[_mapWidth,_mapHeight];
            for(int m=0; m<_mapWidth; m++ ) {
                for(int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=mapData[m,n];
                }
            };
            break;
        case 2:
            if(KWUtility.Random( 0, 2)==0 ) {
                _mapWidth=4;
                _mapHeight=3;
                _mapData = new int[_mapWidth,_mapHeight];
                for(int m=0; m<_mapWidth; m++ ) {
                    for(int n=0; n<_mapHeight; n++ ) {
                        _mapData[m,n]=mapData[m,n];
                    }
                };
            }
            else {
                _mapWidth=3;
                _mapHeight=4;
                _mapData = new int[_mapWidth,_mapHeight];
                for(int m=0; m<_mapWidth; m++ ) {
                    for(int n=0; n<_mapHeight; n++ ) {
                        _mapData[m,n]=mapData[n,m];
                    }
                }
            }

            break;
        case 3:
            _mapWidth=4;
            _mapHeight=4;
            _mapData = new int[_mapWidth,_mapHeight];
            for(int m=0; m<_mapWidth; m++ ) {
                for(int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=mapData[m,n];
                }
            };
            break;
        }
        int temp;
        if(KWUtility.Random( 0, 2 )==0) {
            for(int m=0;m<_mapWidth/2;m++ ) {
                for(int n=0;n<_mapHeight;n++ ) {
                    temp = _mapData[m,n];
                    _mapData[m,n]=_mapData[_mapWidth-1-m,n];
                    _mapData[_mapWidth-1-m,n]=temp;
                }
            }

        }
        if(KWUtility.Random( 0, 2 )==0) {
            for(int m=0;m<_mapWidth;m++ ) {
                for(int n=0;n<_mapHeight/2;n++ ) {
                    temp = _mapData[m,n];
                    _mapData[m,n]=_mapData[m,_mapHeight-1-n];
                    _mapData[m,_mapHeight-1-n]=temp;
                }
            }
        }

        MapBlockSize = (int) _gameController.boardWidth/(_mapWidth+2);

        for( int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
                if((_mapData[m,n]==1)||(_mapData[m,n]==2)||(_mapData[m,n]==3)) {
                    Vector2 pos = GetPosition( m, n );
                    Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
                    imgBoard.gameObject.SetActive( true );
                    imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
                    imgBoard.color = new Color(0,0,0,0.2f);
                    imgBoard.sprite = MainPage.instance.SptShapes[0];

                    imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
                    imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
                    imgBoard.rectTransform.localScale = Vector3.one;
                    _goList.Add( imgBoard.gameObject );
                }

                if(_mapData[m,n]==2) {
                    _actorX = m;
                    _actorY = n;


                }

                if(_mapData[m,n]==3) {
                   
                    Vector2 pos = GetPosition( m, n );
                    _imgExit = (Text) GameObject.Instantiate( _gameController.goBoardChar);
                    _imgExit.gameObject.SetActive( true );
                    _imgExit.transform.SetParent( _gameController.goBoardArea.transform );
                    _imgExit.color = Color.green;
                    _imgExit.text = "EXIT";

                    _imgExit.rectTransform.localScale =  Vector3.one*0.75f;
                    _imgExit.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
                    _goList.Add( _imgExit.gameObject );
                }
            }
        }

        Vector2 pos1 = GetPosition( _actorX, _actorY );
        _imgActor = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        _imgActor.gameObject.SetActive( true );
        _imgActor.transform.SetParent( _gameController.goBoardArea.transform );
        _imgActor.color = Color.white;
        _imgActor.sprite = MainPage.instance.SptActor;

        _imgActor.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
        _imgActor.rectTransform.localPosition = new Vector3( pos1.x, pos1.y, 0 );
        _imgActor.rectTransform.localScale =  Vector3.one*0.75f;
        _goList.Add( _imgActor.gameObject );

        Button button1 = (Button) GameObject.Instantiate( _gameController.goBoardButton);
        button1.gameObject.SetActive( true );
        button1.transform.SetParent( _gameController.goBoardButtonArea.transform );

        button1.onClick.AddListener(delegate() { 
            if(_actorX>0) {
                if(_mapData[_actorX-1,_actorY]!=0) {
                    MoveActor( _actorX-1, _actorY ); 
                }
            }
        });

        RectTransform rect = (RectTransform) button1.GetComponent<RectTransform>();

        int buttonSize = _gameController.boardWidth/6;
        rect.sizeDelta =  new Vector2( buttonSize, buttonSize );
        rect.localPosition = new Vector3( _gameController.boardWidth*3/-10, _gameController.boardWidth*-0.55f, 0 );
        rect.localScale = Vector3.one;

        _goList.Add( button1.gameObject );

        Image buttonImage = (Image)button1.transform.FindChild( "Image" ).GetComponent<Image>();
        buttonImage.gameObject.SetActive( true );
        buttonImage.sprite = MainPage.instance.SptArrow;
        buttonImage.color = MainPage.instance.GameBoardColor[1];
        buttonImage.transform.localEulerAngles = new Vector3( 0, 0, 180 );

        button1 = (Button) GameObject.Instantiate( _gameController.goBoardButton);
        button1.gameObject.SetActive( true );
        button1.transform.SetParent( _gameController.goBoardButtonArea.transform );

        button1.onClick.AddListener(delegate() { 
            if(_actorX<_mapWidth-1) {
                if(_mapData[_actorX+1,_actorY]!=0) {
                    MoveActor( _actorX+1, _actorY ); 
                }
            }
        });

        rect = (RectTransform) button1.GetComponent<RectTransform>();

        rect.sizeDelta =  new Vector2( buttonSize, buttonSize );
        rect.localPosition = new Vector3( _gameController.boardWidth*1/-10, _gameController.boardWidth*-0.55f, 0 );
        rect.localScale = Vector3.one;
        _goList.Add( button1.gameObject );

        buttonImage = (Image)button1.transform.FindChild( "Image" ).GetComponent<Image>();
        buttonImage.gameObject.SetActive( true );
        buttonImage.sprite = MainPage.instance.SptArrow;
        buttonImage.color = MainPage.instance.GameBoardColor[1];
        //buttonImage.transform.localEulerAngles = new Vector3( 0, 0, 180 );

        button1 = (Button) GameObject.Instantiate( _gameController.goBoardButton);
        button1.gameObject.SetActive( true );
        button1.transform.SetParent( _gameController.goBoardButtonArea.transform );

        button1.onClick.AddListener(delegate() { 
            if(_actorY>0) {
                if(_mapData[_actorX,_actorY-1]!=0) {
                    MoveActor( _actorX, _actorY-1 ); 
                }
            }
        });

        rect = (RectTransform) button1.GetComponent<RectTransform>();

        rect.sizeDelta =  new Vector2( buttonSize, buttonSize );
        rect.localPosition = new Vector3( _gameController.boardWidth*1/10, _gameController.boardWidth*-0.55f, 0 );
        rect.localScale = Vector3.one;
        _goList.Add( button1.gameObject );

        buttonImage = (Image)button1.transform.FindChild( "Image" ).GetComponent<Image>();
        buttonImage.gameObject.SetActive( true );
        buttonImage.sprite = MainPage.instance.SptArrow;
        buttonImage.color = MainPage.instance.GameBoardColor[1];
        buttonImage.transform.localEulerAngles = new Vector3( 0, 0, 90 );

        button1 = (Button) GameObject.Instantiate( _gameController.goBoardButton);
        button1.gameObject.SetActive( true );
        button1.transform.SetParent( _gameController.goBoardButtonArea.transform );

        button1.onClick.AddListener(delegate() { 
            if(_actorY<_mapHeight-1) {
                if(_mapData[_actorX,_actorY+1]!=0) {
                    MoveActor( _actorX, _actorY+1 ); 
                }
            }
        });

        rect = (RectTransform) button1.GetComponent<RectTransform>();

        rect.sizeDelta =  new Vector2( buttonSize, buttonSize );
        rect.localPosition = new Vector3( _gameController.boardWidth*3/10, _gameController.boardWidth*-0.55f, 0 );
        rect.localScale = Vector3.one;
        _goList.Add( button1.gameObject );

        buttonImage = (Image)button1.transform.FindChild( "Image" ).GetComponent<Image>();
        buttonImage.gameObject.SetActive( true );
        buttonImage.sprite = MainPage.instance.SptArrow;
        buttonImage.color = MainPage.instance.GameBoardColor[1];
        buttonImage.transform.localEulerAngles = new Vector3( 0, 0, 270 );
    }

    public void MoveActor( int x, int y ) {
        MainPage.instance.PlaySound( MainPage.Sound_Tap );

        Vector2 pos = GetPosition( x, y );

        _actorX = x;
        _actorY = y;

        DOTween.Play( _imgActor.rectTransform.DOLocalMove( new Vector3( pos.x, pos.y, 0 ), 0.1f ).OnComplete( ()=> {
            if(_mapData[_actorX,_actorY]==3) {
                _gameController.SendGameResult( true );
            }
        } ) );
    }


    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta) );
    }



}
