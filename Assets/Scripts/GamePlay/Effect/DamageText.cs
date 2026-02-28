using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float _holdTime = 0.5f;
    [SerializeField] private float _fadeOutDuration = 0.5f;

    private DamageTextManager _owner;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    public void SetOwner(DamageTextManager owner)
    {
        _owner = owner;
    }

    public void Setup(HitInfo hitInfo)
    {
        _text.text = (hitInfo.attackResult.damage * hitInfo.rate).ToString("0.#");

        if (hitInfo.hitType == HitType.Normal) _text.color = Color.white;
        else _text.color = Color.red;

        StartCoroutine(UIUpdate());
    }

    IEnumerator UIUpdate()
    {
        DOTween.Kill(_text);

        _text.alpha = 1f;

        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(_holdTime);

        seq.Append(
            _text.DOFade(0f, _fadeOutDuration)
                .SetEase(Ease.OutQuad)
        );

        yield return new WaitForSecondsRealtime(_holdTime + _fadeOutDuration);

        _owner.Return(this.gameObject);
    }
}