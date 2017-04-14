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
    public Text TxtGameName;
    public Text TxtGameDesc1;
    public Text TxtGameDesc2;
    public Text TxtGameMainText;
    public Image ImgBackground;
    public Image ImgResultShadow;
    public Image ImgResult;
    public Button[] Buttons;
    public Text[] ButtonText;
    public Image[] ButtonBGImage;
    public Image[] ButtonImage;


    public const int Button_None = 0;
    public const int Button_One = 1;
    public const int Button_Two = 2;
    public const int Button_Three = 3;

    public const int DIR_LEFT = 0;
    public const int DIR_RIGHT = 1;


    GameLogic _gameLogic;
    int _buttonMode;
    public int boardWidth {
        get{
            return (int)(ImgResultShadow.rectTransform.rect.width);
        }
    }
    public int boardHeight {
        get{
            return (int)(ImgResultShadow.rectTransform.rect.height);
        }
    }


    public void SetGameLogic( GameLogic gameLogic ) {
        _gameLogic = gameLogic;

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

    void FixedUpdate() {
        _gameLogic.FixedUpdate();
    }

    public void StartGame() {
        _gameLogic.StartGame();
    }

    public void SendGameResult( bool isWin ) {
        Debug.Log( "Out SendGameResult!!!" );
        ImgResult.gameObject.SetActive( true );
        ImgResult.gameObject.transform.localScale = Vector3.zero;
        if(isWin==true) {
            ImgResult.sprite = MainPage.instance.SptAnswerRight;
        }
        else  {
            ImgResult.sprite = MainPage.instance.SptAnswerWrong;
        }

        ImgResultShadow.gameObject.SetActive( true );
        Color color = ImgResultShadow.color;
        float alpha = color.a;
        color.a=0;
        ImgResultShadow.color = color;

        color.a = alpha;
        DOTween.Play( ImgResultShadow.DOColor( color, 0.4f) );

        DOTween.Play( ImgResult.gameObject.transform.DOScale( 1.0f, 0.6f ).SetEase( Ease.OutBack ).OnComplete( ()=>{
            Debug.Log( "Inner SendGameResult!!!" );
            MainPage.instance.SendGameResult( isWin );
        } ) );
    }

    public void SetGameNameAndDescription( string name, string desc1, string desc2 ) {
        TxtGameName.text = name;
        TxtGameDesc1.text = desc1;
        if(desc2==null) {
            TxtGameDesc2.gameObject.SetActive( false );
        }
        else {
            TxtGameDesc2.gameObject.SetActive( true );
            TxtGameDesc2.text = desc2;
        }
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
            Buttons[0].gameObject.SetActive( true );
            pos = Buttons[0].transform.localPosition;
            pos.x = 0;
            Buttons[0].transform.localPosition = pos;
            Buttons[1].gameObject.SetActive( false );
            Buttons[2].gameObject.SetActive( false );
            break;
        case Button_Two:
            Buttons[0].gameObject.SetActive( true );
            pos = Buttons[0].transform.localPosition;
            pos.x = -1*boardWidth/4;
            Buttons[0].transform.localPosition = pos;
            Buttons[1].gameObject.SetActive( true );
            pos = Buttons[0].transform.localPosition;
            pos.x = boardWidth/4;
            Buttons[1].transform.localPosition = pos;

            Buttons[2].gameObject.SetActive( false );
            break;
        case Button_Three:
            Buttons[0].gameObject.SetActive( true );
            pos = Buttons[0].transform.localPosition;
            pos.x = -1*boardWidth/3;
            Buttons[0].transform.localPosition = pos;
            Buttons[1].gameObject.SetActive( true );
            Buttons[2].gameObject.SetActive( true );
            pos.x*=-1;
            Buttons[2].transform.localPosition = pos;
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

        ButtonBGImage[0].color = MainPage.instance.GameBoardColor[index];
        ButtonBGImage[1].color = MainPage.instance.GameBoardColor[index];
        ButtonBGImage[2].color = MainPage.instance.GameBoardColor[index];
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
