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

    public const int Game_Action_Tap = 20;
    public const int Game_Action_Swipe = 21;

    public const int Game_Decision_HowManyNumber = 22;
    public const int Game_Decision_NoExistNumber = 23;
    public const int Game_Decision_NoExistShape = 24;
    public const int Game_Decision_CharInWord = 25;
    public const int Game_Decision_YES = 26;
    public const int Game_Action_01 = 27;
    public const int Game_Decision_TapNumber = 28;
    public const int Game_Decision_BigToSmall = 29;
    public const int Game_Action_TapAlien = 30;
    public const int Game_Action_TapBall = 31;
    public const int Game_Action_TapTurnRight = 32;
    public const int Game_Resolve_TapLetter = 33;
    public const int Game_Memory_Shape = 34;



    static int dif = 20;

    public static GameLogic GetGameLogic( int gameID, int difficulty, int randomSeed ) {
        GameLogic gameLogic = null;

        difficulty=3;

       

        gameID = dif;

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
        case Game_Action_Tap:
            gameLogic = new GameLogicActionTap( gameID, difficulty, randomSeed );
            break;
        case Game_Action_Swipe:
            gameLogic = new GameLogicActionSwipe( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_HowManyNumber:
            gameLogic = new GameLogicDecisionHowManyNumber( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_NoExistNumber:
            gameLogic = new GameLogicDecisionNoExistNumber( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_NoExistShape:
            gameLogic = new GameLogicDecisionNoExistShape( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_CharInWord:
            gameLogic = new GameLogicDecisionCharInWord( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_YES:
            gameLogic = new GameLogicDecisionYes( gameID, difficulty, randomSeed );
            break;
        case Game_Action_01:
            gameLogic = new GameLogicAction01( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_TapNumber:
            gameLogic = new GameLogicDecisionTapNumber( gameID, difficulty, randomSeed );
            break;
        case Game_Decision_BigToSmall:
            gameLogic = new GameLogicDecisionBigToSmall( gameID, difficulty, randomSeed );
            break;
        case Game_Action_TapAlien:
            gameLogic = new GameLogicActionTapAlien( gameID, difficulty, randomSeed );
            break;
        case Game_Action_TapBall:
            gameLogic = new GameLogicActionTapBall( gameID, difficulty, randomSeed );
            break;
        case Game_Action_TapTurnRight:
            gameLogic = new GameLogicActionTurnRight( gameID, difficulty, randomSeed );
            break;    
        case Game_Resolve_TapLetter:
            gameLogic = new GameLogicResolveTapLetter( gameID, difficulty, randomSeed );
            break;    
        case Game_Memory_Shape:
            gameLogic = new GameLogicMemoryShape( gameID, difficulty, randomSeed );
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
       
        dif++;
        if(dif==26) {
            dif=0;
        }

        //gameLogic = new GameLogicMemoryShape(dif, 3, randomSeed);

        return gameLogic;
    }

    protected const float StartTime = 15.0f;
    protected const float TimeDeltaPerDifficultLevel = 0.5f;
    protected const float MinGameTime = 10.0f;

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

    protected float _totalGameTime;
    protected float _timer;

    public float timer {
        get {
            return _timer;
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
        if(_difficulty>3) {
            _difficulty = 3;
        }

        _totalGameTime = StartTime - TimeDeltaPerDifficultLevel * _difficulty;
        if(_totalGameTime<MinGameTime) {
            _totalGameTime = MinGameTime;
        }
        _timer = _totalGameTime;

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

    public virtual void StopGame() {
        _status = Status_Gameover;
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

    public virtual void SetGameTimeout() {
        _status = Status_Gameover;

        _gameController.SendGameResult( GameController.GameResult_Timeout );
    }

    public virtual float GetTimePercentage() {
        return _timer/_totalGameTime;
    }
	
}
