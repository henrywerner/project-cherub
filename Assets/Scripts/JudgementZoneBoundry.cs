using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementZoneBoundry : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        INote note = other.gameObject.GetComponent<INote>();
        note?.SetInRange(true);
    }

    private void OnTriggerExit(Collider other) {
        INote note = other.gameObject.GetComponent<INote>();
        note?.Miss();
    }
}
