using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

using DG.Tweening;

public class GameLogicMemeoryMissItem : GameLogicThreeButtons {
    const int Status_Remebering = 3;
    const int Status_Deciding = 4;

    const int MapBlockDelta = 16;

    int MapBlockSize;

    //float _timer;

    //int _secondStatus;

    int _shapeNumber;
    List<int> _mapData; 
    List<int> _nonexistData;

    Image[] _mapBoard;
    Image[] _mapShape;

    int _mapWidth;
    int _mapHeight;

    public GameLogicMemeoryMissItem( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {

    }

    // 关于这个游戏的难度
    // 难度支持0-11:
    // 记忆时间从2秒开始，每增加一级多0.5秒。物体从4个开始，每增加3级难度增加一个物体。

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetButtonMode( GameController.Button_None );

        MapBlockSize = (int) _gameController.boardHeight/8;

        _gameController.SetGameName( "MISS  SHAPE" );
        _gameController.SetGameDescription1( 4, "Remember the shapes" );
        _gameController.SetGameDescription2( 5, 46, "Then tap screen" );

        _gameController.SetColorIndex( 3 );

        int difficulty = _difficulty;
        if(difficulty>3){
            difficulty=3;
        }

        _shapeNumber = 3+difficulty;
            
        if(_shapeNumber>8) {
            _shapeNumber = 8;
        }

        _mapData = new List<int>();

        for(int m=0;m<_shapeNumber;m++) {
            int shape;
            do{
                shape = KWUtility.Random( 0, MainPage.instance.SptShapes.Length );
            } while( _mapData.Contains(shape) );
            _mapData.Add( shape );
        }

        _nonexistData = new List<int>();
        for(int m=0; m<MainPage.instance.SptShapes.Length; m++ ) {
            if(_mapData.Contains(m)==false) {
                _nonexistData.Add( m );
            }
        }

        ShowShape( false, _mapData);
    }

    void ShowShape( bool isHigh, List<int> data ) {
        int shapeIndex = 0;

        switch( data.Count ) {
        case 1:
            _mapWidth = 1;
            _mapHeight = 1;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
       case 2:
            _mapWidth = 2;
            _mapHeight = 1;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 3:
            _mapWidth = 3;
            _mapHeight = 1;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 4:
            _mapWidth = 2;
            _mapHeight = 2;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 5:
            _mapWidth = 3;
            _mapHeight = 2;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if((m==1)&&(n==0)) {
                        continue;
                    }
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 6:
            _mapWidth = 3;
            _mapHeight = 2;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 7:
            _mapWidth = 3;
            _mapHeight = 3;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if((n==0)&&((m==0)||(m==2))) {
                        continue;
                    }
                    CreateShape( isHigh, data[shapeIndex], m, n );
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
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 9:
            _mapWidth = 3;
            _mapHeight = 3;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 10:
            _mapWidth = 4;
            _mapHeight = 3;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if(((m==0)||(m==3))&&(n==0)) {
                        continue;
                    }
                    CreateShape( isHigh,data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 11:
            _mapWidth = 4;
            _mapHeight = 3;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if((m==0)&&(n==0)) {
                        continue;
                    }
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 12:
        case 13:
            _mapWidth = 3;
            _mapHeight = 3;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 14:
            _mapWidth = 4;
            _mapHeight = 3;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if((n==0)&&((m==1)||(m==2))) {
                        continue;
                    }
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        case 15:
            _mapWidth = 3;
            _mapHeight = 4;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    if((n==0)&&(m==1)) {
                        continue;
                    }
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        default:
            _mapWidth = 4;
            _mapHeight = 3;
            for( int m=0; m<_mapWidth; m++) {
                for( int n=0; n<_mapHeight; n++ ) {
                    CreateShape( isHigh, data[shapeIndex], m, n );
                    shapeIndex++;
                }
            }
            break;
        }
    }

    void CreateShape(bool isHigh, int shape, int m, int n ) {
        Vector2 pos = GetPosition( isHigh, m, n );
        Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgBoard.gameObject.SetActive( true );
        imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
        imgBoard.color = new Color( 0, 0, 0, 0.2f );
            
        imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
        imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
        //_mapBoard[m,n]=imgBoard;
        _goList.Add( imgBoard.gameObject );

        Image imgShape = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgShape.gameObject.SetActive( true );
        imgShape.transform.SetParent( imgBoard.gameObject.transform );
        imgShape.rectTransform.localPosition = Vector3.zero;
        imgShape.color = Color.white;
        imgShape.sprite = MainPage.instance.SptShapes[shape];
        //_mapShape[m,n]=imgShape;

        imgShape.rectTransform.sizeDelta = new Vector2( MapBlockSize*0.75f, MapBlockSize*0.75f );

        imgBoard.rectTransform.localScale = Vector3.zero;
        DOTween.Play( imgBoard.rectTransform.DOScale( Vector3.one, 0.75f).SetEase( Ease.OutBack ) );
    }

    Vector2 GetPosition( bool isHigh, int x, int y ) {
        if(isHigh==true) {
            return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta) );
        }

        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta) - MapBlockSize );
    }

    void HideCard( int x, int y ) {
       
    }


    public override void StartGame() {
        _status = Status_Playing;

       // _secondStatus = Status_Remebering;

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
        MainPage.instance.PlaySound( MainPage.Sound_Tap );

       // _secondStatus=Status_Deciding;

        _gameController.SetGameDescription1( 0, "Which one has gone?" );

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

                    int removeItem = KWUtility.Random( 0, _mapData.Count );
                    int removeShape = _mapData[removeItem];

                    _mapData.RemoveAt( removeItem );

                    int[] swap = new int[_mapData.Count];
                    for(int n=0; n<_mapData.Count; n++ ) {
                        swap[n] = _mapData[n];
                    }
                    int s, i, j;
                    for( int n=0; n<10; n++ ) {
                        i=KWUtility.Random( 0, _mapData.Count);
                        j=KWUtility.Random( 0, _mapData.Count);

                        s = swap[i];
                        swap[i]=swap[j];
                        swap[j]=s;
                    }
                    _mapData.Clear();
                    for( int n=0; n<swap.Length; n++ ) {
                        _mapData.Add( swap[n] );
                    }

                    ShowShape( true, _mapData );
                    int wrong1 = KWUtility.Random( 0, _nonexistData.Count );
                    int wrong2;

                    do {
                        wrong2=KWUtility.Random( 0, _nonexistData.Count );
                    }while(wrong1==wrong2);

                    _gameController.SetButtonMode( GameController.Button_Three );
                    SetButtonsRandom( MainPage.instance.SptShapes[removeShape], MainPage.instance.SptShapes[_nonexistData[wrong1]], MainPage.instance.SptShapes[_nonexistData[wrong2]] );
                } ) );
            }
        }

        _timer = 6.0f;
        _totalGameTime = 6.0f;
    }

}
