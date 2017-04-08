using UnityEngine;
using System.Collections;

using UnityEngine.UI;

using DG.Tweening;

public class GameLogicMathBigger : GameLogic {

    public GameLogicMathBigger( int difficulty ) : base(difficulty) {
    }

    // 难度0-1，只有两个数比较
    // 难度2-4，一位数加法和一个两位数
    // 难度5-7，增加两位数减一位数
    // 难度8-12，两个加／减法算式
    // 难度13-16，有混合运算了

    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameNameAndDescription( "Bigger", "Tap the bigger one", null );

        _gameController.SetColorIndex( 0 );
        string button1="";
        string button2="";
        int result1=0;
        int result2=0;

        if(_difficulty<2) {
            result1 = UnityEngine.Random.Range( 15, 25 );
            do {
                result2 = UnityEngine.Random.Range( 15, 25 );
            }while(result1==result2);

            button1 = result1.ToString();
            button2 = result2.ToString();
        }  
        else if(_difficulty<5 ) {
            int number1, number2;
            number1 = KWUtility.Random( 3, 8+_difficulty );
            number2 = KWUtility.Random( 3, 8+_difficulty );

            result1 = number1 + number2;
            button1 = number1.ToString()+" + "+number2.ToString();

            switch(KWUtility.Random(0,4)) {
            case 0:
            case 1:
                result2=result1+KWUtility.Random(1,3);
                break;
            case 2:
            case 3:
                result2=result1-KWUtility.Random(1,3);
                break;
            }

            button2=result2.ToString();

            if(KWUtility.Random(0,2)==0) {
                int temp = result1;
                result1=result2;
                result2=temp;
                string tempStr = button1;
                button1=button2;
                button2=tempStr;
            }
        }
        else if(_difficulty<8 ) {
            int number1, number2;
            number1 = KWUtility.Random( _difficulty, 8+_difficulty*3/2 );
            number2 = KWUtility.Random( _difficulty, 8+_difficulty*3/2 );

            if(KWUtility.Random(0,2)==0) {
                result1 = number1 + number2;
                button1 = number1.ToString()+" + "+number2.ToString();
            }
            else {
                result1 = number2;
                button1 = (number1+number2).ToString()+" - "+number1.ToString();
            }

            switch(KWUtility.Random(0,4)) {
            case 0:
            case 1:
                result2=result1+KWUtility.Random(1,3);
                break;
            case 2:
            case 3:
                result2=result1-KWUtility.Random(1,3);
                break;
            }

            button2=result2.ToString();

            if(KWUtility.Random(0,2)==0) {
                int temp = result1;
                result1=result2;
                result2=temp;
                string tempStr = button1;
                button1=button2;
                button2=tempStr;
            }
        }
        else if(_difficulty<13 ) {
            int number1, number2;
            number1 = KWUtility.Random( _difficulty/2, 8+_difficulty );
            number2 = KWUtility.Random( _difficulty/2, 8+_difficulty );

            if(KWUtility.Random(0,2)==0) {
                result1 = number1 + number2;
                button1 = number1.ToString()+" + "+number2.ToString();
            }
            else {
                result1 = number2;
                button1 = (number1+number2).ToString()+" - "+number1.ToString();
            }

            do {
                number1 = KWUtility.Random( _difficulty/2, 8+_difficulty );
                number2 = KWUtility.Random( _difficulty/2, 8+_difficulty );

                if(KWUtility.Random(0,2)==0) {
                    result2 = number1 + number2;
                    button2 = number1.ToString()+" + "+number2.ToString();
                }
                else {
                    result2 = number2;
                    button2 = (number1+number2).ToString()+" - "+number1.ToString();
                }
            }while( result1==result2);
        }
        else  {
            int number1, number2, number3;
            number1 = KWUtility.Random( _difficulty/2, 8+_difficulty );
            number2 = KWUtility.Random( _difficulty/2, 8+_difficulty );
            number3 = KWUtility.Random( _difficulty/2, 8+_difficulty );

            if(KWUtility.Random(0,2)==0) {
                result1 = number1 + number2;
                button1 = number1.ToString()+" + "+number2.ToString();
            }
            else {
                result1 = number2;
                button1 = (number1+number2).ToString()+" - "+number1.ToString();
            }

            if(KWUtility.Random(0,2)==0) {
                result1 = result1 + number3;
                button1 = button1+" + "+number3.ToString();
            }
            else {
                result1 = result1-number3;
                button1 = button1+" - "+number3.ToString();
            }

            do {
                number1 = KWUtility.Random( _difficulty/2, 8+_difficulty );
                number2 = KWUtility.Random( _difficulty/2, 8+_difficulty );
                number3 = KWUtility.Random( _difficulty/2, 8+_difficulty );

                if(KWUtility.Random(0,2)==0) {
                    result2 = number1 + number2;
                    button1 = number1.ToString()+" + "+number2.ToString();
                }
                else {
                    result2 = number2;
                    button1 = (number1+number2).ToString()+" - "+number1.ToString();
                }

                if(KWUtility.Random(0,2)==0) {
                    result2 = result2 + number3;
                    button1 = button1+" + "+number3.ToString();
                }
                else {
                    result2 = result2-number3;
                    button1 = button1+" - "+number3.ToString();
                }
            }while(result1==result2);
        }



        Button button = (Button) GameObject.Instantiate( _gameController.goBoardButton);
        button.gameObject.SetActive( true );
        button.transform.SetParent( _gameController.goBoardButtonArea.transform );

        button.onClick.AddListener(delegate() { 
            if(result1>result2){
                _gameController.SendGameResult( true );
            }
            else {
                _gameController.SendGameResult( true );
            }
        });

        RectTransform rect = (RectTransform) button.GetComponent<RectTransform>();

        rect.sizeDelta =  new Vector2( _gameController.boardWidth/2, _gameController.boardWidth/8 );
        rect.localPosition = new Vector3( 0, _gameController.boardWidth*0.2f, 0 );
        rect.localScale = Vector3.one;
        _goList.Add( button.gameObject );

        Text buttonText = (Text)button.transform.FindChild( "Text" ).GetComponent<Text>();
        buttonText.gameObject.SetActive( true );
        buttonText.text = button1;
        buttonText.color = Color.white;
        /*
        button = (Button) GameObject.Instantiate( _gameController.goBoardButton);
        button.gameObject.SetActive( true );
        button.transform.SetParent( _gameController.goBoardButtonArea.transform );

        button.onClick.AddListener(delegate() { 
            if(result1==result2){
                _gameController.SendGameResult( true );
            }
            else {
                _gameController.SendGameResult( true );
            }
        });

        rect = (RectTransform) button.GetComponent<RectTransform>();

        rect.sizeDelta =  new Vector2( _gameController.boardWidth/2, _gameController.boardWidth/8 );
        rect.localPosition = new Vector3( 0, 0, 0 );
        rect.localScale = Vector3.one;
        _goList.Add( button.gameObject );

        buttonText = (Text)button.transform.FindChild( "Text" ).GetComponent<Text>();
        buttonText.gameObject.SetActive( true );
        buttonText.text = "EQUAL";
        buttonText.color = Color.white;
*/

        button = (Button) GameObject.Instantiate( _gameController.goBoardButton);
        button.gameObject.SetActive( true );
        button.transform.SetParent( _gameController.goBoardButtonArea.transform );

        button.onClick.AddListener(delegate() { 
            if(result1<result2){
                _gameController.SendGameResult( true );
            }
            else {
                _gameController.SendGameResult( true );
            }
        });

        rect = (RectTransform) button.GetComponent<RectTransform>();

        rect.sizeDelta =  new Vector2( _gameController.boardWidth/2, _gameController.boardWidth/8 );
        rect.localPosition = new Vector3( 0, _gameController.boardWidth*-0.2f, 0 );
        rect.localScale = Vector3.one;
        _goList.Add( button.gameObject );

        buttonText = (Text)button.transform.FindChild( "Text" ).GetComponent<Text>();
        buttonText.gameObject.SetActive( true );
        buttonText.text = button2;
        buttonText.color = Color.white;

    }


}
