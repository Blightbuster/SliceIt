using UnityEngine;

namespace Game
{
    public class GroundChecker : MonoBehaviour
    {
        public bool IsGrounded;
        private int _currentTouching;

        void Start()
        {
            InvokeRepeating("CheckStatus", 0, 0.2f);
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject colliderGO = collision.collider.gameObject;
            if (colliderGO.CompareTag("Slice") || colliderGO.CompareTag("Ground"))
            {
                _currentTouching++;
            }
        }

        void OnCollisionExit2D(Collision2D collision)
        {
            GameObject colliderGO = collision.collider.gameObject;
            if (colliderGO.CompareTag("Slice") || colliderGO.CompareTag("Ground"))
            {
                _currentTouching--;
            }
        }

        private void CheckStatus()
        {
            IsGrounded = _currentTouching >= 1;
        }
    }
}
