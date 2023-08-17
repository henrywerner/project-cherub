using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hud_DRAFT : MonoBehaviour
{
    public TMP_Text beatCounter;
    public TMP_Text missText, okayText, goodText, greatText, PerfectText, PerfectPlusText;


    private void Start() {
        UIEvents.current.OnShowJudgement += DisplayJudgementText;
    }

    private void Update() {
        beatCounter.text = "Beat: " + Conductor.Instance.songPositionInBeats;
    }

    private void DisplayJudgementText(int rating) {
        switch (rating)
        {
            case 1:
                StartCoroutine(TextFadeOut(okayText));
                break;
            case 2:
                StartCoroutine(TextFadeOut(goodText));
                break;
            case 3:
                StartCoroutine(TextFadeOut(greatText));
                break;
            case 4:
                StartCoroutine(TextFadeOut(PerfectText));
                break;
            case 5:
                StartCoroutine(TextFadeOut(PerfectPlusText));
                break;
            default:
                StartCoroutine(TextFadeOut(missText));
                break;
        }
    }

    IEnumerator TextFadeOut(TMP_Text t) {
        t.gameObject.SetActive(true);
        t.alpha = 1;

        float textFadeOut = 0.1f;
        float time = 0;

        while (time < textFadeOut)
        {
            t.alpha = Mathf.Lerp(1, 0, time / textFadeOut);
            time += Time.deltaTime;
            yield return null;
        }
        t.alpha = 0;
        t.gameObject.SetActive(false);
    }
}
