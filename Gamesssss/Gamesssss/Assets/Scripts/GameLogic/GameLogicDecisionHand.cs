using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameLogicDecisionHand : GameLogicTwoButtons {

    public GameLogicDecisionHand( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameName( "HAND" );
        _gameController.SetGameDescription1( 0, "Left or right?" );

        _gameController.SetColorIndex( 4 );

        Image handImage = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        handImage.gameObject.SetActive( true );
        handImage.transform.SetParent( _gameController.goBoardArea.transform );
        handImage.color = Color.white;

        handImage.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/2, _gameController.boardWidth/2 );
        handImage.rectTransform.localPosition =  Vector3.zero;
        handImage.rectTransform.localScale = Vector3.one;

        _goList.Add( handImage.gameObject );

        handImage.sprite = MainPage.instance.SptHands[KWUtility.Random(0, MainPage.instance.SptHands.Length)];

        if(KWUtility.Random(0,2)==0) {
            _rightButtonIndex = 0;
            handImage.rectTransform.localEulerAngles = new Vector3( 0, 180, 0 );
        }
        else {
            _rightButtonIndex = 1;
        }
        SetButtons( "LEFT", "RIGHT" );

    }
}
