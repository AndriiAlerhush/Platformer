using UnityEngine;

namespace Platform
{
    public class MovingPlatformController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float startPosX = 1f;
        [SerializeField] private float endPosX = 5f;
        [SerializeField] private float speed = 1f;
        
        private bool _movingRight = true;
        
        private void FixedUpdate()
        {
            if (transform.position.x > endPosX)
            {
                _movingRight = false;
            }
            else if (transform.position.x < startPosX)
            {
                _movingRight = true;
            }
            
            if (_movingRight)
            {
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            }
        }
    }
}
