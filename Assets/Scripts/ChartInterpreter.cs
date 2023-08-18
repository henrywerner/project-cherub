using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

// This script will load the current chart from a file, interpret it, hold it in memory, 
// and tell the NoteSpawner when to spawn the notes.

public class ChartInterpreter : MonoBehaviour
{
    public static ChartInterpreter Instance {get; private set;}
    public string ChartFilename;
    private Chart _chart;
    [SerializeField] TextAsset chartFile; // TODO: make this automatic

    private int _nextNoteIndex = 0; // the index of the next note to load

    private Queue<Note> _noteBuffer = new Queue<Note>(); // buffer for upcoming note spawns
    [SerializeField] private int _noteBufferMaxSize = 300; // maximum size the buffer can be
    [SerializeField] private int _noteBufferChunkSize = 50; // amount of notes to load into buffer at a time

    private Conductor m_Conductor;

    private void Awake() {
        // Singleton logic
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        // Load chart (maybe move this somewhere else?)
        LoadChartFromFile(ChartFilename);
        LoadChunkToBuffer();
    }

    private void Start() {
        m_Conductor = Conductor.Instance;
    }

    private void Update() {
        if (_noteBuffer.Count == 0)
            return;

        Note nextUp = _noteBuffer.Peek(); // I would love to use try peek

        float currentBeat = m_Conductor.songPositionInBeats + m_Conductor.highwayTripDurationInBeats;

        while (nextUp.b <= currentBeat) { // TODO: this might need leeway
            // Spawn current note
            NoteSpawner.Instance.SpawnNote(nextUp.l, nextUp.b); // FIXME: this sucks
            _noteBuffer.Dequeue();

            if (_noteBuffer.Count >= 1) {
                nextUp = _noteBuffer.Peek();
            } else {
                return;
            }
        }
    }

    private void LoadChunkToBuffer() {
        for (int i = 0; i < _noteBufferChunkSize; i++)
        {
            if (_nextNoteIndex >= _chart.notes.Length)
                return;

            if (_noteBuffer.Count >= _noteBufferMaxSize)
                return;

            Note nextNote = _chart.notes[_nextNoteIndex];

            _noteBuffer.Enqueue(nextNote);

            // add hold note children to queue
            if (nextNote.t == 1) {
                // I'm allowing the children of hold notes to exceed the buffer length. This might break things lmao.
                foreach (NoteChild child in nextNote.e)
                {
                    Note kid = new Note(child.b, nextNote.l, child.h, nextNote.t);
                    _noteBuffer.Enqueue(kid);
                }
            } 

            _nextNoteIndex++;
        }
    }

    // in the future i'll make this look for ChartFolder > Song Folder > DIFFICULTY.dat
    // but for now i'll just have it point to a file I guess.
    private void LoadChartFromFile(string fileName) {
        string filePath = "Charts/" + fileName + ".json";
        // TextAsset file = Resources.Load<TextAsset>(filePath);
        TextAsset file = chartFile; // TODO: change this later
        _chart = JsonUtility.FromJson<Chart>(file.text);


        if (_chart.notes[0] != null) {
            Debug.Log("🟢 Chart loaded");
        } else {
            Debug.Log("🔴 Chart not loaded");
        }
    }

    public float GetSongDuration() {
        return _chart.songend;
    }
}

[System.Serializable]
public class Chart 
{
    public string version;
    public float songend;
    public Note[] notes;
}

[System.Serializable]
public class Note
{
    // BEAT - Beat that the note is on
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
    public NoteChild[] e;

    // I hope constructors don't break the JSON parsing...
    public Note(float beat, int lane, int highway, int type) {
        b = beat;
        l = lane;
        h = highway;
        t = type;
    }
}

[System.Serializable]
public class NoteChild
{
    public float b; 
    public int h;
}
