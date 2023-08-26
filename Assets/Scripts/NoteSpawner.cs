using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public static NoteSpawner Instance { get; private set; }

    [Header("Notes")]
    [SerializeField] private GameObject _LeftNote;
    [SerializeField] private GameObject _RightNote;
    [SerializeField] private GameObject _MultiNote;
    [SerializeField] private GameObject _LeftNoteHold;
    [SerializeField] private GameObject _RightNoteHold;
    [SerializeField] private GameObject _MultiNoteHold;
    [SerializeField] private GameObject _LeftNoteFlick;
    [SerializeField] private GameObject _RightNoteFlick;
    [SerializeField] private GameObject _MultiNoteFlick;
    [SerializeField] private GameObject _HiddenNote;

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

    /*
    public void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            // Debug.Log("Spawning left note.");
            SpawnTapNote(1, 0, Conductor.Instance.dspSongTime + Conductor.Instance.highwayTripDuration, 0);
        }
        if (Input.GetKeyDown("o"))
        {
            // Debug.Log("Spawning right note.");
            SpawnTapNote(1, 1, Conductor.Instance.dspSongTime + Conductor.Instance.highwayTripDuration, 0);
        }
    }
    */

    public void SpawnNote(Note note)
    {
        float beat  = note.b;
        ELane lane = (ELane)note.l;
        ENoteType type = (ENoteType)note.t;
        int highway = note.h;
        int children = note?.c ?? 0;
        NoteChild[] childrenArr = note?.e ?? new NoteChild[0];

        GameObject noteObj = GetNotePrefab(lane, type);
        Transform spawnPointTrans;
        Transform noteParentTrans = _noteParents[highway].transform;

        // it's throwing an error if I don't give them a default value.
        spawnPointTrans = _centerHwBlueSpawnpoint.transform;

        switch (lane)
        {
            case ELane.left: // blue
                spawnPointTrans = _blueSpawnpoints[highway].transform;
                break;
            case ELane.right: // red
                spawnPointTrans = _redSpawnpoints[highway].transform;
                break;
            case ELane.all: // multi
                spawnPointTrans = _purpleSpawnpoints[highway].transform;
                break;
            default: 
                // unexpected?
                return;
        }

        GameObject newNote = Instantiate(noteObj, spawnPointTrans.position, noteParentTrans.rotation, noteParentTrans);
        
        if (type == ENoteType.hold)
        {
            if (children == 0 || childrenArr.Length == 0)
            {
                // Throw an error if the hold doesn't have any children
                Debug.Log("Errrrmmmmmm What the fuck??");
                return;
            }

            NoteHold nh = newNote.GetComponent<NoteHold>();
            nh.NoteTiming = beat;
            nh.NoteID = _nextNoteId;
            nh.NoteLane = (int)lane;
            nh.SetChildren(childrenArr);
        }
        else {
            NoteBase nb = newNote.GetComponent<NoteBase>();
            nb.NoteTiming = beat;
            nb.NoteID = _nextNoteId;
            nb.NoteLane = (int)lane;
        }
        
        _nextNoteId++;
    }

    private GameObject GetNotePrefab(ELane lane, ENoteType type)
    {
        switch (type)
        {
            case ENoteType.normal:
                if (lane == ELane.left) 
                    return _LeftNote;
                else if (lane == ELane.right)
                    return _RightNote;
                else if (lane == ELane.all)
                    return _MultiNote;
                break;
            case ENoteType.hold:
                if (lane == ELane.left) 
                    return _LeftNoteHold;
                else if (lane == ELane.right)
                    return _RightNoteHold;
                else if (lane == ELane.all)
                    return _MultiNoteHold;
                break;
            case ENoteType.flick:
                if (lane == ELane.left) 
                    return _LeftNoteFlick;
                else if (lane == ELane.right)
                    return _RightNoteFlick;
                else if (lane == ELane.all)
                    return _MultiNoteFlick;
                break;
            case ENoteType.hidden:
                return _HiddenNote;
            default:
                break;
        }

        return null; // just in case
    }
}
