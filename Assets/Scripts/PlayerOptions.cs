using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOptions : MonoBehaviour
{
    public static PlayerOptions Instance { get; private set; }

    // TODO: replace this with a player pref system. This is just a test for now.
    public float AudioOffset = 0f; // offset the start time of the track
    public float InputOffset = 0f; // offset the judgement timing for each note

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
}
