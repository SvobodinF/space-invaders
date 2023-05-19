using UnityEngine;

[CreateAssetMenu(fileName = nameof(BulletType), menuName = "BulletType", order = 51)]
public class BulletType : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private ExplosionProperty _explosion;

    public Sprite Sprite => _sprite;
    public ExplosionProperty Explosion => _explosion;
}
