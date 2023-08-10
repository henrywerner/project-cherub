using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance { get; private set; }
    public int NotesHit { get; private set; }
    public int NotesMissed { get; private set; }
    public int TotalNotes { get; private set; }
    public float SongAccuracy { get; private set; }
    public int SongScore { get; private set; }
    public bool IsFullCombo { get; private set; }


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    
    private void Start() {
        NotesHit = 0;
        NotesMissed = 0;
        SongAccuracy = 1f;
        SongScore = 0;
        IsFullCombo = true;
    }

    public void JudgeNote(int noteID, float noteTiming, float hitTiming) {
        // noteID - some id to know what note is being judged
        // noteTiming - the time code the note is placed at
        // hitTiming - the time code that the hit was recorded
    }
}
