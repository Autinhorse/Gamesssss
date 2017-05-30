using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameLogicDecisionNoExistShape : GameLogicThreeButtons {

    public GameLogicDecisionNoExistShape( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {
    }

    int _mapWidth;
    int _mapHeight;
    int MapBlockSize;
    const int MapBlockDelta = 24;

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameDescription1( 0, "Which shape is not here?" );

        _gameController.SetColorIndex( 4 );

        int charNumber=6;
        int candidateNumber = 3;



        switch(_difficulty) {
        case 0:
            charNumber=4;
            candidateNumber = 3;
            _mapWidth = 2;
            _mapHeight = 2;
            break;
        case 1:
            charNumber=6;
            candidateNumber = 3;
            _mapWidth = 3;
            _mapHeight = 2;
            break;
        case 2:
            charNumber=9;
            candidateNumber = 3;
            _mapWidth = 3;
            _mapHeight = 3;
            break;
        default:
            charNumber=12;
            candidateNumber = 4;
            _mapWidth = 4;
            _mapHeight = 3;
            break;
        }


        if(_mapWidth>2) {
            MapBlockSize = (int) _gameController.boardHeight/(_mapWidth+5);
        }
        else {
            MapBlockSize = (int) _gameController.boardHeight/7;
        }


        int[] shapes = new int[candidateNumber+1];
        for(int m=0;m<candidateNumber+1; m++ ) {
            bool same;
            do {
                shapes[m]=KWUtility.Random( 0, MainPage.instance.SptShapes.Length );
                same = false;
                for(int n=0;n<m;n++) {
                    if(shapes[m]==shapes[n]) {
                        same=true;
                        break;
                    }
                }
            }while(same==true);
        }

        int result = 0;
        int x = KWUtility.Random(0, _mapWidth);
        int y=KWUtility.Random(0,_mapHeight);

        int shapeType;

        int[] shapeList = new int[_mapWidth*_mapHeight];
        for( int m=0; m<_mapWidth*_mapHeight; m++ ) {
            if(m<candidateNumber) {
                shapeList[m]= shapes[m];
            }
            else {
                shapeList[m]= shapes[KWUtility.Random( 0, candidateNumber )];
            }
        }

        for( int m=0; m<10; m++ ) {
            int n1 = KWUtility.Random( 0, _mapWidth*_mapHeight );
            int n2 = KWUtility.Random( 0, _mapWidth*_mapHeight );

            int temp = shapeList[n1];
            shapeList[n1] = shapeList[n2];
            shapeList[n2] = temp;
        }

        for(int m=0; m<_mapWidth; m++ ) {
            for( int n=0; n<_mapHeight; n++ ) {
                shapeType = shapeList[m*_mapHeight+n];

                CreateShape( shapeType, m, n );
            }
        }

        SetButtonsRandom(MainPage.instance.SptShapes[shapes[candidateNumber]], MainPage.instance.SptShapes[shapes[0]], MainPage.instance.SptShapes[shapes[1]] );
    }

    void CreateShape( int shape, int posX, int posY ) {

        Vector2 pos = GetPosition( posX, posY );
        Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgBoard.gameObject.SetActive( true );
        imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
        imgBoard.color = new Color( 0, 0, 0, 0.2f);

        imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
        imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
        imgBoard.rectTransform.localScale = Vector3.one;

        _goList.Add( imgBoard.gameObject );

        Image imgShape = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        imgShape.gameObject.SetActive( true );
        imgShape.transform.SetParent( imgBoard.gameObject.transform );
        imgShape.rectTransform.localPosition = Vector3.zero;
        imgShape.rectTransform.localScale = Vector3.one;

        imgShape.color = Color.white;//MainPage.instance.GameBoardColor[4];
        imgShape.sprite = MainPage.instance.SptShapes[shape];

        imgShape.rectTransform.sizeDelta = new Vector2( MapBlockSize*0.75f, MapBlockSize*0.75f );

    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta) );
    }
}
