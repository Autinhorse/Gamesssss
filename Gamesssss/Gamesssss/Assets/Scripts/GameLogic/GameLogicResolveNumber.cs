using UnityEngine;
using System.Collections;

using UnityEngine.UI;


class Question {
    public int[] numbers;
    public int answer;
    public int wrong1;
    public int wrong2;
}

public class GameLogicResolveNumber : GameLogicThreeButtons {

    const int MapBlockDelta = 24;
    int MapBlockSize;

    int _mapWidth;
    int _mapHeight;

    public GameLogicResolveNumber( int difficulty ) : base(difficulty) {
    }

    // 难度0-15
    // 难度0-1 题目1，1
    // 难度2-3 题目1，2
    // 难度4-5 题目2，3
    // 难度6-8 题目2，3，4
    // 难度9-11 题目3，4，5
    // 难度12-15 题目3,4，5，6

    // 题目 1 ：正序递增
    // 题目 2 ：2倍递增
    // 题目 3 ：3倍递增
    // 题目 4 ：增量递增
    // 题目 5 ：2倍增量递增
    // 题目 6 ：平方递增递增


    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameName( "PATTERNS" );
        _gameController.SetGameDescription1( 0, "Which number fills the pattern?" );

        _gameController.SetColorIndex( 1 );

        MapBlockSize = _gameController.boardWidth/7;

        int minPuzzle=1;
        int maxPuzzle=1;

        switch(_difficulty){
        case 0:
        case 1:
            minPuzzle = 1;
            maxPuzzle = 1;
            break;
        case 2:
        case 3:
            minPuzzle = 1;
            maxPuzzle = 2;
            break;
        case 4:
        case 5:
            minPuzzle = 2;
            maxPuzzle = 3;
            break;
        case 6:
        case 7:
        case 8:
            minPuzzle = 2;
            maxPuzzle = 4;
            break;
        case 9:
        case 10:
        case 11:
            minPuzzle = 3;
            maxPuzzle = 5;
            break;
        default:
            minPuzzle = 4;
            maxPuzzle = 6;
            break;
        }

        int questionIndex = KWUtility.Random( minPuzzle, maxPuzzle+1);

        Question ques = new Question();
        ques.numbers = new int[5];
        int startNumber;
        int emptyIndex = KWUtility.Random( 0, 5 );
        int increametal;

        switch(questionIndex) {
        case 1:
            startNumber = KWUtility.Random( 5, 25 );
            for(int m=0;m<5;m++ ) {
                ques.numbers[m]=startNumber+m;
            }

            if(KWUtility.Random(0,2)==0) {
                ques.answer=ques.numbers[0];
                ques.wrong1=ques.answer-1;
                ques.wrong2=ques.answer-2;
                ques.numbers[0]=-1;
            }
            else {
                ques.answer=ques.numbers[4];
                ques.wrong1=ques.answer-1;
                ques.wrong2=ques.answer-2;
                ques.numbers[4]=-1;
            }
            break;
        case 2:
        case 3:
            startNumber = KWUtility.Random( 5, 25 );
            for(int m=0;m<5;m++ ) {
                ques.numbers[m]=startNumber+m*questionIndex;
            }


            switch(emptyIndex) {
            case 0:
                if(KWUtility.Random(0,2)==0) {
                    ques.answer=ques.numbers[0];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer-2;
                    ques.numbers[0]=-1;
                }
                else {
                    ques.answer=ques.numbers[0];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer+1;
                    ques.numbers[0]=-1;
                }
                break;
            case 1:
            case 2:
            case 3:
                ques.answer=ques.numbers[emptyIndex];
                ques.wrong1=ques.answer-1;
                ques.wrong2=ques.answer+1;
                ques.numbers[emptyIndex]=-1;
                break;
            case 4:
                if(KWUtility.Random(0,2)==0) {
                    ques.answer=ques.numbers[4];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer-2;
                    ques.numbers[4]=-1;
                }
                else {
                    ques.answer=ques.numbers[4];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer+1;
                    ques.numbers[4]=-1;
                }
                break;
            }
            break;
        case 4:
            startNumber = KWUtility.Random( 5, 10 );
            increametal=1;
            for(int m=0;m<5;m++ ) {
                ques.numbers[m]=startNumber+increametal;
                startNumber+=increametal;
                increametal++;
            }
            switch(emptyIndex) {
            case 0:
                ques.answer=ques.numbers[0];
                ques.wrong1=ques.answer-1;
                ques.wrong2=ques.answer-2;
                ques.numbers[0]=-1;
                break;
            case 1:
            case 2:
            case 3:
                ques.answer=ques.numbers[emptyIndex];
                ques.wrong1=ques.answer-1;
                ques.wrong2=ques.answer+1;
                ques.numbers[emptyIndex]=-1;
                break;
            case 4:
                if(KWUtility.Random(0,2)==0) {
                    ques.answer=ques.numbers[4];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer-2;
                    ques.numbers[4]=-1;
                }
                else {
                    ques.answer=ques.numbers[4];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer+1;
                    ques.numbers[4]=-1;
                }
                break;
            }
            break;
        case 5:
            startNumber = KWUtility.Random( 5, 10 );
            increametal=1;
            for(int m=0;m<5;m++ ) {
                ques.numbers[m]=startNumber+increametal;
                startNumber+=increametal;
                increametal*=2;
            }
            switch(emptyIndex) {
            case 0:
                if(KWUtility.Random(0,2)==0) {
                    ques.answer=ques.numbers[0];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer-2;
                    ques.numbers[0]=-1;
                }
                else {
                    ques.answer=ques.numbers[0];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer+1;
                    ques.numbers[0]=-1;
                }
                break;
            case 1:
            case 2:
            case 3:
                ques.answer=ques.numbers[emptyIndex];
                ques.wrong1=ques.answer-1;
                ques.wrong2=ques.answer+1;
                ques.numbers[emptyIndex]=-1;
                break;
            case 4:
                if(KWUtility.Random(0,2)==0) {
                    ques.answer=ques.numbers[4];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer-2;
                    ques.numbers[4]=-1;
                }
                else {
                    ques.answer=ques.numbers[4];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer+1;
                    ques.numbers[4]=-1;
                }
                break;
            }
            break;
        case 6:
            startNumber = KWUtility.Random( 5, 10 );
            increametal=1;
            for(int m=0;m<5;m++ ) {
                ques.numbers[m]=startNumber+(m+1)*(m+1);
                startNumber+=(m+1)*(m+1);
            }
            switch(emptyIndex) {
            case 0:
                ques.answer=ques.numbers[0];
                ques.wrong1=ques.answer-1;
                ques.wrong2=ques.answer-2;
                ques.numbers[0]=-1;
                break;
            case 1:
            case 2:
            case 3:
                ques.answer=ques.numbers[emptyIndex];
                ques.wrong1=ques.answer-1;
                ques.wrong2=ques.answer+1;
                ques.numbers[emptyIndex]=-1;
                break;
            case 4:
                if(KWUtility.Random(0,2)==0) {
                    ques.answer=ques.numbers[4];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer-2;
                    ques.numbers[4]=-1;
                }
                else {
                    ques.answer=ques.numbers[4];
                    ques.wrong1=ques.answer-1;
                    ques.wrong2=ques.answer+1;
                    ques.numbers[4]=-1;
                }
                break;
            }
            break;
        }

        if(KWUtility.Random(0,2)==0) {
            int temp;
            temp = ques.numbers[0];
            ques.numbers[0]=ques.numbers[4];
            ques.numbers[4]=temp;
            temp = ques.numbers[1];
            ques.numbers[1]=ques.numbers[3];
            ques.numbers[3]=temp;
        }

        _mapWidth = 5;
        _mapHeight = 1;

        for(int m =0; m<_mapWidth; m++ ) {
            Vector2 pos = GetPosition( m, 0 );
            Image imgBoard = (Image) GameObject.Instantiate( _gameController.goBoardImage );
            imgBoard.gameObject.SetActive( true );
            imgBoard.transform.SetParent( _gameController.goBoardArea.transform );
            imgBoard.color = new Color( 0, 0, 0, 0.2f );
                
            imgBoard.rectTransform.sizeDelta = new Vector2( MapBlockSize, MapBlockSize );
            imgBoard.rectTransform.localPosition = new Vector3( pos.x, pos.y, 0 );
            imgBoard.rectTransform.localScale = Vector3.one;
            _goList.Add( imgBoard.gameObject );

            Text txtChar = (Text) GameObject.Instantiate( _gameController.goBoardChar );
            txtChar.gameObject.SetActive( true );
            txtChar.transform.SetParent( imgBoard.gameObject.transform );
            txtChar.rectTransform.localPosition = Vector3.zero;
            txtChar.rectTransform.localScale = Vector3.one;
            txtChar.color = Color.white;
            if(ques.numbers[m]==-1) {
                txtChar.text = "?";
            }
            else {
                txtChar.text = ques.numbers[m].ToString();
            }

        }


        SetButtonsRandom( ques.answer.ToString(), ques.wrong1.ToString(), ques.wrong2.ToString() );
    }

    Vector2 GetPosition( int x, int y ) {
        return new Vector2( (-1*(_mapWidth-1.0f)/2+x)*(MapBlockSize+MapBlockDelta), ((_mapHeight-1.0f)/2-y)*(MapBlockSize+MapBlockDelta));
    }
}
