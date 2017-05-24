using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using UnityEngine.UI;

using DG.Tweening;

class ArrowData {
    public Image imgArrow;
    public Vector3 pos;
    public bool isEmpty;
    public bool isLeft;
}

public class GameLogicSwipeArrow : GameLogic {
    int _arrowNumber;

    List<ArrowData> _arrows;

    int _startArrow;
    int _endArrow;

    public GameLogicSwipeArrow( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    // 这个游戏的难度由箭头数量决定
    // 0-15级 箭头数量从6个到21个，每级增加一个
    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _arrows = new List<ArrowData>();

        _arrowNumber = 4+3*_difficulty;
        if(_arrowNumber>10){
            _arrowNumber=10;
        }

        for(int m=0; m<_arrowNumber;m++ ) {
            ArrowData arrow = new ArrowData();

            arrow.isLeft = (KWUtility.Random( 0, 2 )==0);
            arrow.isEmpty = (KWUtility.Random( 0, 2 )==0);

            if(arrow.isLeft==true) {
            }

            arrow.imgArrow = (Image) GameObject.Instantiate( _gameController.goBoardImage );
            arrow.imgArrow.gameObject.SetActive( true );
            arrow.imgArrow.transform.SetParent( _gameController.goBoardArea.transform );
            arrow.imgArrow.color = Color.white;

            if(arrow.isEmpty) {
                arrow.imgArrow.sprite = MainPage.instance.SptArrowEmpty;
                if(arrow.isLeft==false) {
                    arrow.imgArrow.rectTransform.localEulerAngles = new Vector3( 0, 0, 180);
                }
            }
            else {
                arrow.imgArrow.sprite = MainPage.instance.SptArrow;
                if(arrow.isLeft) {
                    arrow.imgArrow.rectTransform.localEulerAngles = new Vector3( 0, 0, 180);
                }
            }

            arrow.imgArrow.rectTransform.sizeDelta = new Vector2( _gameController.boardHeight/5, _gameController.boardHeight/5 );
            arrow.pos = new Vector3( 0, -_gameController.boardHeight/6+_gameController.boardHeight/8*m, 0 );
            arrow.imgArrow.rectTransform.localPosition = arrow.pos;
            arrow.imgArrow.rectTransform.localScale = Vector3.one*0.25f;
            _goList.Add( arrow.imgArrow.gameObject );

            arrow.imgArrow.gameObject.SetActive( false );

            _arrows.Add( arrow );

        }

        _gameController.SetGameDescription1( 6, "         Swipe same direction." );
        _gameController.SetGameDescription2( 3, "        Swipe opposite direction." );

        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );

        Image arrowImg = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        arrowImg.gameObject.SetActive( true );
        arrowImg.transform.SetParent( _gameController.goBoardArea.transform );
        arrowImg.color = Color.white;
        arrowImg.sprite = MainPage.instance.SptArrow;

        arrowImg.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/10, _gameController.boardWidth/10 );
        arrowImg.rectTransform.localPosition = new Vector3( _gameController.boardWidth*-3/8, _gameController.boardHeight*27/80, 0 );
        arrowImg.rectTransform.localScale = Vector3.one;
        _goList.Add( arrowImg.gameObject );

        Image arrowEmpty = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        arrowEmpty.gameObject.SetActive( true );
        arrowEmpty.transform.SetParent( _gameController.goBoardArea.transform );
        arrowEmpty.color = Color.white;
        arrowEmpty.sprite = MainPage.instance.SptArrowEmpty;

        arrowEmpty.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/10, _gameController.boardWidth/10 );
        arrowEmpty.rectTransform.localPosition = new Vector3( _gameController.boardWidth*-3/8, _gameController.boardHeight*22/80, 0 );
        arrowEmpty.rectTransform.localScale = Vector3.one;
        _goList.Add( arrowEmpty.gameObject );

        SetArrowPosition();
    }

    public void SetArrowPosition( ) {
        
        float targetY = -_gameController.boardHeight/3;

        for( int m=0; m<4; m++ ) {
            if(_startArrow+m>=_arrows.Count) {
                return;
            }

            float deltaPos = _gameController.boardHeight/(6+m*2);
            targetY+=deltaPos;

            _arrows[m+_startArrow].imgArrow.gameObject.SetActive( true );
           
            DOTween.Play( _arrows[m+_startArrow].imgArrow.rectTransform.DOLocalMoveY( targetY, 0.3f) );

            float alpha = 1.0f-0.1f*m;
            Color color = _arrows[m+_startArrow].imgArrow.color;
            color.a = alpha;
            DOTween.Play( _arrows[m+_startArrow].imgArrow.DOColor( color, 0.3f ) );

            float scale = 1.0f-0.25f*m;
            DOTween.Play( _arrows[m+_startArrow].imgArrow.rectTransform.DOScale( Vector3.one*scale, 0.3f ) );


        }
    }

    public override void OnTouchSwipe( int dir ) {
        if(_status!=Status_Playing) {
            return;
        }

        bool result = false;
        if(_arrows[_startArrow].isLeft==true) {
            if(dir==GameController.DIR_LEFT) {
                result = true;
            }
            else {
                result = false;
            }
        }
        else {
            if(dir==GameController.DIR_LEFT) {
                result = false;
            }
            else {
                result = true;
            }
        }

        if(result==false) {
            _status = Status_Gameover;
            _gameController.SendGameResult( false );
        }
        else {
            if(_startArrow==_arrows.Count-1) {
                _status = Status_Gameover;
                _gameController.SendGameResult( true );
            }   

            _arrows[_startArrow].imgArrow.gameObject.SetActive( true );
            float targetY = -_gameController.boardHeight/6-_gameController.boardHeight/8;
            DOTween.Play( _arrows[_startArrow].imgArrow.rectTransform.DOLocalMoveY( targetY, 0.3f) );

            float alpha = 0.0f;
            Color color = _arrows[_startArrow].imgArrow.color;
            color.a = alpha;
            DOTween.Play( _arrows[_startArrow].imgArrow.DOColor( color, 0.3f ) );

            int currentArrow = _startArrow;
            DOTween.Play( _arrows[_startArrow].imgArrow.rectTransform.DOScale( Vector3.zero, 0.3f ).OnComplete( ()=> {
                _arrows[currentArrow].imgArrow.gameObject.SetActive( false );
            } ) );

            _startArrow++;
            SetArrowPosition();
        }
    }
}
