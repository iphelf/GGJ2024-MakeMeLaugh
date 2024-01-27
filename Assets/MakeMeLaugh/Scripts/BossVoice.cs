using System.Collections.Generic;
using UnityEngine;

namespace MakeMeLaugh.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class BossVoice : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> clips = new();
        private AudioSource _audio;
        private const float FadeOutDuration = 0.7f;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
        }

        private int _currentVoiceId;

        public int StartNew()
        {
            AudioClip clip = clips[Random.Range(0, clips.Count)];
            _audio.clip = clip;
            Volume = 1.0f;
            _audio.Play();
            return ++_currentVoiceId;
        }

        private float _volume = 1.0f;

        private float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                if (!_muted)
                    _audio.volume = value;
            }
        }

        private int _fadingVoiceId = -1;
        private float _remainingTime;

        public void FadeOut(int id)
        {
            if (id < _currentVoiceId) return;
            _fadingVoiceId = id;
            _remainingTime = FadeOutDuration;
        }

        private void UpdateFadeOut()
        {
            if (_fadingVoiceId < _currentVoiceId) return;
            _remainingTime -= Time.deltaTime;
            if (_remainingTime <= 0.0f)
            {
                _remainingTime = 0.0f;
                _audio.Stop();
                _audio.clip = null;
                Volume = 1.0f;
                _fadingVoiceId = -1;
            }
            else
                Volume = _remainingTime / FadeOutDuration;
        }

        private bool _muted;

        private void Mute()
        {
            _audio.mute = true;
            _muted = true;
        }

        private void Unmute()
        {
            _audio.mute = false;
            _audio.volume = Volume;
            _muted = false;
        }

        private void Update()
        {
            bool blocked = PlayerCondition.earsHeld;
            if (blocked && !_muted)
                Mute();
            else if (!blocked && _muted)
                Unmute();
            UpdateFadeOut();
        }
    }
}