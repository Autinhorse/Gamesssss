﻿using UnityEngine;
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

    public GameLogicActionSpark( int difficulty ) : base(difficulty) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _target = 8 + _difficulty/2;

        _gameController.SetGameNameAndDescription( "Spark", "Tap to shoot.", _target.ToString() );

        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );

        _rotateSpeed = 90+15*_difficulty;

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
        _goList.Add( _coreImage.gameObject );

        _spark = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        _spark.gameObject.SetActive( true );
        _spark.transform.SetParent( _gameController.goBoardArea.transform );
        _spark.color = Color.white;
        _spark.sprite = MainPage.instance.SptShapes[3];

        _spark.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/16, _gameController.boardWidth/16 );
        _spark.rectTransform.localPosition = new Vector3( 0, _gameController.boardWidth/-2, 0 );
        _goList.Add( _spark.gameObject );
    }

    public override void FixedUpdate() {
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
                            break;
                        }
                    }

                    Image bar = (Image) GameObject.Instantiate( _gameController.goBoardImage );
                    bar.gameObject.SetActive( true );
                    bar.transform.SetParent( spark.transform );
                    bar.color = Color.white;
                    bar.sprite = MainPage.instance.SptShapes[0];

                    bar.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/128, _gameController.boardWidth/6 );
                    bar.rectTransform.localPosition = new Vector3( 0, _gameController.boardWidth/12, 0 );

                    _sparkList.Add( spark );
                    _target--;
                    _gameController.SetGameNameAndDescription( "Spark", "Tap to shoot.", _target.ToString() );

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
