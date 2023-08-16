using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor Instance { get; private set; }

    //Song beats per minute
    //This is determined by the song you're trying to sync up to
    public float songBpm;

    //The number of seconds for each song beat
    public float secPerBeat;

    //Current song position, in seconds
    public float songPosition;

    //Current song position, in beats
    public float songPositionInBeats;

    //How many seconds have passed since the song started
    public float dspSongTime;

    //The offset to the first beat of the song in seconds
    public float firstBeatOffset;

    //an AudioSource attached to this GameObject that will play the music.
    private AudioSource _musicSource;

    // NEW! Control the speed that all notes travel 
    [Range(0.0f, 100.0f)]
    public float noteSpeed;

    private const float HIGHWAY_LENGTH = 30f; // the distance of the highway

    // How long it takes for a note to travel from it's spawn point to the judgment line
    public float highwayTripDuration => HIGHWAY_LENGTH / noteSpeed;
    public float highwayTripDurationInBeats => highwayTripDuration / secPerBeat;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    void Start()
    {
        //Load the AudioSource attached to the Conductor GameObject
        _musicSource = GetComponent<AudioSource>();

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;

        //Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        //Start the music
        _musicSource.Play();
    }

    void Update()
    {
        //determine how many seconds since the song started
        //(also account for first beat offset)
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        //determine how many beats since the song started
        songPositionInBeats = songPosition / secPerBeat;
    }

}
