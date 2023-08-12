using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteBase : MonoBehaviour, INote
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ParticleSystem _hitParticles, _missParticles;
    private float _NoteSpeed;
    public int NoteID;
    public float NoteTiming;
    public bool IsInRange = false;

    // this WILL NOT WORK
    protected NoteBase(int noteID, float noteTiming) {
        NoteID = noteID;
        NoteTiming = noteTiming;
    } 

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        _NoteSpeed = Conductor.Instance.noteSpeed;
    }

    private void Update() {
        Movement(_rb);
    }

    protected virtual void Movement(Rigidbody rb) {
        Vector3 moveOffset = -transform.forward * _NoteSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + moveOffset);
    }

    public void SetInRange(bool inRange) {
        IsInRange = inRange;
    }

    public bool GetInRange() {
        return IsInRange;
    }

    public void Hit() {
        // Call ScoreKeeper
        ScoreKeeper.Instance.JudgeNote(NoteID, NoteTiming, Conductor.Instance.songPosition);
        
        HitAction();
    }

    public void Miss()
    {
        // Call ScoreKeeper
        ScoreKeeper.Instance.JudgeNote(NoteID, NoteTiming, Conductor.Instance.songPosition);

        MissAction();
    }

    protected virtual void HitAction() {
        // play note hit animation

        // play hit effects
        HitFeedback();
        // hide note
        gameObject.SetActive(false);
    }

    protected virtual void MissAction() {
        MissFeedback();
        gameObject.SetActive(false);
    }

    private void HitFeedback() {
        if (_hitParticles != null)
        {
            _hitParticles = Instantiate(_hitParticles, transform.position, Quaternion.identity);
            _hitParticles.gameObject.SetActive(true);
            //print("Hit Particles: " + _hitParticles.transform.rotation.eulerAngles);
        }

        Destroy(gameObject); // Delete note
    }

    private void MissFeedback() {
        if (_missParticles != null)
        {
            _missParticles = Instantiate(_hitParticles, transform.position, Quaternion.identity);
            _missParticles.gameObject.SetActive(true);
        }

        Destroy(gameObject); // Delete note
    }

}
