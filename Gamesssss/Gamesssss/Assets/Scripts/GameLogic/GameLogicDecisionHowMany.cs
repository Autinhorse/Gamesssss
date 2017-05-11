using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameLogicDecisionHowMany : GameLogicThreeButtons {



    public GameLogicDecisionHowMany( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {
    }

    public override void SetGameController( GameController controller ) {
        Debug.Log( "GameLogicDecisionHowMany SetGameController!!!" );

        base.SetGameController( controller );

        _gameController.SetGameName( "COUNT" );
        _gameController.SetGameDescription1( 2, "How many         are here?" );

        _gameController.SetColorIndex( 4 );

        int mapWidth = 4;
        int mapHeight = 3;

        int difficulty = _difficulty;
        if(difficulty>10) {
            difficulty=10;
        }

        mapWidth=3+difficulty/4;
        mapHeight=2+(difficulty+2)/4;

        int mapBlockDelta = 24;
        int mapBlockSize = (int) _gameController.boardWidth/(mapWidth+4);

        float startPointX = -1*(mapWidth-1.0f)/2*(mapBlockSize+mapBlockDelta);
        float startPointY = (mapHeight-1.0f)/2*(mapBlockSize+mapBlockDelta);

        int[] shapes = new int[mapWidth];
        shapes[0] = KWUtility.Random( 0, MainPage.instance.SptShapes.Length);
        for(int m=1;m<mapWidth;m++ ) {
            bool sameFlag;
            do {
                sameFlag = false;
                shapes[m] = KWUtility.Random( 0, MainPage.instance.SptShapes.Length);
                for(int n=0; n<m; n++ ) {
                    if(shapes[m]==shapes[n]) {
                        sameFlag = true;
                        break;
                    }
                }
            }while(sameFlag==true);
        }

        int[] shapeCount = new int[mapWidth];
        for(int m=0;m<mapWidth;m++) {
            shapeCount[m]=0;
        }

        int maxValue = -1;
        int maxIndex = -1;

        for( int m=0; m<mapWidth; m++ ) {
            for( int n=0; n<mapHeight; n++ ) {
                Image go = (Image) GameObject.Instantiate( _gameController.goBoardImage );
                _goList.Add( go.gameObject );
                go.gameObject.SetActive( true );

                go.gameObject.transform.SetParent( _gameController.goBoardArea.transform );
                go.rectTransform.localPosition = new Vector3( startPointX+(mapBlockSize+mapBlockDelta)*m, startPointY-(mapBlockSize+mapBlockDelta)*n, 0 ); 

                go.rectTransform.sizeDelta = new Vector2( mapBlockSize, mapBlockSize );
                go.rectTransform.localScale = Vector3.one;

                go.color = Color.white;
                int shapeIndex = UnityEngine.Random.Range(0,3);
                go.sprite = MainPage.instance.SptShapes[shapes[shapeIndex]];

                shapeCount[shapeIndex]++;
                if(shapeCount[shapeIndex]>maxValue) {
                    maxValue=shapeCount[shapeIndex];
                    maxIndex = shapeIndex;
                }
            }
        }

        Image go1 = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        _goList.Add( go1.gameObject );
        go1.gameObject.SetActive( true );

        go1.gameObject.transform.SetParent( _gameController.goBoardArea.transform );
        go1.rectTransform.localPosition = new Vector3( 12, 372, 0 ); 

        go1.rectTransform.sizeDelta = new Vector2( 48, 48 );
        go1.rectTransform.localScale = Vector3.one;

        go1.color = Color.white;

        go1.sprite = MainPage.instance.SptShapes[shapes[maxIndex]];

        switch(UnityEngine.Random.Range(0,3)){
        case 0:
            SetButtonsRandom( maxValue.ToString(), (maxValue-2).ToString(), (maxValue-1).ToString() );
            break;
        case 1:
            SetButtonsRandom( maxValue.ToString(), (maxValue+1).ToString(), (maxValue-1).ToString() );
            break;
        case 2:
            SetButtonsRandom( maxValue.ToString(), (maxValue+2).ToString(), (maxValue+1).ToString() );
            break;
        }

    }
}
