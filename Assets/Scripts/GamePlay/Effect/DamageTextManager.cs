using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    private Vector3 _damageShowOffset = new Vector3(0, 2f, 0);
    private GameObject _damageTextPrefab;
    private Camera _cam;

    private async void Awake()
    {
        _cam = Camera.main;

        await Manager.Data.WaitForReady();

        _damageTextPrefab = await AssetLoaderProvider.Loader.LoadAsync<GameObject>("DamageText");

        Manager.Pool.CreatePool("DamageText", _damageTextPrefab, 4, "TextPool", transform);
    }

    public void Show(HitInfo hitInfo)
    {
        if (hitInfo.attackResult.damage <= 0)
            return;

        var text = Manager.Pool.Get("DamageText");
        text.SetActive(true);

        text.transform.position = _cam.WorldToScreenPoint(hitInfo.position + _damageShowOffset);

        var txt = text.GetComponent<DamageText>();
        txt.SetOwner(this);
        txt.Setup(hitInfo);
    }

    public void Return(GameObject text)
    {
        Manager.Pool.Return("DamageText", text);
    }
}