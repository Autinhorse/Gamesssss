using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameLogicReactionGreenLight : GameLogic {

    public GameLogicReactionGreenLight( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );


        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );

        _gameController.SetGameDescription1( 7, "Tap screen when light turns green");
    }


    public override void OnBoardTapped( Vector3 pos ) {
        _gameController.SendGameResult( true );
    }
}
