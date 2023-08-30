using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor Instance { get; private set; }

    public float songBpm; // Don't forget to change this for each song    
    public float secPerBeat; // The number of seconds for each song beat
    public float beatsPerSec; 
    public float songPosition; // Current song position, in seconds
    public float songPositionInBeats; // Current song position, in beats
    public float dspSongTime; // Seconds passed since the song started
    public float firstBeatOffset; //The offset to the first beat of the song in seconds
    private AudioSource _musicSource; // AudioSource attached to this GameObject that plays music.

    [Range(0.1f, 2.0f)]
    public float highwayLengthModifier;
    private const float HIGHWAY_TRUE_LENGTH = 30f; // length of highway in world units
    public float highwayLength { get; private set; }

    [Range(0.0f, 10.0f)]
    [SerializeField] private float noteSpeedModifier = 1.0f;
    public float noteSpeed = 0.2f; // Starts in Units per Second. Becomes Highways per beat in Start()

    // How long it takes for a note to travel from it's spawn point to the judgment line
    public float highwayTripDurationInBeats => highwayLength / noteSpeed;
    public float highwayTripDuration => highwayTripDurationInBeats / beatsPerSec;

    [Header("Song Play Statuses")]
    private bool _isSongStarted = false;
    public float songBeatDuration { get; private set; }
    public float songSecDuration => songBeatDuration / beatsPerSec;



    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        // TODO: This is a hack and needs a real solution
        // The songPos is 0 by default, which means that all early notes spawn at once inside each other.
        // So I just set it at -99 until we define it later as the highway duration.
        songPosition = -99; 
        songPositionInBeats = -99;

        // Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;
        beatsPerSec = songBpm / 60f;

        // Set speed vars
        highwayLength = HIGHWAY_TRUE_LENGTH * highwayLengthModifier;
        noteSpeed *= noteSpeedModifier * highwayLength; // note speed in Highways Per Second
        noteSpeed *= secPerBeat; // convert to Highways per Beat (H/b)

        // firstBeatOffset = highwayTripDuration;
        firstBeatOffset = 0;
    }

    void Start()
    {
        // Load the AudioSource attached to the Conductor GameObject
        _musicSource = GetComponent<AudioSource>();

        // Set the song duration
        songBeatDuration = ChartInterpreter.Instance.GetSongDuration();

        StartCoroutine(StartingCountdown(1f));
    }

    IEnumerator StartingCountdown(float additionRtDelay) {
        yield return new WaitForSecondsRealtime(additionRtDelay);

        dspSongTime = (float)AudioSettings.dspTime; // record the time when the music starts
        double startTime = AudioSettings.dspTime + highwayTripDuration;
        _musicSource.PlayScheduled(startTime); // apparently .play() can introduce up to 0.5s on some devices? 
        _isSongStarted = true; // start counting the beats in
    }

    void Update()
    {
        if (!_isSongStarted) return;

        if (songPositionInBeats >= songBeatDuration) {
            /* Trigger song ending event */
            Debug.Log("Song End Reached: " + songPositionInBeats + " beats out of " + songBeatDuration);
            _isSongStarted = false; // TEMP - i'm just pausing the song timer
            _musicSource.mute = true; // fade out music if music is still playing
            return;
        }

        //determine how many seconds since the song started
        //(also account for first beat offset)
        //(also account for initial highway travel time, otherwise everything will spawn at zero)
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset - highwayTripDuration);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;
    }

}
