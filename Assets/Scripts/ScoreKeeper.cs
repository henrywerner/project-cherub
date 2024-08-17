using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static ScoreKeeper Instance { get; private set; }
    private const int POINT_BASE_VALUE = 100;

    [SerializeField] private float _NOTE_VALUE_PERFECTPLUS = 1.05f;
    [SerializeField] private float _NOTE_VALUE_PERFECT = 1f;
    [SerializeField] private float _NOTE_VALUE_GREAT = 0.9f;
    [SerializeField] private float _NOTE_VALUE_GOOD = 0.75f;
    [SerializeField] private float _NOTE_VALUE_OKAY = 0.5f;



    /// <summary>
    /// NOTE COUNTERS
    /// </summary>
    public int TotalNotes { get; private set; }
    public int NotesMissed { get; private set; }
    public int NotesHit { get; private set; }
    public int PerfectPlusHits { get; private set; }
    public int PerfectHits { get; private set; }
    public int GreatHits { get; private set; }
    public int GoodHits { get; private set; }
    public int OkayHits { get; private set; }
    private Queue<NoteLog> _noteHistory = new Queue<NoteLog>(); // there's definitely a better way

    /// <summary>
    /// SCORING
    /// </summary>
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

    /// <summary>
    /// COMBO TRACKING
    /// </summary>
    public int CurrentCombo { get; private set; }
    public bool IsFullCombo => !(NotesMissed > 0);
    private int CurrentMultiplierState; // multiplier bonus will use 2^(this var)
    private Dictionary<ERating, int> _adjustedNoteValues;
    

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
        CurrentMultiplierState = 0;
        PerfectPlusHits = 0;
        PerfectHits = 0;
        GreatHits = 0;
        GoodHits = 0;
        OkayHits = 0;

        _adjustedNoteValues = new Dictionary<ERating, int>
        {
            { ERating.PERFECTPLUS, (int)(POINT_BASE_VALUE * _NOTE_VALUE_PERFECTPLUS) },
            { ERating.PERFECT, (int)(POINT_BASE_VALUE * _NOTE_VALUE_PERFECT) },
            { ERating.GREAT, (int)(POINT_BASE_VALUE * _NOTE_VALUE_GREAT) },
            { ERating.GOOD, (int)(POINT_BASE_VALUE * _NOTE_VALUE_GOOD) },
            { ERating.OKAY, (int)(POINT_BASE_VALUE * _NOTE_VALUE_OKAY) }
        };
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
        
        float notePointsMultiplier = 0f;

        switch (timingDelta) {
            case float d when d <= FRAME_DURATION * 1f:
                // super secret ultra perfect
                currentNote.Judgement = ERating.PERFECTPLUS;
                notePointsMultiplier = _NOTE_VALUE_PERFECTPLUS;
                PerfectPlusHits++;
                break;
            case float d when d <= FRAME_DURATION * 3f:
                // perfect
                currentNote.Judgement = ERating.PERFECT;
                notePointsMultiplier = _NOTE_VALUE_PERFECT;
                PerfectHits++;
                break;
            case float d when d <= FRAME_DURATION * 5f:
                // great
                currentNote.Judgement = ERating.GREAT;
                notePointsMultiplier = _NOTE_VALUE_GREAT;
                GreatHits++;
                break;
            case float d when d <= FRAME_DURATION * 9f:
                // good
                currentNote.Judgement = ERating.GOOD;
                notePointsMultiplier = _NOTE_VALUE_GOOD;
                GoodHits++;
                break;
            case float d when d <= FRAME_DURATION * 14f:
                // okay
                currentNote.Judgement = ERating.OKAY;
                notePointsMultiplier = _NOTE_VALUE_OKAY;
                OkayHits++;
                break;
            default:
                // miss
                currentNote.Judgement = ERating.MISS;
                NotesMissed++;
                break;
        }

        // Update combo
        if (currentNote.Judgement == ERating.MISS) {
            CurrentCombo = 0; // Reset combo
            if (CurrentMultiplierState != 0) { // Reduce combo multiplier
                CurrentMultiplierState--;
            }
        } else {
            NotesHit++;
            SongScore += POINT_BASE_VALUE * notePointsMultiplier * Mathf.Pow(2, CurrentMultiplierState);
            CurrentCombo++;
        }

        // Display judgment hud action
        UIEvents.current.ShowJudgement((int)currentNote.Judgement);
        UIEvents.current.UpdateCombo(CurrentCombo);

        // Add to note history
        _noteHistory.Enqueue(currentNote);
    }

    private void UpdateComboMultiplier(ERating hit) {
        if (hit == ERating.MISS) {
            if (CurrentMultiplierState > 0) {
                CurrentMultiplierState--;
            }
            return;
        }

        

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