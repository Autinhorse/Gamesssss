using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine.SocialPlatforms;

using UnityEngine.UI;

using DG.Tweening;

[Serializable]
public class GameRecord {
    public GameRecord() {
        bestScore = 0;
        bestHellScore = 0;
        firstPlay = true;
        soundFlag = true;
        adRemoved = false;
        data1 = 0;
        data2 = 0;
        data3 = 0;
        data4 = 0;
        data5 = 0;
    }

    public int bestScore;
    public int bestHellScore;
    public bool firstPlay;
    public bool adRemoved;
    public bool soundFlag;
    public int data1;
    public int data2;
    public int data3;
    public int data4;
    public int data5;
}

public class MainPage : MonoBehaviour {
    static MainPage _instance;
    public static MainPage instance {
        get {
            return _instance;
        }
    }

    public const int Status_Main = 1;
    public const int Status_Playing = 2;

    public const int Mode_Challenge = 1;
    public const int Mode_Random = 2;



    [Header("----------这里是预定义的游戏参数----------")]
    public Color[] GameBoardColor;

    public Sprite SptAnswerRight;
    public Sprite SptAnswerWrong;

    public Sprite[] SptShapes;
    public Sprite[] SptHands;
    public Sprite[] SptDices;

    public Sprite SptActor;
    public Sprite SptArrow;
    public Sprite SptUFO;
    public Sprite SptCannon;
    public Sprite SptAlien;

    public Texture2D[] TexPuzzles;

    /*
     * Retangle;
    public Sprite SptTrangle;
    public Sprite SptStar;
    public String SptCircle;
    public String SptHexagon;
    public String SptHeart;
    public String SptFlower;
    public String SptDiamond;
    */

    [Header("----------这里是预定义的游戏对象----------")]
    public GameObject goGameArea;
    public GameObject goGameItem;

    [Header("----------这里是UI对象----------")]
    public Text TxtTime;
    public Text TxtScore;
    public Text TxtGameNumber;

    public Text Txt321Go;


    float ScreenWidth;
    float ScreenHeight;

    /* ------------------ Game related data -------------------------- */
    GameRecord _record;
    int _status;

    int _gameMode;

    GameController[] _currentGameController;
    RectTransform[] _currentGameRect;

    int _nextGameIndex;
    int _nextBoardIndex;

    int _score;
    int _gameNumber;
    float _gameTime;

    // 触摸相关处理
    public struct TouchInfo
    {
        public TouchPhase phase;
        public Vector2 position;
        public Vector2 deltaPosition;
        public float deltaTime;
        public int fingerId;
    }

    int _touchCount;
    TouchInfo[] _touches;
    bool _mousePressed;
    float _lastMouseTime;
    Vector2[] _lastMousePosition;

    Vector2 _testMousePostion;

    void Awake() {
        _instance = this;

        _record = LoadGame();
    }

	// Use this for initialization
	void Start () {
        Txt321Go.gameObject.SetActive( false );

        _status = Status_Main;

        ScreenHeight = 1920;
        ScreenWidth = ScreenHeight/Screen.height*Screen.width;

        _currentGameController = new GameController[2];
        _currentGameRect = new RectTransform[2];

        for( int i=0; i<2; i++ ) {
            GameObject go = GameObject.Instantiate( goGameItem );
            go.transform.SetParent( goGameArea.transform );
            //go.SetActive( true );

            _currentGameController[i] = (GameController) go.GetComponent<GameController>();
            _currentGameRect[i] = (RectTransform) go.GetComponent<RectTransform>();

            _currentGameRect[i].localPosition = new Vector3( ScreenWidth, 0, 0 );
        }

        _gameMode = Mode_Challenge;

        StartGame();
	}
	
	// Update is called once per frame
	void Update () {
        // Touch control
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _touchCount = Input.touchCount;
            if (_touchCount > 0)
            {
                _touches = new TouchInfo[_touchCount];

                for (int i=0; i<_touchCount; i++)
                {
                    _touches[i] = new TouchInfo();
                    _touches[i].phase = Input.touches[i].phase;
                    _touches[i].fingerId = Input.touches[i].fingerId;
                    _touches[i].position = new Vector2(Input.touches[i].position.x, Screen.height - Input.touches[i].position.y);
                    _touches[i].deltaPosition = new Vector2(Input.touches[i].deltaPosition.x, -Input.touches[i].deltaPosition.y);
                    _touches[i].deltaTime = Input.touches[i].deltaTime;
                }
            }
            else
            {
                _touches = null;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _touchCount = 1;
                _touches = new TouchInfo[1];
                _touches[0] = new TouchInfo();
                _touches[0].phase = TouchPhase.Began;
                _touches[0].fingerId = 1;
                _touches[0].position = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                _testMousePostion = _touches[0].position;

                _mousePressed = true;
                _lastMouseTime = Time.time;
            }
            else if (_mousePressed && Input.GetMouseButtonUp(0))
            {
                _touchCount = 1;
                _touches = new TouchInfo[1];
                _touches[0] = new TouchInfo();
                _touches[0].phase = TouchPhase.Ended;
                _touches[0].fingerId = 1;
                _touches[0].position = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                _touches[0].deltaPosition = _touches[0].position - _testMousePostion;
                _touches[0].deltaTime = Time.time - _lastMouseTime;
                _testMousePostion = _touches[0].position;

                _mousePressed = false;
            }
            else if (_mousePressed && Input.GetMouseButton(0))
            {
                _touchCount = 1;
                _touches = new TouchInfo[1];
                _touches[0] = new TouchInfo();
                _touches[0].phase = TouchPhase.Moved;
                _touches[0].fingerId = 1;
                _touches[0].position = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                _touches[0].deltaPosition = _touches[0].position - _testMousePostion;
                _touches[0].deltaTime = Time.time - _lastMouseTime;
                _testMousePostion = _touches[0].position;

                _lastMouseTime = Time.time;
            }
            else
            {
                if (_mousePressed)
                {
                    _touchCount = 1;
                    _touches = new TouchInfo[1];
                    _touches[0] = new TouchInfo();
                    _touches[0].phase = TouchPhase.Ended;
                    _touches[0].fingerId = 1;
                    _touches[0].position = _testMousePostion;
                    _touches[0].deltaPosition = Vector2.zero;
                    _touches[0].deltaTime = 0;

                    _mousePressed = false;
                }
                else
                {
                    _touches = null;
                    _touchCount = 0;
                }
            }

            // 键盘的处理可以在这里。
            /*
            if (Input.GetKeyDown(KeyCode.A))
            {
                _leftDir = DIR_LEFT;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _leftDir = DIR_RIGHT;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                _leftDir = DIR_UP;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                _leftDir = DIR_DOWN;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                _rightDir = DIR_LEFT;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                _rightDir = DIR_RIGHT;
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                _rightDir = DIR_UP;
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                _rightDir = DIR_DOWN;
            }*/
        }
	}


    void FixedUpdate() {
    }

    void StartGame() {
        _status = Status_Playing;

        _nextGameIndex = 0;
        _nextBoardIndex = 0;

        Txt321Go.gameObject.SetActive( true );
        Txt321Go.text = "3";
        Txt321Go.transform.localScale = Vector3.zero;

        CreateNextGame();

        /*
        Txt321Go.rectTransform.localPosition = new Vector3( ScreenWidth*2/3, 0, 0 );

        Sequence seq = DOTween.Sequence();
        Sequence seq1 = DOTween.Sequence();
        seq1.Append( Txt321Go.transform.DOScale( Vector3.one, 0.6f ).SetEase( Ease.OutBack ).SetDelay( 0.1f ) );
        seq1.Insert( 0, Txt321Go.rectTransform.DOLocalMoveX( 0, 0.6f ).SetEase( Ease.OutCubic ).SetDelay( 0.1f ) );

        seq.Append( seq1 );
        seq.Append( Txt321Go.DOColor( new Color( 1, 1, 1, 0 ), 0.2f ).SetDelay( 0.3f ).OnComplete( ()=> {
            Txt321Go.rectTransform.localPosition = new Vector3( ScreenWidth*2/3, 0, 0 );
            Txt321Go.text = "2";
            Txt321Go.transform.localScale = Vector3.zero;
            Txt321Go.color = new Color( 1, 1, 1, 1 );
        } ) );

        Sequence seq2 = DOTween.Sequence();
        seq2.Append( Txt321Go.transform.DOScale( Vector3.one, 0.6f ).SetEase( Ease.OutBack ).SetDelay( 0.1f ) );
        seq2.Insert( 0, Txt321Go.rectTransform.DOLocalMoveX( 0, 0.6f ).SetEase( Ease.OutCubic ).SetDelay( 0.1f ) );

        seq.Append( seq2 );
        seq.Append( Txt321Go.DOColor( new Color( 1, 1, 1, 0 ), 0.2f ).SetDelay( 0.3f ).OnComplete( ()=> {
            Txt321Go.rectTransform.localPosition = new Vector3( ScreenWidth*2/3, 0, 0 );
            Txt321Go.text = "1";
            Txt321Go.transform.localScale = Vector3.zero;
            Txt321Go.color = new Color( 1, 1, 1, 1 );
        } ) );

        Sequence seq3 = DOTween.Sequence();
        seq3.Append( Txt321Go.transform.DOScale( Vector3.one, 0.6f ).SetEase( Ease.OutBack ).SetDelay( 0.1f ) );
        seq3.Insert( 0, Txt321Go.rectTransform.DOLocalMoveX( 0, 0.6f ).SetEase( Ease.OutCubic ).SetDelay( 0.1f ) );

        seq.Append( seq3 );
        seq.Append( Txt321Go.DOColor( new Color( 1, 1, 1, 0 ), 0.2f ).SetDelay( 0.3f ).OnComplete( ()=> {
            Txt321Go.rectTransform.localPosition = new Vector3( ScreenWidth*2/3, 0, 0 );
            Txt321Go.text = "GO";
            Txt321Go.transform.localScale = Vector3.zero;
            Txt321Go.color = new Color( 1, 1, 1, 1 );
        } ) );

        Sequence seq4 = DOTween.Sequence();
        seq4.Append( Txt321Go.transform.DOScale( Vector3.one, 0.6f ).SetEase( Ease.OutBack ).SetDelay( 0.1f ) );
        seq4.Insert( 0, Txt321Go.rectTransform.DOLocalMoveX( 0, 0.6f ).SetEase( Ease.OutCubic ).SetDelay( 0.1f ) );

        seq.Append( seq4 );
        seq.Append( Txt321Go.DOColor( new Color( 1, 1, 1, 0 ), 0.2f ).SetDelay( 0.3f ).OnComplete( ()=> {
           
        } ) );*/

        Sequence seq = DOTween.Sequence();  
        seq.Append( Txt321Go.transform.DOScale( Vector3.one, 0.4f ).SetEase( Ease.OutBack ).SetDelay( 0.1f ) );
        seq.Append( Txt321Go.DOColor( new Color( 1, 1, 1, 0 ), 0.2f ).SetDelay( 0.3f ).OnComplete( ()=> {
            Txt321Go.text = "2";
            Txt321Go.transform.localScale = Vector3.zero;
            Txt321Go.color = new Color( 1, 1, 1, 1 );
        } ) );
        seq.Append( Txt321Go.transform.DOScale( Vector3.one, 0.4f ).SetEase( Ease.OutBack ) );
        seq.Append( Txt321Go.DOColor( new Color( 1, 1, 1, 0 ), 0.2f ).SetDelay( 0.3f ).OnComplete( ()=> {
            Txt321Go.text = "1";
            Txt321Go.transform.localScale = Vector3.zero;
            Txt321Go.color = new Color( 1, 1, 1, 1 );
        } ) );
        seq.Append( Txt321Go.transform.DOScale( Vector3.one, 0.4f ).SetEase( Ease.OutBack ) );
        seq.Append( Txt321Go.DOColor( new Color( 1, 1, 1, 0 ), 0.2f ).SetDelay( 0.3f ).OnComplete( ()=> {
            Txt321Go.text = "Go";
            Txt321Go.transform.localScale = Vector3.zero;
            Txt321Go.color = new Color( 1, 1, 1, 1 );
        } ) );

        seq.Append( Txt321Go.transform.DOScale( Vector3.one, 0.4f ).SetEase( Ease.OutBack ) );
        seq.Append( Txt321Go.DOColor( new Color( 1, 1, 1, 0 ), 0.2f ).SetDelay( 0.3f ).OnComplete( ()=> {
            MoveGameIn( 0 );
        } ) );

        seq.Append( Txt321Go.transform.DOScale( Vector3.zero, 0.6f ).SetEase( Ease.OutBack ) );

        DOTween.Play( seq );

    }

    void CreateNextGame() {
        Debug.Log( "Create next game!!" );
        switch(_gameMode ) {
        case Mode_Challenge:
            int gameID = _nextGameIndex%5*8+_nextGameIndex/5 ;

            GameLogic gameLogic = GameLogic.GetGameLogic( gameID, 0 );
            _currentGameController[_nextBoardIndex].SetGameLogic( gameLogic );
            //gameLogic.SetGameController( _currentGameController[_nextBoardIndex] );

            _nextGameIndex++;

            break;
        case Mode_Random:
            break;
        }
    }

    void MoveGameOut( int boardIndex ) {
        DOTween.Play( _currentGameRect[boardIndex].DOScale( Vector3.one*0.6f, 0.65f ).SetEase( Ease.InCubic )  );
        DOTween.Play( _currentGameRect[boardIndex].DOLocalMoveX( -1*ScreenWidth, 0.65f ).SetEase( Ease.InCubic ).OnComplete( () => {
            _currentGameController[boardIndex].Clear();
        } ) );
    }

    void MoveGameIn( int boardIndex ) {
        _currentGameController[boardIndex].gameObject.SetActive( true );

        _currentGameRect[boardIndex].localScale = Vector3.one*0.6f;
        _currentGameRect[boardIndex].localPosition = new Vector3( ScreenWidth, 0, 0 );

        DOTween.Play( _currentGameRect[boardIndex].DOScale( Vector3.one*1.1f, 0.65f ).SetEase( Ease.OutCubic ).SetDelay( 0.5f )  );
        DOTween.Play( _currentGameRect[boardIndex].DOLocalMoveX( 0, 0.65f ).SetEase( Ease.OutCubic ).SetDelay( 0.5f ).OnComplete( ()=> {
            _currentGameController[_nextBoardIndex].StartGame();
        } ) );
    }

    public void SendGameResult( bool isWin ) {
        Debug.Log( "Mainpage SendGameResult!!!" );

        MoveGameOut( _nextBoardIndex );

        _nextBoardIndex = 1-_nextBoardIndex;

        CreateNextGame();

        MoveGameIn( _nextBoardIndex );

    }

    public  void SaveGame(  ) {
        string  json = JsonUtility.ToJson(_record);
        string  path=Application.persistentDataPath+"/GameRecord.dat";
        StreamWriter sw = new StreamWriter(path,false);

        sw.WriteLine (json);

        sw.Close ();

    }

    public  GameRecord LoadGame(  ) {
        string  path=Application.persistentDataPath+"/GameRecord.dat";

        GameRecord record;

        if(File.Exists( path)) {
            StreamReader sr = new StreamReader (path);
            string line = sr.ReadLine ();
            sr.Close ();

            record = JsonUtility.FromJson<GameRecord>(line);  
        }
        else {
            record = new GameRecord();
        }

        return record;
    }
}
