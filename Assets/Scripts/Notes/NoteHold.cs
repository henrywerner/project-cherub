using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteHold : NoteBase
{
    [SerializeField] private HoldTrailGenerator trailGenerator;
    public float releaseTarget;
    private NoteChild[] _children;

    private bool _isCurrentlyHeld = false;

    public void SetChildren(NoteChild[] children)
    {
        _children = children;

        if (children.Length > 0)
        {
            // set release target to the beat timing of the last child
            releaseTarget = children[children.Length - 1].b;
        }
    }

    void Start()
    {
        // trailGenerator = gameObject.GetComponent<HoldTrailGenerator>();
    }

    private void Update()
    {
        if (!_isCurrentlyHeld) Movement(_rb);

        UpdateTrailGeneration();
    }

    private void UpdateTrailGeneration()
    {
        float trailEnding = GetWorldPositionOfBeat(releaseTarget);
        float startingPos = _rb.position.z > 0 ? _rb.position.z : 0;
        float distanceDiff = trailEnding - startingPos;

        trailGenerator.tailLength = distanceDiff;
        trailGenerator.GenerateTrail();
    }

    // TODO: I should move this into the NoteBase class
    private float GetWorldPositionOfBeat(float beat)
    {
        float beatDelta = beat - Conductor.Instance.songPositionInBeats;
        float distanceInHighways = _NoteSpeed * beatDelta; // (Beats per Highway) * (Beats) = distance in "highways"
        float highwayRatio = 30f / Conductor.Instance.highwayLength; // WARNING: The 30f is hardcoded and it shouldn't be
        float zPos = distanceInHighways * highwayRatio; // convert from theoretical highways to physical units 

        return zPos;
    }

    protected override void HitAction(int lane) 
    {
        _isCurrentlyHeld = true;

        JudgeNote(); // Judge the initial hit as a complete note

        HitFeedback(); // Play standard hit feedback for initial impact. (Change later?)

        StartHoldFeedback(); // Start playing while-holding animation/effects

        this.gameObject.GetComponent<BoxCollider>().enabled = false; // Disable the box collider.
        // (Otherwise *I think* it'll flag as a miss since the initial note wasn't destroyed)

        this.gameObject.GetComponent<MeshRenderer>().enabled = false; // Hide the initial note.

        // Subscribe to listen for when the button is released
        if (lane == 0) {
            InputEvents_DRAFT.current.OnReleaseLeft += ReleaseAction;
            // not sure if I'll have to remove these references myself, or if garbage collection will handle it
        } else if (lane == 1) {
            InputEvents_DRAFT.current.OnReleaseRight += ReleaseAction;
        }
    }

    private void ReleaseAction()
    {
        // This might break lmao
        InputEvents_DRAFT.current.OnReleaseLeft -= ReleaseAction;
        InputEvents_DRAFT.current.OnReleaseRight -= ReleaseAction;

        StopHoldFeedback();

        const float FRAME_DURATION = 0.0167f; // millisecond duration of a frame @ 60fps
        float timingDeltaInBeats = Mathf.Abs(Conductor.Instance.songPositionInBeats - releaseTarget);
        float timingDelta = timingDeltaInBeats / Conductor.Instance.beatsPerSec;

        switch (timingDelta) {
            case float d when d <= FRAME_DURATION * 5f:
                // perfect release bonus

                // Display perfect release text

                // Add bonus score

                Debug.Log("Hold Released: Perfect Release");
                break;
            case float d when d <= FRAME_DURATION * 9f: // TODO: tweak this timing
                // normal release 

                // Maybe play a cool release vfx?

                // Add max hold bonus? Or should the score just keep counting up while holding?
                
                Debug.Log("Hold Released: Normal Release");
                break;
            default:
                // outside the target release window

                Debug.Log("Hold Released BAD TIMING");

                ReleaseFailAction();
                // TODO: make sure this only effects EARLY releases. Holding too long shouldn't be punished.
                break;
        }

        _isCurrentlyHeld = false;
    }

    private void ReleaseFailAction()
    {
        // change the tail color
    }

    private void StartHoldFeedback()
    {
        // Spawn looping vfx
    }

    private void StopHoldFeedback()
    {
        // Destroy looping vfx
    }
}
