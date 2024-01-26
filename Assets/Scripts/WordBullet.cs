using TMPro;
using UnityEngine;

public class WordBullet : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private RectTransform textRect;
    [SerializeField] private Transform backgroundTransform;
    [SerializeField] private BoxCollider boxCollider;

    private float _damage;

    public void SetDamage(float damage) => _damage = damage;

    public void SetWord(string word) => SetWord(word, out _);

    public void SetWord(string word, out float width)
    {
        text.text = word;

        width = word.Length / 4.0f;
        {
            Vector2 newSize = textRect.sizeDelta;
            newSize.x = width;
            textRect.sizeDelta = newSize;
        }
        {
            Vector3 newSize = backgroundTransform.localScale;
            newSize.z = width;
            backgroundTransform.localScale = newSize;
        }
        {
            Vector3 newSize = boxCollider.size;
            newSize.z = width;
            boxCollider.size = newSize;
        }
    }

    private float _speed;

    public void Shoot(float speed) => _speed = speed;

    public void FlipUpsideDown() => textRect.Rotate(0.0f, 0.0f, 180.0f);

    private void Update() => transform.Translate(0.0f, 0.0f, _speed * Time.deltaTime);

    private void OnTriggerEnter(Collider other)
    {
        var playerControl = other.GetComponent<PlayerControl>();
        if (playerControl != null)
            playerControl.TakeJokeHit(_damage);
        Destroy(gameObject);
    }
}