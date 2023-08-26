using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteFlick : NoteBase
{
    [Header("Phantom Tap Window")]
    [SerializeField] private const int PHANTOM_TAP_FRAMES = 3;

    [Header("Note Flick Direction")]
    public EFlickDirection FlickDirection = EFlickDirection.Left;

    private bool _wasHit = false;
    private bool _wasSwiped = false;
    private bool _isCriteriaMet => _wasHit && _wasSwiped;

    public void Flick(EFlickDirection direction)
    {
        if (direction == FlickDirection)
        {
            _wasSwiped = true;
        }

        // check if already hit
        if (_isCriteriaMet)
        {
            ActuallyHitTheNote();
        }
        else
        {
            StartCoroutine(PhantomTapDetector());
        }
    }

    // TODO: find a way to either exclude the lane var, or to make it less confusing.
    protected override void HitAction(int lane) // Ignore lane, it's not needed
    {
        _wasHit = true;

        // check if already swiped
        if (_isCriteriaMet)
        {
            ActuallyHitTheNote();
        }
    }

    private void ActuallyHitTheNote()
    {
        // Hit timing will match the timing of the most recent input. Not sure if this will feel good or not.
        JudgeNote();
        
        StopAllCoroutines();

        // force unsubscribe from all events, just in case.
        // TODO: find a better way to do this.
        InputEvents_DRAFT.current.OnTapLeft -= () => { Hit(0); };
        InputEvents_DRAFT.current.OnTapRight -= () => { Hit(1); };

        HitFeedback();
        Destroy(gameObject);
    }

    IEnumerator PhantomTapDetector()
    {
        // I set fixedTimestep to 60fps (it's 50fps by default)

        if (NoteLane == (int)ELane.left || NoteLane == (int)ELane.all)
        {
            InputEvents_DRAFT.current.OnTapLeft += () => { Hit(0); };
        }
        if (NoteLane == (int)ELane.right || NoteLane == (int)ELane.all)
        {
            InputEvents_DRAFT.current.OnTapRight += () => { Hit(1); };
        }

        for (int i = PHANTOM_TAP_FRAMES; i > 0; i--)
        {
            yield return new WaitForFixedUpdate();
        }

        InputEvents_DRAFT.current.OnTapLeft -= () => { Hit(0); };
        InputEvents_DRAFT.current.OnTapRight -= () => { Hit(1); };
    }
}

public enum EFlickDirection
{
    Left,
    Right
}