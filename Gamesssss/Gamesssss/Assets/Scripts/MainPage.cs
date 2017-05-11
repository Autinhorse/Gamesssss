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

using Fenderrio.ImageWarp;

[Serializable]
public class GameRecord {
    public GameRecord() {
        bestScore = 0;
        bestGameNumber = 0;
        maxGameLogic = 0;
        firstPlay = true;
        soundFlag = true;
        adRemoved = false;
        energy = 5;
        startSecond = 0;
        playerID = -1;
        loginID = 0;
        gamecenterIDReported = false;
        gamePlayIndex = 0;
        logoutTime = "";
        duration = 0;
        data1 = 0;
        data2 = 0;
        data3 = 0;
        data4 = 0;
        data5 = 0;
    }

    public int bestScore;
    public int bestGameNumber;
    public int maxGameLogic;
    public bool firstPlay;
    public bool adRemoved;
    public bool soundFlag;
    public int energy;
    public long startSecond;
    public int playerID;
    public int loginID;
    public bool gamecenterIDReported;
    public int gamePlayIndex;
    public string logoutTime;
    public int duration;
    public int data1;
    public int data2;
    public int data3;
    public int data4;
    public int data5;
}

public delegate void WWWCallbackFunction( WWW www );

public delegate void CallbackFunction();

public class CallbackItem {
    static int itemIndex = 0;

    public CallbackItem( CallbackFunction vCallback, float vDelay, bool vRepeat ) {
        callback = vCallback;
        delay = vDelay;
        repeat = vRepeat;

        timer = delay;

        index = itemIndex;
        itemIndex++;
    }

    public int index;
    public float delay;
    public bool repeat;
    public CallbackFunction callback;

    public float timer;
}

public class MainPage : MonoBehaviour {
    static MainPage _instance;
    public static MainPage instance {
        get {
            return _instance;
        }
    }

    List<CallbackItem> _callbackList;

    const int MaxGameNumber = 20;

    public const int Status_NoArtistDelay = 0;
    public const int Status_NoArtist = 1;
    public const int Status_Playing = 2;
    public const int Status_Main = 3;
    public const int Status_ShowFiveWithBoard = 4;
    public const int Status_GameOver = 5;

    public const int Mode_Challenge = 1;
    public const int Mode_Random = 2;

    // local test server of local host
    /*
    public const string URLRegisterUser = "http://localhost/5Seconds/Register.php";
    public const string URLUpdateGameCenterID = "http://localhost/5Seconds/UpdateGameCenterID.php";
    public const string URLLogin = "http://localhost/5Seconds/Login.php";
    public const string URLPlayGame = "http://localhost/5Seconds/PlayGame.php";
*/
    // public server on linode, NoArtistStudio.com
    public const string URLRegisterUser = "https://NoArtistStudio.com/Games5Seconds/Register.php";
    public const string URLUpdateGameCenterID = "https://NoArtistStudio.com/Games5Seconds/UpdateGameCenterID.php";
    public const string URLLogin = "https://NoArtistStudio.com/Games5Seconds/Login.php";
    public const string URLPlayGame = "https://NoArtistStudio.com/Games5Seconds/PlayGame.php";

    [Header("----------这里是预定义的游戏参数----------")]
    public Color[] GameBoardColor;

    public Sprite SptAnswerRight;
    public Sprite SptAnswerWrong;
    public Sprite SptAnswerTimeout;

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

    public AnimationCurve CurveSecondsDropAngle;
    public float SecondDropDuration = 0.6f;

    public float FiveWithBoardFarDelta = 40;

    public AnimationCurve CurveNoArtistScaleX;
    public AnimationCurve CurveNoArtistScaleY;
    public AnimationCurve CurveNoArtistPosY;

    public float NoArtistHideDuration = 0.6f;
    public float GameOverHideDuration = 0.6f;
    public int NoArtistHidePosY = 240;
    public int NoArtistHideXWide = 360;
    public float NoArtistHideAngle = 5;

    public AnimationCurve CurveFiveDrop;
    public float FiveDropDuration;

    public AnimationCurve CurvePlayButtonZoom;
    public float PlayButtonZoomDuration;

    public AnimationCurve CurveBoardMoveX;
    public AnimationCurve CurveBoardMoveY;
    public AnimationCurve CurveBoardMoveZoom;

    public AnimationCurve CurveBoardMoveXOut;
    public AnimationCurve CurveBoardMoveYOut;
    public AnimationCurve CurveBoardMoveZoomOut;


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

    public Camera MainCamera;

    public Sprite SptSoundOn;
    public Sprite SptSoundOff;

    public Sprite Spt321GoHandGo;
    public Sprite Spt321GoHand3;
    public Sprite Spt321GoHand2;
    public Sprite Spt321GoHand1;

    public URLController urlController;


    [Header("----------这里是UI对象----------")]
    public RectTransform RectFiveWithBoard;
    //public Image ImgFive;
    //public Image ImgFiveBoard;

    public RectTransform RectBtnPlay;
    public RectTransform RectBtnLeaderboard;
    public RectTransform RectBtnMoreGames;
    public RectTransform RectBtnSounds;
    public RectTransform RectImgButtonBG;

    public Image ImgBtnPlay;
    public Image ImgBtnLeaderboard;
    public Image ImgBtnMoreGames;
    public Image ImgBtnSound;


    public RectTransform RectGameOver;
    public Image ImgGameOverBG;
    public Text TxtGameOverTimeup;
    public Text TxtGameOver;
    public Text TxtGameOverScore;
    public Text TxtGameOverScoreValue;
    public Text TxtGameOverScoreValueBest;
    public Text TxtGameOverBestScore;
    public Text TxtGameOverBestScoreValue;
    public Text TxtGameOverGames;
    public Text TxtGameOverGamesValue;
    public Text TxtGameOverGamesValueBest;
    public Text TxtGameOverBestGames;
    public Text TxtGameOverBestGamesValue;
    public Button BtnGameOverRetry;
    public Button BtnGameOverHome;

    public Image ImgBtnGameOverRetry;
    public Image ImgBtnGameOverHome;
    public Text TxtBtnGameOverRetry;
    public Text TxtBtnGameOverHome;


    public RectTransform RectTopBar;

    public Text TxtTimeSecond;
    public Text TxtTimeColon;
    public Text TxtTimeMiSecond;
    //public Text TxtTimePlusFive;
    public Text TxtScore;
    public Text TxtGameRightNumber;
    //public Text TxtGameWrongNumber;

    public Image[] ImgLives;

    public RectTransform RectNoArtist;
    public Image  ImgNoArtist;
    public Image    ImgWhiteBar;
    public ImageWarp ImgSeconds;

    public Image Img321Go;

    public RectTransform RectEnergy;
    public Text TxtEnergyNumber;
    public Text TxtEnergyChange;
    public Text TxtEnergyTimeSecond;
    public Text TxtEnergyTimeColon;
    public Text TxtEnergyTimeMiSecond;
    public Button BtnEnergy;
    public Button BtnEnergyAwardVideo;
    public Text TxtEnergyTitle;
    public Image ImgEnergyBG;
    public Image ImgTouchCover;
    public Image ImgEnergyIcon;
    public Image ImgEnergyVideo;
    public Text TxtBtnEnergyVideo;
    public Image ImgFreeVideoBG;
    public Image ImgFreeVideoEnergy;
    public Button BtnEnergyClose;



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

    float _fiveSpeedY;
    float Gravity;

    bool _buttonShowed;
    /* ------------------ Game related data -------------------------- */
    GameRecord _record;
    int _status;

    int _btnPlayCallbackIndex;
    int _btnOtherCallbackIndex;

    GameController[] _currentGameController;
    RectTransform[] _currentGameRect;

    int _nextBoardIndex;

    int _score;
    int _gameNumber;
    float _gameTime;
    float _totalGameTime;
    string _gameResult="";
    int _lives;

    int _nextGameIndex;

    float _currentGameTimer;
    int _rightGameNumber;
    int _wrongGameNumber;

    bool[] _gamePlayed;

    float _timer;

    float _secondDropTimer;

    int _gamePlayedCounter;
    int _gameSeed;
    string _levelResult;
   
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

    DateTime _loginTime;

    protected int AddCallbackFunction( CallbackFunction callback ) {
        return AddCallbackFunction( callback, 0, false );
    }

    protected int AddCallbackFunction( CallbackFunction callback, float delay, bool repeat ) {
        CallbackItem item = new CallbackItem( callback, delay, repeat );
        _callbackList.Add( item );

        return item.index;
    }

    protected void KillRepeatCallback( int index ) {
        foreach( CallbackItem item in _callbackList ) {
            if(item.index==index){
                _callbackList.Remove(item);
                break;
            }
        }
    }

    void Awake() {
        Debug.Log( "Application awake!" );

        _instance = this;

        _callbackList = new List<CallbackItem>();

        _record = LoadGame();

        if(_record.startSecond<1000) {
            _record.startSecond = DateTime.Now.Ticks/10000000;
            Debug.Log( "Set startSecond:"+_record.startSecond );
            SaveGame();
            _showAd = false;
        }
        else {
            long currentSecond = DateTime.Now.Ticks/10000000;
            Debug.Log( "Set CurrentSecond:"+currentSecond );
            Debug.Log( "Delta Time:"+(currentSecond-_record.startSecond) );
            _showAd = (currentSecond-_record.startSecond)>86400;
        }

        if(_record.soundFlag==true) {
            SndMusic.Play();
        }

        _gamePlayedCounter = 0;



    }

    //bool _adBannerLoaded;
    BannerView _bannerView;
    InterstitialAd _interstitial;
    //int _gameOverCount;
    bool _showAd;

	// Use this for initialization
	void Start () {
        Debug.Log( "Application start!" );

        //MainCamera.backgroundColor = new Color( 0/255.0f, 144/255.0f, 118/255.0f, 1.0f );

        //MainCamera.backgroundColor = new Color( 36/255.0f, 48/255.0f, 56/255.0f, 1.0f );
        MainCamera.backgroundColor = new Color( 32/255.0f, 48/255.0f, 64/255.0f, 1.0f );
                         
        HeyzapAds.Start("fb1f6eda98f7cf0ce219322d5f7df381", HeyzapAds.FLAG_NO_OPTIONS);

        //HeyzapAds.Start("fb1f6eda98f7cf0ce219322d5f7df381", HeyzapAds.FLAG_NO_OPTIONS);
        Color color;

        HZIncentivizedAd.AdDisplayListener listener = delegate(string adState, string adTag){
            Debug.Log( "HZIncentivizedAd listener is callled! - status:"+adState+"   Tag:"+adTag );
            Debug.Log( "Status is :"+_status );

            if ( adState.Equals("incentivized_result_complete") ) {
                
                // The user has watched the entire video and should be given a reward.
                //ShowGameOver();
                color = TxtGameOverTimeup.color;
                color.a = 0;
                Sequence seq = DOTween.Sequence();
                seq.Append( TxtGameOverTimeup.DOColor( color, 0.2f ).OnComplete(()=> {
                    TxtGameOverTimeup.text = "THANKS";
                } ) );
                color.a = 1;
                seq.Append( TxtGameOverTimeup.DOColor( color, 0.5f ).OnComplete( ()=> {
                    DOTween.Play( RectGameOver.DOLocalMoveX( -1*ScreenWidth, 0.35f ).SetEase( Ease.InCubic ).SetDelay(1.0f).OnComplete( ()=> {
                        //StartGame();
                        Show321Go();
                    } ) );

                } ) );

                DOTween.Play( seq );
            }
            if ( adState.Equals("incentivized_result_incomplete") ) {
                // The user did not watch the entire video and should not be given a   reward.
               /* if(_status==Status_GameOverShowVideo) {
                    _status = Status_GameOverVideoInComplete;
                }
                else {
                    _status=Status_FreePowerUpVideoInComplete;
                }*/
            }
            if ( adState.Equals("hide") ) {
                // The user did not watch the entire video and should not be given a   reward.
                /*if(_status==Status_GameOverVideoInComplete) {
                    _status = Status_GameOver;
                }
                else if(_status==Status_FreePowerUpVideoInComplete)  {
                    _status=Status_Playing;
                }
                else if(_status==Status_GameOverVideoComplete)  {
                    _status=Status_GameOverDoubleCoin;
                    _record.totalCoin+=_coin;
                    _timer=1.0f;
                    _coinMulti*=2;
                    ImgDouble.gameObject.SetActive( true );
                    TxtDouble.text = _coinMulti.ToString()+"X";

                    Analytics.CustomEvent("Double Coin Video Played!", null );

                }
                else if(_status==Status_FreePowerUpVideoComplete)  {
                    _status=Status_Playing;

                    PlaySound( Sound_PurchaseCoin );

                    _record.bombNumber++;
                    BtnBomb.interactable = true;

                    SaveGame();

                    Sequence seq = DOTween.Sequence();
                    seq.Append( BtnBomb.transform.DOScale( Vector3.one*1.2f, 0.6f ).OnComplete( ()=> {
                        TxtBombNumber.text = _record.bombNumber.ToString();
                    } ) );
                    seq.Append( BtnBomb.transform.DOScale( Vector3.one, 0.6f ) );

                    DOTween.Play( seq );

                    Analytics.CustomEvent("Free Powerup Video Played!", null );

                }*/
            }
        };

        if(_record.soundFlag==true) {
            ImgBtnSound.sprite = SptSoundOn;
        }
        else {
            ImgBtnSound.sprite = SptSoundOff;
        }

        HZIncentivizedAd.SetDisplayListener(listener);

        HZIncentivizedAd.Fetch();

        //HeyzapAds.ShowMediationTestSuite();

       //_adBannerLoaded = false;
        //_gameOverCount=0;
        RequestInterstitial();
        RequestBanner();

        Social.localUser.Authenticate( success => {
            if (success) {
                Debug.Log ("Authentication successful");
                string userInfo = "Username: " + Social.localUser.userName + 
                    "\nUser ID: " + Social.localUser.id + 
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log (userInfo);

                if((_record.gamecenterIDReported==false)&&(_record.playerID!=-1)) {
                    Dictionary<string,string> postData = new Dictionary<string,string>();
                    postData.Add( "playerID", _record.playerID.ToString() );
                    postData.Add( "gamecenterID", Social.localUser.userName );


                    urlController.POST( URLUpdateGameCenterID,  postData );
                }
            }
            else {
                Debug.Log ("Authentication failed");
            }
        });


        GA.StartWithAppKeyAndChannelId("58f2431af5ade43ea8000e79", "App Store");

        //GameBoardColor[0] = new Color( 32/255.0f, 216/255.0f, 160/255.0f );
        //GameBoardColor[0] = new Color( 88/255.0f, 218/255.0f, 164/255.0f );
        GameBoardColor[0] = new Color( 48/255.0f, 160/255.0f, 120/255.0f );
        GameBoardColor[1] = new Color( 56/255.0f, 168/255.0f, 208/255.0f );
        GameBoardColor[2] = new Color( 255/255.0f, 96/255.0f, 128/255.0f );
        GameBoardColor[3] = new Color( 218/255.0f, 186/255.0f, 82/255.0f );
        //GameBoardColor[4] = new Color( 212/255.0f, 96/255.0f, 255/255.0f );
        GameBoardColor[4] = new Color( 132/255.0f, 120/255.0f, 244/255.0f );

        Img321Go.gameObject.SetActive( false );


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

        _gamePlayed = new bool[MaxGameNumber];

        Vector3 pos = RectTopBar.localPosition;
        pos.y+=160;
        RectTopBar.localPosition = pos;

        _status = Status_NoArtistDelay;
        _timer = 1.5f;

        RectFiveWithBoard.localScale = Vector3.one;//*0.6f;

        pos = RectFiveWithBoard.localPosition;
        pos.y+=ScreenHeight/2;
        RectFiveWithBoard.localPosition = pos;

        pos = RectBtnPlay.localPosition;

        pos.x = -1*ScreenWidth*3/9;
        RectBtnSounds.localPosition = pos;

        pos.x+=2*ScreenWidth/9;
        RectBtnMoreGames.localPosition = pos;

        pos.x+=2*ScreenWidth/9;
        RectBtnLeaderboard.localPosition = pos;

        pos.x+=2*ScreenWidth/9;
        RectBtnPlay.localPosition = pos;


        RectBtnPlay.localScale = Vector3.zero;
        RectBtnLeaderboard.localScale = Vector3.zero;
        RectBtnMoreGames.localScale = Vector3.zero;
        RectBtnSounds.localScale = Vector3.zero;
        RectImgButtonBG.localScale = new Vector3(1, 0, 1);
            

        pos = RectGameOver.localPosition;
        pos.x = ScreenWidth;
        RectGameOver.localPosition = pos;

        _secondDropTimer = -1;
      
        ImgSeconds.gameObject.SetActive( false );
        SetFiveWithBoardAngle( 0 );

        RectEnergy.localPosition = new Vector3( 0, ScreenHeight/-2-200, 0 );
        TxtEnergyTitle.gameObject.SetActive( false );
        TxtEnergyTimeSecond.gameObject.SetActive( false );
        TxtEnergyTimeColon.gameObject.SetActive( false );
        TxtEnergyTimeMiSecond.gameObject.SetActive( false );

        ImgEnergyBG.rectTransform.localScale = Vector3.one;
        BtnEnergyClose.gameObject.SetActive( false );
        BtnEnergyAwardVideo.gameObject.SetActive( false );

        ImgEnergyIcon.rectTransform.localPosition = new Vector3( -32, 48,0 );
        TxtEnergyNumber.rectTransform.localPosition = new Vector3( 32, 48,0 );

        ImgTouchCover.gameObject.SetActive( false );

        //StartGame();

        urlController.wwwCallback = MainWWWCallbackFunction;



       // Debug.Log( "WWW result:"+www.text );
	}

    void OnApplicationPause(bool paused)
    {
        Debug.Log( "OnApplicationPause:"+paused );

        if(paused==true) {
            _gamePlayedCounter = 0;

            DateTime logoutTime = DateTime.Now;
           
            _record.logoutTime = (logoutTime.Year%100).ToString("00")+logoutTime.Month.ToString("00")+logoutTime.Day.ToString("00")
                +logoutTime.Hour.ToString("00")+logoutTime.Minute.ToString("00")+logoutTime.Second.ToString("00");
            _record.duration =(int) (logoutTime-_loginTime).TotalSeconds;

            SaveGame();
        }
    }

    void OnApplicationFocus(bool focused) {
        Debug.Log( "OnApplicationFocus:"+focused );

        if(focused==true) {
            if(_record.playerID==-1) {
                urlController.GET( URLRegisterUser );
            }
            else {
                Dictionary<string,string> postData = new Dictionary<string,string>();
                postData.Add( "playerID", _record.playerID.ToString() );
                postData.Add( "loginID", _record.loginID.ToString() );
                postData.Add( "logoutTime", _record.logoutTime );
                postData.Add( "duration", _record.duration.ToString() );

                urlController.POST( URLLogin,  postData );

                Debug.Log( "Send Login:"+_record.playerID.ToString()+"---"+_record.loginID.ToString()+"---"+_record.logoutTime+"---"+ _record.duration.ToString() );

                _record.loginID++;
                SaveGame();
            }

            _loginTime = DateTime.Now;
        }
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
        }
	}

    void FixedUpdate() {

        foreach( CallbackItem item in _callbackList ) {
            item.timer-=Time.fixedDeltaTime;
            if(item.timer<0) {
                item.callback();
                if(item.repeat==true){
                    item.timer=item.delay;
                }
            }
        }

        bool deleteFlag;
        do {
            foreach( CallbackItem item in _callbackList ) {
                if(item.timer<0) {
                    _callbackList.Remove(item);
                    deleteFlag=true;
                    break;
                }
            }
            deleteFlag = false;
        }while(deleteFlag==true);

        Vector3 pos;

        switch(_status ) {
        case Status_NoArtistDelay:
            _timer-=Time.fixedDeltaTime;
            if(_timer<0){
                _timer=NoArtistHideDuration;
                _status=Status_NoArtist;
            }
            break;
        case Status_NoArtist:

            _timer-=Time.fixedDeltaTime;
            if(_timer<0){
                
                _fiveSpeedY=0;
                Gravity = 3200;
                _status=Status_ShowFiveWithBoard;

                pos = RectFiveWithBoard.localPosition;
                pos.y=1280;
                RectFiveWithBoard.localPosition = pos;
            }
          
            float scale = (NoArtistHideDuration-_timer)/NoArtistHideDuration;
            //float rate = CurveNoArtist.Evaluate( scale );

            float targetY=NoArtistHidePosY*CurveNoArtistPosY.Evaluate( scale );

            pos =RectNoArtist.localPosition;
            pos.y = targetY;
            RectNoArtist.localPosition=pos;

            float scaleY=1-0.98f*CurveNoArtistScaleY.Evaluate(scale);
            if(scaleY<0.02f) {
                scaleY = 0.02f;
            }
            float scaleX=((640-NoArtistHideXWide)*(1-CurveNoArtistScaleX.Evaluate(scale))+NoArtistHideXWide)/640.0f;

            RectNoArtist.localScale = new Vector3( scaleX, scaleY, 1);

            if(scale<0.4f) {
                Color sourceColor = new Color( 198/255.0f, 80/255.0f, 80/255.0f, 1.0f );
                Color targetColor = new Color( 80/255.0f, 160/255.0f, 212/255.0f, 1.0f );
                    
                ImgWhiteBar.color = Color.Lerp( sourceColor, targetColor, scale/0.6f );

                sourceColor = ImgNoArtist.color;
                sourceColor.a = 1.0f;
                targetColor = ImgNoArtist.color;
                targetColor.a = 0.0f;

                ImgNoArtist.color = Color.Lerp( sourceColor, targetColor, scale/ 0.6f );
            }
            else {
                ImgWhiteBar.color = new Color( 80/255.0f, 160/255.0f, 212/255.0f, 1.0f );
                ImgNoArtist.color = new Color( 80/255.0f, 160/255.0f, 212/255.0f, 1.0f );
            }

            break;

        case Status_ShowFiveWithBoard:
            _fiveSpeedY+=Gravity*Time.fixedDeltaTime;
            pos = RectFiveWithBoard.localPosition;
            pos.y-=_fiveSpeedY*Time.fixedDeltaTime;

            if(pos.y<320){
                pos.y=320;
                Gravity/=1.5f;
                _fiveSpeedY*=-0.32f;

                if(_secondDropTimer<0) {
                    if(Mathf.Abs(_fiveSpeedY)<600){
                        
                        _secondDropTimer = SecondDropDuration+0.5f;
                        _buttonShowed = false;
                    }
                }

                if((_secondDropTimer>0)&&(_secondDropTimer<0.5f)&&(_buttonShowed==false)) {
                    _buttonShowed=true;
                    DOTween.Play( RectImgButtonBG.DOScale( Vector3.one, 0.2f).SetEase( Ease.OutBack ) );

                    DOTween.Play( RectBtnSounds.DOScale( Vector3.one, 0.25f).SetEase( Ease.OutBack ) );
                    DOTween.Play( RectBtnMoreGames.DOScale( Vector3.one, 0.25f).SetEase( Ease.OutBack ).SetDelay(0.1f) );
                    DOTween.Play( RectBtnLeaderboard.DOScale( Vector3.one, 0.25f).SetEase( Ease.OutBack ).SetDelay(0.2f) );
                    DOTween.Play( RectBtnPlay.DOScale( Vector3.one, 0.25f).SetEase( Ease.OutBack ).SetDelay(0.3f).OnComplete( ()=> {
                        Sequence seq = DOTween.Sequence();
                        seq.Append( RectBtnPlay.DOScale( Vector3.one*1.5f, 1.25f));
                        seq.Append( RectBtnPlay.DOScale( Vector3.one, 1.25f));
                        DOTween.Play( seq );

                        pos = RectEnergy.localPosition;
                        pos.y+=200;
                        DOTween.Play( RectEnergy.DOLocalMoveY( pos.y, 1.0f).SetEase( Ease.OutCubic ) );

                        _btnPlayCallbackIndex = AddCallbackFunction( () => {
                            seq = DOTween.Sequence();
                            seq.Append( RectBtnPlay.DOScale( Vector3.one*1.5f, PlayButtonZoomDuration).SetEase(CurvePlayButtonZoom));
                            seq.Append( RectBtnPlay.DOScale( Vector3.one, PlayButtonZoomDuration).SetEase(CurvePlayButtonZoom));
                            DOTween.Play( seq);

                        }, 2.6f, true );
                        _btnOtherCallbackIndex = AddCallbackFunction( ()=> {
                            if(KWUtility.Random(0,2)==0) {
                                seq = DOTween.Sequence();
                                seq.Append( RectBtnSounds.DOScale( Vector3.one*1.3f, 0.4f));
                                seq.Append( RectBtnSounds.DOScale( Vector3.one, 0.4f));
                                DOTween.Play( seq );

                                Sequence seq1 = DOTween.Sequence();
                                seq1.Append( RectBtnMoreGames.DOScale( Vector3.one*1.3f, 0.4f).SetDelay( 0.15f) );
                                seq1.Append( RectBtnMoreGames.DOScale( Vector3.one, 0.4f));
                                DOTween.Play( seq1 );

                                Sequence seq2 = DOTween.Sequence();
                                seq2.Append( RectBtnLeaderboard.DOScale( Vector3.one*1.3f, 0.4f).SetDelay( 0.3f) );
                                seq2.Append( RectBtnLeaderboard.DOScale( Vector3.one, 0.4f));
                                DOTween.Play( seq2 );
                            }
                            else {
                                seq = DOTween.Sequence();
                                seq.Append( RectBtnLeaderboard.DOScale( Vector3.one*1.3f, 0.4f));
                                seq.Append( RectBtnLeaderboard.DOScale( Vector3.one, 0.4f));
                                DOTween.Play( seq );

                                Sequence seq1 = DOTween.Sequence();
                                seq1.Append( RectBtnMoreGames.DOScale( Vector3.one*1.3f, 0.4f).SetDelay( 0.15f) );
                                seq1.Append( RectBtnMoreGames.DOScale( Vector3.one, 0.4f));
                                DOTween.Play( seq1 );

                                Sequence seq2 = DOTween.Sequence();
                                seq2.Append( RectBtnSounds.DOScale( Vector3.one*1.3f, 0.4f).SetDelay( 0.3f) );
                                seq2.Append( RectBtnSounds.DOScale( Vector3.one, 0.4f));
                                DOTween.Play( seq2 );
                            }
                        }, 6.0f, true ) ;
                    } ) );
                }

            }
            RectFiveWithBoard.localPosition=pos;
            break;
        }

        if(_secondDropTimer>0) {
            bool biggerFlag = _secondDropTimer>SecondDropDuration;
            _secondDropTimer-=Time.fixedDeltaTime;
            if(_secondDropTimer<0){
                _secondDropTimer=0;
                _status = Status_Main;
            }
            if(_secondDropTimer<SecondDropDuration) {
                if(biggerFlag==true){
                    ImgSeconds.gameObject.SetActive( true );
                    ImgWhiteBar.gameObject.SetActive(false);
                    RectNoArtist.gameObject.SetActive(false);
                }
                float angle = CurveSecondsDropAngle.Evaluate(1.0f-_secondDropTimer/SecondDropDuration )*90;
                SetFiveWithBoardAngle( angle );
            }
        }

        if((_currentGameController!=null)&&(_status==Status_Playing)) {
            if(_currentGameController[_nextBoardIndex].status==GameLogic.Status_Playing) {
                ShowGameTime();
            }
        }
    }

    Tween tweenSecond, tweenColon, tweenMiSecond;

    void ShowGameTime() {
        int secondValue = (int)_currentGameTimer;

        _totalGameTime+=Time.fixedDeltaTime;
        _currentGameTimer-=Time.fixedDeltaTime;

        if(secondValue!=(int)_currentGameTimer) {
            if(secondValue<=3) {
                Sequence seq = DOTween.Sequence();
                seq = DOTween.Sequence();
                seq.Append( TxtTimeSecond.rectTransform.DOScale( 1.2f, 0.2f) );
                seq.Append( TxtTimeSecond.rectTransform.DOScale( 1.0f, 0.5f) );
                DOTween.Play( seq );

                seq = DOTween.Sequence();
                seq.Append( TxtTimeColon.rectTransform.DOScale( 1.2f, 0.2f) );
                seq.Append( TxtTimeColon.rectTransform.DOScale( 1.0f, 0.5f) );
                DOTween.Play( seq );

                seq = DOTween.Sequence();
                seq.Append( TxtTimeMiSecond.rectTransform.DOScale( 1.2f, 0.2f) );
                seq.Append( TxtTimeMiSecond.rectTransform.DOScale( 1.0f, 0.5f) );
                DOTween.Play( seq );

                Color color = new Color( 1.0f, 0.5f-0.1f*(6-secondValue), 0.5f-0.1f*(6-secondValue), 1.0f );
                seq = DOTween.Sequence();
                seq.Append( TxtTimeSecond.DOColor(  color, 0.2f) );
                color = new Color( 1.0f, 0.5f-0.1f*(5-secondValue), 0.5f-0.1f*(5-secondValue), 1.0f );
                seq.Append( TxtTimeSecond.DOColor(  color, 0.5f));
                DOTween.Play( seq );
                tweenSecond = seq;

                color = new Color( 1.0f, 0.5f-0.1f*(6-secondValue), 0.5f-0.1f*(6-secondValue), 1.0f );
                seq = DOTween.Sequence();
                seq.Append( TxtTimeColon.DOColor(  color, 0.2f) );
                color = new Color( 1.0f, 0.5f-0.1f*(5-secondValue), 0.5f-0.1f*(5-secondValue), 1.0f );
                seq.Append( TxtTimeColon.DOColor(  color, 0.5f) );
                DOTween.Play( seq );
                tweenColon = seq;

                color = new Color( 1.0f, 0.5f-0.1f*(6-secondValue), 0.5f-0.1f*(6-secondValue), 1.0f );
                seq = DOTween.Sequence();
                seq.Append( TxtTimeMiSecond.DOColor(  color, 0.2f) );
                color = new Color( 1.0f, 0.5f-0.1f*(5-secondValue), 0.5f-0.1f*(5-secondValue), 1.0f );
                seq.Append( TxtTimeMiSecond.DOColor(  color, 0.5f) );
                DOTween.Play( seq );
                tweenMiSecond = seq;

            }
        }

        if(_currentGameTimer<0) {
            _currentGameController[_nextBoardIndex].gameLogic.SetGameTimeout(  );
            _currentGameTimer = 0;
        }

        int second = (int)_currentGameTimer;
        int miSecond = (int)((_currentGameTimer-second)*100);

        if(second<100) {
            TxtTimeSecond.text = second.ToString("00");
        }
        else {
            TxtTimeSecond.text = second.ToString("000");
        }
        TxtTimeMiSecond.text = miSecond.ToString( "00" );
    }


    void HideGameOverInfo() {
        Color color = TxtGameOver.color;
        color.a = 0;
        TxtGameOver.color = color;

        color = TxtGameOverScore.color;
        color.a = 0;
        TxtGameOverScore.color = color;

        color = TxtGameOverScoreValue.color;
        color.a = 0;
        TxtGameOverScoreValue.color = color;
        TxtGameOverScoreValue.text = _score.ToString();

        color = TxtGameOverBestScore.color;
        color.a = 0;
        TxtGameOverBestScore.color = color;

        color = TxtGameOverBestScoreValue.color;
        color.a = 0;
        TxtGameOverBestScoreValue.color = color;
        TxtGameOverBestScoreValue.text = _record.bestScore.ToString();

        color = TxtGameOverScoreValueBest.color;
        color.a = 0;
        TxtGameOverScoreValueBest.color = color;
        TxtGameOverScoreValueBest.gameObject.SetActive( false );

        color = TxtGameOverGames.color;
        color.a = 0;
        TxtGameOverGames.color = color;

        color = TxtGameOverGamesValue.color;
        color.a = 0;
        TxtGameOverGamesValue.color = color;
        TxtGameOverGamesValue.text = _rightGameNumber.ToString();

        color = TxtGameOverGamesValueBest.color;
        color.a = 0;
        TxtGameOverGamesValueBest.color = color;

        color = TxtGameOverBestGames.color;
        color.a = 0;
        TxtGameOverBestGames.color = color;

        color = TxtGameOverBestGamesValue.color;
        color.a = 0;
        TxtGameOverBestGamesValue.color = color;
        TxtGameOverBestGamesValue.text = _record.bestGameNumber.ToString();

        color = TxtGameOverGamesValueBest.color;
        color.a = 0;
        TxtGameOverGamesValueBest.color = color;
        TxtGameOverGamesValueBest.gameObject.SetActive( false );

        color = TxtBtnGameOverRetry.color;
        color.a = 0;
        TxtBtnGameOverRetry.color = color;

        color = TxtBtnGameOverHome.color;
        color.a = 0;
        TxtBtnGameOverHome.color = color;

        color = ImgBtnGameOverRetry.color;
        color.a = 0;
        ImgBtnGameOverRetry.color = color;

        color = ImgBtnGameOverHome.color;
        color.a = 0;
        ImgBtnGameOverHome.color = color;
    }

    void StartGame() {
        _status = Status_Playing;

        _nextGameIndex = 0;
        _nextBoardIndex = 0;

        _gameSeed = KWUtility.Random( 0, 1000000 );
        _levelResult = "";

        TxtTimeColon.color = Color.white;
        TxtTimeSecond.color = Color.white;
        TxtTimeMiSecond.color = Color.white;


        CreateNextGame();

        Vector3 pos = RectTopBar.localPosition;
        pos.y-=160;

        DOTween.Play( RectTopBar.DOLocalMoveY(pos.y, 0.35f ).SetEase( Ease.OutCubic ).SetDelay( 0.25f )  );

        MoveGameIn( 0 );

        _gameTime = 10.0f;
        _totalGameTime = 0;

        _score = 0;
        TxtScore.text = _score.ToString();

        _lives = 5;

        _gameResult = "";

        _rightGameNumber = 0;
        _wrongGameNumber = 0;

        TxtGameRightNumber.text = _rightGameNumber.ToString();

        if(_showAd==true) {
            _bannerView.Show();
        }

        _record.gamePlayIndex++;
        SaveGame();

        TxtTimeSecond.color = new Color( 242/255.0f, 232/255.0f, 232/255.0f, 1.0f );
        TxtTimeSecond.text = "05";
        TxtTimeColon.color = new Color( 242/255.0f, 232/255.0f, 232/255.0f, 1.0f );
        TxtTimeMiSecond.color = new Color( 242/255.0f, 232/255.0f, 232/255.0f, 1.0f );
        TxtTimeMiSecond.text = "00";

        for( int m=0; m<5; m++ ) {
            ImgLives[m].color = new Color( 72/255.0f, 232/255.0f, 32/255.0f, 1.0f );
        }
    }

    void SetFiveWithBoardAngle( float angle ) {
        float deltaX, deltaY;

        angle=angle*3.1415927f/180;

        float imageWidth, imageHeight;
        imageWidth= ImgSeconds.rectTransform.rect.width;
        imageHeight= ImgSeconds.rectTransform.rect.height/2;

        deltaY = imageHeight-imageHeight*Mathf.Sin( angle );
        deltaX = FiveWithBoardFarDelta*Mathf.Cos( angle );

        if(angle<=90) {
            
        }
        else {
            
        }

        ImgSeconds.cornerOffsetBL = new Vector3( deltaX, deltaY, 0 );
        ImgSeconds.cornerOffsetBR = new Vector3( -1*deltaX, deltaY, 0 );
    }

    void CreateNextGame() {
        Debug.Log( "Create next game!!"+_nextGameIndex );

        if(_nextGameIndex%MaxGameNumber==0) {
            for(int m=0;m<MaxGameNumber;m++ ) {
                _gamePlayed[m]=false;
            }
        }

        KWUtility.SetRandomSeed( _gameSeed );

        for( int m=0; m<_nextGameIndex;m++ ) {
            KWUtility.Random( 0, 10000 );
        }

        int gameID;
        do {
            gameID = KWUtility.Random( 0, MaxGameNumber );
        } while( _gamePlayed[gameID]==true );

        Debug.Log( "Game ID:"+gameID+"---"+_gamePlayed[gameID] );

        _gamePlayed[gameID] = true;


        //int gameID = _nextGameIndex%MaxGameNumber;

        GameLogic gameLogic = GameLogic.GetGameLogic( gameID, _nextGameIndex/4, KWUtility.Random( 0, 10000 ) );
        _currentGameController[_nextBoardIndex].SetGameLogic( gameLogic );

        _nextGameIndex++;

        _currentGameTimer = 5.0f;
    }

    void MoveGameOut( int boardIndex ) {
        
        DOTween.Play( _currentGameRect[boardIndex].DOScale( Vector3.one*0.2f, 0.85f ).SetEase( CurveBoardMoveZoomOut )  );
        DOTween.Play( _currentGameRect[boardIndex].DOLocalMoveY(  -1*ScreenHeight*5/12, 0.85f ).SetEase( CurveBoardMoveYOut ) );
        DOTween.Play( _currentGameRect[boardIndex].DOLocalMoveX( -2.0f/3*ScreenWidth, 0.85f ).SetEase( CurveBoardMoveXOut ).OnComplete( () => {
            _currentGameController[boardIndex].Clear();
        } ) );
    }

    void MoveGameIn( int boardIndex ) {
        _currentGameController[boardIndex].gameObject.SetActive( true );

        _currentGameRect[boardIndex].localScale = Vector3.one*0.2f;
        _currentGameRect[boardIndex].localPosition = new Vector3( ScreenWidth*2.0f/3, ScreenHeight/3, 0 );

        DOTween.Play( _currentGameRect[boardIndex].DOLocalMoveX(  0, 0.65f ).SetEase( CurveBoardMoveX ).SetDelay( 0.85f ));
        DOTween.Play( _currentGameRect[boardIndex].DOLocalMoveY(  0, 0.65f ).SetEase( CurveBoardMoveY ).SetDelay( 0.85f ) );
        DOTween.Play( _currentGameRect[boardIndex].DOScale( Vector3.one*1.1f, 1.0f ).SetEase( CurveBoardMoveZoom ).SetDelay( 0.85f ).OnComplete( ()=> {
            _currentGameController[boardIndex].StartGame();
        } ) );


    }

    public void SendGameResult(bool isWin) {


        int duration = (int)(_currentGameTimer*1000);
        int score=0;
        int success=0;

        if(isWin) {
            _gameTime+=5;

          
            TxtTimeMiSecond.color = Color.white;
            TxtTimeSecond.DOKill(); 
            TxtTimeSecond.color = Color.white;
            TxtTimeColon.DOKill(); 
            TxtTimeColon.color = Color.white;

            if(_currentGameTimer>0) {
                float baseScore=20;
                int difficult = (_nextGameIndex-1)/5+1;
                for(int m=0;m<difficult;m++){
                    baseScore*=1.5f;
                }
                int baseScoreInt = ((int)(baseScore/10))*10;
                score = (int)(baseScoreInt*_currentGameTimer)*5;
                _score+=score;
                TxtScore.text = _score.ToString();
            }



            _rightGameNumber++;
            TxtGameRightNumber.text = _rightGameNumber.ToString();

            success = 1;
        }
        else {
            _gameTime-=5;


            score = 0;

            //_wrongGameNumber++;
           // TxtGameWrongNumber.text = _wrongGameNumber.ToString();

            _lives--;

            Color color = new Color( 72/255.0f, 232/255.0f, 32/255.0f, 1.0f );
            color = new Color( 218/255.0f, 32/255.0f, 32/255.0f, 1.0f );

            DOTween.Play( ImgLives[_lives].DOColor( color, 0.35f ) );

            Sequence seq = DOTween.Sequence();
            seq.Append( ImgLives[_lives].rectTransform.DOScale( Vector3.one*2, 0.25f ) );
            seq.Append( ImgLives[_lives].rectTransform.DOScale( Vector3.one, 0.25f ) );
            DOTween.Play( seq );

            success = 0;
        }

        TxtTimeSecond.color = Color.white;
        TxtTimeMiSecond.color = Color.white;
        TxtTimeColon.color = Color.white;

        GameLogic gameLogic = _currentGameController[_nextBoardIndex].gameLogic;

        _gameResult += gameLogic.gameID.ToString()+"I";
        _gameResult += gameLogic.difficulty.ToString()+"I";
        _gameResult += duration.ToString()+"I";
        _gameResult += score+"I";
        _gameResult += success+"I";
        _gameResult += gameLogic.seed.ToString()+"L";

        if(_lives==0) {
            PlaySound( Sound_GameOver );
            _status = Status_GameOver;

            // Show game over

            Dictionary<string,string> postData = new Dictionary<string,string>();
            postData.Add( "playerID", _record.playerID.ToString() );
            postData.Add( "gameplayid", _record.gamePlayIndex.ToString() );
            postData.Add( "duration", ((int)_totalGameTime*1000).ToString() );
            postData.Add( "levelplayed", _rightGameNumber.ToString() );
            postData.Add( "seed", _gameSeed.ToString() );
            postData.Add( "score", _score.ToString() );
            postData.Add( "levelData", _gameResult );

            urlController.POST( URLPlayGame,  postData );

            _bannerView.Hide();

            Vector3 pos = RectTopBar.localPosition;
            pos.y+=160;
            DOTween.Play( RectTopBar.DOLocalMoveY(pos.y, 0.35f ).SetEase( Ease.OutCubic ).SetDelay( 0.25f )  );

            RectGameOver.gameObject.SetActive( true );

            ImgGameOverBG.color = new Color( 242/255.0f, 242/255.0f, 218/255.0f, 1.0f );

            TxtGameOverTimeup.color = new Color( 208/255.0f, 64/255.0f, 64/255.0f, 1.0f );
            TxtGameOverTimeup.rectTransform.localPosition = Vector3.zero;
            TxtGameOverTimeup.rectTransform.localScale = new Vector3( 1, 3, 1 );
            TxtGameOverTimeup.text = "GAMEOVER";
            RectGameOver.localScale = new Vector3( 1, 0.333333f, 1 );
            RectGameOver.localPosition = new Vector3( ScreenWidth, 0, 0 );

            HideGameOverInfo();

            MoveGameOut( _nextBoardIndex );

            DOTween.Play( RectGameOver.DOLocalMoveX( 0, 0.35f ).SetEase( Ease.OutBack ).SetDelay( 0.45f ).OnComplete( ()=> {
                ShowGameOver();
            } ) );
        }
        else {
            _currentGameTimer = 5.0f;
            tweenSecond.Kill( true );
            tweenColon.Kill( true );
            tweenMiSecond.Kill( true );


            TxtTimeSecond.color = new Color( 242/255.0f, 232/255.0f, 232/255.0f, 1.0f );
            TxtTimeSecond.text = "05";
            TxtTimeColon.color = new Color( 242/255.0f, 232/255.0f, 232/255.0f, 1.0f );
            TxtTimeMiSecond.color = new Color( 242/255.0f, 232/255.0f, 232/255.0f, 1.0f );
            TxtTimeMiSecond.text = "00";
        }

    }

    public void ExecGameResult(  ) {
        if(_status==Status_GameOver) {
            return;
        }

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

    public void OnButtonPlay() {
        Vector3 pos = RectBtnPlay.localPosition;
        DOTween.Play( RectImgButtonBG.DOScaleY(0, 0.1f));
        DOTween.Play( RectBtnPlay.DOLocalMoveY( pos.y-ScreenHeight/5, 0.15f).SetEase( Ease.InCubic ) );
        DOTween.Play( RectBtnLeaderboard.DOLocalMoveY( pos.y-ScreenHeight/5, 0.15f).SetEase( Ease.InCubic ).SetDelay( 0.05f) );
        DOTween.Play( RectBtnMoreGames.DOLocalMoveY( pos.y-ScreenHeight/5, 0.15f).SetEase( Ease.InCubic ).SetDelay( 0.1f) );
        DOTween.Play( RectBtnSounds.DOLocalMoveY( pos.y-ScreenHeight/5, 0.15f).SetEase( Ease.InCubic ).SetDelay( 0.15f).OnComplete( ()=> {
            KillRepeatCallback( _btnPlayCallbackIndex );
            KillRepeatCallback( _btnOtherCallbackIndex );

            Show321Go();
        } ) );

        pos = RectFiveWithBoard.localPosition;
        DOTween.Play( RectFiveWithBoard.DOLocalMoveY( pos.y+ScreenHeight/1.5f, 0.25f).SetEase( Ease.InCubic ) );

        pos = ImgSeconds.rectTransform.localPosition;
        DOTween.Play( ImgSeconds.rectTransform.DOLocalMoveY( pos.y+ScreenHeight/1.5f, 0.25f).SetEase( Ease.InCubic ) );

    }

    void Show321Go() {
        Img321Go.gameObject.SetActive( true );

        Img321Go.rectTransform.localPosition = Vector3.zero;
        Img321Go.rectTransform.localEulerAngles = new Vector3( 0, 0, -90 );
        Img321Go.sprite = Spt321GoHand3;
        Color color = Img321Go.color;
        color.a = 0;
        Img321Go.color = color;

        color.a = 1;
        DOTween.Play( Img321Go.DOColor( color, 0.2f ).SetDelay(0.4f) );
        Sequence seq = DOTween.Sequence();
        seq.Append( Img321Go.rectTransform.DORotate(Vector3.zero, 0.25f).SetEase( Ease.OutBack ).SetDelay(0.4f).OnComplete( ()=> {
            color.a = 0;
            DOTween.Play( Img321Go.DOColor( color, 0.15f ).SetDelay(0.5f) );
        } ) );
        seq.Append( Img321Go.rectTransform.DORotate(new Vector3(0,0,90), 0.25f).SetEase( Ease.OutBack ).SetDelay(0.5f) );
        seq.Append( Img321Go.rectTransform.DORotate(new Vector3(0,0,-90), 0.01f).OnComplete( ()=> {
            Img321Go.sprite = Spt321GoHand2;
            color.a =1;
            DOTween.Play( Img321Go.DOColor( color, 0.2f ) );
        } ));
        seq.Append( Img321Go.rectTransform.DORotate(Vector3.zero, 0.25f).SetEase( Ease.OutBack ).OnComplete( ()=> {
            color.a = 0;
            DOTween.Play( Img321Go.DOColor( color, 0.15f ).SetDelay(0.5f) );
        } ) );
        seq.Append( Img321Go.rectTransform.DORotate(new Vector3(0,0,90), 0.25f).SetEase( Ease.OutBack ).SetDelay(0.5f) );
        seq.Append( Img321Go.rectTransform.DORotate(new Vector3(0,0,-90), 0.01f).OnComplete( ()=> {
            Img321Go.sprite = Spt321GoHand1;
            color.a =1;
            DOTween.Play( Img321Go.DOColor( color, 0.2f ) );
        } ));
        seq.Append( Img321Go.rectTransform.DORotate(Vector3.zero, 0.25f).SetEase( Ease.OutBack ).OnComplete( ()=> {
            color.a = 0;
            DOTween.Play( Img321Go.DOColor( color, 0.15f ).SetDelay(0.5f) );
        } ) );
        seq.Append( Img321Go.rectTransform.DORotate(new Vector3(0,0,90), 0.25f).SetEase( Ease.OutBack ).SetDelay(0.5f) );
        seq.Append( Img321Go.rectTransform.DORotate(new Vector3(0,0,-90), 0.01f).OnComplete( ()=> {
            Img321Go.sprite = Spt321GoHandGo;
            color.a =1;
            DOTween.Play( Img321Go.DOColor( color, 0.2f ) );
        } ));
        seq.Append( Img321Go.rectTransform.DORotate(Vector3.zero, 0.5f).SetEase( Ease.OutBack ).OnComplete( ()=> {
            DOTween.Play( Img321Go.rectTransform.DOLocalMoveX( -1*ScreenWidth*3/5, 0.5f ).SetEase( Ease.InCubic).OnComplete( ()=> {
                StartGame();
            } ) );
        } ) );

        DOTween.Play( seq );
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

            Sequence seq = DOTween.Sequence();
            seq.Append( ImgBtnSound.rectTransform.DOScale( Vector3.one*0.1f, 0.2f).OnComplete( ()=>{
                ImgBtnSound.sprite = SptSoundOff;
            } ) );
            seq.Append( ImgBtnSound.rectTransform.DOScale( Vector3.one, 0.2f).SetEase( Ease.OutBack) );
            DOTween.Play( seq );
                
        }
        else {
            _record.soundFlag=true;

            Sequence seq = DOTween.Sequence();
            seq.Append( ImgBtnSound.rectTransform.DOScale( Vector3.one*0.1f, 0.2f).OnComplete( ()=>{
                ImgBtnSound.sprite = SptSoundOn;
            } ) );
            seq.Append( ImgBtnSound.rectTransform.DOScale( Vector3.one, 0.2f).SetEase( Ease.OutBack) );
            DOTween.Play( seq );

            SndMusic.Play();
        }
        SaveGame();
    }

    void ShowGameOver() {
        DOTween.Play( RectGameOver.DOScaleY( 1.0f, 0.3f).SetDelay( 1.0f) );

        DOTween.Play( TxtGameOverTimeup.rectTransform.DOScaleY( 0.333333f, 0.3f).SetDelay( 1.0f) );
        DOTween.Play( TxtGameOverTimeup.rectTransform.DOLocalMoveY( 360, 0.3f).SetDelay( 1.0f) );
        Color color = new Color( 208/255.0f, 64/255.0f, 64/255.0f, 1.0f );
        color.a = 0;
        DOTween.Play( TxtGameOverTimeup.DOColor( color, 0.3f).SetDelay( 1.0f).OnComplete( ()=> {
            color = TxtGameOver.color;
            color.a = 1;
            DOTween.Play( TxtGameOver.DOColor( color, 0.15f).SetDelay(0.3f) );

            color = TxtGameOverScore.color;
            color.a = 1;
            DOTween.Play( TxtGameOverScore.DOColor( color, 0.15f).SetDelay(0.35f) );

            color = TxtGameOverScoreValue.color;
            color.a = 1;
            DOTween.Play( TxtGameOverScoreValue.DOColor( color, 0.15f).SetDelay(0.35f) );

            color = TxtGameOverBestScore.color;
            color.a = 1;
            DOTween.Play( TxtGameOverBestScore.DOColor( color, 0.15f).SetDelay(0.4f) );

            color = TxtGameOverBestScoreValue.color;
            color.a = 1;
            DOTween.Play( TxtGameOverBestScoreValue.DOColor( color, 0.15f).SetDelay(0.4f) );

            color = TxtGameOverGames.color;
            color.a = 1;
            DOTween.Play( TxtGameOverGames.DOColor( color, 0.15f).SetDelay(0.45f) );

            color = TxtGameOverGamesValue.color;
            color.a = 1;
            DOTween.Play( TxtGameOverGamesValue.DOColor( color, 0.15f).SetDelay(0.45f) );

            color = TxtGameOverBestGames.color;
            color.a = 1;
            DOTween.Play( TxtGameOverBestGames.DOColor( color, 0.15f).SetDelay(0.5f) );

            color = TxtGameOverBestGamesValue.color;
            color.a = 1;
            DOTween.Play( TxtGameOverBestGamesValue.DOColor( color, 0.15f).SetDelay(0.5f) );

            color = TxtBtnGameOverRetry.color;
            color.a = 1;
            DOTween.Play( TxtBtnGameOverRetry.DOColor( color, 0.15f).SetDelay(0.55f) );

            color = TxtBtnGameOverHome.color;
            color.a = 1;
            DOTween.Play( TxtBtnGameOverHome.DOColor( color, 0.15f).SetDelay(0.55f) );

            color = ImgBtnGameOverRetry.color;
            color.a = 1;
            DOTween.Play( ImgBtnGameOverRetry.DOColor( color, 0.15f).SetDelay(0.55f) );

            color = ImgBtnGameOverHome.color;
            color.a = 1;
            DOTween.Play( ImgBtnGameOverHome.DOColor( color, 0.15f).SetDelay(0.55f).OnComplete( () => {
                int addEnergy = 0;
                if(_score>_record.bestScore) {
                //if(true) {
                    TxtGameOverScoreValueBest.gameObject.SetActive( true );
                    color = TxtGameOverScoreValueBest.color;
                    color.a = 0.2f;
                    TxtGameOverScoreValueBest.color = color;
                    color.a = 1.0f;
                    DOTween.Play( TxtGameOverScoreValueBest.DOColor( color,0.75f));
                    _record.bestScore = _score;

                    Social.ReportScore (_record.bestScore, "com.kylinworks.games5seconds.lb.bestscore", success => {
                        Debug.Log(success ? "Reported best score successfully" : "Failed to report score");
                    });

                    addEnergy+=3;
                }

                if(_rightGameNumber>_record.bestGameNumber) {
                //ßif(true) {
                    TxtGameOverGamesValueBest.gameObject.SetActive( true );
                    color = TxtGameOverGamesValueBest.color;
                    color.a = 0.2f;
                    TxtGameOverGamesValueBest.color = color;
                    color.a = 1.0f;
                    DOTween.Play( TxtGameOverGamesValueBest.DOColor( color,0.75f) );
                    _record.bestGameNumber = _rightGameNumber;

                    addEnergy+=3;
                }

                if(addEnergy!=0) {
                    SaveGame();
                }
            } ) );
        } ) );
    }

    public void OnButtonGameOverRetry() {
        _gamePlayedCounter++;
        //if(true) {
        if((_gamePlayedCounter%5==0)&&(HZIncentivizedAd.IsAvailable()==true)) {
            _gamePlayedCounter=0;

            HideGameOverInfoAnimation();

            DOTween.Play( RectGameOver.DOScaleY( 0.33333f, 0.5f ).SetDelay( 0.1f ).OnComplete(()=> {
                TxtGameOverTimeup.rectTransform.localScale = new Vector3( 1, 3, 1 );
                TxtGameOverTimeup.rectTransform.localPosition = Vector3.zero;

                TxtGameOverTimeup.text = "HAVE A REST";

                Color color = TxtGameOverTimeup.color;
                color.a = 1.0f;
                DOTween.Play( TxtGameOverTimeup.DOColor( color, 0.4f).OnComplete( ()=> {
                    AddCallbackFunction( ()=> {
                        HZIncentivizedAd.Show();


                    }, 1.0f, false );
                } ) );
            } ) );
              
        }
        else {
            DOTween.Play( RectGameOver.DOLocalMoveX( -1*ScreenWidth, 0.35f ).SetEase( Ease.InCubic ).OnComplete( ()=> {
                //StartGame();
                Show321Go();
            } ) );
        }
    }

    public void OnButtonGameOverHome() {

        HideGameOverInfoAnimation();

        Vector3 pos = ImgSeconds.rectTransform.localPosition;
        pos.y -= ScreenHeight/1.5f;
        ImgSeconds.rectTransform.localPosition = pos;
        ImgSeconds.gameObject.SetActive( false );

        pos = RectBtnPlay.localPosition;
        pos.y+=ScreenHeight/5;
        RectBtnPlay.localPosition = pos;
        RectBtnPlay.localScale = Vector3.zero;

        pos = RectBtnSounds.localPosition;
        pos.y+=ScreenHeight/5;
        RectBtnSounds.localPosition = pos;
        RectBtnSounds.localScale = Vector3.zero;

        pos = RectBtnLeaderboard.localPosition;
        pos.y+=ScreenHeight/5;
        RectBtnLeaderboard.localPosition = pos;
        RectBtnLeaderboard.localScale = Vector3.zero;

        pos = RectBtnMoreGames.localPosition;
        pos.y+=ScreenHeight/5;
        RectBtnMoreGames.localPosition = pos;
        RectBtnMoreGames.localScale = Vector3.zero;

        DOTween.Play( RectGameOver.DOScaleY( 0.003f, GameOverHideDuration/3 )/*.SetEase( CurveNoArtistScaleY )*/ );
        DOTween.Play( RectGameOver.DOScaleX( 0.6f, GameOverHideDuration ).SetEase( CurveNoArtistScaleX ) );
        Color color = new Color( 80/255.0f, 160/255.0f, 212/255.0f, 1.0f);
        DOTween.Play( ImgGameOverBG.DOColor( color, GameOverHideDuration ) );
        DOTween.Play( RectGameOver.DOLocalMoveY( NoArtistHidePosY, GameOverHideDuration ).SetEase( CurveNoArtistPosY ).OnComplete( ()=> {
            RectGameOver.gameObject.SetActive( false );
            ImgWhiteBar.gameObject.SetActive(true);
            RectNoArtist.gameObject.SetActive(true);

            _fiveSpeedY=0;
            Gravity = 3200;
            _status=Status_ShowFiveWithBoard;
            pos = RectFiveWithBoard.localPosition;
            pos.y=1280;
            RectFiveWithBoard.localPosition = pos;
            _secondDropTimer = -1;
        } ) );

    }

    void HideGameOverInfoAnimation() {
        Color color = TxtGameOver.color;
        color.a = 0;
        DOTween.Play( TxtGameOver.DOColor( color, 0.15f));

        color = TxtGameOverScore.color;
        color.a = 0;
        DOTween.Play( TxtGameOverScore.DOColor( color, 0.15f) );

        color = TxtGameOverScoreValue.color;
        color.a = 0;
        DOTween.Play( TxtGameOverScoreValue.DOColor( color, 0.15f) );

        color = TxtGameOverScoreValueBest.color;
        color.a = 0;
        DOTween.Play( TxtGameOverScoreValueBest.DOColor( color, 0.15f) );

        color = TxtGameOverBestScore.color;
        color.a = 0;
        DOTween.Play( TxtGameOverBestScore.DOColor( color, 0.15f) );

        color = TxtGameOverBestScoreValue.color;
        color.a = 0;
        DOTween.Play( TxtGameOverBestScoreValue.DOColor( color, 0.15f) );

        color = TxtGameOverScoreValueBest.color;
        color.a = 0;
        DOTween.Play( TxtGameOverScoreValueBest.DOColor( color, 0.15f) );


        color = TxtGameOverGames.color;
        color.a = 0;
        DOTween.Play( TxtGameOverGames.DOColor( color, 0.15f));

        color = TxtGameOverGamesValue.color;
        color.a = 0;
        DOTween.Play( TxtGameOverGamesValue.DOColor( color, 0.15f) );

        color = TxtGameOverGamesValueBest.color;
        color.a = 0;
        DOTween.Play( TxtGameOverGamesValueBest.DOColor( color, 0.15f) );

        color = TxtGameOverBestGames.color;
        color.a = 0;
        DOTween.Play( TxtGameOverBestGames.DOColor( color, 0.15f));

        color = TxtGameOverBestGamesValue.color;
        color.a = 0;
        DOTween.Play( TxtGameOverBestGamesValue.DOColor( color, 0.15f) );

        color = TxtGameOverGamesValueBest.color;
        color.a = 0;
        DOTween.Play( TxtGameOverGamesValueBest.DOColor( color, 0.15f) );

        color = TxtBtnGameOverRetry.color;
        color.a = 0;
        DOTween.Play( TxtBtnGameOverRetry.DOColor( color, 0.15f) );

        color = TxtBtnGameOverHome.color;
        color.a = 0;
        DOTween.Play( TxtBtnGameOverHome.DOColor( color, 0.15f));

        color = ImgBtnGameOverRetry.color;
        color.a = 0;
        DOTween.Play( ImgBtnGameOverRetry.DOColor( color, 0.15f) );

        color = ImgBtnGameOverHome.color;
        color.a = 0;
        DOTween.Play( ImgBtnGameOverHome.DOColor( color, 0.15f));


    }

    public void OnButtonEnergy() {

        ShowEnergyPanel( "ENERGY" );

    }

    Tween videoZoomTween;

    void ShowEnergyPanel( string title ) {
        Vector3 pos = RectBtnMoreGames.localPosition;
        pos.y+=200;
        pos.x = ScreenWidth/-2+48;

        DOTween.Play(  RectBtnMoreGames.DOLocalMove( pos, 0.75f ).SetEase( Ease.OutCubic ) );

        pos.x = ScreenWidth/2-48;
        DOTween.Play(  RectBtnLeaderboard.DOLocalMove( pos, 0.75f ).SetEase( Ease.OutCubic ) );

        pos = RectBtnPlay.localPosition;
        DOTween.Play(  RectBtnPlay.DOLocalMoveX( ScreenWidth/2-48, 0.6f ).SetEase( Ease.InOutCubic ) );
        DOTween.Play(  RectBtnSounds.DOLocalMoveX( ScreenWidth/-2+48, 0.6f ).SetEase( Ease.InOutCubic ) );

        Color color = ImgBtnPlay.color;
        color.a = 0.3f;
        DOTween.Play( ImgBtnPlay.DOColor( color, 0.5f ).SetDelay( 0.8f ) );

        color = ImgBtnLeaderboard.color;
        color.a = 0.3f;
        DOTween.Play( ImgBtnLeaderboard.DOColor( color, 0.5f ).SetDelay( 0.8f ) );

        color = ImgBtnMoreGames.color;
        color.a = 0.3f;
        DOTween.Play( ImgBtnMoreGames.DOColor( color, 0.5f ).SetDelay( 0.8f ) );

        color = ImgBtnSound.color;
        color.a = 0.3f;
        DOTween.Play( ImgBtnSound.DOColor( color, 0.5f ).SetDelay( 0.8f ) );


        DOTween.Play( RectEnergy.DOLocalMoveY( ScreenHeight/-2+360, 0.85f).SetEase( Ease.OutBack ) );

        DOTween.Play( ImgEnergyBG.rectTransform.DOScale( new Vector3( 2.4f, 2.4f, 1 ), 0.9f ).SetEase( Ease.OutBack ) );

        BtnEnergy.interactable = false;

        KillRepeatCallback( _btnPlayCallbackIndex );
        KillRepeatCallback( _btnOtherCallbackIndex );

        ImgTouchCover.gameObject.SetActive( true );

        DOTween.Play( ImgEnergyIcon.rectTransform.DOLocalMoveX( -136, 0.1f ) );
        DOTween.Play( TxtEnergyNumber.rectTransform.DOLocalMoveX( -72, 0.1f ) );

        color = ImgEnergyIcon.color;
        color.a = 0;
        ImgEnergyIcon.color = color;
        color.a = 1;
        DOTween.Play( ImgEnergyIcon.DOColor( color, 0.5f ).SetDelay( 0.8f ) );

        color = TxtEnergyNumber.color;
        color.a = 0;
        TxtEnergyNumber.color = color;
        color.a = 1;
        DOTween.Play( TxtEnergyNumber.DOColor( color, 0.5f ).SetDelay( 0.8f ) );

        TxtEnergyTitle.gameObject.SetActive( true );
        TxtEnergyTitle.text = title;
        color = TxtEnergyTitle.color;
        color.a = 0;
        TxtEnergyTitle.color = color;
        color.a = 1;
        DOTween.Play( TxtEnergyTitle.DOColor( color, 0.5f ).SetDelay( 0.6f) );

        TxtEnergyTimeSecond.gameObject.SetActive( true );
        color = TxtEnergyTimeSecond.color;
        color.a = 0;
        TxtEnergyTimeSecond.color = color;
        color.a = 1;
        DOTween.Play( TxtEnergyTimeSecond.DOColor( color, 0.5f ).SetDelay( 0.8f ) );

        TxtEnergyTimeColon.gameObject.SetActive( true );
        color = TxtEnergyTimeColon.color;
        color.a = 0;
        TxtEnergyTimeColon.color = color;
        color.a = 1;
        DOTween.Play( TxtEnergyTimeColon.DOColor( color, 0.5f ).SetDelay( 0.8f ) );

        TxtEnergyTimeMiSecond.gameObject.SetActive( true );
        color = TxtEnergyTimeMiSecond.color;
        color.a = 0;
        TxtEnergyTimeMiSecond.color = color;
        color.a = 1;
        DOTween.Play( TxtEnergyTimeMiSecond.DOColor( color, 0.5f ).SetDelay( 0.8f ) );

        BtnEnergyAwardVideo.gameObject.SetActive( true );

        if(HZIncentivizedAd.IsAvailable() ) {
            BtnEnergy.interactable = true;
            color = ImgEnergyVideo.color;
            color.a = 0;
            ImgEnergyVideo.color = color;
            color.a = 1;
            DOTween.Play( ImgEnergyVideo.DOColor( color, 0.5f ).SetDelay( 1.0f ) );

            ImgEnergyVideo.rectTransform.localScale = Vector3.one*0.9f;
            videoZoomTween=ImgEnergyVideo.rectTransform.DOScale( Vector3.one,0.5f).SetLoops(-1, LoopType.Yoyo );
            DOTween.Play( videoZoomTween );

            color = TxtBtnEnergyVideo.color;
            TxtBtnEnergyVideo.text = "FREE FOR 5";
            color.a = 0;
            TxtBtnEnergyVideo.color = color;
            color.a = 1;
            DOTween.Play( TxtBtnEnergyVideo.DOColor( color, 0.5f ).SetDelay( 1.0f ) );

            color = ImgFreeVideoEnergy.color;
            color.a = 0;
            ImgFreeVideoEnergy.color = color;
            color.a = 1;
            DOTween.Play( ImgFreeVideoEnergy.DOColor( color, 0.5f ).SetDelay( 1.0f ) );


            color = ImgFreeVideoEnergy.color;
            color.a = 0;
            ImgFreeVideoEnergy.color = color;
            color.a = 1;
            DOTween.Play( ImgFreeVideoEnergy.DOColor( color, 0.5f ).SetDelay( 1.0f ) );


            color = ImgFreeVideoBG.color;
            color.a = 0;
            ImgFreeVideoBG.color = color;
            color.a=0.08f;
            DOTween.Play( ImgFreeVideoBG.DOColor( color, 0.5f ).SetDelay( 1.0f ) );
        }
        else {
            BtnEnergy.interactable = false;
            color = ImgEnergyVideo.color;
            color.a = 0;
            ImgEnergyVideo.color = color;
            color.a = 1;
            DOTween.Play( ImgEnergyVideo.DOColor( color, 0.5f ).SetDelay( 1.0f ) );

            ImgEnergyVideo.rectTransform.localScale = Vector3.one*0.9f;
            videoZoomTween=ImgEnergyVideo.rectTransform.DOScale( Vector3.one,0.5f).SetLoops( -1, LoopType.Yoyo );
            DOTween.Play( videoZoomTween );

            color = TxtBtnEnergyVideo.color;
            TxtBtnEnergyVideo.text = "NO VIDEO NOW";
            color.a = 0;
            TxtBtnEnergyVideo.color = color;
            color.a = 0.3f;
            DOTween.Play( TxtBtnEnergyVideo.DOColor( color, 0.5f ).SetDelay( 1.0f ) );


            color = ImgEnergyVideo.color;
            color.a = 0;
            ImgEnergyVideo.color = color;
            color.a = 0.3f;
            DOTween.Play( ImgEnergyVideo.DOColor( color, 0.5f ).SetDelay( 1.0f ) );

            color = ImgFreeVideoEnergy.color;
            color.a = 0;
            ImgFreeVideoEnergy.color = color;
           
            color = ImgFreeVideoBG.color;
            color.a = 0;
            ImgFreeVideoBG.color = color;

        }

        BtnEnergyClose.gameObject.SetActive( true );
        BtnEnergy.transform.localScale = Vector3.zero;

        BtnEnergy.transform.localEulerAngles = Vector3.zero;

        DOTween.Play( BtnEnergyClose.transform.DORotate( new Vector3( 0, 0, 270), 0.8f).SetDelay( 0.7f ) );
        DOTween.Play( BtnEnergyClose.transform.DOScale( Vector3.one, 0.5f ).SetEase( Ease.OutBack ).SetDelay( 0.7f ) );
    }
        

    public void OnButtonEnergyClose() {
            
    }

    public void OnButtonEnergyAwardVideo() {
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
        //_adBannerLoaded = true;
        _bannerView.Hide();
    }

    public void MainWWWCallbackFunction( WWW www ) {

        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!: " + www.text+ "------"+www.url);
            if(www.url==URLRegisterUser) {
                Debug.Log( "WWW Callback:"+www.text );
                int playerID = Int32.Parse( www.text );
                _record.playerID = playerID;
                SaveGame();
            }
            else if(www.url==URLUpdateGameCenterID) {
                _record.gamecenterIDReported = true;
                SaveGame();
            }
            else if(www.url==URLLogin) {
                _record.gamecenterIDReported = true;
                SaveGame();
            }
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }

    }

    /*
    Social.ReportScore (_record.bestScore, "com.kylinworks.swipeduel.lb.bestscore", success => {
        Debug.Log(success ? "Reported score successfully" : "Failed to report score");
    });*/
}
