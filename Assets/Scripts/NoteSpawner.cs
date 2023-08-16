using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public static NoteSpawner Instance { get; private set; }

    [SerializeField] private GameObject _LeftNote;
    [SerializeField] private GameObject _RightNote;
    [SerializeField] private GameObject _LeftSpawnpoint;
    [SerializeField] private GameObject _RightSpawnpoint;
    [SerializeField] private GameObject _NoteParent;

    private int _nextNoteId = 0;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public void Update() {
        if (Input.GetKeyDown("i"))
        {
            // Debug.Log("Spawning left note.");
            SpawnNote(0, Conductor.Instance.dspSongTime + Conductor.Instance.highwayTripDuration);
        }
        if (Input.GetKeyDown("o"))
        {
            // Debug.Log("Spawning right note.");
            SpawnNote(1, Conductor.Instance.dspSongTime + Conductor.Instance.highwayTripDuration);
        }
    }

    public void SpawnNote(int side, float timing) {
        // type - normal, hold, swipe, etc.
        // side - L, R

        // TODO: Change this!
        if (side == 0) {
            GameObject newNote = Instantiate(_LeftNote, _LeftSpawnpoint.transform.position, Quaternion.identity, _NoteParent.transform);
            NoteBase nb = newNote.GetComponent<NoteBase>();
            nb.NoteID = _nextNoteId;
            nb.NoteTiming = timing;
            _nextNoteId++;
        } else {
            GameObject newNote = Instantiate(_RightNote, _RightSpawnpoint.transform.position, Quaternion.identity, _NoteParent.transform);
            NoteBase nb = newNote.GetComponent<NoteBase>();
            nb.NoteID = _nextNoteId;
            nb.NoteTiming = timing;
            _nextNoteId++;
        }
    }
}
