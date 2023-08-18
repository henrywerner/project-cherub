using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Hud_DRAFT : MonoBehaviour
{
    private ScoreKeeper m_scoreKeeper;

    [Header("Time Displays")]
    public TMP_Text beatCounter;
    public TMP_Text timer;

    [Header("Judgement Indicators Text")]
    public TMP_Text missText;
    public TMP_Text okayText;
    public TMP_Text goodText;
    public TMP_Text greatText;
    public TMP_Text PerfectText;
    public TMP_Text PerfectPlusText;

    [Header("Note Rating Counters")]
    public TMP_Text missCounterText;
    public TMP_Text okayCounterText;
    public TMP_Text goodCounterText;
    public TMP_Text greatCounterText;
    public TMP_Text PerfectCounterText;

    [Header("FC Indicator")]
    public TMP_Text fcText;


    private void Start() {
        m_scoreKeeper = ScoreKeeper.Instance;

        UIEvents.current.OnShowJudgement += DisplayJudgementText;
        UIEvents.current.OnShowJudgement += UpdateScores;
    }

    private void Update() {
        beatCounter.text = "Beat: " + Conductor.Instance.songPositionInBeats;
        timer.text = "Time: " + ConvertTime(Conductor.Instance.songPosition) + " / " + ConvertTime(Conductor.Instance.songSecDuration);
    }

    private string ConvertTime(float time) {
        float min = Mathf.FloorToInt(time / 60);
        float sec = Mathf.FloorToInt(time % 60);
        // float ms = (time % 1) * 1000;
        return $"{min:00}:{sec:00}";
    }

    private void UpdateScores(int noteRating) {
        PerfectCounterText.text = "PERFECT: " + (m_scoreKeeper.PerfectPlusHits + m_scoreKeeper.PerfectHits);
        greatCounterText.text = "GREAT: " + m_scoreKeeper.GreatHits;
        goodCounterText.text = "GOOD: " + m_scoreKeeper.GoodHits;
        okayCounterText.text = "OKAY: " + m_scoreKeeper.OkayHits;
        missCounterText.text = "MISS: " + m_scoreKeeper.NotesMissed;
        fcText.text = "FC: " + m_scoreKeeper.IsFullCombo;
        fcText.color = m_scoreKeeper.IsFullCombo ? Color.yellow : Color.white;
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
