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
        Tap(GunLeft, 0);
    }

    public void TapRight() {
        Tap(GunRight, 1);
    }

    // laneID - tells the note which lane it was hit from
    // TODO: remove the transform parameter and just use the laneID to determine left or right
    private void Tap(Transform t, int laneID) {
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
                note.Hit(laneID);
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

    public void Swipe(EFlickDirection direction)
    {
        // fire a raycast that only hits swipe notes
        RaycastHit leftHit, rightHit;

        Ray leftRay = new Ray(GunLeft.position, GunLeft.TransformDirection(Vector3.forward));
        Ray rightRay = new Ray(GunRight.position, GunRight.TransformDirection(Vector3.forward));

        // fire Left raycast
        if (Physics.Raycast(leftRay, out leftHit, maxRaycastDistance, targetLayer))
        {
            NoteFlick noteFlick = leftHit.collider.gameObject.GetComponent<NoteFlick>();

            // TODO: rewrite this to not be as lame
            if (noteFlick != null)
            {
                if ((bool)(noteFlick?.GetInRange()))
                {
                    noteFlick.Flick(direction);
                }
            }

        }

        // fire Right raycast
        if (Physics.Raycast(rightRay, out rightHit, maxRaycastDistance, targetLayer))
        {
            NoteFlick noteFlick = rightHit.collider.gameObject.GetComponent<NoteFlick>();

            if (noteFlick != null)
            {
                if ((bool)(noteFlick?.GetInRange()))
                {
                    noteFlick.Flick(direction);
                }
            }
        }
    }
}
