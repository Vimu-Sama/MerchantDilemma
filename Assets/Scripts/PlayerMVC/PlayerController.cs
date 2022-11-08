using UnityEngine;
using Player.UI;
using Collectible;
using Events;

namespace Player
{
    public class PlayerController
    {
        private PlayerView playerView;
        private PlayerModel playerModel;
        private int packageCount = 0;
        private Collectibles droppedPackage;

        public PlayerController(PlayerView _playerView, PlayerModel _playerModel, Camera _cam)
        {
            playerView = playerView = GameObject.Instantiate<PlayerView>(_playerView);
            playerModel = _playerModel;
            playerView.SetPlayerController(this);
            playerModel.SetPlayerController(this);
            _cam.transform.SetParent(playerView.transform, true);
            _cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            EventService.Instance.ObjectPickedUp += IncreamentPackageCounter;
            EventService.Instance.ObjectDropped += DecreamentPackageCounter;
        }


        private void IncreamentPackageCounter(GameObject _collectibleGameObject)
        {
            if (packageCount >= playerModel.PackageCarryLimit())
                return;
            packageCount++;
            playerView.DestroyGameObject(_collectibleGameObject);
        }

        private void DecreamentPackageCounter()
        {
            packageCount--;
        }

        public void MovePlayer(float _keyInput)
        {
            if (_keyInput != 0)
            {
                playerView.transform.position += new Vector3(playerModel.PlayerSpeed() * _keyInput, 0f, 0f);
            }
        }

        public void AddPlayerJump(bool _canJump)
        {
            if (_canJump)
            {
                playerView.GetComponent<Rigidbody2D>().
                    AddForce(new Vector2(0f, playerModel.PlayerJumpForce()), ForceMode2D.Impulse);
            }
        }

        public void DropPackage(Collectibles collectibles)
        {
            if (packageCount > 0)
            {
                Vector2 packageTransform= new Vector2(playerView.transform.position.x, playerView.transform.position.y);
                droppedPackage=  GameObject.Instantiate(collectibles,packageTransform, Quaternion.identity);
                droppedPackage.gameObject.GetComponent<Rigidbody2D>().AddForce
                    (new Vector2(0f,playerModel.PackageThrowForce()), ForceMode2D.Impulse); //using playerModel, thus, MVC can allow interaction between Model->Controller->View
                EventService.Instance.ObjectDropped();
            }
            else
            {
                Debug.Log("Nothing to throw!!");
            }
        }
    }
}