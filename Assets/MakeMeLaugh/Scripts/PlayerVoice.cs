using UnityEngine;
using UnityEngine.Audio;

namespace MakeMeLaugh.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerVoice : MonoBehaviour
    {
        [SerializeField] private AudioResource l1Laugh;
        [SerializeField] private AudioResource l2Laugh;
        [SerializeField] private AudioResource l3Laugh;
        private AudioSource _audio;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
        }

        public void Laugh()
        {
            if (_audio.isPlaying) return;
            Play(l1Laugh);
        }

        public void Lol()
        {
            if (_audio.isPlaying) return;
            Play(l2Laugh);
        }

        public void Lofl()
        {
            if (_audio.isPlaying) return;
            Play(l3Laugh);
        }

        private void Play(AudioResource resource)
        {
            _audio.resource = resource;
            _audio.Play();
        }
    }
}