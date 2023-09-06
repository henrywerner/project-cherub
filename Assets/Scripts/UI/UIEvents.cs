using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    private static UIEvents _current;

    // OnEnable and Awake can happen simultaneously which causes errors.
    // So I'm just going to borrow some code from telemancer just in case lmao.
    public static UIEvents current
    {
        get
        {
            if (_current == null) {
                _current = FindObjectOfType<UIEvents>();
            }
            return _current;
        }
        set => _current = value;
    }

    private void Awake()
    {
        current = this;
    }


    public event Action<int> OnShowJudgement;

    public void ShowJudgement(int rating) {
        OnShowJudgement?.Invoke(rating);
    }

    public event Action<int> OnUpdateCombo;

    public void UpdateCombo(int combo) {
        OnUpdateCombo?.Invoke(combo);
    }
}
