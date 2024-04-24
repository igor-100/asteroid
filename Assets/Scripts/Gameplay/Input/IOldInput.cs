using System;
using UnityEngine;

public interface IOldInput
{
    event Action Fire;
    event Action Reload;
    event Action Escape;
    event Action<Vector2> Move;

    void Disable();
    void Enable();
}
