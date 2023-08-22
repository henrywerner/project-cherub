using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public static NoteSpawner Instance { get; private set; }

    [SerializeField] private GameObject _LeftNote;
    [SerializeField] private GameObject _RightNote;
    [SerializeField] private GameObject _MultiNote;

    [Header("Center Highway")]
    [SerializeField] private GameObject _centerHwBlueSpawnpoint;
    [SerializeField] private GameObject _centerHwRedSpawnpoint;
    [SerializeField] private GameObject _centerHwPurpleSpawnpoint;
    [SerializeField] private GameObject _centerHwNoteParent;

    [Header("Left Highway")]
    [SerializeField] private GameObject _leftHwBlueSpawnpoint;
    [SerializeField] private GameObject _leftHwRedSpawnpoint;
    [SerializeField] private GameObject _leftHwPurpleSpawnpoint;
    [SerializeField] private GameObject _leftHwNoteParent;

    [Header("Right Highway")]
    [SerializeField] private GameObject _rightHwBlueSpawnpoint;
    [SerializeField] private GameObject _rightHwRedSpawnpoint;
    [SerializeField] private GameObject _rightHwPurpleSpawnpoint;
    [SerializeField] private GameObject _rightHwNoteParent;


    private GameObject[] _blueSpawnpoints = new GameObject[3];
    private GameObject[] _redSpawnpoints = new GameObject[3];
    private GameObject[] _purpleSpawnpoints = new GameObject[3];
    private GameObject[] _noteParents = new GameObject[3];


    private int _nextNoteId = 0;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // put the spawnpoint references in an array so I can simplify the spawn note logic.
        _blueSpawnpoints[0] = _leftHwBlueSpawnpoint;
        _blueSpawnpoints[1] = _centerHwBlueSpawnpoint;
        _blueSpawnpoints[2] = _rightHwBlueSpawnpoint;

        _redSpawnpoints[0] = _leftHwRedSpawnpoint;
        _redSpawnpoints[1] = _centerHwRedSpawnpoint;
        _redSpawnpoints[2] = _rightHwRedSpawnpoint;

        _purpleSpawnpoints[0] = _leftHwPurpleSpawnpoint;
        _purpleSpawnpoints[1] = _centerHwPurpleSpawnpoint;
        _purpleSpawnpoints[2] = _rightHwPurpleSpawnpoint;

        _noteParents[0] = _leftHwNoteParent;
        _noteParents[1] = _centerHwNoteParent;
        _noteParents[2] = _rightHwNoteParent;
    }

    public void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            // Debug.Log("Spawning left note.");
            SpawnNote(1, 0, Conductor.Instance.dspSongTime + Conductor.Instance.highwayTripDuration, 0);
        }
        if (Input.GetKeyDown("o"))
        {
            // Debug.Log("Spawning right note.");
            SpawnNote(1, 1, Conductor.Instance.dspSongTime + Conductor.Instance.highwayTripDuration, 0);
        }
    }

    public void SpawnNote(int highway, int color, float timing, int type)
    {
        // type - normal, hold, swipe, etc.
        // side - L, R

        GameObject noteObj;
        Transform spawnPointTrans;
        Transform noteParentTrans;

        // it's throwing an error if I don't give them a default value.
        noteObj = _LeftNote;
        spawnPointTrans = _centerHwBlueSpawnpoint.transform;
        noteParentTrans = _centerHwNoteParent.transform;

        switch (color)
        {
            case 0: // blue
                noteObj = _LeftNote;
                spawnPointTrans = _blueSpawnpoints[highway].transform;
                noteParentTrans = _noteParents[highway].transform;
                break;
            case 1: // red
                noteObj = _RightNote;
                spawnPointTrans = _redSpawnpoints[highway].transform;
                noteParentTrans = _noteParents[highway].transform;
                break;
            case 2: // multi
                // TODO: add multi-notes
                noteObj = _MultiNote;
                spawnPointTrans = _purpleSpawnpoints[highway].transform;
                noteParentTrans = _noteParents[highway].transform;
                break;
            default: 
                // unexpected?
                return;
        }

        GameObject newNote = Instantiate(noteObj, spawnPointTrans.position, noteParentTrans.rotation, noteParentTrans);
        NoteBase nb = newNote.GetComponent<NoteBase>();
        nb.NoteID = _nextNoteId;
        nb.NoteTiming = timing;
        _nextNoteId++;
    }
}
