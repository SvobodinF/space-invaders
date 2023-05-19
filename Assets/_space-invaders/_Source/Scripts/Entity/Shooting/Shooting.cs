using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private BulletType _bulletType;
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] protected Transform _spawnPoint;
    [SerializeField] protected AudioClip _clip;

    private Coroutine _action;

    protected void OnShoot(BulletType bulletType)
    {
        Bullet bullet = Instantiate(_bulletPrefab, _spawnPoint.position, Quaternion.identity);
        bullet.Change(bulletType);
        AudioController.Instance.PlaySound(_clip);
    }

    private IEnumerator WaitCooldownShoot(float cooldown)
    {
        OnShoot(_bulletType);

        yield return new WaitForSecondsRealtime(cooldown);

        _action = null;
    }

    public void StartShoot(float cooldown)
    {
        if (_action != null)
            return;

        _action = StartCoroutine(WaitCooldownShoot(cooldown));
    }

    public void StopShoot()
    {
        StopAllCoroutines();
    }
}
