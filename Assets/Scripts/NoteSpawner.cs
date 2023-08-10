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
            Debug.Log("Spawning left note.");
            SpawnNote('l');
        }
        if (Input.GetKeyDown("o"))
        {
            Debug.Log("Spawning right note.");
            SpawnNote('r');
        }
    }

    public void SpawnNote(char side) {
        // type - normal, hold, swipe, etc.
        // side - L, R

        // TODO: Change this!
        if (side == 'l') {
            Instantiate(_LeftNote, _LeftSpawnpoint.transform.position, Quaternion.identity, _NoteParent.transform);
        } else {
            Instantiate(_RightNote, _RightSpawnpoint.transform.position, Quaternion.identity, _NoteParent.transform);
        }
    }
}
