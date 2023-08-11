using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementLine : MonoBehaviour
{
    [SerializeField] private Transform GunLeft, GunRight;
    [SerializeField] private LayerMask targetLayer; // i'm just going to select it myself, fuck this.

    private void Start() {
        if (GunLeft == null || GunRight == null ) {
            Debug.Log("UH OH " + gameObject.name + "DOESN'T HAVE IT'S GUNS!!!");
        }
    }

    public void TapLeft() {
        Tap(GunLeft);
    }

    public void TapRight() {
        Tap(GunRight);
    }

    private void Tap(Transform t) {
        int layerMask = 6; // only hit objects on the "Notes" layer
        // float maxDistance = 0.875f; 
        float maxDistance = 2f; 

        // Using raycasts is such a 200iq move
        RaycastHit hit;
        Ray ray = new Ray(t.position, t.TransformDirection(Vector3.forward));
        if (Physics.Raycast(ray, out hit, maxDistance, targetLayer))
        {
            Debug.DrawRay(t.position, t.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            Debug.Log("Did Hit");

            GameObject noteObject = hit.collider.gameObject;
            NoteBase note = noteObject.GetComponent<NoteBase>();
            
            // Call ScoreKeeper
            ScoreKeeper.Instance.JudgeNote(note.NoteID, note.NoteTiming, Conductor.Instance.songPosition);

            // Destroy Note
            note.Hit();
        }
        else
        {
            Debug.DrawRay(t.position, t.TransformDirection(Vector3.forward) * maxDistance, Color.red);
            Debug.Log("Did not Hit");
        }
    }
}
