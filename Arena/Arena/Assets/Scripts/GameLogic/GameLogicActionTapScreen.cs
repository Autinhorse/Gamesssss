using UnityEngine;
using System.Collections;

public class GameLogicActionTapScreen : GameLogic {

    int _target;
    int _count;

    public GameLogicActionTapScreen( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        int difficult = _difficulty;
        if(difficult>2) {
            difficult=2;
        }

        _target = KWUtility.Random( 4+difficult, 4+difficult*4 );
        if(_target>12) {
            _target=12;
        }

        _gameController.SetGameName( "TAP!" );
        _gameController.SetGameDescription1( 2, "Tap screen "+_target.ToString()+" times." );
        _gameController.SetGameDescription2( 2, "Then press OK." );


        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_One );
        _gameController.SetButtons( 0, "OK", Color.clear );
    }

    public override void OnButtonPressed( int buttonIndex ) {
        _status = Status_Gameover;
        if(_count==_target) {
            _gameController.SendGameResult( true );
        }
        else {
            _gameController.SendGameResult( false );
        }
    }

    public override void OnBoardTapped( Vector3 pos ) {
        MainPage.instance.PlaySound( MainPage.Sound_Tap );

        _count++;
    }
}
