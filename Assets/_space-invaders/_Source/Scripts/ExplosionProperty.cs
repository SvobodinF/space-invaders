using UnityEngine;

[CreateAssetMenu(fileName = nameof(ExplosionProperty), menuName = "Explosion", order = 51)]
public class ExplosionProperty : ScriptableObject
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private AudioClip _clip;

    public GameObject Prefab => _prefab;
    public AudioClip Clip => _clip;
}
