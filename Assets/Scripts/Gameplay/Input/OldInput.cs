using System;
using UnityEngine;

public class OldInput : MonoBehaviour, IOldInput
{
    private const string FireButton = "Fire1";

    public event Action Fire = () => { };
    public event Action Reload = () => { };
    public event Action Escape = () => { };
    public event Action<Vector2> Move = moveVector => { };

    private void Update()
    {
        ListenToFire();
        ListenToReload();
        ListenToEscape();
        ListenToMove();
    }

    public void Enable()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    public void Disable()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void ListenToFire()
    {
        if (Input.GetButton(FireButton))
        {
            Fire();
        }
    }

    private void ListenToReload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void ListenToEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Escape();
        }
    }
    private void ListenToMove()
    {
        var moveVector = Vector2.zero;
        moveVector.x = Input.GetAxisRaw("Horizontal");
        moveVector.y = Input.GetAxisRaw("Vertical");

        Move(moveVector.normalized);
    }
}
