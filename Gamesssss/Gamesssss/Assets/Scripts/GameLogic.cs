using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic  {

    // Factory of game logic instance

    public const int Game_Math_Sum = 0;
    public const int Game_Math_Math = 1;
    public const int Game_Math_WhichBig = 2;
    public const int Game_Math_DiceSum = 3;

    public const int Game_Decision_HowMany = 4;
    public const int Game_Decision_Hand = 5;
    public const int Game_Decision_NoExistChar = 6;
    public const int Game_Decision_TapShape = 7;

    public const int Game_Action_TapScreen = 8;
    public const int Game_Action_Spark = 9;
    public const int Game_Action_ShootUFO = 10;
    public const int Game_Action_SwipeArrow = 11;

    public const int Game_Memory_Pair = 12;
    public const int Game_Memory_MissShape = 13;
    public const int Game_Memory_NewShape = 14;
    public const int Game_Memory_Order = 15;

    public const int Game_Resolve_Maze = 16;
    public const int Game_Resolve_Number = 17;
    public const int Game_Resolve_Headup = 18;
    public const int Game_Resolve_RotatePuzzle = 19;

    static int dif = 0;

    public static GameLogic GetGameLogic( int gameID, int difficulty, int randomSeed ) {
        GameLogic gameLogic = null;

        switch( gameID ) {
        case Game_Math_Sum:
            gameLogic = new GameLogicMathSum( gameID, difficulty, randomSeed );
            break;
        case Game_Math_Math:
            gameLogic = new GameLogicMathMath( gameID, difficulty, randomSeed );
            break;
        case Game_Math_WhichBig:
            gameLogic = new GameLogicMathBigger( gameID, difficulty, randomSeed );
            break;
        case Game_Math_DiceSum:
            gameLogic = new GameLogicMathDice( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_HowMany:
            gameLogic = new GameLogicDecisionHowMany( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_Hand:
            gameLogic = new GameLogicDecisionHand( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_NoExistChar:
            gameLogic = new GameLogicDecisionNoExistChar( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_TapShape:
            gameLogic = new GameLogicDecisionTapShape( gameID, difficulty, randomSeed );
            break;
        case Game_Action_TapScreen:
            gameLogic = new GameLogicActionTapScreen( gameID, difficulty, randomSeed );
            break;
        case  Game_Action_Spark:
            gameLogic = new GameLogicActionSpark( gameID, difficulty, randomSeed );
            break;
        case Game_Action_ShootUFO:
            gameLogic = new GameLogicActionShootUFO( gameID, difficulty, randomSeed );
            break;
        case Game_Action_SwipeArrow:
            gameLogic = new GameLogicSwipeArrow( gameID, difficulty, randomSeed );
            break;
        
        case Game_Memory_Pair:
            gameLogic = new GameLogicMemoryPair( gameID, difficulty, randomSeed );
            break;
        case  Game_Memory_MissShape:
            gameLogic = new GameLogicMemeoryMissItem( gameID, difficulty, randomSeed );
            break;
        case Game_Memory_NewShape:
            gameLogic = new GameLogicMemoryNewItem( gameID, difficulty, randomSeed );
            break;
        case Game_Memory_Order:
            gameLogic = new GameLogicMemoryOrder( gameID, difficulty, randomSeed );
            break;

        case Game_Resolve_Headup:
            gameLogic = new GameLogicResolveHeadup( gameID, difficulty, randomSeed );
            break;
        case Game_Resolve_Maze:
            gameLogic = new GameLogicResolveMaze( gameID, difficulty, randomSeed );
            break;
        case Game_Resolve_Number:
            gameLogic = new GameLogicResolveNumber( gameID, difficulty, randomSeed );
            break;
        case Game_Resolve_RotatePuzzle:
            gameLogic = new GameLogicResolveRotatePuzzle( gameID, difficulty, randomSeed );
            break;

        }

        //gameLogic = new GameLogicActionSpark( dif );
        /*
        switch(dif%5){
        case 0:
            gameLogic = new GameLogicMathSum( dif );
            break;
        case 1:
            gameLogic = new GameLogicDecisionHowMany( dif );
            break;
        case 2:
            gameLogic = new GameLogicMemoryOrder( dif );
            break;
        case 3:
            gameLogic = new GameLogicResolveNumber( dif );
            break;
        case 4:
            gameLogic = new GameLogicActionShootUFO( dif );
            break;
        }
        */
        //dif=7;
        dif=5;
        if(dif==20) {
            dif=0;
        }
        //gameLogic = new GameLogicResolveMaze(Game_Action_Spark, dif, randomSeed);

        return gameLogic;
    }

    // --------------------------------------------------------------------------------------------------------
    public const int Status_Waiting = 0;
    public const int Status_Playing = 1;
    public const int Status_Gameover = 2;
    protected int _status;
    public int status {
        get {
            return _status;
        }
    }

    protected GameController _gameController;
    protected GameObject _boardArea;

    protected int _difficulty;
    public int difficulty {
        get {
            return _difficulty;
        }
    }

    protected int _gameID;
    public int gameID {
        get {
            return _gameID;
        }
    }


    protected List<GameObject> _goList;

    protected int _seed;
    public int seed {
        get {
            return _seed;
        }
    }

    public GameLogic( int id, int diff, int seedValue ) {
        _gameID = id;

        _difficulty = diff;

        _goList = new List<GameObject>();

        _seed = seedValue;

        KWUtility.SetRandomSeed( _seed );

        _status = Status_Waiting;
    }

    public  virtual void SetGameController( GameController gameController ) {
        _gameController = gameController;
        _boardArea = _gameController.goBoardArea;

        _gameController.Reset();
    }

    public virtual void StartGame() {
        _status = Status_Playing;
    }

    public virtual bool IsTimerRunning() {
        return _status==Status_Playing;
    }

    public virtual void OnButtonPressed( int buttonIndex ) {
        
    }

    public virtual void OnBoardTapped( Vector3 pos ) {
        
    }

    public virtual void OnTouchSwipe( int dir ) {
    }

    public virtual void Clear() {
        foreach( GameObject go in _goList ) {
            GameObject.Destroy( go );
        }
        _goList.Clear();

        _status = Status_Gameover;
    }

    public virtual void Update() {
    }

    public virtual void FixedUpdate() {
    }

    public void SetGameTimeout() {
        _status = Status_Gameover;

        _gameController.SendGameResult( GameController.GameResult_Timeout );
    }
	
}
