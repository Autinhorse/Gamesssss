using UnityEngine;
using System.Collections;

using System.Collections.Generic;

using UnityEngine.UI;

public class GameLogicActionTurnRight : GameLogic {

    int _ballNumber;
    List<Image> _ballList;

    Image _master;
    int _masterDir;

    int _direction;


    public GameLogicActionTurnRight( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );


        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );

        _direction = KWUtility.Random( 0, 2 );

        if(_direction==0) {
            _gameController.SetGameDescription1( 8,"Tap to turn left" );
        }
        else {
            _gameController.SetGameDescription1( 8,"Tap to turn right" );
        }
        _gameController.SetGameDescription2( 7,"Get all dots" );


        switch(_difficulty){
        case 0:
            _ballNumber = 1;
            break;
        case 1:
            _ballNumber = 2;
            break;
        case 2:
            _ballNumber = 3;
            break;
        default:
            _ballNumber = 4;
            break;
        }

        Image border = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        border.gameObject.SetActive( true );
        border.transform.SetParent( _gameController.goBoardArea.transform );
        border.color = Color.white;
        border.sprite = MainPage.instance.SptWhiteBlock;

        border.rectTransform.sizeDelta = new Vector2( 24, 460 );

        border.rectTransform.localPosition = new Vector3( 218, _gameController.boardHeight/-2+346, 0 );
        border.rectTransform.localScale = Vector3.one;
        _goList.Add( border.gameObject );

        border = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        border.gameObject.SetActive( true );
        border.transform.SetParent( _gameController.goBoardArea.transform );
        border.color = Color.white;
        border.sprite = MainPage.instance.SptWhiteBlock;

        border.rectTransform.sizeDelta = new Vector2( 24, 460 );

        border.rectTransform.localPosition = new Vector3( -218, _gameController.boardHeight/-2+346, 0 );
        border.rectTransform.localScale = Vector3.one;
        _goList.Add( border.gameObject );

        border = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        border.gameObject.SetActive( true );
        border.transform.SetParent( _gameController.goBoardArea.transform );
        border.color = Color.white;
        border.sprite = MainPage.instance.SptWhiteBlock;

        border.rectTransform.sizeDelta = new Vector2( 460, 24);

        border.rectTransform.localPosition = new Vector3( 0, _gameController.boardHeight/-2+128, 0);
        border.rectTransform.localScale = Vector3.one;
        _goList.Add( border.gameObject );

        border = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        border.gameObject.SetActive( true );
        border.transform.SetParent( _gameController.goBoardArea.transform );
        border.color = Color.white;
        border.sprite = MainPage.instance.SptWhiteBlock;

        border.rectTransform.sizeDelta = new Vector2( 460, 24 );

        border.rectTransform.localPosition = new Vector3( 0, _gameController.boardHeight/-2+564, 0 );
        border.rectTransform.localScale = Vector3.one;
        _goList.Add( border.gameObject );

        _master = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        _master.gameObject.SetActive( true );
        _master.transform.SetParent( _gameController.goBoardArea.transform );
        _master.color = Color.white;
        _master.sprite = MainPage.instance.SptShapes[0];

        _master.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/16, _gameController.boardWidth/16 );

        _master.rectTransform.localPosition = new Vector3( 0, _gameController.boardHeight/-10, 0 );
        _master.rectTransform.localScale = Vector3.one;
        _goList.Add( _master.gameObject );

        _masterDir = 0;

        _ballList = new List<Image>();

        for(int m=0; m<_ballNumber; m++ ) {
            Image imgball = (Image) GameObject.Instantiate( _gameController.goBoardImage );
            imgball.gameObject.SetActive( true );
            imgball.transform.SetParent( _gameController.goBoardArea.transform );
            imgball.color = Color.white;
            imgball.sprite = MainPage.instance.SptShapes[3];

            imgball.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/24, _gameController.boardWidth/24 );
            int posX, posY;
            bool closeFlag=false;

            do {
                posX = KWUtility.Random( -160, 160 );
                posY = KWUtility.Random( -160, 160 )-_gameController.boardHeight/2+346;
                closeFlag=false;
                for(int n=0;n<m;n++ ) {
                    if(Vector3.Distance( _ballList[n].rectTransform.localPosition, new Vector3( posX, posY, 0) )<_gameController.boardWidth/7 ) {
                        closeFlag=true;
                    }
                }

                if((posX>-48)&&(posX<48)) {
                    closeFlag=true;
                }
                if((posY>_gameController.boardHeight/-2+298)&&(posY<_gameController.boardHeight/-2+394)) {
                    closeFlag=true;
                }
            }while(closeFlag);

            imgball.rectTransform.localPosition = new Vector3( posX, posY, 0 );
            imgball.rectTransform.localScale = Vector3.one;
            _goList.Add( imgball.gameObject );

            _ballList.Add( imgball );

        }
    }

    public override void FixedUpdate() {
        if(_status!=Status_Playing) {
            return;
        }

        Vector3 masterPos = _master.rectTransform.localPosition;
        int speed = 300;
        switch( _masterDir ) {
        case 0:
            masterPos.x+=speed*Time.fixedDeltaTime;
            if(masterPos.x>200){
                masterPos.x=202;
                _masterDir=2;
            }
            break;
        case 1:
            masterPos.y-=speed*Time.fixedDeltaTime;
            if(masterPos.y<_gameController.boardHeight/-2+158){
                masterPos.y=_gameController.boardHeight/-2+156;
                _masterDir=3;
            }
            break;
        case 2:
            masterPos.x-=speed*Time.fixedDeltaTime;
            if(masterPos.x<-200){
                masterPos.x=-202;
                _masterDir=0;
            }
            break;
        case 3:
            masterPos.y+=speed*Time.fixedDeltaTime;
            if(masterPos.y>_gameController.boardHeight/-2+534){
                masterPos.y=_gameController.boardHeight/-2+532;
                _masterDir=1;
            }
            break;
        }

        _master.rectTransform.localPosition = masterPos;

        for(int m=0; m<_ballNumber; m++ ) {
            Vector3 pos =  _ballList[m].rectTransform.localPosition;
            if(Vector3.Distance( pos, masterPos)<40) {
                _ballList[m].gameObject.SetActive( false );
                _ballNumber--;
                _ballList.Remove( _ballList[m] );

                if(_ballNumber==0) {
                    _gameController.SendGameResult(true);
                }
                else {
                    MainPage.instance.PlaySound( MainPage.Sound_Tap );
                }
                break;
            }
        }
    }

    public override void OnBoardTapped( Vector3 pos ) {
        //_gameController.SendGameResult( true );
        if(_direction==1) {
            _masterDir++;
            if(_masterDir==4) {
                _masterDir = 0;
            }
        }
        else {
            _masterDir--;
            if(_masterDir==-1) {
                _masterDir = 3;
            }
        }
            
    }
}