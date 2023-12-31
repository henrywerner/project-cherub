using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteBase : MonoBehaviour, INote
{
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] private ParticleSystem _hitParticles, _missParticles;
    protected float _NoteSpeed;
    public int NoteID;
    public float NoteTiming;
    public int NoteLane;
    public bool IsInRange = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _NoteSpeed = Conductor.Instance.noteSpeed;
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        Movement(_rb);
    }

    protected virtual void Movement(Rigidbody rb)
    {
        Vector3 newPos = rb.position;
        float beatDelta = NoteTiming - Conductor.Instance.songPositionInBeats;
        float distanceInHighways = _NoteSpeed * beatDelta; // (Beats per Highway) * (Beats) = distance in "highways"
        float highwayRatio = 30f / Conductor.Instance.highwayLength; // WARNING: The 30f is hardcoded and it shouldn't be
        newPos.z = distanceInHighways * highwayRatio; // convert from theoretical highways to physical units 

        rb.position = newPos;
    }

    public void SetInRange(bool inRange)
    {
        IsInRange = inRange;
    }

    public bool GetInRange()
    {
        return IsInRange;
    }

    public void Hit(int lane)
    {
        // "lane" lets the note know which button is was hit by. Needed for multi-button notes.
        HitAction(lane);
    }

    public void Miss()
    {
        MissAction();
    }

    protected virtual void HitAction(int lane)
    {
        JudgeNote();
        HitFeedback(); // play hit effects
        gameObject.SetActive(false); // hide note
        Destroy(gameObject); // Delete note
    }

    protected virtual void MissAction()
    {
        JudgeNote();
        MissFeedback();
        gameObject.SetActive(false);
        Destroy(gameObject); // Delete note
    }

    protected void JudgeNote()
    {
        // Call ScoreKeeper
        ScoreKeeper.Instance.JudgeNote(NoteID, NoteTiming, Conductor.Instance.songPositionInBeats);
    }

    protected void HitFeedback()
    {
        if (_hitParticles != null)
        {
            _hitParticles = Instantiate(_hitParticles, transform.position, Quaternion.identity);
            _hitParticles.gameObject.SetActive(true);
            //print("Hit Particles: " + _hitParticles.transform.rotation.eulerAngles);
        }
    }

    private void MissFeedback()
    {
        if (_missParticles != null)
        {
            _missParticles = Instantiate(_missParticles, transform.position, Quaternion.identity);
            _missParticles.gameObject.SetActive(true);
        }
    }

}
