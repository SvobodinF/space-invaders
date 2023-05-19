using System.Collections;
using UnityEngine;

public class PlayerEntity : Entity
{
    public bool IsAlive => _health > 0;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _health;

    [SerializeField] private float _speed = 500f;
    [SerializeField] private float _respawnTime = 2f;
    [SerializeField] private float _cooldown;
    [SerializeField] private Shooting _shooting;
    [SerializeField] private Collider2D _collider2D;
    private IController _controller;
    private Vector2 _defaultPosition;

    private bool _canShoot => _collider2D.enabled;

    public override void Initialize(Vector2 startPosition, IController controller)
    {
        _health = _maxHealth;
        _defaultPosition = startPosition;
        _controller = controller;
        InGameState.UpdateLives(ref _health, _maxHealth);
    }

    public override void Update()
    {
        transform.Translate(_speed * _controller.Direction.x * Time.deltaTime, 0, 0);

        if (_controller.Shoot && _canShoot == true)
        {
            _shooting.StartShoot(_cooldown);
        }
    }

    public override void OnDamage(Collider2D other)
    {
        _health--;
        InGameState.UpdateLives(ref _health, _maxHealth);
        StopAllCoroutines();
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        _collider2D.enabled = false;
        ChangeSpriteAlpha(0.0f);

        yield return new WaitForSeconds(0.25f * _respawnTime);

        transform.position = _defaultPosition;
        ChangeSpriteAlpha(0.25f);

        yield return new WaitForSeconds(0.75f * _respawnTime);

        ChangeSpriteAlpha(1.0f);
        _collider2D.enabled = true;
    }

    public override void OnDisable()
    {
        _shooting.StopShoot();
    }
}
