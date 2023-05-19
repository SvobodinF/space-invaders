using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    public UnityAction OnDie;

    public abstract void OnDisable();
    public abstract void Initialize(Vector2 startPosition, IController controller);
    public abstract void Update();
    public abstract void OnDamage(Collider2D other);

    protected void TranslateMove(Vector2 to)
    {
        transform.Translate(to.x, to.y, 0);
    }

    protected void ChangeSpriteAlpha(float value)
    {
        var color = _spriteRenderer.color;
        color.a = value;
        _spriteRenderer.color = color;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<Bullet>() == false)
            return;

        OnDamage(other.otherCollider);
    }
}
