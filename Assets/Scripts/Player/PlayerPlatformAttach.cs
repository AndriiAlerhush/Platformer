using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerPlatformAttach : MonoBehaviour
    {
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("MovingPlatform"))
            {
                _rb.transform.parent = collision.transform;
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("MovingPlatform"))
            {
                // _rb.transform.parent = null;
                StartCoroutine(DetachFromPlatform());
            }
        }
        
        private IEnumerator DetachFromPlatform()
        {
            yield return null;
            _rb.transform.parent = null;
        }
    }
}
