using UnityEngine;

public class InvanderEntity : Entity
{
    private IController _controller;
    private InvaderType _invaderType;

    public override void Initialize(Vector2 startPosition, IController controller)
    {
        transform.position = startPosition;
        _controller = controller;
    }

    public void SetType(InvaderType invaderType)
    {
        _invaderType = invaderType;
        _spriteRenderer.sprite = invaderType.Sprite;
    }

    public override void OnDamage(Collider2D other)
    {
        OnDie?.Invoke();

        _spriteRenderer.enabled = false;
        gameObject.SetActive(false);
    }

    public override void OnDisable()
    {
    }

    public override void Update()
    {
        transform.Translate(_controller.Direction.x, _controller.Direction.y, 0);
    }

    internal BulletType GetBullet() => _invaderType.BulletType;
}
