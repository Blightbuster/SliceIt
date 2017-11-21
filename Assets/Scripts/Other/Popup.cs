using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Popup : MonoBehaviour
    {
        public string Text;
        public int DisplayTime;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            GetComponentInChildren<Text>().text = Text;
            StartCoroutine(Disappear());
        }

        private IEnumerator Disappear()
        {
            yield return new WaitForSeconds(DisplayTime);   // Wait until set displaytime is over
            _animator.SetFloat("Speed", -1);                 // Reverse animation
            _animator.Play("PopupIn", -1);                   // Play the animation
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length + DisplayTime);    // Destroy gameobject when the animation finished
        }
    }
}
