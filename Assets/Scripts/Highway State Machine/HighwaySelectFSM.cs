using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighwaySelectFSM : StateMachine
{
    public static HighwaySelectFSM Instance {get; private set;}
    internal HighwaySelectedState leftState, neutralState, rightState;

    [Header("Judgement Lines")]
    public JudgementLine leftJudger;
    public JudgementLine centerJudger;
    public JudgementLine rightJudger;

    [Header("Indicator")]
    public GameObject HighwayIndicator;

    [Header("Indicator Positions")]
    [SerializeField] private Transform _indicatorPosLeft;
    [SerializeField] private Transform _indicatorPosCenter;
    [SerializeField] private Transform _indicatorPosRight;


    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        leftState = new LaneStateLeft(_indicatorPosLeft, leftJudger);
        neutralState = new LaneStateNeutral(_indicatorPosCenter, centerJudger);
        rightState = new LaneStateRight(_indicatorPosRight, rightJudger);
    }

    private void Start() {
        ChangeState(neutralState);
    }
}
