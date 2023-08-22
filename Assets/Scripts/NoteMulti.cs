using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMulti : NoteBase
{
    private bool _isHitLeft, _isHitRight;
    private float _hitTimeLeft, _hitTimeRight;

    public NoteMulti(int noteID, float noteTiming) : base(noteID, noteTiming)
    {
    }

    protected override void HitAction(int lane) 
    {
        if (lane == 0 && !_isHitLeft) {
            _isHitLeft = true;
            _hitTimeLeft = Conductor.Instance.songPositionInBeats;
        } else if (lane == 1 && !_isHitRight) {
            _isHitRight = true;
            _hitTimeRight = Conductor.Instance.songPositionInBeats;
        }

        if (_isHitLeft && _isHitRight) {
            // either the average or which ever hit closest
            float avg = (_hitTimeLeft + _hitTimeRight) / 2f;

            // I hate programming
            float[] temp = {avg, _hitTimeLeft, _hitTimeRight};
            float best = 999;
            int bestIndex = 0;

            for (int i = 0; i < 3; i++)
            {
                float delta1 = Mathf.Abs(temp[i] - NoteTiming);
                if (delta1 < best) {
                    best = delta1;
                    bestIndex = i;
                }
            }
            
            // Judge note
            ScoreKeeper.Instance.JudgeNote(NoteID, NoteTiming, temp[bestIndex]);

            HitFeedback();

            gameObject.SetActive(false); // do I even need this if HitFeedback() destroys the note?
        }
    }
}
