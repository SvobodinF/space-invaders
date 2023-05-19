using UnityEngine;

public class ObstacleEntity : Entity
{
    [SerializeField] private Texture2D _damageTexture;

    [SerializeField] private Sprite _sprite;
    private Color[] _damagePixelArray;

    //anyone in layer 'Damage' will trigger this
    private void OnTriggerStay2D(Collider2D other)
    {
        OnDamage(other);
    }

    public override void Initialize(Vector2 startPosition, IController controller)
    {
        _damagePixelArray = _damageTexture.GetPixels();

        var sprite = _spriteRenderer.sprite;
        var normalizedPivot = sprite.pivot / sprite.rect.size;
        _spriteRenderer.sprite = Sprite.Create(Instantiate(sprite.texture),
                                    sprite.rect, normalizedPivot, sprite.pixelsPerUnit);
    }

    public override void OnDamage(Collider2D other)
    {
        bool damage = CheckForDamage(_spriteRenderer.sprite.texture,
             _spriteRenderer.transform.InverseTransformPoint(other.transform.position));

        if (other.GetComponent<Bullet>() && damage)
        {
            other.GetComponent<Bullet>().DestroySelf();
        }
    }

    public override void OnDisable()
    {
    }

    public override void Update()
    {  
    }

    private bool CheckForDamage(Texture2D tex, Vector2 contactPosition)
    {
        int coordX = Mathf.RoundToInt(contactPosition.x * _sprite.pixelsPerUnit + _sprite.pivot.x);
        int coordY = Mathf.RoundToInt(contactPosition.y * _sprite.pixelsPerUnit + _sprite.pivot.y);

        if (tex.GetPixel(coordX, coordY).a == 0)
        {
            return false;
        }

        var direction = (Random.value > 0.5) ? -1 : 1;
        var startX = coordX + _damageTexture.width / 2 * -direction;
        coordY += _damageTexture.height / 2 * -direction;
        for (int y = 0; y < _damageTexture.height; y++)
        {
            coordX = startX;
            for (int x = 0; x < _damageTexture.width; x++)
            {
                var thisPix = tex.GetPixel(coordX, coordY);
                thisPix.a *= _damagePixelArray[x + y * _damageTexture.width].a;
                tex.SetPixel(coordX, coordY, thisPix);
                coordX += direction;
            }

            coordY += direction;
        }

        tex.Apply();
        return true;
    }
}
