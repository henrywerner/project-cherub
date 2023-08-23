using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputEvents_DRAFT : MonoBehaviour
{
    private static InputEvents_DRAFT _current;

    public static InputEvents_DRAFT current
    {
        get
        {
            if (_current == null)
            {
                _current = FindObjectOfType<InputEvents_DRAFT>();
            }
            return _current;
        }
        set => _current = value;
    }

    private void Awake()
    {
        current = this;
    }

    public event Action OnTapLeft;

    public void TapLeft()
    {
        OnTapLeft?.Invoke();
    }

    public event Action OnReleaseLeft;

    public void ReleaseLeft()
    {
        OnReleaseLeft?.Invoke();
    }

    public event Action OnTapRight;

    public void TapRight()
    {
        OnTapRight?.Invoke();
    }

    public event Action OnReleaseRight;

    public void ReleaseRight()
    {
        OnReleaseRight?.Invoke();
    }

    public event Action<bool, bool> OnSwitchLanePressed;

    public void SwitchLanePressed(bool l, bool r)
    {
        OnSwitchLanePressed?.Invoke(l, r);
    }

    public event Action<bool, bool> OnSwitchLaneReleased;

    public void SwitchLaneReleased(bool l, bool r)
    {
        OnSwitchLaneReleased?.Invoke(l, r);
    }
}