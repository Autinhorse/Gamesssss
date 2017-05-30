using UnityEngine;
using System.Collections;

public class GameLogicActionSwipe : GameLogic {

    int _dir;

    public GameLogicActionSwipe( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );


        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_None );

        _dir = KWUtility.Random( 0, 2 );

        switch(_dir) {
        case GameController.DIR_LEFT:
            _gameController.SetMainText( "SWIPE TO LEFT", Color.white );
            break;
        case GameController.DIR_RIGHT:
            _gameController.SetMainText( "SWIPE TO RIGHT", Color.white );
            break;
        }

    }

    public override void OnTouchSwipe( int dir ) {
        if(_status!=Status_Playing) {
            return;
        }

        if(dir==_dir) {
            _gameController.SendGameResult( true );
        }
        else {
            _gameController.SendGameResult( false );
        }
    }
}
