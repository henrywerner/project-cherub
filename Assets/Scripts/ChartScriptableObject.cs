using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Figure out how to properly write this shit

public class ChartScriptableObject : ScriptableObject
{
    // BEAT - Beat that the note is one
    public float b; 

    // LANE - 0: left, 1: right, 2: both
    public int l;

    // HIGHWAY - 0, 1, 2
    public int h;

    // TYPE - Note type - 0: normal, 1: hold, 2: hidden
    public int t;

    // CHILDREN - (IF HOLD NOTE) How many children does the hold have
    public int c;

    // ADDITIONAL
    public ChartScriptableObject[] e;
}
