using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using DG.Tweening;

public class GameLogicMemoryShape : GameLogicTwoButtons {

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
    Text[] _mapText;

    int _mapWidth;
    int _mapHeight;

    int _targetNumber;
    int _targetType;
    int _targetIndex;

    public GameLogicMemoryShape( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {

    }

    // 关于这个游戏的难度
    // 难度支持0-15:
    // 记忆时间从2秒开始，每增加一级多0.5秒。物体从2个开始，每增加2级难度增加一个物体。

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetButtonMode( GameController.Button_None );

        MapBlockSize = (int) _gameController.boardWidth/6;

        _gameController.SetColorIndex( 3 );

        int difficulty = _difficulty;
        if(difficulty>3){
            difficulty=3;
        }

        int[] blockNumber = { 3, 5, 7, 9, 4, 5, 5, 6, 6, 6, 7, 7, 7, 8, 8, 8 }; 
        int[] shapeNumber = { 2, 2, 3, 3 };
        _shapeNumber = shapeNumber[difficulty];

        _gameController.SetGameDescription1( 4, "Remember the shapes" );
        _gameController.SetGameDescription2( 5, 46,"Then tap screen" );

        _existData = new List<int>();

        _targetNumber = 0;
        _targetIndex = KWUtility.Random( 0, _shapeNumber);

        for(int m=0;m<blockNumber[difficulty];m++) {
            int shape = KWUtility.Random( 0, _shapeNumber);
            if(shape==_targetIndex) {
                _targetNumber++;
            }
            _existData.Add( shape );
        }

        ShowShape(_existData);
    }

    void ShowShape( List<int> data ) {
        int shapeIndex = 0;

        _targetType = KWUtility.Random(0, 3);

        switch(_targetType){
        case 0:
            _targetType=-1;
            break;
        case 1:
            _targetType=KWUtility.Random(48,53);
            break;
        case 2:
            _targetType=KWUtility.Random(65,85);
            break;
        }

        switch( data.Count ) {
       
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

    void CreateShape(  int shape, int m, int n ) {
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

        if(_targetType==-1) {
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
        else {
            Text txtShape = (Text) GameObject.Instantiate( _gameController.goBoardChar );
            txtShape.gameObject.SetActive( true );
            txtShape.transform.SetParent( imgBoard.gameObject.transform );
            txtShape.rectTransform.localPosition = Vector3.zero;
            txtShape.rectTransform.localScale = Vector3.one;
            txtShape.color = Color.white;
            txtShape.text = ((char)(_targetType+shape)).ToString();
            //_mapShape[m,n]=imgShape;

            txtShape.rectTransform.sizeDelta = new Vector2( MapBlockSize-32, MapBlockSize-32 );
            txtShape.rectTransform.localScale = Vector3.zero;
            DOTween.Play( txtShape.rectTransform.DOScale( Vector3.one, 0.75f).SetEase( Ease.OutBack ) );
        }
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

                        if(_targetType==-1){
                            _gameController.SetGameDescription1( 6, "How many          did you see?" );

                            Image go1 = (Image) GameObject.Instantiate( _gameController.goBoardImage );
                            _goList.Add( go1.gameObject );
                            go1.gameObject.SetActive( true );

                            go1.gameObject.transform.SetParent( _gameController.goBoardArea.transform );
                            go1.rectTransform.localPosition = new Vector3( 44, _gameController.boardHeight*27/80, 0 ); 

                            go1.rectTransform.sizeDelta = new Vector2( 48, 48 );
                            go1.rectTransform.localScale = Vector3.one;

                            go1.color = Color.white;

                            go1.sprite = MainPage.instance.SptShapes[_targetIndex];
                        }
                        else {
                            string question = "How many "+((char)(_targetIndex+_targetType)).ToString()+" did you see?";
                            _gameController.SetGameDescription1( 6, question );
                        }

                        int answer = KWUtility.Random(0,2);

                        if(_targetNumber==0){
                            answer=1;
                        }
                        if(_targetNumber==_shapeNumber){
                            answer=0;
                        }

                        switch(answer){
                        case 0:
                            SetButtonsRandom( _targetNumber.ToString(), (_targetNumber-1).ToString());
                            break;
                        case 1:
                            SetButtonsRandom( _targetNumber.ToString(), (_targetNumber+1).ToString());
                            break;
                        }

                    } ) );
                }
            }

            _secondStatus=Status_Deciding;

        }

    }


}