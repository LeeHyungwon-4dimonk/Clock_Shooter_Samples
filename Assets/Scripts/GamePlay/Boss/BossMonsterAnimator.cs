using DG.Tweening;
using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BossMonsterAnimator : MonoBehaviour
{
    public event Action OnBossAppeared;
    public event Action OnBossDisappear;

    [SerializeField] private CinemachineCamera _cinematicCam;
    [SerializeField] private GameObject _monsterModel;
    [SerializeField] private GameObject _disablePanel;

    private Sequence _sequence;

    #region Appear

    public void PlayAppearCinematic(System.Action onStart, System.Action onEnd)
    {
        StartCoroutine(AppearCoroutine(onStart, onEnd));
    }

    private IEnumerator AppearCoroutine(System.Action onStart, System.Action onEnd)
    {
        _disablePanel.SetActive(true);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _monsterModel.transform.rotation = Quaternion.Euler(0, -120f, 0);

        Manager.Game.GamePause(true);
        Manager.Sound.StopBGM(2f);
        onStart?.Invoke();

        _cinematicCam.Priority = 30;

        yield return new WaitForSecondsRealtime(6.5f);

        _cinematicCam.Priority = 10;

        Manager.Sound.PlayBGM(ConditionType.Boss);
        Manager.Game.GamePause(false);

        onEnd?.Invoke();
        OnBossAppeared?.Invoke();

        _disablePanel.SetActive(false);
    }

    #endregion

    #region Damage

    public void PlayDamageCinematic(System.Action onStart, System.Action onEnd)
    {
        StartCoroutine(DamageCoroutine(onStart, onEnd));
    }

    private IEnumerator DamageCoroutine(System.Action onStart, System.Action onEnd)
    {
        _disablePanel.SetActive(true);

        Manager.Game.GamePause(true);

        _cinematicCam.Priority = 30;

        yield return new WaitForSecondsRealtime(3f);

        onStart?.Invoke();

        yield return new WaitForSecondsRealtime(3f);

        _cinematicCam.Priority = 10;

        Manager.Game.GamePause(false);

        onEnd?.Invoke();

        _disablePanel.SetActive(false);
    }

    #endregion

    #region Runaway

    public void PlayRunawayCinematic(System.Action onStart, System.Action onEnd)
    {
        StartCoroutine(RunawayCoroutine(onStart, onEnd));
    }

    private IEnumerator RunawayCoroutine(System.Action onStart, System.Action onEnd)
    {
        _disablePanel.SetActive(true);

        Manager.Game.GamePause(true);
        _cinematicCam.Priority = 30;

        onStart?.Invoke();

        yield return new WaitForSecondsRealtime(1.5f);

        _sequence?.Kill();
        _sequence = DOTween.Sequence().SetUpdate(true)
            .Append(_monsterModel.transform.DORotate(new Vector3(0, -280, 0), 1.5f))
            .Append(transform.DOLocalMove(new Vector3(3f, 0, 1f), 2f));

        yield return _sequence.WaitForCompletion();

        _cinematicCam.Priority = 10;
        Manager.Game.GamePause(false);

        onEnd?.Invoke();
        OnBossDisappear?.Invoke();

        _disablePanel.SetActive(false);
    }

    #endregion
}