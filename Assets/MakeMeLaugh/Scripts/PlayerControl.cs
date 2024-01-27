using System;
using UnityEngine;

namespace MakeMeLaugh.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerVoice))]
    public class PlayerControl : MonoBehaviour
    {
        private CharacterController _controller;
        private PlayerStats _stats;
        private PlayerVoice _voice;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Camera closedCamera;
        [SerializeField] private SpriteRenderer face;
        [SerializeField] private GameObject sound;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _stats = GetComponent<PlayerStats>();
            _voice = GetComponent<PlayerVoice>();
            PlayerCondition.eyesClosed = false;
            PlayerCondition.earsHeld = false;
            PlayerCondition.moving = false;
        }

        private void Update()
        {
            HandleMovement();
            if (Input.GetKeyDown(KeyCode.LeftShift))
                HoldEars();
            if (Input.GetKeyUp(KeyCode.LeftShift))
                ReleaseEars();
            if (Input.GetKeyDown(KeyCode.Space))
                CloseEyes();
            if (Input.GetKeyUp(KeyCode.Space))
                OpenEyes();

            DecreaseLaugh(_stats.laughAttenuation * Time.deltaTime);
            face.sprite = ConcludeLaughType(_stats.currentLaugh)
                switch
                {
                    GameStats.BattleResult.LaughType.None => _stats.notLaughingSprite,
                    GameStats.BattleResult.LaughType.Normal => _stats.normalLaughSprite,
                    GameStats.BattleResult.LaughType.Lol => _stats.lolSprite,
                    GameStats.BattleResult.LaughType.Lofl => _stats.loflSprite,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            float currentGainRate = _stats.gainRate.Evaluate(_stats.currentLaugh);
            if (currentGainRate < 0.0f)
                GameStats.lastResult.laughingTime += Time.deltaTime;
            _stats.currentScore += currentGainRate * Time.deltaTime;
            GameStats.lastResult.score = _stats.currentScore;
        }

        private void HandleMovement()
        {
            Vector3 direction = new(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (!Mathf.Approximately(direction.sqrMagnitude, 0.0f))
            {
                _controller.SimpleMove(_stats.speed * direction.normalized);
                transform.rotation = Quaternion.LookRotation(direction);
                PlayerCondition.moving = true;
                sound.SetActive(true);
            }
            else
            {
                PlayerCondition.moving = false;
                sound.SetActive(false);
            }
        }

        private void HoldEars()
        {
            Debug.Log(nameof(HoldEars));
            PlayerCondition.earsHeld = true;
        }

        private void ReleaseEars()
        {
            Debug.Log(nameof(ReleaseEars));
            PlayerCondition.earsHeld = false;
        }

        private void CloseEyes()
        {
            Debug.Log(nameof(CloseEyes));
            PlayerCondition.eyesClosed = true;
            mainCamera.enabled = false;
            closedCamera.enabled = true;
        }

        private void OpenEyes()
        {
            Debug.Log(nameof(OpenEyes));
            PlayerCondition.eyesClosed = false;
            closedCamera.enabled = false;
            mainCamera.enabled = true;
        }

        public void TakeTickleHit(float damage) => IncreaseLaugh(damage);

        public void TakeJokeHit(float damage)
        {
            if (PlayerCondition.earsHeld) return;
            IncreaseLaugh(damage);
        }

        public void TakeActHit(float damage)
        {
            if (PlayerCondition.eyesClosed) return;
            IncreaseLaugh(damage);
        }

        private void IncreaseLaugh(float laugh)
        {
            float lastGainRate = _stats.gainRate.Evaluate(_stats.currentLaugh);

            _stats.currentLaugh = Mathf.Min(_stats.maximumLaugh, _stats.currentLaugh + laugh);

            float currentGainRate = _stats.gainRate.Evaluate(_stats.currentLaugh);
            if (lastGainRate > 0.0f && currentGainRate <= 0.0f)
                ++GameStats.lastResult.laughCount;
            var currentLaughType = ConcludeLaughType(_stats.currentLaugh);
            if (currentLaughType > GameStats.lastResult.laughPeak)
                GameStats.lastResult.laughPeak = currentLaughType;
            switch (currentLaughType)
            {
                case GameStats.BattleResult.LaughType.Lol:
                    _voice.Lol();
                    break;
                case GameStats.BattleResult.LaughType.Lofl:
                    _voice.Lofl();
                    break;
                default:
                    _voice.Laugh();
                    break;
            }
        }

        private GameStats.BattleResult.LaughType ConcludeLaughType(float laugh)
        {
            float ratio = laugh / _stats.maximumLaugh;
            if (ratio < _stats.normalLaughThreshold) return GameStats.BattleResult.LaughType.None;
            if (ratio < _stats.lolThreshold) return GameStats.BattleResult.LaughType.Normal;
            if (ratio < _stats.loflThreshold) return GameStats.BattleResult.LaughType.Lol;
            return GameStats.BattleResult.LaughType.Lofl;
        }

        private void DecreaseLaugh(float laugh)
        {
            _stats.currentLaugh = Mathf.Max(0.0f, _stats.currentLaugh - laugh);
        }
    }
}