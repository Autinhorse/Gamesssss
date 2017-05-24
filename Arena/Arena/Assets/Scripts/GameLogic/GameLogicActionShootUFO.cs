using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;


class UFOData {
    public Image imgUFO;
    public Vector3 pos;
    public float speed;
    public bool isLive;
}

public class GameLogicActionShootUFO : GameLogic {

    int _UFONumber;
    float _UFOSize;
    float _UFOSpeed;

    List<UFOData> _ufos;

    Image _bullet;
    Vector3 _bulletPos;
    float _bulletSpeed;

    public GameLogicActionShootUFO( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {
    }

    // 这个游戏的难度由UFO数量，UFO大小，和移动速度决定
    // 0-16级 UFO数量从1个到6个，UFO体积缩小一半，速度增加1倍
    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _totalGameTime = StartTime;
        _timer = _totalGameTime;

        _ufos = new List<UFOData>();


        switch(_difficulty) {
        case 0:
            _UFONumber = 1;
            break;
        case 1:
            _UFONumber = 1;
            break;
        default:
            _UFONumber = 3;
            break;
        }

        _UFOSize = _gameController.boardWidth/6;
        _UFOSize/=(1+(_UFONumber-1)/7.0f);
        int posX, posY;
        //loat speed;

        int[] line = new int[8];
        line[0] = 0;
        line[1] = 4;
        line[2] = 2;
        line[3] = 0;
        line[4] = 5;
        line[5] = 1;
        line[6] = 8;
        line[7] = 7;




        for(int m=0; m<_UFONumber;m++ ) {
            UFOData ufo = new UFOData();
            ufo.isLive = true;

            posX = KWUtility.Random( -1*_gameController.boardWidth/5, _gameController.boardHeight/5 );
            posY = _gameController.boardWidth*5/12-_gameController.boardWidth/12*line[m];//KWUtility.Random( 0, _gameController.boardWidth/3 );

            ufo.pos = new Vector3( posX, posY, 0 );

            ufo.speed = KWUtility.Random( _gameController.boardWidth/6, _gameController.boardWidth/2);
            ufo.speed*=(KWUtility.Random(0,2)*2-1);

            ufo.speed*=(1+(_UFONumber-1)/7.0f);

            ufo.imgUFO = (Image) GameObject.Instantiate( _gameController.goBoardImage );
            ufo.imgUFO.gameObject.SetActive( true );
            ufo.imgUFO.transform.SetParent( _gameController.goBoardArea.transform );
            ufo.imgUFO.color = Color.white;
            ufo.imgUFO.sprite = MainPage.instance.SptUFO;

            ufo.imgUFO.rectTransform.sizeDelta = new Vector2( _UFOSize, _UFOSize );
            ufo.imgUFO.rectTransform.localPosition = ufo.pos;
            ufo.imgUFO.rectTransform.localScale = Vector3.one;
            _goList.Add( ufo.imgUFO.gameObject );

            _ufos.Add( ufo );

        }

        Image imgCannon = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgCannon.gameObject.SetActive( true );
        imgCannon.transform.SetParent( _gameController.goBoardArea.transform );
        imgCannon.color = Color.white;
        imgCannon.sprite = MainPage.instance.SptCannon;

        imgCannon.rectTransform.sizeDelta = new Vector2( _gameController.boardHeight/16, _gameController.boardHeight/16 );
        imgCannon.rectTransform.localPosition = new Vector3( 0, _gameController.boardHeight*19/-48, 0 );
        imgCannon.rectTransform.localScale = Vector3.one;
        _goList.Add( imgCannon.gameObject );

        _gameController.SetGameDescription1( 5, "Shoot them down!");

        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );
        //_gameController.SetButtons( 0, "SHOOT", Color.clear );

        _bullet = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        _bullet.gameObject.SetActive( false );
        _bullet.transform.SetParent( _gameController.goBoardArea.transform );
        _bullet.color = Color.white;
        _bullet.sprite = MainPage.instance.SptShapes[3];

        _bullet.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/48, _gameController.boardWidth/48 );
        _bullet.rectTransform.localPosition = new Vector3( 0, _gameController.boardHeight*19/-48, 0 );
        _bullet.rectTransform.localScale = Vector3.one;
        _goList.Add( _bullet.gameObject );

        _bulletSpeed = -1;
        _bulletPos = new Vector2( 0, _gameController.boardHeight*11/-32 );
    }

    public override void FixedUpdate() {
        
        if(_status==Status_Gameover) {
            return;
        }

        base.FixedUpdate();

        foreach( UFOData data in _ufos ) {
            if(data.isLive==false) {
                continue;
            }

            data.pos.x+=data.speed*Time.fixedDeltaTime;
            if((data.pos.x<_gameController.boardWidth/-3)||(data.pos.x>_gameController.boardWidth/3)) {
                data.pos.x-=data.speed*Time.fixedDeltaTime;
                data.speed*=-1;
            }


            if(((data.pos.x-_UFOSize*3/8)<0)&&((data.pos.x+_UFOSize*3/8)>0)&&((data.pos.y-_UFOSize/4)<_bulletPos.y)&&((data.pos.y+_UFOSize/4)>_bulletPos.y)) {
                data.imgUFO.gameObject.SetActive( false );
                data.pos.x = _gameController.boardWidth*2;
                _bullet.gameObject.SetActive( false );
                _bulletSpeed=-1;
                _gameController.SetButtonEnable( 0, true );

                data.isLive = false;

                _UFONumber--;
                if(_UFONumber==0){
                    _status = Status_Gameover;
                    _gameController.SendGameResult( true );
                    MainPage.instance.PlaySound( MainPage.Sound_Shoot );
                    _ufos.Clear();
                    return;
                }
                else {
                    MainPage.instance.PlaySound( MainPage.Sound_Shoot );
                }
            }

            data.imgUFO.rectTransform.localPosition = data.pos;
        }

        if(_bulletSpeed>0) {
            _bulletPos.y+=_bulletSpeed*Time.fixedDeltaTime;
            _bullet.rectTransform.localPosition = _bulletPos;

            if(_bulletPos.y>_gameController.boardWidth/2) {
                _bullet.gameObject.SetActive( false );
                _bulletSpeed=-1;
                _gameController.SetButtonEnable( 0, true );
            }
        }
    }

    public override void OnBoardTapped( Vector3 pos ) {
        _gameController.SetButtonEnable( 0, false );

        _bullet.gameObject.SetActive( true );

        _bulletSpeed = _gameController.boardHeight*1.25f;

        _bulletPos = new Vector2( 0, _gameController.boardHeight*19/-48 );

        _bullet.rectTransform.localPosition = _bulletPos;
    }
}
