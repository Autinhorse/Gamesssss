﻿using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameLogicDecisionHowMany : GameLogicThreeButtons {



    public GameLogicDecisionHowMany( int difficulty ) : base(difficulty) {
    }

    public override void SetGameController( GameController controller ) {
        Debug.Log( "GameLogicDecisionHowMany SetGameController!!!" );

        base.SetGameController( controller );

        _gameController.SetGameNameAndDescription( "COUNT", "How many       are here?", null );

        _gameController.SetColorIndex( 1 );

        int mapWidth = 4;
        int mapHeight = 3;

        int difficulty = _difficulty*2/3;
        if(difficulty>7) {
            difficulty=7;
        }

        mapWidth=3+(difficulty+1)/2;
        mapHeight=3+difficulty/2;

        int mapBlockDelta = 16;
        int mapBlockSize = (int) _gameController.boardWidth/(mapWidth+2);

        float startPointX = -1*(mapWidth-1.0f)/2*(mapBlockSize+mapBlockDelta);
        float startPointY = (mapHeight-1.0f)/2*(mapBlockSize+mapBlockDelta);

        int[] shapes = new int[3];
        shapes[0] = KWUtility.Random( 0, MainPage.instance.SptShapes.Length);
        do {
            shapes[1] = KWUtility.Random( 0, MainPage.instance.SptShapes.Length);
        }while(shapes[1]==shapes[0]);
        do {
            shapes[2] = KWUtility.Random( 0, MainPage.instance.SptShapes.Length);
        }while((shapes[2]==shapes[0])||(shapes[2]==shapes[1]));

        int[] shapeCount = new int[3];
        for(int m=0;m<3;m++) {
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

                go.color = Color.black;
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
        go1.rectTransform.localPosition = new Vector3( 16, 420, 0 ); 

        go1.rectTransform.sizeDelta = new Vector2( 48, 48 );

        go1.color = Color.black;

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