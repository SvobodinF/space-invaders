using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _speed = 200f;
    [SerializeField] private float _lifeTime = 5f;

    private BulletType _bulletType;

    public void Change(BulletType bulletType)
    {
        _bulletType = bulletType;
        _spriteRenderer.sprite = bulletType.Sprite;
    }

    internal void DestroySelf()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);

        if (_bulletType.Explosion != null)
            CreateExplosion();
    }

    private void Awake()
    {
        Invoke("DestroySelf", _lifeTime);
    }

    private void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector2.up);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DestroySelf();
    }

    private void CreateExplosion()
    {
        AudioController.Instance.PlaySound(_bulletType.Explosion.Clip);

        var explosion = Instantiate(_bulletType.Explosion.Prefab, transform.position,
            Quaternion.Euler(0f, 0f, Random.Range(-180f, 180f)));
        Destroy(explosion, 1f);
    }
}

