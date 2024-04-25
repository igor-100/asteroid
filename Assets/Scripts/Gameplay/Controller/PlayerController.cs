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
        private InputAction attackAction;

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
            attackAction = playerInput.actions.FindAction(InputActionConstants.ATTACK);
            attackAction.performed += OnAttackActionPerformed;
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
        
        private void OnAttackActionPerformed(InputAction.CallbackContext obj)
        {
            player.TryFire();
        }

        private void OnDestroy()
        {
            if (attackAction != null)
                attackAction.performed -= OnAttackActionPerformed;
        }
    }
}
