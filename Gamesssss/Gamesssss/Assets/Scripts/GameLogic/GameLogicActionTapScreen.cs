using UnityEngine;
using System.Collections;

public class GameLogicActionTapScreen : GameLogic {

    int _target;
    int _count;

    public GameLogicActionTapScreen( int difficulty ) : base(difficulty) {
    }

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _target = 3+_difficulty+KWUtility.Random( 0, 5+_difficulty );

        _gameController.SetGameName( "TAP!" );
        _gameController.SetGameDescription1( 1, "Tap screen "+_target.ToString()+" times." );
        _gameController.SetGameDescription2( 1, "Then press OK." );


        _gameController.SetColorIndex( 2 );

        _gameController.SetButtonMode( GameController.Button_One );
        _gameController.SetButtons( 0, "OK", Color.clear );
    }

    public override void OnButtonPressed( int buttonIndex ) {
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
