using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic  {

    // Factory of game logic instance

    public const int Game_Math_Sum = 0;
    public const int Game_Math_RightWrong = 1;
    public const int Game_Math_WhichBig = 2;
    public const int Game_Math_DiceSum = 3;
    public const int Game_Math_SumItem = 4;
    public const int Game_Math_EvenOdd = 5;
    public const int Game_Math_SmallToBig = 6;
    public const int Game_Math_Sign = 7;

    public const int Game_Decision_HowMany = 8;
    public const int Game_Decision_LeftRight = 9;
    public const int Game_Decision_SameBlock = 10;
    public const int Game_Decision_TapSpecial = 11;
    public const int Game_Decision_Category = 12;
    public const int Game_Decision_WordColor = 13;
    public const int Game_Decision_NoExistChar = 14;
    public const int Game_Decision_NoExistShape = 15;

    public const int Game_Action_TapScreen = 16;
    public const int Game_Action_AA = 17;
    public const int Game_Action_ShootUFO = 18;
    public const int Game_Action_Frog = 19;
    public const int Game_Action_ArrowShoot = 20;
    public const int Game_Action_TopMovingObject = 21;
    public const int Game_Action_0101 = 22;
    public const int Game_Action_MoveToTarget = 23;

    public const int Game_Memory_TapScreen = 24;
    public const int Game_Memory_MissShape = 25;
    public const int Game_Memory_AddShape = 26;
    public const int Game_Memory_Tap = 27;
    public const int Game_Memory_HideShape = 28;
    public const int Game_Memory_TurnNumber = 29;
    public const int Game_Memory_ChangeShape = 30;
    public const int Game_Memory_MineField = 31;

    public const int Game_Resolve_Mirror = 32;
    public const int Game_Resolve_MoveObject = 33;
    public const int Game_Resolve_ConnectTube = 34;
    public const int Game_Resolve_Rotate = 35;
    public const int Game_Resolve_TopView = 36;
    public const int Game_Resolve_SideView = 37;
    public const int Game_Resolve_Heaviest = 38;
    public const int Game_Resolve_MovePicture = 39;

    static int dif = 0;

    public static GameLogic GetGameLogic( int gameID, int difficulty ) {
        GameLogic gameLogic = null;


        switch( gameID ) {
        case Game_Math_Sum:
            gameLogic = new GameLogicMathSum( difficulty );
            //gameLogic = new GameLogicDecisionHand( difficulty );
            break;
        case Game_Math_RightWrong:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Math_WhichBig:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Math_DiceSum:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Math_SumItem:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Math_EvenOdd:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Math_SmallToBig:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Math_Sign:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Decision_HowMany:
            gameLogic = new GameLogicDecisionHowMany( difficulty );
            break;
        case Game_Decision_LeftRight:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Decision_SameBlock:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Decision_TapSpecial:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Decision_Category:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Decision_WordColor:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Decision_NoExistChar:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Decision_NoExistShape:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Action_TapScreen:
            gameLogic = new GameLogicActionTapScreen( difficulty );
            break;
        case  Game_Action_AA:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Action_ShootUFO:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Action_Frog:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Action_ArrowShoot:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Action_TopMovingObject:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Action_0101:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Action_MoveToTarget:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Memory_TapScreen:
            gameLogic = new GameLogicMemoryPair( difficulty );
            break;
        case  Game_Memory_MissShape:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Memory_AddShape:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Memory_Tap:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Memory_HideShape:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case  Game_Memory_TurnNumber:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Memory_ChangeShape:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Memory_MineField:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Resolve_Mirror:
            gameLogic = new GameLogicResolveHeadup( difficulty );
            break;
        case Game_Resolve_MoveObject:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Resolve_ConnectTube:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Resolve_Rotate:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Resolve_TopView:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Resolve_SideView:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Resolve_Heaviest:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        case Game_Resolve_MovePicture:
            gameLogic = new GameLogicMathSum( difficulty );
            break;
        }

        gameLogic = new GameLogicMathDice( dif );
        dif++;


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

    protected List<GameObject> _goList;

    public GameLogic( int difficulty ) {
        _difficulty = difficulty;

        _goList = new List<GameObject>();

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

    public virtual void Clear() {
        foreach( GameObject go in _goList ) {
            GameObject.Destroy( go );
        }
        _goList.Clear();
    }

    public virtual void Update() {
    }

    public virtual void FixedUpdate() {
    }
	
}
