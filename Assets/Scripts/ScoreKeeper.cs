using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance { get; private set; }
    public int TotalNotes { get; private set; }
    public int NotesMissed { get; private set; }
    public int NotesHit { get; private set; }
    public int PerfectPlusHits { get; private set; }
    public int PerfectHits { get; private set; }
    public int GreatHits { get; private set; }
    public int GoodHits { get; private set; }
    public int OkayHits { get; private set; }
    public float SongAccuracy {
        get {
            if (TotalNotes == 0)
                return 1f;
            if (NotesHit == 0)
                return 0f;
            return TotalNotes / (float)NotesHit;
        }
    }
    public float SongScore { get; private set; } //TODO: add scoring
    public int CurrentCombo { get; private set; }
    public bool IsFullCombo => !(NotesMissed > 0);

    // there's definitely a better way
    private Queue<NoteLog> _noteHistory = new Queue<NoteLog>();

    private const int POINT_BASE_VALUE = 100;


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
        CurrentCombo = 0;
        PerfectPlusHits = 0;
        PerfectHits = 0;
        GreatHits = 0;
        GoodHits = 0;
        OkayHits = 0;
    }

    public void JudgeNote(int noteID, float noteTiming, float hitTiming) {
        // noteID - some id to know what note is being judged
        // noteTiming - the time code the note is placed at
        // hitTiming - the time code that the hit was recorded

        const float FRAME_DURATION = 0.0167f; // millisecond duration of a frame @ 60fps

        hitTiming -= PlayerOptions.Instance.InputOffset; // Adjust hit timing based on the player's InputOffset

        NoteLog currentNote = new NoteLog(noteID, noteTiming);

        float timingDeltaInBeats = Mathf.Abs(hitTiming - noteTiming);
        float timingDelta = timingDeltaInBeats / Conductor.Instance.beatsPerSec;
        // Debug.Log("note | timing delta: " + timingDelta);
        
        float pointsMultiplier = 0f;

        switch (timingDelta) {
            case float d when d <= FRAME_DURATION * 1f:
                // super secret ultra perfect
                currentNote.Judgement = ERating.PERFECTPLUS;
                pointsMultiplier = 1.05f;
                PerfectPlusHits++;
                break;
            case float d when d <= FRAME_DURATION * 3f:
                // perfect
                currentNote.Judgement = ERating.PERFECT;
                pointsMultiplier = 1f;
                PerfectHits++;
                break;
            case float d when d <= FRAME_DURATION * 5f:
                // great
                currentNote.Judgement = ERating.GREAT;
                pointsMultiplier = 0.9f;
                GreatHits++;
                break;
            case float d when d <= FRAME_DURATION * 9f:
                // good
                currentNote.Judgement = ERating.GOOD;
                pointsMultiplier = 0.75f;
                GoodHits++;
                break;
            case float d when d <= FRAME_DURATION * 14f:
                // okay
                currentNote.Judgement = ERating.OKAY;
                pointsMultiplier = 0.5f;
                OkayHits++;
                break;
            default:
                // miss
                currentNote.Judgement = ERating.MISS;
                break;
        }

        // Update stats
        if (currentNote.Judgement == ERating.MISS) {
            NotesMissed++;
            CurrentCombo = 0;
        } else {
            NotesHit++;
            SongScore += POINT_BASE_VALUE * pointsMultiplier;
            CurrentCombo++;
        }

        // Display judgment hud action
        UIEvents.current.ShowJudgement((int)currentNote.Judgement);
        UIEvents.current.UpdateCombo(CurrentCombo);

        // Add to note history
        _noteHistory.Enqueue(currentNote);
    }
}

public class NoteLog
{
    public int NoteID;
    public float Beat;
    public ERating Judgement;

    public NoteLog(int noteID, float beat) {
        NoteID = noteID;
        Beat = beat;
    }
}

public enum ERating
{
    MISS = 0,
    OKAY = 1,
    GOOD = 2,
    GREAT = 3,
    PERFECT = 4,
    PERFECTPLUS = 5
}