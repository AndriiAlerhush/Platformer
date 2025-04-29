using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player
{
    public class Player : MonoBehaviour
    {
        private static readonly int RealState = Animator.StringToHash("state");
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private AudioSource _audioSource;
        private bool _isJumpKeyPressed;
    
        public bool EnableControl { get; set; }
    
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5.0f;
        private float _horizontalMovement;
    
        [Header("Jumping")]
        [SerializeField] private float jumpForce = 10.0f;
    
        [Header("Lives")]
        [SerializeField] private int lives = 5;
    
        [Header("GroundCheck")]
        [SerializeField] private Transform groundCheckPos;
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
        [SerializeField] private LayerMask groundLayer;
    
        [Header("Gravity Settings")]
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private float lowJumpMultiplier = 2f; 
    
        [Header("Audio Effects")]
        [SerializeField] private AudioClip jumpSound;
        [SerializeField] private AudioClip spawnSound;
    
        [Header("Hearts")]
        [SerializeField] private Image[] hearts;
        [SerializeField] private Sprite aliveHeart;
        [SerializeField] private Sprite deadHeart;
    
        private States State
        {
            // get => (States)_animator.GetInteger(RealState);
            set => _animator.SetInteger(RealState, (int)value);
        }
    
        void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();
            EnableControl = true;
        }
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        // void Start()
        // {
        //     
        // }
    
        // Update is called once per frame
        void Update()
        {
            if (!EnableControl) return;
        
            // movement
            _rigidbody.linearVelocity = new Vector2(_horizontalMovement * moveSpeed, _rigidbody.linearVelocity.y);
        
            // long jump
            if (_rigidbody.linearVelocity.y < 0)
            {
                _rigidbody.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }
            // short jump
            else if (_rigidbody.linearVelocity.y > 0 && !_isJumpKeyPressed)
            {
                _rigidbody.linearVelocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
            }
        
            UpdateAnimationState();
        }
    
        public void Move(InputAction.CallbackContext context)
        {
            if (!EnableControl) return;
        
            if (IsGrounded())
                State = States.Run;
        
            _horizontalMovement = context.ReadValue<Vector2>().x;
            // _spriteRenderer.flipX = _horizontalMovement < 0.0f;
            
            _spriteRenderer.flipX = _horizontalMovement switch
            {
                > 0.0f => false,
                < 0.0f => true,
                _ => _spriteRenderer.flipX
            };
        }
        
        public void Jump(InputAction.CallbackContext context)
        {
            if (!EnableControl) return;
        
            if (context.performed && IsGrounded())
            {
                PlayJumpSound();
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, jumpForce);
                _isJumpKeyPressed = true;
            }

            if (context.canceled && _rigidbody.linearVelocity.y > 0)
            {
                _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, _rigidbody.linearVelocity.y * 0.5f);
                _isJumpKeyPressed = false;
            }
        
            if (!IsGrounded())
                State = States.Jump;
        }
    
        public bool IsGrounded()
        {
            return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
        }
    
        private void UpdateAnimationState()
        {
            if (!IsGrounded())
            {
                State = States.Jump;
            }
            else if (Mathf.Abs(_horizontalMovement) > 0.1f)
            {
                State = States.Run;
            }
            else
            {
                State = States.Idle;
            }
        }
    
        public bool MinusLive()
        {
            if (lives > 0 && lives <= hearts.Length)
            {
                hearts[lives - 1].sprite = deadHeart;
                lives--;
                return true;
            }

            return false;
        }

        public Rigidbody2D GetRigidbody()
        {
            return _rigidbody;
        }

        public SpriteRenderer GetSpriteRenderer()
        {
            return _spriteRenderer;
        }
    
        public void ResetMovement()
        {
            _horizontalMovement = 0f;
            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.angularVelocity = 0f;
            _rigidbody.simulated = false;
            _rigidbody.simulated = true;
        }

        public void PlayJumpSound()
        {
            _audioSource.PlayOneShot(jumpSound);
        }
    
        public void PlaySpawnSound()
        {
            _audioSource.PlayOneShot(spawnSound);
        }
        
        public void ForceState(States state)
        {
            _animator.SetInteger(RealState, (int)state);
        }
    }

    public enum States
    {
        Idle,
        Run,
        Jump
    }
}