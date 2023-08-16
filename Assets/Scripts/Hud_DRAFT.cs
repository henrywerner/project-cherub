using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hud_DRAFT : MonoBehaviour
{
    public TMP_Text beatCounter;

    private void Update() {
        beatCounter.text = "Beat: " + Conductor.Instance.songPositionInBeats;
    }
}
