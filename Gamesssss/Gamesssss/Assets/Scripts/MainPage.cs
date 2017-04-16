using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine.SocialPlatforms;

using UnityEngine.UI;

using DG.Tweening;

using Umeng;

using GoogleMobileAds.Api;

using Heyzap; 

[Serializable]
public class GameRecord {
    public GameRecord() {
        bestScore = 0;
        bestRandomScore = 0;
        maxGameLogic = 0;
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
    public int bestRandomScore;
    public int maxGameLogic;
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
    public Sprite SptArrowEmpty;
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
    public Text TxtTimeSecond;
    public Text TxtTimeColon;
    public Text TxtTimeMiSecond;
    public Text TxtTimePlusFive;
    public Text TxtScore;
    public Text TxtGameRightNumber;
    public Text TxtGameWrongNumber;


    public Text Txt321Go;

    [Header("----------这里是声音对象----------")]
    public AudioSource SndMusic;
    public AudioSource SndRight;
    public AudioSource SndWrong;
    public AudioSource SndTap;
    public AudioSource SndTapWrong;
    public AudioSource SndGameOver;
    public AudioSource SndShoot;
    public AudioSource SndButton;

    public const int Sound_Right = 0;
    public const int Sound_Wrong = 1;
    public const int Sound_Tap = 2;
    public const int Sound_TapWrong = 3;
    public const int Sound_GameOver = 4;
    public const int Sound_Shoot = 5;
    public const int Sound_Button = 6;


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

    float _currentGameTimer;
    int _rightGameNumber;
    int _wrongGameNumber;

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
    Vector2 _lastMousePosition;

    Vector2 _testMousePostion;

    void Awake() {
        _instance = this;

        _record = LoadGame();
    }

    bool _adBannerLoaded;
    BannerView _bannerView;
    InterstitialAd _interstitial;
    int _gameOverCount;
    bool _showAd;

	// Use this for initialization
	void Start () {

        HeyzapAds.Start("fb1f6eda98f7cf0ce219322d5f7df381", HeyzapAds.FLAG_NO_OPTIONS);

        _adBannerLoaded = false;
        _gameOverCount=0;
        RequestInterstitial();
        RequestBanner();

        Social.localUser.Authenticate( success => {
            if (success) {
                Debug.Log ("Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName + 
                    "\nUser ID: " + Social.localUser.id + 
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log (userInfo);
            }
            else {
                Debug.Log ("Authentication failed");
            }
        });


        GA.StartWithAppKeyAndChannelId("58f2431af5ade43ea8000e79", "App Store");




        GameBoardColor[0] = new Color( 57/255.0f, 198/255.0f, 160/255.0f );
        GameBoardColor[1] = new Color( 36/255.0f, 204/255.0f, 242/255.0f );
        GameBoardColor[2] = new Color( 255/255.0f, 96/255.0f, 128/255.0f );
        GameBoardColor[3] = new Color( 242/255.0f, 186/255.0f, 72/255.0f );
        GameBoardColor[4] = new Color( 212/255.0f, 96/255.0f, 255/255.0f );

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

        Color color = TxtTimePlusFive.color;
        color.a = 0;
        TxtTimePlusFive.color = color;

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
        if(_touchCount>0) {
            //Debug.Log( "Touched!!!"+_touches[0].phase+"---"+_touches[0].position );
        }

        int second=10;
        int miSecond=0;


        if(_currentGameController!=null) {
            if(_currentGameController[_nextBoardIndex].status==GameLogic.Status_Playing) {
                _gameTime-=Time.fixedDeltaTime;
                _currentGameTimer-=Time.fixedDeltaTime;


            }
        }
        if(_gameTime<0){
            _gameTime=0;
        }
        second = (int)_gameTime;
        miSecond = (int)((_gameTime-second)*100);

        if(second<100) {
            TxtTimeSecond.text = second.ToString("00");
        }
        else {
            TxtTimeSecond.text = second.ToString("000");
        }
        TxtTimeMiSecond.text = miSecond.ToString( "00" );
    }

    void StartGame() {
        _status = Status_Playing;

        _nextGameIndex = 0;
        _nextBoardIndex = 0;

        Txt321Go.gameObject.SetActive( true );
        Txt321Go.text = "3";
        Txt321Go.transform.localScale = Vector3.zero;

        CreateNextGame();

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

        _gameTime = 20.0f;

        _score = 0;
        TxtScore.text = _score.ToString();

        _rightGameNumber = 0;
        _wrongGameNumber = 0;

        TxtGameRightNumber.text = _rightGameNumber.ToString();
        TxtGameWrongNumber.text = _wrongGameNumber.ToString();
    }

    void CreateNextGame() {
        Debug.Log( "Create next game!!" );
        switch(_gameMode ) {
        case Mode_Challenge:
            int gameID = _nextGameIndex%5*8+_nextGameIndex/5 ;

            GameLogic gameLogic = GameLogic.GetGameLogic( gameID, _nextGameIndex/5 );
            _currentGameController[_nextBoardIndex].SetGameLogic( gameLogic );
            //gameLogic.SetGameController( _currentGameController[_nextBoardIndex] );

            _nextGameIndex++;

            break;
        case Mode_Random:
            break;
        }

        _currentGameTimer = 5.0f;
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
            _currentGameController[boardIndex].StartGame();
        } ) );
    }

    public void SendGameResult(bool isWin) {

        if(isWin) {
            _gameTime+=5;

            TxtTimePlusFive.text = "+5";
            TxtTimePlusFive.color = new Color( 0.4f, 1.0f, 0.4f, 0.0f );

            Sequence seq = DOTween.Sequence();
            Color color = TxtTimePlusFive.color;
            color.a = 1.0f;

            seq.Append( TxtTimePlusFive.DOColor( color, 0.1f) );
            color.a = 0;
            seq.Append( TxtTimePlusFive.DOColor( color, 1.0f) );

            DOTween.Play( seq );

            if(_currentGameTimer>0) {
                int baseScore=100;
                int difficult = (_nextGameIndex-1)/5+1;
                for(int m=0;m<difficult;m++){
                    baseScore*=2;
                }
                _score+=(int)(baseScore*_currentGameTimer);
                TxtScore.text = _score.ToString();
            }

            _currentGameTimer = 5.0f;

            _rightGameNumber++;
            TxtGameRightNumber.text = _rightGameNumber.ToString();
        }
        else {
            _gameTime-=5;

            TxtTimePlusFive.text = "-5";
            TxtTimePlusFive.color = new Color( 1.0f, 0.4f, 0.4f, 0.0f );

            Sequence seq = DOTween.Sequence();
            Color color = TxtTimePlusFive.color;
            color.a = 1.0f;

            seq.Append( TxtTimePlusFive.DOColor( color, 0.1f) );
            color.a = 0;
            seq.Append( TxtTimePlusFive.DOColor( color, 1.0f) );

            DOTween.Play( seq );

            _currentGameTimer = 5.0f;

            _wrongGameNumber++;
            TxtGameWrongNumber.text = _wrongGameNumber.ToString();
        }
    }

    public void ExecGameResult(  ) {
        Debug.Log( "Mainpage SendGameResult!!!" );

        MoveGameOut( _nextBoardIndex );

        _nextBoardIndex = 1-_nextBoardIndex;

        CreateNextGame();

        MoveGameIn( _nextBoardIndex );

    }

    public void PlaySound( int soundIndex ) {
        if(_record.soundFlag==false) {
            return;
        }

        switch( soundIndex ) {
        case Sound_Tap:
            SndTap.Play();
            break;
        case Sound_TapWrong:
            SndTapWrong.Play();
            break;
        case Sound_Right:
            SndRight.Play();
            break;
        case Sound_Wrong:
            SndWrong.Play();
            break;
        case Sound_Button:
            SndButton.Play();
            break;
        case Sound_GameOver:
            SndGameOver.Play();
            break;
        case Sound_Shoot:
            SndShoot.Play();
            break;
        }
    }

    public void OnButtonLeaderboard() {
        if(_record.soundFlag==true) {
            PlaySound( Sound_Button);
        }

        Social.ShowLeaderboardUI();
    }

    public void OnButtonMoreGames() {
        if(_record.soundFlag==true) {
            PlaySound( Sound_Button);
        }

        Application.OpenURL("https://itunes.apple.com/us/developer/kylinworks-software/id348568981");
    }

    public void OnButtonSound() {
        if(_record.soundFlag==true) {
            PlaySound( Sound_Button);
        }

        if(_record.soundFlag==true) {
            _record.soundFlag=false;
            //ImgSoundButton.sprite = SptSoundOff;
            SndMusic.Stop();
        }
        else {
            _record.soundFlag=true;
            //ImgSoundButton.sprite = SptSoundOn;
            SndMusic.Play();
        }
        SaveGame();
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

    private void RequestBanner()
    {
        #if UNITY_ANDROID
        string adUnitId = "INSERT_ANDROID_BANNER_AD_UNIT_ID_HERE";
        #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-5622495864296527/5917302917";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        //if((_showAd==false)||(_record.adRemoved==true)) {
        //    return;
        //}

        // Create a 320x50 banner at the top of the screen.
        //_bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        _bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        _bannerView.OnAdLoaded += HandleOnAdLoaded;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        _bannerView.LoadAd(request);
    }

    private void RequestInterstitial()
    {
        #if UNITY_ANDROID
        string adUnitId = "INSERT_ANDROID_INTERSTITIAL_AD_UNIT_ID_HERE";
        #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-5622495864296527/2824235712";
        #else
        string adUnitId = "unexpected_platform";
        #endif

        // Initialize an InterstitialAd.
        _interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        _interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args) {
        _adBannerLoaded = true;
    }

    /*
    Social.ReportScore (_record.bestScore, "com.kylinworks.swipeduel.lb.bestscore", success => {
        Debug.Log(success ? "Reported score successfully" : "Failed to report score");
    });*/
}
