using UnityEngine;
using UnityEngine.Audio;

namespace MakeMeLaugh.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class EntryControl : MonoBehaviour
    {
        private AudioSource _audio;
        [SerializeField] private AudioResource _huh;
        [SerializeField] private AudioResource _hmmm;
        [SerializeField] private GameObject mainPanel;
        [SerializeField] private GameObject helpPanel;

        private void Start()
        {
            _audio = GetComponent<AudioSource>();
        }

        public void OnPointerEnterPlayButton()
        {
            if (!_audio.isPlaying)
            {
                _audio.resource = _huh;
                _audio.Play();
            }
        }

        public void OnPointerEnterQuitButton()
        {
            if (!_audio.isPlaying)
            {
                _audio.resource = _hmmm;
                _audio.Play();
            }
        }

        public void Play()
        {
            GameControl.OpenBattle();
        }

        public void Quit()
        {
            GameControl.QuitGame();
        }

        public void Help()
        {
            helpPanel.SetActive(true);
            mainPanel.SetActive(false);
        }

        public void BackFromHelp()
        {
            mainPanel.SetActive(true);
            helpPanel.SetActive(false);
        }

        public void Credit()
        {
        }
    }
}