using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneStateLeft : HighwaySelectedState
{
    public LaneStateLeft(Transform indicatorPos, JudgementLine jLine) : base(indicatorPos, jLine)
    {
    }

    protected override void CheckInputs(bool leftPressed, bool rightPressed) {
        if (leftPressed == rightPressed) {
            // neutral
            _highwayFsm.ChangeState(_highwayFsm.neutralState);
            _judgementLine.Swipe(EFlickDirection.Right);
        }
        else if (rightPressed) {
            // right
            _highwayFsm.ChangeState(_highwayFsm.rightState);
            _judgementLine.Swipe(EFlickDirection.Right);
        }
    }
}
