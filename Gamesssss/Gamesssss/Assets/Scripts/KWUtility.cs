using UnityEngine;
using System.Collections;
using System;

public class KWUtility  {

    public static int Random( int min, int max ) {
        int result = UnityEngine.Random.Range( min, max );
        return result;
    }
}
