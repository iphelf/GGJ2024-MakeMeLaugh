using System.Collections.Generic;
using UnityEngine;

namespace MakeMeLaugh.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerVoice : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> l1Laughs = new();
        [SerializeField] private List<AudioClip> l2Laughs = new();
        [SerializeField] private List<AudioClip> l3Laughs = new();
        private AudioSource _audio;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
        }

        public void Laugh()
        {
            if (_audio.isPlaying) return;
            Play(l1Laughs[Random.Range(0, l1Laughs.Count)]);
        }

        public void Lol()
        {
            if (_audio.isPlaying) return;
            Play(l2Laughs[Random.Range(0, l2Laughs.Count)]);
        }

        public void Lofl()
        {
            if (_audio.isPlaying) return;
            Play(l3Laughs[Random.Range(0, l3Laughs.Count)]);
        }

        private void Play(AudioClip clip)
        {
            _audio.clip = clip;
            _audio.Play();
        }
    }
}