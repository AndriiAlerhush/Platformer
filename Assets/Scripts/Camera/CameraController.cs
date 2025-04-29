// // // using UnityEngine;
// // // using UnityEngine.InputSystem;
// // //
// // // namespace Camera
// // // {
// // //     public class CameraController : MonoBehaviour
// // //     {
// // //         [Header("Camera Settings")]
// // //         [SerializeField] private Vector3 startPos;
// // //         [SerializeField] private Transform target;
// // //         [SerializeField] private float cameraSpeed = 5f;
// // //         [SerializeField] private Vector3 offset = new(0, 0, -10f);
// // //         [SerializeField] private float minCameraHeight = -10f;
// // //
// // //         [Header("Look Ahead Settings")]
// // //         [SerializeField] private float lookAheadDistance = 2f;
// // //         [SerializeField] private float lookAheadSpeed = 5f;
// // //         [SerializeField] private float lookHoldTime = 0.3f;
// // //
// // //         private PlayerInput _playerInput;
// // //         private InputAction _lookAction;
// // //
// // //         private float _currentLookAheadX;
// // //         private float _targetLookAheadX;
// // //         private float _lookReleaseTimer;
// // //
// // //         public bool CanFollow { get; set; } = true;
// // //
// // //         private void Awake()
// // //         {
// // //             _playerInput = FindAnyObjectByType<PlayerInput>();
// // //             
// // //             if (_playerInput != null)
// // //             {
// // //                 _lookAction = _playerInput.actions["Look"];
// // //             }
// // //             
// // //             // TeleportToTarget();
// // //             transform.position = startPos;
// // //         }
// // //
// // //         private void Start()
// // //         {
// // //             if (target == null)
// // //             {
// // //                 var player = FindAnyObjectByType<Player.Player>();
// // //                 if (player != null)
// // //                     target = player.transform;
// // //             }
// // //             
// // //             // TeleportToTarget();
// // //         }
// // //
// // //         private void FixedUpdate()
// // //         {
// // //             if (!CanFollow || !target) return;
// // //
// // //             Vector3 desiredPosition = target.position + offset;
// // //
// // //             float lookInput = 0f;
// // //             if (_lookAction != null)
// // //             {
// // //                 lookInput = _lookAction.ReadValue<Vector2>().x;
// // //             }
// // //
// // //             if (Mathf.Abs(lookInput) > 0.01f)
// // //             {
// // //                 _targetLookAheadX = lookAheadDistance * Mathf.Sign(lookInput);
// // //                 _lookReleaseTimer = lookHoldTime; // сбросить таймер при нажатии
// // //             }
// // //             else
// // //             {
// // //                 if (_lookReleaseTimer > 0)
// // //                 {
// // //                     _lookReleaseTimer -= Time.fixedDeltaTime;
// // //                 }
// // //                 else
// // //                 {
// // //                     _targetLookAheadX = 0f; // вернуть обратно только после паузы
// // //                 }
// // //             }
// // //
// // //             _currentLookAheadX = Mathf.Lerp(_currentLookAheadX, _targetLookAheadX, Time.fixedDeltaTime * lookAheadSpeed);
// // //
// // //             desiredPosition.x += _currentLookAheadX;
// // //
// // //             if (desiredPosition.y < minCameraHeight)
// // //             {
// // //                 desiredPosition.y = minCameraHeight;
// // //             }
// // //
// // //             transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.fixedDeltaTime * cameraSpeed);
// // //         }
// // //
// // //         public void TeleportToTarget()
// // //         {
// // //             if (target != null)
// // //                 transform.position = target.position + offset;
// // //         }
// // //
// // //         public void SetPosition(Vector3 position)
// // //         {
// // //             transform.position = position;
// // //         }
// // //     }
// // // }
// //
// // using UnityEngine;
// // using UnityEngine.InputSystem;
// //
// // namespace Camera
// // {
// //     public class CameraController : MonoBehaviour
// //     {
// //         [Header("Camera Settings")]
// //         [SerializeField] private Vector3 startPos;
// //         [SerializeField] private float followStartX = 2f;
// //         [SerializeField] private Transform target;
// //         [SerializeField] private float cameraSpeed = 5f;
// //         [SerializeField] private Vector3 offset = new(0, 0, -10f);
// //         [SerializeField] private float minCameraHeight = -10f;
// //
// //         [Header("Look Ahead Settings")]
// //         [SerializeField] private float lookAheadDistance = 2f;
// //         [SerializeField] private float lookAheadSpeed = 5f;
// //         [SerializeField] private float lookHoldTime = 0.3f;
// //         
// //         private PlayerInput _playerInput;
// //         private InputAction _lookAction;
// //
// //         private float _currentLookAheadX;
// //         private float _targetLookAheadX;
// //         private float _lookReleaseTimer;
// //
// //         private bool _canStartFollow = false;
// //
// //         public bool CanFollow { get; set; } = true;
// //
// //         private void Awake()
// //         {
// //             _playerInput = FindAnyObjectByType<PlayerInput>();
// //
// //             if (_playerInput != null)
// //             {
// //                 _lookAction = _playerInput.actions["Look"];
// //             }
// //
// //             transform.position = startPos; // Ставим стартовую позицию камеры
// //         }
// //
// //         private void Start()
// //         {
// //             if (target == null)
// //             {
// //                 var player = FindAnyObjectByType<Player.Player>();
// //                 if (player != null)
// //                     target = player.transform;
// //             }
// //         }
// //
// //         private void FixedUpdate()
// //         {
// //             if (!CanFollow || !target) return;
// //
// //             // Проверяем, достиг ли игрок нужной позиции
// //             if (!_canStartFollow)
// //             {
// //                 if (target.position.x >= followStartX)
// //                 {
// //                     _canStartFollow = true; // Только теперь разрешаем двигать камеру
// //                 }
// //                 else
// //                 {
// //                     return; // До нужной точки — камера стоит на месте
// //                 }
// //             }
// //
// //             Vector3 desiredPosition = target.position + offset;
// //
// //             float lookInput = 0f;
// //             if (_lookAction != null)
// //             {
// //                 lookInput = _lookAction.ReadValue<Vector2>().x;
// //             }
// //
// //             if (Mathf.Abs(lookInput) > 0.01f)
// //             {
// //                 _targetLookAheadX = lookAheadDistance * Mathf.Sign(lookInput);
// //                 _lookReleaseTimer = lookHoldTime;
// //             }
// //             else
// //             {
// //                 if (_lookReleaseTimer > 0)
// //                 {
// //                     _lookReleaseTimer -= Time.fixedDeltaTime;
// //                 }
// //                 else
// //                 {
// //                     _targetLookAheadX = 0f;
// //                 }
// //             }
// //
// //             _currentLookAheadX = Mathf.Lerp(_currentLookAheadX, _targetLookAheadX, Time.fixedDeltaTime * lookAheadSpeed);
// //
// //             desiredPosition.x += _currentLookAheadX;
// //
// //             if (desiredPosition.y < minCameraHeight)
// //             {
// //                 desiredPosition.y = minCameraHeight;
// //             }
// //             
// //             transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.fixedDeltaTime * cameraSpeed);
// //         }
// //
// //         public void TeleportToTarget()
// //         {
// //             if (target != null)
// //                 transform.position = target.position + offset;
// //         }
// //
// //         public void SetPosition(Vector3 position)
// //         {
// //             transform.position = position;
// //         }
// //         
// //         public void ResetPosition()
// //         {
// //             transform.position = startPos;
// //         }
// //     }
// // }

using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Vector3 startPos;
        [SerializeField] private float followStartX = 2f;
        [SerializeField] private Transform target;
        [SerializeField] private float cameraSpeed = 5f;
        [SerializeField] private Vector3 offset = new(0, 0, -10f);
        [SerializeField] private float minCameraHeight = -10f;

        [Header("Look Ahead Settings")]
        [SerializeField] private float lookAheadDistance = 2f;
        [SerializeField] private float lookAheadSpeed = 5f;
        [SerializeField] private float lookHoldTime = 0.3f;

        private PlayerInput _playerInput;
        private InputAction _lookAction;

        private float _currentLookAheadX;
        private float _targetLookAheadX;
        private float _lookReleaseTimer;

        private bool _canStartFollow;
        private bool _isTransitioning;

        public bool CanFollow { get; set; } = true;

        private void Awake()
        {
            _playerInput = FindAnyObjectByType<PlayerInput>();

            if (_playerInput != null)
            {
                _lookAction = _playerInput.actions["Look"];
            }

            transform.position = startPos;
        }

        private void Start()
        {
            if (target == null)
            {
                var player = FindFirstObjectByType<Player.Player>();
                if (player != null)
                {
                    target = player.transform;
                }
            }
        }
        
        private void FixedUpdate()
        {
            if (!CanFollow || !target) return;

            if (!_canStartFollow)
            {
                if (target.position.x >= followStartX)
                {
                    _isTransitioning = true; // запустить плавное приближение
                    _canStartFollow = true;
                }
                else
                {
                    return;
                }
            }

            Vector3 desiredPosition = target.position + offset;

            float lookInput = 0f;
            if (_lookAction != null)
            {
                lookInput = _lookAction.ReadValue<Vector2>().x;
            }

            if (Mathf.Abs(lookInput) > 0.01f)
            {
                _targetLookAheadX = lookAheadDistance * Mathf.Sign(lookInput);
                _lookReleaseTimer = lookHoldTime;
            }
            else
            {
                if (_lookReleaseTimer > 0)
                {
                    _lookReleaseTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    _targetLookAheadX = 0f;
                }
            }

            _currentLookAheadX = Mathf.Lerp(_currentLookAheadX, _targetLookAheadX, Time.fixedDeltaTime * lookAheadSpeed);

            desiredPosition.x += _currentLookAheadX;

            if (desiredPosition.y < minCameraHeight)
            {
                desiredPosition.y = minCameraHeight;
            }

            // Если ещё идёт плавное приближение — двигаться медленно
            if (_isTransitioning)
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.fixedDeltaTime * (cameraSpeed * 0.5f)); // медленнее
                // Как только камера очень близко к нужной позиции — заканчиваем переход
                if (Vector3.Distance(transform.position, desiredPosition) < 0.1f)
                {
                    _isTransitioning = false;
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.fixedDeltaTime * cameraSpeed);
            }
        }

        public void TeleportToTarget()
        {
            if (target != null)
                transform.position = target.position + offset;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void ResetPosition()
        {
            transform.position = startPos;
            _canStartFollow = false;
            _isTransitioning = false;
        }
    }
}
