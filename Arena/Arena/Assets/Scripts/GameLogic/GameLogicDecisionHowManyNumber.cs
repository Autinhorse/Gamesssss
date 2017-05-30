using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameLogicDecisionHowManyNumber  : GameLogicThreeButtons {



    public GameLogicDecisionHowManyNumber( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {
    }

    public override void SetGameController( GameController controller ) {
        Debug.Log( "GameLogicDecisionHowMany SetGameController!!!" );

        base.SetGameController( controller );

        int numberFlag = KWUtility.Random( 0, 2);

        _gameController.SetGameName( "COUNT" );
        if(numberFlag==0) {
            _gameController.SetGameDescription1( 6, "How many evens?" );
        }
        else {
            _gameController.SetGameDescription1( 6, "How many odd?" );
        }

        _gameController.SetColorIndex( 4 );

        int mapWidth = 4;
        int mapHeight = 3;

        int difficulty = _difficulty;
        if(difficulty>10) {
            difficulty=10;
        }

        switch(_difficulty){
        case 0:
            mapWidth = 2;
            mapHeight = 2;
            break;
        case 1:
            mapWidth = 3;
            mapHeight = 2;
            break;
        case 2:
            mapWidth = 3;
            mapHeight = 3;
            break;
       default:
            mapWidth = 3;
            mapHeight = 4;
            break;
        }

        int mapBlockDelta = 24;
        int mapBlockSize = (int) _gameController.boardHeight/(mapWidth+8);

        float startPointX = -1*(mapWidth-1.0f)/2*(mapBlockSize+mapBlockDelta);
        float startPointY = (mapHeight-1.0f)/2*(mapBlockSize+mapBlockDelta);



        int result = 0;

        for( int m=0; m<mapWidth; m++ ) {
            for( int n=0; n<mapHeight; n++ ) {
                Text go = (Text) GameObject.Instantiate( _gameController.goBoardChar );
                _goList.Add( go.gameObject );
                go.gameObject.SetActive( true );

                go.gameObject.transform.SetParent( _gameController.goBoardArea.transform );
                go.rectTransform.localPosition = new Vector3( startPointX+(mapBlockSize+mapBlockDelta)*m, startPointY-(mapBlockSize+mapBlockDelta)*n, 0 ); 

                go.rectTransform.sizeDelta = new Vector2( mapBlockSize, mapBlockSize );
                go.rectTransform.localScale = Vector3.one;

                go.color = Color.white;
                int data = KWUtility.Random(4,40);
                if((numberFlag==0)&&(data%2==0)) {
                    result++;
                }
                if((numberFlag==1)&&(data%2==1)) {
                    result++;
                }
                go.text = data.ToString();
            }
        }

        int buttonPos = KWUtility.Random(0,3);

        if(result==0) {
            buttonPos = 2;
        }
        else if((result==1)&&(buttonPos==0)) {
            buttonPos = 1+KWUtility.Random(0,2);
        }

        switch(buttonPos){
        case 0:
            SetButtonsRandom( result.ToString(), (result-2).ToString(), (result-1).ToString() );
            break;
        case 1:
            SetButtonsRandom( result.ToString(), (result+1).ToString(), (result-1).ToString() );
            break;
        case 2:
            SetButtonsRandom( result.ToString(), (result+2).ToString(), (result+1).ToString() );
            break;
        }

    }
}
