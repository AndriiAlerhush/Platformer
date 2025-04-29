using System.Collections;
using Camera;
using Enviroment;
using UnityEngine;

namespace Player
{
    public class PlayerRespawn : MonoBehaviour
    {
        [SerializeField] private Vector2 respawnPoint;
        [SerializeField] private Player player;
        [SerializeField] private float fallThreshold = -10f;
        
        // [SerializeField] private Image fadeImage;
        // [SerializeField] private float fadeDuration = 0.5f;
        
        [SerializeField] private float waitBeforeDrop = 1.0f;
        [SerializeField] private float dropHeight = 5f;
        [SerializeField] private ScreenFader screenFader;
        // [SerializeField] private float cameraDelay = 0.5f;
        
        private bool _isRespawning;
        private CameraController _cameraController;
    
        private void Awake()
        {
            _cameraController = FindFirstObjectByType<CameraController>();
        }

        private void Update()
        {
            if (!_isRespawning && player.transform.position.y < fallThreshold)
            {
                StartCoroutine(RespawnSequence());
            }
        }
        
        private IEnumerator RespawnSequence()
        {
            _isRespawning = true;

            // Locking Player Control
            player.EnableControl = false;

            // Fade In
            yield return StartCoroutine(screenFader.FadeIn());
            
            // Camera to Center of Scene
            _cameraController.CanFollow = false;
            _cameraController.SetPosition(new Vector3(0, 0, -10f));

            // Pause
            yield return new WaitForSeconds(waitBeforeDrop);

            player.PlaySpawnSound();

            // Teleport
            // player.transform.position = respawnPoint.position + Vector3.up * dropHeight;
            player.transform.position = 
                new Vector3(respawnPoint.x, respawnPoint.y, player.transform.position.z) + Vector3.up * dropHeight;
            
            // Reset Forces
            player.ResetMovement();
            player.MinusLive();
            
            _cameraController.ResetPosition();
            _cameraController.CanFollow = false;

            // Fade Out
            yield return StartCoroutine(screenFader.FadeOut());

            // Wait Until Grounded
            yield return StartCoroutine(WaitUntilGrounded());
            player.ForceState(States.Idle);
            
            player.PlayJumpSound();
            
            // yield return new WaitForSeconds(cameraDelay);
            
            _cameraController.CanFollow = true;

            // Delocking Player Control
            player.EnableControl = true;

            _isRespawning = false;
        }

        private IEnumerator WaitUntilGrounded()
        {
            while (!player.IsGrounded())
            {
                yield return null;
            }
        }
    }
}
