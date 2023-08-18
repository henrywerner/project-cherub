using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HighwaySelectedState : IState
{
    protected Transform _indicatorTargetTrans; // this should include rotation
    protected JudgementLine _judgementLine;
    protected HighwaySelectFSM _highwayFsm;
    protected GameObject _indicator;

    public HighwaySelectedState(Transform indicatorPos, JudgementLine jLine)
    {
        _indicatorTargetTrans = indicatorPos;
        _judgementLine = jLine;

        // just so that I don't have to write them over and over
        _highwayFsm = HighwaySelectFSM.Instance;
        _indicator = _highwayFsm.HighwayIndicator;
    }

    public void TapLeft()
    {
        _judgementLine.TapLeft();
    }

    public void TapRight()
    {
        _judgementLine.TapRight();
    }

    public virtual void ButtonPressed(bool leftPressed, bool rightPressed) 
    {
        // TODO: change this to new input system
        CheckInputs(leftPressed, rightPressed);
    }

    public virtual void ButtonReleased(bool leftPressed, bool rightPressed) 
    {
        // TODO: change this to new input system
        // NOTE: how am I going to handle the subtle analog stick changes? How will that modify the indicator? 
        CheckInputs(leftPressed, rightPressed);
    }

    protected virtual void CheckInputs(bool leftPressed, bool rightPressed) 
    {
    }

    // You can't use Coroutines because this isn't a monobehaviour

    // IEnumerator LerpIndicatorToPos(float duration) 
    // {
    //     float time = 0;
    //     Vector3 startPosition = _indicator.transform.position;
    //     while (time < duration)
    //     {
    //         _indicator.transform.position = Vector3.Lerp(startPosition, _indicatorTargetPos.position, time / duration);
    //         time += Time.deltaTime;
    //         yield return null;
    //     }
    //     _indicator.transform.position = _indicatorTargetPos.position;
    // }

    public void Enter()
    {
        _indicator.transform.position = _indicatorTargetTrans.position;
        _indicator.transform.rotation = _indicatorTargetTrans.rotation;


        // TODO: THIS WILL NOT WORK BECAUSE NOT A MONO!!!!!!!
        InputEvents_DRAFT.current.OnTapLeft += TapLeft;
        InputEvents_DRAFT.current.OnTapRight += TapRight;
        InputEvents_DRAFT.current.OnSwitchLanePressed += ButtonPressed;
        InputEvents_DRAFT.current.OnSwitchLaneReleased += ButtonReleased;
    }

    public void Exit()
    {
        InputEvents_DRAFT.current.OnTapLeft -= TapLeft;
        InputEvents_DRAFT.current.OnTapRight -= TapRight;
        InputEvents_DRAFT.current.OnSwitchLanePressed -= ButtonPressed;
        InputEvents_DRAFT.current.OnSwitchLaneReleased -= ButtonReleased;
    }

    public void FixedTick()
    {
    }

    public void Tick()
    {
    }
}
