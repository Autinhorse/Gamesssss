using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using DG.Tweening;

public class GameLogicMemoryNewItem : GameLogic {

    const int Status_Remebering = 3;

    const int Status_Deciding = 4;

    int _secondStatus;


    const int MapBlockDelta = 16;

    int MapBlockSize;

    //float _timer;
    int _shapeNumber;
    List<int> _existData; 
    List<int> _nonexistData;

    int[,] _mapData;
    int _newShape;

    Image[] _mapBoard;
    Image[] _mapShape;

    int _mapWidth;
    int _mapHeight;

    public GameLogicMemoryNewItem( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {

    }

    // 关于这个游戏的难度
    // 难度支持0-15:
    // 记忆时间从2秒开始，每增加一级多0.5秒。物体从2个开始，每增加2级难度增加一个物体。

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetButtonMode( GameController.Button_None );

        MapBlockSize = (int) _gameController.boardHeight/8;

        _gameController.SetGameName( "NEW SHAPE" );

        _gameController.SetColorIndex( 3 );

        int difficulty = _difficulty;
        if(difficulty>3){
            difficulty=3;
        }
       
        int[] shapeNumber = { 2, 3, 4, 5, 4, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8 }; 
        _shapeNumber = shapeNumber[difficulty];

        _gameController.SetGameDescription1( 4, "Remember the shapes" );
        _gameController.SetGameDescription2( 5, 46,"Then tap screen" );
        /*
        if((_shapeNumber==11)||(_shapeNumber==10)){
            _gameController.SetGameDescription1( 5, "Remember these shapes." );
        }
        else {
            _gameController.SetGameDescription1( 2, "Remember these shapes." );
        }*/

        _existData = new List<int>();

        for(int m=0;m<_shapeNumber;m++) {
            int shape;
            do{
                shape = KWUtility.Random( 0, MainPage.instance.SptShapes.Length );
            } while( _existData.Contains(shape) );
            _existData.Add( shape );
        }

        _nonexistData = new List<int>();
        for(int m=0; m<MainPage.instance.SptShapes.Length; m++ ) {
            if(_existData.Contains(m)==false) {
                _nonexistData.Add( m );
            }
        }

        ShowShape(_existData);
    }

    void ShowShape( List<int> data ) {
        int shapeIndex = 0;

        switch( data.Count ) {
        case 1:
            _mapWidth = 1;
            _mapHeight = 1;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=-1;
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 2:
            _mapWidth = 2;
            _mapHeight = 1;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=-1;
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 3:
            _mapWidth = 3;
            _mapHeight = 1;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=-1;
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 4:
            _mapWidth = 2;
            _mapHeight = 2;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=-1;
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 5:
            _mapWidth = 3;
            _mapHeight = 2;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=-1;
                    if((m==1)&&(n==0)) {
                        continue;
                    }
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 6:
            _mapWidth = 3;
            _mapHeight = 2;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=-1;
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 7:
            _mapWidth = 3;
            _mapHeight = 3;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=-1;
                    if((n==0)&&((m==0)||(m==2))) {
                        continue;
                    }
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 8:
            _mapWidth = 3;
            _mapHeight = 3;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=-1;
                    if((m==1)&&(n==0)) {
                        continue;
                    }
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 9:
            _mapWidth = 3;
            _mapHeight = 3;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    _mapData[m,n]=-1;
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        
        case 10:
            _mapWidth = 4;
            _mapHeight = 3;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if((n==0)&&((m==1)||(m==2))) {
                        continue;
                    }
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 11:
            _mapWidth = 3;
            _mapHeight = 4;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if((n==0)&&(m==1)) {
                        continue;
                    }
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 12:
            _mapWidth = 4;
            _mapHeight = 3;
            _mapData = new int[_mapWidth,_mapHeight];
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateShape( data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        }
    }

    void CreateShape( int shape, int m, int n ) {
        _mapData[m,n]=shape;

        Vector2 pos = GetPosition( m, n );
        Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgBoard.gameObject.SetActive( true );
        imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
        imgBoard.color = new Color( 0, 0, 0, 0.2f );
            
        imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
        imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
        imgBoard.rectTransform.localScale = Vector3.one;
        //_mapBoard[m,n]=imgBoard;
        _goList.Add( imgBoard.gameObject );

        Image imgShape = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgShape.gameObject.SetActive( true );
        imgShape.transform.SetParent( imgBoard.gameObject.transform );
        imgShape.rectTransform.localPosition = Vector3.zero;
        imgShape.rectTransform.localScale = Vector3.one;
        imgShape.color = Color.white;
        imgShape.sprite = MainPage.instance.SptShapes[shape];
        //_mapShape[m,n]=imgShape;

        imgShape.rectTransform.sizeDelta = new Vector2( MapBlockSize-32, MapBlockSize-32 );
        imgShape.rectTransform.localScale = Vector3.zero;
        DOTween.Play( imgShape.rectTransform.DOScale( Vector3.one, 0.75f).SetEase( Ease.OutBack ) );
    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta) -MapBlockSize );
    }

 
    public override void StartGame() {
        _status = Status_Playing;
        _secondStatus = Status_Remebering;

    }

    public override void Update() {
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

            MainPage.instance.PlaySound( MainPage.Sound_Tap );

            _status=Status_Playing;

            _gameController.SetGameDescription1( "Tap the new one." );

            for(int m=0;m<_goList.Count;m++) {
                GameObject shapeBoard = _goList[m];
                if(m!=_goList.Count-1) {
                    DOTween.Play( shapeBoard.transform.DOScale( Vector3.zero, 0.5f).SetEase( Ease.InBack ) );
                }
                else {
                    DOTween.Play( shapeBoard.transform.DOScale( Vector3.zero, 0.5f).SetEase( Ease.InBack ).OnComplete( ()=> {
                        foreach( GameObject go in _goList ) {
                            GameObject.Destroy( go );
                        }
                        _goList.Clear();

                        int newItem = KWUtility.Random( 0, _nonexistData.Count );
                        Debug.Log( "New Item:"+newItem );

                        _newShape = _nonexistData[newItem];

                        _existData.Add( _newShape );

                        int[] swap = new int[_existData.Count];
                        for(int n=0; n<_existData.Count; n++ ) {
                            swap[n] = _existData[n];
                        }
                        int s, i, j;
                        for( int n=0; n<10; n++ ) {
                            i=KWUtility.Random( 0, _existData.Count);
                            j=KWUtility.Random( 0, _existData.Count);

                            s = swap[i];
                            swap[i]=swap[j];
                            swap[j]=s;
                        }
                        _existData.Clear();
                        for( int n=0; n<swap.Length; n++ ) {
                            _existData.Add( swap[n] );
                        }

                        ShowShape( _existData );


                    } ) );
                }
            }

            _secondStatus=Status_Deciding;

            _timer = 5.0f;
            _totalGameTime = 5.0f;
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
                Debug.Log( "Tapped Shape:"+tapX+"-"+tapY+"=="+_mapData[tapX,tapY]+"--------"+_newShape );
                _status = Status_Gameover;
                if(_mapData[tapX,tapY]==_newShape) {
                    _gameController.SendGameResult( true );
                }
                else {
                    _gameController.SendGameResult( false );
                }

            }
        }
    }

   
}
