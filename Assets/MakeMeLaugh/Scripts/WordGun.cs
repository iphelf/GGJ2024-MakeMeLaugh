using UnityEngine;

namespace MakeMeLaugh.Scripts
{
    public class WordGun : MonoBehaviour
    {
        public string sentence = "The quick brown fox jumps over the lazy dog.";

        [SerializeField] private GameObject wordBulletPrefab;

        private async void Start()
        {
            string[] tokens = sentence.Split(" ");
            await ShootOut(tokens);
        }

        private async Awaitable ShootOut(string[] tokens)
        {
            float space = 0.1f;
            float speed = 5.0f;
            foreach (var token in tokens)
            {
                var go = Instantiate(wordBulletPrefab, transform);
                await Awaitable.NextFrameAsync();
                var wordBullet = go.GetComponent<WordBullet>();
                wordBullet.SetWord(token, out var width);
                go.transform.Translate(0.0f, 0.0f, -width / 2.0f);
                wordBullet.Shoot(speed);
                await Awaitable.WaitForSecondsAsync((width + space) / speed);
            }
        }

        private async Awaitable LayOut(string[] tokens)
        {
            float space = 0.1f;
            float z = space;
            foreach (var token in tokens)
            {
                var go = Instantiate(wordBulletPrefab);
                await Awaitable.NextFrameAsync();
                var wordBullet = go.GetComponent<WordBullet>();
                wordBullet.SetWord(token, out var width);
                go.transform.Translate(0.0f, 0.0f, z + width / 2.0f);
                z += width + space;
                await Awaitable.WaitForSecondsAsync(1f);
            }
        }
    }
}