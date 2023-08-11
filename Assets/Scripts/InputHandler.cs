using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    public int State {get; private set;} // what highway is focused
    [SerializeField] private JudgementLine[] _judgers; // array of judgement lines

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        State = 0;
    }

    //TODO: Switch to New Input System
    private void Update() {
        // Handle lane switch logic
        DetermineLane();

        // Handle tap input logic
        HandleHits();
    }

    private void HandleHits() {
        // I'm saving these as vars so it'll be easier to switch to the other input system later.
        bool tapLeft = Input.GetKeyDown("j");
        bool releaseLeft = Input.GetKeyUp("j");
        bool tapRight = Input.GetKeyDown("k");
        bool releaseRight = Input.GetKeyUp("k");

        if (tapLeft) {
            _judgers[State].TapLeft();
        }
        if (tapRight) {
            _judgers[State].TapRight();
        }
    }

    private void DetermineLane() {
        bool holdingLaneLeft = Input.GetKey("d");
        bool LaneLeftKeyDown = Input.GetKeyDown("d");
        bool LaneLeftKeyUp = Input.GetKeyUp("d");
        bool holdingLaneRight = Input.GetKey("f");
        bool LaneRightKeyDown = Input.GetKeyDown("f");
        bool LaneRightKeyUp = Input.GetKeyUp("f");


        if (holdingLaneLeft && holdingLaneRight) {
            // neutral
            return;
        }

        if (holdingLaneLeft) {
            // switch to left lane
            return;
        }

        if (holdingLaneRight) {
            // switch to left lane
            return;
        }
    }
}
