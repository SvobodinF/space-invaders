using UnityEngine;

public class SwarmController : IController
{
    public Vector2 Direction => _direction;
    private Vector2 _direction;

    public bool Shoot => true;

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
}
