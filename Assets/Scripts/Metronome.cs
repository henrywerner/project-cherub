using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metronome : MonoBehaviour
{
    [SerializeField] private GameObject _arm;
    private Conductor _conductor;

    [SerializeField] private Color _defaultColor, _beatColor;


    void Start()
    {
        _conductor = Conductor.Instance;
    }

    void Update()
    {
        float rotationDegrees = Mathf.Sin(_conductor.songPositionInBeats * Mathf.PI);
        rotationDegrees *= 20f; // increase wavelength

        _arm.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, rotationDegrees);

        float beatWindow = 5f;
        if (rotationDegrees < beatWindow && rotationDegrees > -beatWindow) {
            _arm.GetComponent<Image>().color = _beatColor;
        } else {
            _arm.GetComponent<Image>().color = _defaultColor;
        }
    }
}
