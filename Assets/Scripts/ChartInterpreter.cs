using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script will load the current chart from a file, interpret it, hold it in memory, 
// and tell the NoteSpawner when to spawn the notes.

public class ChartInterpreter : MonoBehaviour
{
    public static ChartInterpreter Instance {get; private set;}

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
