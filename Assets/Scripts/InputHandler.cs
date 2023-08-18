using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    public int State { get; private set; } // what highway is focused
    [SerializeField] private JudgementLine[] _judgers; // array of judgement lines
    private HighwaySelectFSM m_highwayFsm;

    // FIXME: OMFG change this!
    private bool _previousHoldingLaneLeft = false;
    private bool _previousHoldingLaneRight = false;

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
    }

    private void Start()
    {
        m_highwayFsm = HighwaySelectFSM.Instance;

        State = 0;
    }

    //TODO: Switch to New Input System
    private void Update()
    {
        // Handle lane switch logic
        DetermineLane();

        // Handle tap input logic
        HandleHits();
    }

    private void HandleHits()
    {
        // I'm saving these as vars so it'll be easier to switch to the other input system later.
        bool tapBlue = Input.GetKeyDown("j");
        bool releaseBlue = Input.GetKeyUp("j");
        bool tapRed = Input.GetKeyDown("k");
        bool releaseRed = Input.GetKeyUp("k");

        if (tapBlue)
        {
            InputEvents_DRAFT.current.TapLeft();
        }
        if (tapRed)
        {
            InputEvents_DRAFT.current.TapRight();
        }
    }

    private void DetermineLane()
    {
        bool holdingLaneLeft = Input.GetKey("d");
        bool holdingLaneRight = Input.GetKey("f");

        bool leftChanged = (holdingLaneLeft != _previousHoldingLaneLeft);
        bool rightChanged = (holdingLaneRight != _previousHoldingLaneRight);

        if (leftChanged || rightChanged)
        { // THIS IS SO SHIT
            if ((leftChanged && holdingLaneLeft) || (rightChanged && holdingLaneRight))
            {
                InputEvents_DRAFT.current.SwitchLanePressed(holdingLaneLeft, holdingLaneRight);
            }
            else
            {
                // THEY DO THE SAME THING IT DOESN'T MATTER WHY DID I WASTE MY TIME I'M JUST GOING TO REPLACE IT WITH THE NEW SYSTEM ANYWAYS
                InputEvents_DRAFT.current.SwitchLaneReleased(holdingLaneLeft, holdingLaneRight);
            }
        }

        _previousHoldingLaneLeft = holdingLaneLeft;
        _previousHoldingLaneRight = holdingLaneRight;
    }
}
