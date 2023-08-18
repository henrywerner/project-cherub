using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneStateNeutral : HighwaySelectedState
{
    public LaneStateNeutral(Transform indicatorPos, JudgementLine jLine) : base(indicatorPos, jLine)
    {
    }

    protected override void CheckInputs(bool leftPressed, bool rightPressed) {
        if (leftPressed == rightPressed) {
            // we're already here.
        }
        else if (leftPressed) {
            // left
            _highwayFsm.ChangeState(_highwayFsm.leftState);
        }
        else if (rightPressed) {
            // right
            _highwayFsm.ChangeState(_highwayFsm.rightState);
        }
    }
}
