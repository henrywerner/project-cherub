using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementLine : MonoBehaviour
{
    [SerializeField] private Transform GunLeft, GunRight;
    [SerializeField] private float maxRaycastDistance = 2f;

    // only hit objects on the "Notes" layer
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
        // Using raycasts is such a 200iq move
        RaycastHit hit;
        Ray ray = new Ray(t.position, t.TransformDirection(Vector3.forward));

        void DrawFunnyRaycast(Color color, String debugText) {
            Debug.DrawRay(t.position, t.TransformDirection(Vector3.forward) * maxRaycastDistance, color);
            // Debug.Log("🎵 " + debugText);
        }
        
        if (Physics.Raycast(ray, out hit, maxRaycastDistance, targetLayer))
        {
            // Get the note using the note interface
            INote note = hit.collider.gameObject.GetComponent<INote>();
            if ((bool)(note?.GetInRange())) { // the '?.' function returns type "bool?" LMAO
                note.Hit();
                DrawFunnyRaycast(Color.green, "Note hit");
            } else {
                DrawFunnyRaycast(Color.yellow, "Note out of range");
            }
        }
        else
        {
            DrawFunnyRaycast(Color.red, "Note missed");
        }
    }
}
