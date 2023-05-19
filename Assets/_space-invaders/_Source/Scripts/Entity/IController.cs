using UnityEngine;

public interface IController 
{
    public Vector2 Direction { get; }
    public bool Shoot { get; }
}
