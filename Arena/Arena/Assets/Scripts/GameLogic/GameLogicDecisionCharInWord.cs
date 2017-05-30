using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogicDecisionCharInWord : GameLogicThreeButtons {

    static string[] Words = {
        "ONE", "TWO", "THREE","FOUR","FIVE","SIX","SEVEN","EIGHT","NINE","TEN","ELEVEN","TWELVE","THIRTEEN",
        "WHITE","BLACK","YELLOW","GREEN","PURPLE","PINK",  "BLUE",  "ORANGE",  
        "MONDAY",  "TUESDAY",  "WEDNESDAY",  "THURSDAY",  "FRIDAY",  "SATURDAY",  "SUNDAY",  
        "PUT",  "GET",  "RUN",  "JUMP",  "SWIM",  "baseball",  "tennis",  "basketball", "football", "soccer", "polo", "diving", "surfing", "boxing", "hockey",
        "DOG",  "COW",  "DUCK",  "HORSE",  "MONCKY",  "DONCKY",  "COCK",  "CHICK", "MULE", "BULL", "PIG", "GOAT", "ZEBRA", "DEER", "ELEPHANT","CAMEL","GIRAFFE",
        "FISH",  "SQUIRREL",  "KANGAROO",  "LOBSTER",  "CUTTLEFISH",  "CROCODILE",  "GORILLA",  "LEOPARD", "RABBIT",  "TORTOISE",  "PENGUIN",  "BUFFALO", 
        "COBRA",  "CONDOR",  "EAGLE",  "GOOSE", "KOALA", "LION", "LIZARD", "MOUSE", "PUMA", "SNAKE", "SALMON", "SARDINE", "SEAL", "SHARK", "TOAD", "WHALE", 
        "January",  "February",  "March",  "April",  "May",  "June",  "July",  "August",  "September",  "October",  "November",  "December", 
        "GameArena",  "BEAUTIFUL",  "CAREFULLY",  "ACHIEVEMENT",  "Washington"
    };

    static List<string>[] WordListArray;

    static GameLogicDecisionCharInWord() {
        WordListArray = new List<string>[7];
        for( int m=0; m<7; m++ ) {
            WordListArray[m] = new List<string>();
        }
        for(int n=0;n<Words.Length;n++ ) {
            int length = Words[n].Length-3;
            if(length>6){
                length=6;
            }

            WordListArray[length].Add( Words[n].ToUpper() );
        }
    }

    public GameLogicDecisionCharInWord( int gameID, int difficulty, int randomSeed  ) : base(gameID,difficulty,randomSeed)  {
    }


    public override void SetGameController( GameController controller ) {
        base.SetGameController( controller );

        _gameController.SetGameDescription1( 0,"How many charactors" );
        _gameController.SetGameDescription2( 0,44,"in the word?" );

        _gameController.SetColorIndex( 4 );

        int targetWord = 0;

        switch(_difficulty) {
        case 0:
            targetWord=KWUtility.Random( 0, 3);
            break;
        case 1:
            targetWord=2+KWUtility.Random( 0, 3);
            break;
        case 2:
            targetWord=3+KWUtility.Random( 0, 3);
            break;
        default:
            targetWord=4+KWUtility.Random( 0, 3);
            break;
        }

        string word = WordListArray[targetWord][KWUtility.Random(0, WordListArray[targetWord].Count)];

        _gameController.SetMainText( word, Color.white );

        int result = KWUtility.Random(0,3);
        if(word.Length==3) {
            result = 2;
        }
        if(word.Length==4) {
            result = KWUtility.Random( 0, 2)+1;
        }

        switch(result){
        case 0:
            SetButtonsRandom( word.Length.ToString(), (word.Length-2).ToString(), (word.Length-1).ToString() );
            break;
        case 1:
            SetButtonsRandom( word.Length.ToString(), (word.Length+1).ToString(), (word.Length-1).ToString() );
            break;
        case 2:
            SetButtonsRandom( word.Length.ToString(), (word.Length+2).ToString(), (word.Length+1).ToString() );
            break;
        }

    }
}
