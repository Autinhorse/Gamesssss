using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class GameLogicActionSpark : GameLogic {

    Image _coreImage;
    List<Image> _sparkList;
    Image _spark;

    int _target;
    float _rotateSpeed;

    float _sparkSpeed;
    List<Image> _flyingSpark;

    public GameLogicActionSpark( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _target = 3 + _difficulty;

        if(_target>10){
            _target=9;
        }

        _gameController.SetGameNameAndDescription( "SPARK", "Tap to pin the ball.", _target.ToString() );

        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );

        _rotateSpeed = 90+10*_difficulty;
        if(_rotateSpeed>180)  {
            _rotateSpeed = 180;
        }

        _sparkList = new List<Image>();
        _flyingSpark = new List<Image>();

        _sparkSpeed = _gameController.boardWidth;


        _coreImage = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        _coreImage.gameObject.SetActive( true );
        _coreImage.transform.SetParent( _gameController.goBoardArea.transform );
        _coreImage.color = Color.white;
        _coreImage.sprite = MainPage.instance.SptShapes[10];

        _coreImage.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/3, _gameController.boardWidth/3 );
        _coreImage.rectTransform.localPosition = Vector3.zero;
        _coreImage.rectTransform.localScale = Vector3.one;
        _goList.Add( _coreImage.gameObject );

        _spark = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        _spark.gameObject.SetActive( true );
        _spark.transform.SetParent( _gameController.goBoardArea.transform );
        _spark.color = Color.white;
        _spark.sprite = MainPage.instance.SptShapes[3];

        _spark.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/16, _gameController.boardWidth/16 );
        _spark.rectTransform.localPosition = new Vector3( 0, _gameController.boardWidth/-2, 0 );
        _spark.rectTransform.localScale = Vector3.one;
        _goList.Add( _spark.gameObject );
    }

    public override void FixedUpdate() {
        if(_status==Status_Gameover) {
            return;
        }

        if(_status==Status_Playing) {
            Vector3 angle = _coreImage.rectTransform.localEulerAngles;
            angle.z+=_rotateSpeed*Time.fixedDeltaTime;
            _coreImage.rectTransform.localEulerAngles = angle;

            foreach( Image spark in _flyingSpark ) {
                Vector3 pos = spark.rectTransform.localPosition;
                pos.y+=_sparkSpeed*Time.fixedDeltaTime;
                if(pos.y>_gameController.boardWidth/-3) {
                    _flyingSpark.Remove( spark );
                    pos.y = _gameController.boardWidth/-3;
                    spark.rectTransform.localPosition = pos;
                    spark.transform.SetParent( _coreImage.transform );

                    pos = spark.rectTransform.localPosition;
                    foreach( Image dot in _sparkList ) {
                        if(Vector3.Distance( pos, dot.rectTransform.localPosition )<    _gameController.boardWidth/16) {
                            _status = Status_Gameover;
                            _gameController.SendGameResult( false );
                            return;
                        }
                    }

                    Image bar = (Image) GameObject.Instantiate( _gameController.goBoardImage );
                    bar.gameObject.SetActive( true );
                    bar.transform.SetParent( spark.transform );
                    bar.color = Color.white;
                    bar.sprite = MainPage.instance.SptShapes[0];

                    bar.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/64, _gameController.boardWidth/5 );
                    bar.rectTransform.localPosition = new Vector3( 0, _gameController.boardWidth/12, 0 );
                    bar.rectTransform.localScale = Vector3.one;

                    _sparkList.Add( spark );
                    _target--;
                    _gameController.SetGameNameAndDescription( "SPARK", "Tap to pin the ball.", _target.ToString() );

                    if(KWUtility.Random(0,2)==0) {
                        _rotateSpeed*=-1;
                    }

                    if(_target==0) {
                        _status = Status_Gameover;
                        _gameController.SendGameResult( true );
                    }

                    break;
                }
                else {
                    spark.rectTransform.localPosition = pos;
                }
            }
        }
    }

    public override void OnBoardTapped( Vector3 pos ) {
        if(_status!=Status_Playing) {
            return;
        }

        _flyingSpark.Add( _spark );

        _spark = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        _spark.gameObject.SetActive( true );
        _spark.transform.SetParent( _gameController.goBoardArea.transform );
        _spark.color = Color.white;
        _spark.sprite = MainPage.instance.SptShapes[3];

        _spark.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/16, _gameController.boardWidth/16 );
        _spark.rectTransform.localPosition = new Vector3( 0, _gameController.boardWidth/-2, 0 );
        _spark.rectTransform.localScale = Vector3.one;
        _goList.Add( _spark.gameObject );
    }
}
