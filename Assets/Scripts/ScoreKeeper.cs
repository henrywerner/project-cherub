using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance { get; private set; }
    public int NotesHit { get; private set; }
    public int NotesMissed { get; private set; }
    public int TotalNotes { get; private set; }
    public float SongAccuracy {
        get {
            if (TotalNotes == 0)
                return 1f;
            if (NotesHit == 0)
                return 0f;
            return TotalNotes / (float)NotesHit;
        }
    }
    public int SongScore { get; private set; } //TODO: add scoring
    public bool IsFullCombo => NotesMissed > 0;

    // there's definitely a better way
    private Queue<NoteLog> _noteHistory = new Queue<NoteLog>();

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
        SongScore = 0;
    }

    public void JudgeNote(int noteID, float noteTiming, float hitTiming) {
        // noteID - some id to know what note is being judged
        // noteTiming - the time code the note is placed at
        // hitTiming - the time code that the hit was recorded

        const float FRAME_DURATION = 0.0167f; // millisecond duration of a frame @ 60fps

        NoteLog currentNote = new NoteLog(noteID, noteTiming);

        float timingDeltaInBeats = Mathf.Abs(hitTiming - noteTiming);
        float timingDelta = timingDeltaInBeats / Conductor.Instance.beatsPerSec;
        Debug.Log("note | timing delta: " + timingDelta);

        switch (timingDelta) {
            case float d when d <= FRAME_DURATION * 1f:
                // super secret ultra perfect
                currentNote.Judgement = 5;
                break;
            case float d when d <= FRAME_DURATION * 3f:
                // perfect
                currentNote.Judgement = 4;
                break;
            case float d when d <= FRAME_DURATION * 5f:
                // great
                currentNote.Judgement = 3;
                break;
            case float d when d <= FRAME_DURATION * 9f:
                // good
                currentNote.Judgement = 2;
                break;
            case float d when d <= FRAME_DURATION * 14f:
                // okay
                currentNote.Judgement = 1;
                break;
            default:
                // miss?
                currentNote.Judgement = 0;
                break;
        }

        // Update stats
        if (currentNote.Judgement > 0) {
            NotesHit++;
        } else {
            NotesMissed++;
        }

        // Display judgment hud action
        // TODO: use event listener pattern
        UIEvents.current.ShowJudgement(currentNote.Judgement);
        string[] judgments = new string[] {"miss", "okay", "good", "great", "perfect", "perfect+"};
        // Debug.Log("Note " + noteID + " Judged: " + judgments[currentNote.Judgement] + " " + currentNote.Judgement);

        // Add to note history
        _noteHistory.Enqueue(currentNote);
    }
}

public class NoteLog
{
    public int NoteID;
    public float Beat;
    public int Judgement;

    public NoteLog(int noteID, float beat) {
        NoteID = noteID;
        Beat = beat;
    }
}
