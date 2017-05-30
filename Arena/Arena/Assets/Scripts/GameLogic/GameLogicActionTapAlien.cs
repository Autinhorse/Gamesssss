using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using DG.Tweening;

public class GameLogicActionTapAlien : GameLogic {

    int _alienNumber;
    Vector2[] _alienSpeed;

    float _alienBaseSpeed;

    List<Image> _alienList;


    public GameLogicActionTapAlien( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _alienBaseSpeed = 10;


        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );

        _gameController.SetGameDescription1( 6,"Tap aliens" );

        switch(_difficulty){
        case 0:
        case 1:
            _alienNumber = 1;
            _alienBaseSpeed = _gameController.boardHeight/6;
            break;
        case 2:
            _alienNumber = 2;
            _alienBaseSpeed = _gameController.boardHeight/4;
            break;
        default:
            _alienNumber = 3;
            _alienBaseSpeed = _gameController.boardHeight/2;
            break;
        }

        _alienList = new List<Image>();
        _alienSpeed = new Vector2[_alienNumber];

        for(int m=0; m<_alienNumber; m++ ) {
            Image imgAlien = (Image) GameObject.Instantiate( _gameController.goBoardImage );
            imgAlien.gameObject.SetActive( true );
            imgAlien.transform.SetParent( _gameController.goBoardArea.transform );
            imgAlien.color = Color.white;
            imgAlien.sprite = MainPage.instance.SptAlien;

            imgAlien.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/8, _gameController.boardWidth/8 );
            imgAlien.rectTransform.localPosition = new Vector3( 0, 0, 0 );
            imgAlien.rectTransform.localScale = Vector3.one;
            _goList.Add( imgAlien.gameObject );

            _alienList.Add( imgAlien );

            float angle = KWUtility.Random(0, 36000)/100*3.1415927f/180;

            float speed = (KWUtility.Random(0,60)+70.0f)/100*_alienBaseSpeed;

            _alienSpeed[m] = new Vector2( speed*Mathf.Sin(angle), speed*Mathf.Cos(angle) );
        }
    }

    public override void FixedUpdate ()
    {
        base.FixedUpdate ();

        Vector3 pos;

        for(int m=0; m<_alienNumber; m++ ) {
            pos = _alienList[m].rectTransform.localPosition;
            pos.x+=_alienSpeed[m].x*Time.fixedDeltaTime;
            pos.y+=_alienSpeed[m].y*Time.fixedDeltaTime;

            _alienList[m].rectTransform.localPosition = pos;

            if((pos.x>_gameController.boardWidth*3/8)||(pos.x<_gameController.boardWidth*3/-8)) {
                _alienSpeed[m].x*=-1;
            }
            if((pos.y>_gameController.boardHeight*2/8)||(pos.y<_gameController.boardHeight*3/-8)) {
                _alienSpeed[m].y*=-1;
            }
        }
    }


    public override void OnBoardTapped( Vector3 pos ) {
        //_gameController.SendGameResult( true );
        Vector3 alienPos;
        for(int m=0; m<_alienNumber; m++ ) {

            alienPos = _alienList[m].rectTransform.localPosition;
           
            if(Vector3.Distance( alienPos, pos)<_gameController.boardWidth/10 ) {
                _alienNumber--;
                _alienList[m].gameObject.SetActive( false );

                if(_alienNumber==0) {
                    _gameController.SendGameResult( true );
                }
                else {
                    MainPage.instance.PlaySound( MainPage.Sound_Tap );
                   
                    _alienList.Remove( _alienList[m] );
                }
            }
        }
    }
}
