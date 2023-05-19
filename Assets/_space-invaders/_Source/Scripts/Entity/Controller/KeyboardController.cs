using UnityEngine;

public class KeyboardController : IController
{
    private KeyCode _left => KeyCode.A;
    private KeyCode _right => KeyCode.D;
    private KeyCode _shoot => KeyCode.P;
    public Vector2 Direction => GetInput();
    public bool Shoot => Input.GetKeyDown(_shoot);

    public Vector2 GetInput()
    {
        int x = 0;

        if (Input.GetKey(_left))
        {
            x = -1;
        }
        else if (Input.GetKey(_right))
        {
            x = 1;
        }

        return new Vector2(x, 0f);
    }
}
