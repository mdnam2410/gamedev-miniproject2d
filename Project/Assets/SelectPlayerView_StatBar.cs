using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SelectPlayerView_StatBar : MonoBehaviour
{
    [SerializeField] TMP_Text abilityName;
    [SerializeField] RectTransform progressBar;

    private float maxValue;
    private float maxWidth;
    private Tween currentTween;

    public void Start()
    {
        maxWidth = progressBar.sizeDelta.x;
        progressBar.sizeDelta = new Vector2(0, progressBar.sizeDelta.y);
    }

    public void Init(string abilityName, float maxValue)
    {
        this.abilityName.text = abilityName;
        this.maxValue = maxValue;
    }

    public void SetStat(float value)
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(SetStatCoroutine(value));
    }

    public IEnumerator SetStatCoroutine(float value)
    {
        yield return new WaitForEndOfFrame();
        value = Mathf.Clamp(value, 0, maxValue);
        float desiredWidth = value / maxValue * maxWidth;
        //progressBar.sizeDelta = 

        if (currentTween != null)
        {
            currentTween.Kill();
        }

        //progressBar.localScale = new Vector3(0, 1, 1);
        currentTween = progressBar.DOSizeDelta(new Vector2(desiredWidth, progressBar.sizeDelta.y), duration: 0.3f)
                                  .SetEase(Ease.OutQuad)
                                  .OnComplete(() => { currentTween = null; });
    }
}
