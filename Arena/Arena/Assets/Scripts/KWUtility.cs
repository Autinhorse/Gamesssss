using UnityEngine;
using System.Collections;
using System;

public class KWUtility  {

    public static int Random( float min, float max ) {
        int result = UnityEngine.Random.Range( (int)min, (int)max );
        return result;
    }

    public static void SetRandomSeed( int seed ) {
        UnityEngine.Random.InitState( seed );
    }
}
