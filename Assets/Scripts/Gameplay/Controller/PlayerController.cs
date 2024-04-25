using Asteroid.Gameplay.Player;
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
        private IPlayer player;
        private InputAction lookAction;
        private Camera gameCamera;
        private InputAction attack1Action;
        private InputAction attack2Action;

        public bool IsEnabled { get; set; }
        
        public void SetPlayer(IPlayer player)
        {
            this.player = player;
        }
        
        public void SetCamera(Camera gameCamera)
        {
            this.gameCamera = gameCamera;
        }

        private void Awake()
        {
            moveAction = playerInput.actions.FindAction(InputActionConstants.MOVE);
            lookAction = playerInput.actions.FindAction(InputActionConstants.LOOK);
            attack1Action = playerInput.actions.FindAction(InputActionConstants.ATTACK1);
            attack2Action = playerInput.actions.FindAction(InputActionConstants.ATTACK2);
            attack1Action.performed += OnAttack1ActionPerformed;
            attack2Action.performed += OnAttack2ActionPerformed;
        }

        private void Update()
        {
            if (!IsEnabled || player == null)
                return;

            ProcessMove();
            ProcessLook();
        }

        private void ProcessMove()
        {
            if (moveAction.IsPressed())
                player.IncreaseSpeed();
            else
                player.DecreaseSpeed();
        }
        
        private void ProcessLook()
        {
            var lookVector = lookAction.ReadValue<Vector2>();
            var worldPos = gameCamera.ScreenToWorldPoint(lookVector);
            var lookPos = new Vector2(worldPos.x, worldPos.y);
            player.LookAt(lookPos);
        }
        
        private void OnAttack1ActionPerformed(InputAction.CallbackContext obj) => player.TryFireAttack1();

        private void OnAttack2ActionPerformed(InputAction.CallbackContext obj) => player.TryFireAttack2();

        private void OnDestroy()
        {
            if (attack1Action != null)
                attack1Action.performed -= OnAttack1ActionPerformed;
            if (attack2Action != null)
                attack2Action.performed -= OnAttack2ActionPerformed;
        }
    }
}
