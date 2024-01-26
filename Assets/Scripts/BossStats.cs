using So;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("General")] public float speed = 3.5f;
    [Header("Tickling")] public float tickleDamage = 100.0f;
    public float tickleRange = 1.5f;
    [Header("Joking")] public JokesBankSo jokes;
    public float jokeDamage = 150.0f;
    public float jokeSpeed = 5.0f;
    public float jokeTokenSpace = 0.1f;
    public GameObject jokeTokenPrefab;
    public float jokePreparationTime = 0.5f;
    public float jokeRecoveryTime = 0.5f;
}