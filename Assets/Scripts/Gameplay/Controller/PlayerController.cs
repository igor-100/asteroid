using Constants;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Gameplay
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [SerializeField]
        private PlayerInput playerInput;
        
        private InputAction moveAction;

        private void Awake()
        {
            moveAction = playerInput.actions.FindAction(InputActionConstants.MOVE);
            moveAction.started += OnInputMovement_Started;
            moveAction.canceled += OnInputMovement_Canceled;
            
        }

        private void Update()
        {
            if (moveAction.IsInProgress())
            {
                // Debug.Log("in progress");
            }
        }

        public void OnInputMovement_Started(InputAction.CallbackContext value)
        {
            Debug.Log($"space started");
        }
        private void OnInputMovement_Canceled(InputAction.CallbackContext obj)
        {
            Debug.Log($"space canceled");
        }
    }
}
