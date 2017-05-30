using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class GameLogicActionTapBall : GameLogic {

    int _ballNumber;
    List<Image> _ballList;
    List<Text> _charList;

    int _targetType;

    public GameLogicActionTapBall( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

       
        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );

        switch(_difficulty){
        case 0:
            _ballNumber = 2;
            break;
        case 1:
            _ballNumber = 4;
            break;
        case 2:
            _ballNumber = 6;
            break;
        default:
            _ballNumber = 8;
            break;
        }

        _targetType = KWUtility.Random( 0, 2 );

        if(_targetType==0) {

            _gameController.SetGameDescription1( 6,"Tap balls" );

            _ballList = new List<Image>();
           
            for(int m=0; m<_ballNumber; m++ ) {
                Image imgball = (Image) GameObject.Instantiate( _gameController.goBoardImage );
                imgball.gameObject.SetActive( true );
                imgball.transform.SetParent( _gameController.goBoardArea.transform );
                imgball.color = Color.white;
                imgball.sprite = MainPage.instance.SptShapes[3];

                imgball.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/9, _gameController.boardWidth/9 );
                int posX, posY;
                bool closeFlag=false;

                do {
                    posX = KWUtility.Random( 0, _gameController.boardWidth*3/4 )-_gameController.boardWidth*3/8;
                    posY = KWUtility.Random( 0, _gameController.boardHeight*3/5 )-_gameController.boardHeight*2/5;
                    closeFlag=false;
                    for(int n=0;n<m;n++ ) {
                        if(Vector3.Distance( _ballList[n].rectTransform.localPosition, new Vector3( posX, posY, 0) )<_gameController.boardWidth/7 ) {
                            closeFlag=true;
                        }
                    }
                }while(closeFlag);

                imgball.rectTransform.localPosition = new Vector3( posX, posY, 0 );
                imgball.rectTransform.localScale = Vector3.one;
                _goList.Add( imgball.gameObject );

                _ballList.Add( imgball );
            }
        }
        else {
            

            _charList = new List<Text>();
            byte startChar=0;

            if(KWUtility.Random(0,2)==0) {
                _gameController.SetGameDescription1( 6,"Tap numbers" );
                startChar=48;
            }
            else {
                _gameController.SetGameDescription1( 6,"Tap letters" );
                startChar=(byte)KWUtility.Random(65,80);
            }

            for(int m=0; m<_ballNumber; m++ ) {
                Text txtChar = (Text) GameObject.Instantiate( _gameController.goBoardChar );
                txtChar.gameObject.SetActive( true );
                txtChar.transform.SetParent( _gameController.goBoardArea.transform );
                txtChar.color = Color.white;
                txtChar.text = ((char)(startChar+m)).ToString();

                txtChar.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/9, _gameController.boardWidth/9 );
                int posX, posY;
                bool closeFlag=false;

                do {
                    posX = KWUtility.Random( 0, _gameController.boardWidth*3/4 )-_gameController.boardWidth*3/8;
                    posY = KWUtility.Random( 0, _gameController.boardHeight*3/5 )-_gameController.boardHeight*2/5;
                    closeFlag=false;
                    for(int n=0;n<m;n++ ) {
                        if(Vector3.Distance( _charList[n].rectTransform.localPosition, new Vector3( posX, posY, 0) )<_gameController.boardWidth/7 ) {
                            closeFlag=true;
                        }
                    }
                }while(closeFlag);

                txtChar.rectTransform.localPosition = new Vector3( posX, posY, 0 );
                txtChar.rectTransform.localScale = Vector3.one;
                _goList.Add( txtChar.gameObject );

                _charList.Add( txtChar );
            }
        }
    }

    public override void OnBoardTapped( Vector3 pos ) {
        //_gameController.SendGameResult( true );
        Vector3 ballPos;
        for(int m=0; m<_ballNumber; m++ ) {

            if(_targetType==0) {
                ballPos = _ballList[m].rectTransform.localPosition;
            }
            else {
                ballPos = _charList[m].rectTransform.localPosition;
            }

            if(Vector3.Distance( ballPos, pos)<_gameController.boardWidth/12 ) {
                _ballNumber--;

                if(_targetType==0) {
                    _ballList[m].gameObject.SetActive( false );
                }
                else {
                    _charList[m].gameObject.SetActive( false );
                }

                if(_ballNumber==0) {
                    _gameController.SendGameResult( true );
                }
                else {
                    MainPage.instance.PlaySound( MainPage.Sound_Tap );

                    if(_targetType==0) {
                        _ballList.Remove( _ballList[m] );
                    }
                    else {
                        _charList.Remove( _charList[m] );
                    }
                }
            }
        }
    }
}
