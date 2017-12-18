using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SoundManager : MonoBehaviour
    {
        public List<AudioClip> SlicingSounds = new List<AudioClip>();
        public AudioSource Source;
        public static SoundManager Instance = null;

        private void Start()
        {
            if (Instance == null) Instance = this;
        }

        public void PlayRandomSound()
        {
            int randomSoundId = Random.Range(0, SlicingSounds.Count);
            Source.PlayOneShot(SlicingSounds[randomSoundId]);
        }
    }
}