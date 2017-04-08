using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameLogicDecisionHand : GameLogicTwoButtons {

    public GameLogicDecisionHand( int difficulty ) : base(difficulty) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameNameAndDescription( "Hand", "Left or right?", null );

        _gameController.SetColorIndex( 0 );

        Image handImage = (Image) GameObject.Instantiate( _gameController.goBoardImage );
        handImage.gameObject.SetActive( true );
        handImage.transform.SetParent( _gameController.goBoardArea.transform );
        handImage.color = Color.white;

        handImage.rectTransform.sizeDelta = new Vector2( _gameController.boardWidth/3, _gameController.boardWidth/3 );
        handImage.rectTransform.localPosition =  Vector3.zero;
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
