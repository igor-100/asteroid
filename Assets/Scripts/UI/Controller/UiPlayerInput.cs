using System;
using Asteroid.UI.Controller;
using Constants;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Asteroid.UI.UI.Controller
{
    public class UiPlayerInput : MonoBehaviour, IUiPlayerInput
    {
        [SerializeField]
        private PlayerInput playerInput;
        
        private InputAction escAction;

        public bool IsEnabled { get; set; }
        
        public event Action EscPerformed = () => { }; 

        private void Awake()
        {
            escAction = playerInput.actions.FindAction(InputActionConstants.TOGGLE_PAUSE);
            escAction.performed += OnEscActionPerformed;
        }
        
        private void OnEscActionPerformed(InputAction.CallbackContext obj) => EscPerformed();
        private void OnDestroy()
        {
            if (escAction != null)
                escAction.performed -= OnEscActionPerformed;
        }
    }
}
