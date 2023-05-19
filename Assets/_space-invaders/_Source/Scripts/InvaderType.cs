using UnityEngine;

[CreateAssetMenu(fileName = nameof(InvaderType), menuName = "EnemyType", order = 51)]
public class InvaderType : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private int _points;
    [SerializeField] private int _rowCount;
    [SerializeField] private BulletType _bullet;

    public string Name => _name;
    public Sprite Sprite => _sprite;
    public int Points => _points;
    public int RowCount => _rowCount;
    public BulletType BulletType => _bullet;
}
