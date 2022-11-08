using UnityEngine;

namespace Player
{
    public class PlayerService : GenericSingleton<PlayerService>
    {
        [SerializeField] private PlayerView playerView;
        [SerializeField] private float playerSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float packageThrowForce;
        [SerializeField] private int packageCarryLimit;

        private Camera cam ;
        private PlayerController playerController;
        private PlayerModel playerModel;

        protected override void Awake()
        {
            base.Awake();
            cam= GameObject.FindObjectOfType<Camera>(); 
            playerModel = new PlayerModel(packageThrowForce,playerSpeed, jumpForce, packageCarryLimit);
            playerController = new PlayerController(playerView, playerModel, cam);            
        }

    }
}
