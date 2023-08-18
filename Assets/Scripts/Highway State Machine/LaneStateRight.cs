using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneStateRight : HighwaySelectedState
{
    public LaneStateRight(Transform indicatorPos, JudgementLine jLine) : base(indicatorPos, jLine)
    {
    }

    protected override void CheckInputs(bool leftPressed, bool rightPressed) {
        if (leftPressed == rightPressed) {
            // neutral
            _highwayFsm.ChangeState(_highwayFsm.neutralState);
        }
        else if (leftPressed) {
            // left
            _highwayFsm.ChangeState(_highwayFsm.leftState);
        }
    }
}
