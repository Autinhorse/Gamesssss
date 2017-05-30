using UnityEngine;
using System.Collections;

public class GameLogicActionTap : GameLogic {

    public GameLogicActionTap( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );


        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );

        _gameController.SetMainText( "TAP", Color.white );
    }

    
    public override void OnBoardTapped( Vector3 pos ) {
        _gameController.SendGameResult( true );
    }
}
