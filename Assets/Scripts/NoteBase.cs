using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoteBase : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private ParticleSystem _hitParticles;
    private float _NoteSpeed;

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

    public void Hit() {
        HitAction();
    }

    protected virtual void HitAction() {
        // play note hit animation

        // play hit effects
        Feedback();
        // hide note
        gameObject.SetActive(false);
    }

    private void Feedback() {
        if (_hitParticles != null)
        {
            _hitParticles = Instantiate(_hitParticles, transform.position, Quaternion.identity);
            _hitParticles.gameObject.SetActive(true);
            //print("Hit Particles: " + _hitParticles.transform.rotation.eulerAngles);
        }

        Destroy(gameObject); // Delete note
    }

}
