using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using DG.Tweening;

public class GameController : MonoBehaviour {

    [Header("----------这里是预定义的游戏对象----------")]
    public Button goGameButton;
    public GameObject goBoardArea;
    public GameObject goBoardButtonArea;

    public Image goBoardImage;
    public Text goBoardChar;
    public Button goBoardButton;

    [Header("----------这里是UI对象----------")]
    public Text TxtGameDesc1;
    public Text TxtGameDesc2;
    public Text TxtGameMainText;
    public Image ImgBackground;
    public Image ImgTouchBoard;
    public Image ImgResultShadow;
    public Image ImgResult;
    public Button[] Buttons;
    public Text[] ButtonText;
    public Image[] ButtonBGImage;
    public Image[] ButtonImage;
    //public Image ImgContentCover;


    public const int Button_None = 0;
    public const int Button_One = 1;
    public const int Button_Two = 2;
    public const int Button_Three = 3;

    public const int DIR_LEFT = 0;
    public const int DIR_RIGHT = 1;

    int _boardIndex;
    public int boardIndex {
        get {
            return boardIndex;
        }
    }

    float _boardWidth;
    float _boardHeight;

    public int status {
        get {
            if(_gameLogic==null) {
                return GameLogic.Status_Gameover;
            }
            else {
                return _gameLogic.status;
            }
        }
    }



    GameLogic _gameLogic;
    public GameLogic gameLogic {
        get {
            return _gameLogic;
        }
    }

    int _buttonMode;
    public int boardWidth {
        get{
            return (int)(ImgBackground.rectTransform.rect.width);
        }
    }
    public int boardHeight {
        get{
            return (int)(ImgBackground.rectTransform.rect.height);
        }
    }

    public float timeLeft {
        get {
            return _gameLogic.timer;
        }
    }


    public void SetGameLogic( GameLogic gameLogic ) {
        _gameLogic = gameLogic;

        TxtGameDesc1.gameObject.SetActive( false );
        TxtGameDesc2.gameObject.SetActive( false );

        if(gameLogic==null){
            int x=0;
            x++;
        }

        _gameLogic.SetGameController( this );
    }

    public void Reset() {
        ImgResult.gameObject.SetActive( false );
        TxtGameMainText.gameObject.SetActive( false );
        ImgResultShadow.gameObject.SetActive( false );
    }

    public void Clear() {
        Reset();

        _gameLogic.Clear();
    }

	// Use this for initialization
	void Start () {
        //Reset();
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void InitBoard( int boardIndexV ) {
        _boardIndex = boardIndexV;

        _boardWidth = MainPage.instance.ScreenWidth/2;
        _boardHeight = (MainPage.instance.ScreenHeight-MainPage.instance.TopBarHeight)/2;

        ImgBackground.rectTransform.sizeDelta = new Vector2( _boardWidth-32, _boardHeight-32 );

        ImgTouchBoard.rectTransform.sizeDelta = new Vector2( _boardWidth-32, _boardHeight-32 );
    }

    void FixedUpdate() {
        _gameLogic.FixedUpdate();
    }

    public void StartGame() {
        _gameLogic.StartGame();
    }

    public void StopGame() {
        _gameLogic.StopGame();
    }

    public const int GameResult_Win = 1;
    public const int GameResult_Lose = 2;
    public const int GameResult_Timeout = 3;

    public void SendGameResult( bool isWin ) {
        if(isWin==true){
            SendGameResult( GameResult_Win );
        }
        else {
            SendGameResult( GameResult_Lose );
        }
    }

    public void SendGameResult( int gameResult ) {
        Debug.Log( "Out SendGameResult!!!" );
        ImgResult.gameObject.SetActive( true );
        bool resultFlag = true;
        ImgResult.gameObject.transform.localScale = Vector3.zero;
        switch(gameResult) {
        case GameResult_Win:
            ImgResult.sprite = MainPage.instance.SptAnswerRight;
            MainPage.instance.PlaySound( MainPage.Sound_Right);
            break;
        case GameResult_Lose:
            ImgResult.sprite = MainPage.instance.SptAnswerWrong;
            MainPage.instance.PlaySound( MainPage.Sound_Wrong);
            resultFlag = false;
            break;
        case GameResult_Timeout:
            ImgResult.sprite = MainPage.instance.SptAnswerTimeout;
            MainPage.instance.PlaySound( MainPage.Sound_Wrong);
            resultFlag = false;
            break;

        }

        MainPage.instance.SendGameResult(resultFlag,_boardIndex);

        ImgResultShadow.gameObject.SetActive( true );
        Color color = ImgResultShadow.color;
        float alpha = color.a;
        color.a=0;
        ImgResultShadow.color = color;

        color.a = alpha;
        DOTween.Play( ImgResultShadow.DOColor( color, 0.4f) );

        DOTween.Play( ImgResult.gameObject.transform.DOScale( 1.0f, 0.6f ).SetEase( Ease.OutBack ).OnComplete( ()=>{
            Debug.Log( "Inner SendGameResult!!!" );
            MainPage.instance.ExecGameResult( _boardIndex );
        } ) );
    }

    public void SetGameName( string name ) {
        TxtGameDesc1.gameObject.SetActive( false );
        TxtGameDesc2.gameObject.SetActive( false );
    }

    public void SetGameNameAndDescription( string name, string desc1, string desc2 ) {
        TxtGameDesc1.gameObject.SetActive( true );
        TxtGameDesc1.text = desc1;
        TxtGameDesc1.rectTransform.localPosition = new Vector3( 0, boardHeight*23/80, 0 );

        if(desc2==null) {
            TxtGameDesc2.gameObject.SetActive( false );
        }
        else {
            TxtGameDesc2.gameObject.SetActive( true );
            TxtGameDesc2.text = desc2;
            TxtGameDesc2.rectTransform.localPosition = new Vector3( 0, boardHeight*18/80, 0 );
        }
    }

    public void SetGameDescription1( string desc1 ) {
        TxtGameDesc1.rectTransform.localPosition = new Vector3( 0, boardHeight*5/16, 0 );
        TxtGameDesc1.gameObject.SetActive( true );
        TxtGameDesc2.gameObject.SetActive( false );
        TxtGameDesc1.text = desc1;
    }

    public void SetGameDescription1( int posIndex, string desc1 ) {
        TxtGameDesc1.gameObject.SetActive( true );
        TxtGameDesc2.gameObject.SetActive( false );
        switch(posIndex) {
        case 0:
            TxtGameDesc1.rectTransform.localPosition = new Vector3( 0, boardHeight*5/16, 0 );
            break;
        case 1:
            TxtGameDesc1.rectTransform.localPosition = new Vector3( 0, boardHeight/12, 0 );
            break;
        case 2:
            TxtGameDesc1.rectTransform.localPosition = new Vector3( 0, boardHeight*19/80, 0 );
            break;
        case 3:
            TxtGameDesc1.rectTransform.localPosition = new Vector3( 0, boardHeight*21/80, 0 );
            break;
        case 4:
            TxtGameDesc1.rectTransform.localPosition = new Vector3( 0, boardHeight*25/80, 0 );
            break;
        case 5:
            TxtGameDesc1.rectTransform.localPosition = new Vector3( 0, boardHeight*33/80, 0 );
            break;
        case 6:
            TxtGameDesc1.rectTransform.localPosition = new Vector3( 0,  boardHeight*27/80, 0 );
            break;
        case 7:
            TxtGameDesc1.rectTransform.localPosition = new Vector3( 0, boardHeight*32/80, 0 );
            break;
        }
        TxtGameDesc1.text = desc1;
    }

    public void SetGameDescription2( int posIndex, string desc2 ) {
        TxtGameDesc2.gameObject.SetActive( true );
        switch(posIndex) {
        case 0:
            TxtGameDesc2.rectTransform.localPosition = new Vector3( 0, boardHeight/4, 0 );
            break;
        case 1:
            TxtGameDesc2.rectTransform.localPosition = new Vector3( 0, boardHeight/-12, 0 );
            break;
        case 2:
            TxtGameDesc2.rectTransform.localPosition = new Vector3( 0, 0, 0 );
            break;
        case 3:
            TxtGameDesc2.rectTransform.localPosition = new Vector3( 0, boardHeight*22/80, 0 );
            break;
        case 4:
            TxtGameDesc2.rectTransform.localPosition = new Vector3( 0, boardHeight*26/80, 0 );
            break;
        }
        TxtGameDesc2.text = desc2;
    }

    public void SetButtonMode( int buttonMode ) {
        
        Vector3 pos;
        switch( buttonMode ) {
        case Button_None:
            Buttons[0].gameObject.SetActive( false );
            Buttons[1].gameObject.SetActive( false );
            Buttons[2].gameObject.SetActive( false );
            break;
        case Button_One:
            ButtonBGImage[0].rectTransform.sizeDelta = new Vector2( _boardWidth/3.0f, _boardHeight/11.0f);
            ButtonBGImage[1].rectTransform.sizeDelta = new Vector2( _boardWidth/3.0f, _boardHeight/11.0f);
            ButtonBGImage[2].rectTransform.sizeDelta = new Vector2( _boardWidth/3.0f, _boardHeight/11.0f);

            Buttons[0].gameObject.SetActive( true );
            pos = Buttons[0].transform.localPosition;
            pos.x = 0;
            pos.y = -1.0f*_boardHeight/3;//*7/18;
            Buttons[0].transform.localPosition = pos;
            Buttons[1].gameObject.SetActive( false );
            Buttons[2].gameObject.SetActive( false );
            break;
        case Button_Two:
            ButtonBGImage[0].rectTransform.sizeDelta = new Vector2( _boardWidth/3.2f, _boardHeight/11.0f);
            ButtonBGImage[1].rectTransform.sizeDelta = new Vector2( _boardWidth/3.2f, _boardHeight/11.0f);
            ButtonBGImage[2].rectTransform.sizeDelta = new Vector2( _boardWidth/3.2f, _boardHeight/11.0f);

            Buttons[0].gameObject.SetActive( true );
            pos = Buttons[0].transform.localPosition;
            pos.y = -1.0f*_boardHeight/3;//*7/18;
            pos.x = -1*boardWidth/5;
            Buttons[0].transform.localPosition = pos;
            Buttons[1].gameObject.SetActive( true );
            pos = Buttons[0].transform.localPosition;
            pos.x = boardWidth/5;
            Buttons[1].transform.localPosition = pos;

            Buttons[2].gameObject.SetActive( false );
            break;
        case Button_Three:
            ButtonBGImage[0].rectTransform.sizeDelta = new Vector2( _boardWidth/4.5f, _boardHeight/11.0f);
            ButtonBGImage[1].rectTransform.sizeDelta = new Vector2( _boardWidth/4.5f, _boardHeight/11.0f);
            ButtonBGImage[2].rectTransform.sizeDelta = new Vector2( _boardWidth/4.5f, _boardHeight/11.0f);

            Buttons[0].gameObject.SetActive( true );
            pos = Buttons[0].transform.localPosition;
            pos.y = -1.0f*_boardHeight/3;//*7/18;
            pos.x = -1*boardWidth*11/40;
            Buttons[0].transform.localPosition = pos;
            Buttons[1].gameObject.SetActive( true );
            Buttons[2].gameObject.SetActive( true );
            pos.x*=-1;
            Buttons[2].transform.localPosition = pos;
            pos.x=0;
            Buttons[1].transform.localPosition = pos;
            break;
        }
    }

    public void SetButtons( int index, string buttonText, Color color) {
        ButtonText[index].text = buttonText;
        if(color!=Color.clear) {
            ButtonText[index].color = color;
        }
        ButtonImage[index].gameObject.SetActive( false );
        ButtonText[index].gameObject.SetActive( true );
    }

    public void SetButtons( int index, Sprite buttonImage, Color color) {
        ButtonImage[index].sprite = buttonImage;
        if(color!=Color.clear) {
            ButtonImage[index].color = color;
        }
        else {
            
        }
        ButtonImage[index].gameObject.SetActive( true );
        ButtonText[index].gameObject.SetActive( false );
    }

    public void SetButtonEnable( int index, bool enabled ) {
        Buttons[index].interactable = enabled;
    }

    public void SetMainText( string text, Color color ) {
        TxtGameMainText.gameObject.SetActive( true );
        TxtGameMainText.text = text;
        if(color!=Color.clear) {
            TxtGameMainText.color = color;
        }
    }

    public void SetColorIndex( int index ) {
        ImgBackground.color = MainPage.instance.GameBoardColor[index];
        /*
        ButtonBGImage[0].color = MainPage.instance.GameBoardColor[index];
        ButtonBGImage[1].color = MainPage.instance.GameBoardColor[index];
        ButtonBGImage[2].color = MainPage.instance.GameBoardColor[index];
        */

        ButtonBGImage[0].color = Color.white;
        ButtonBGImage[1].color = Color.white;
        ButtonBGImage[2].color = Color.white;


        ButtonText[0].color = MainPage.instance.GameBoardColor[index];
        ButtonText[1].color = MainPage.instance.GameBoardColor[index];
        ButtonText[2].color = MainPage.instance.GameBoardColor[index];

        ButtonImage[0].color = MainPage.instance.GameBoardColor[index];
        ButtonImage[1].color = MainPage.instance.GameBoardColor[index];
        ButtonImage[2].color = MainPage.instance.GameBoardColor[index];

    }

    public void OnButtonPressed( int index ) {
        _gameLogic.OnButtonPressed( index );
    }

    public void OnBoardTapped( Vector3 pos ) {
        Debug.Log( "Tap Pos:"+pos );
        _gameLogic.OnBoardTapped( pos );
    }


    public void OnTouchSwipe( int dir ) {
        Debug.Log( "OnTouchSwipe!");
        _gameLogic.OnTouchSwipe( dir );
    }

}
