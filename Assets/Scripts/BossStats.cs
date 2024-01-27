using So;
using UnityEngine;

public class BossStats : MonoBehaviour
{
    [Header("General")] public float speed = 3.5f;
    public float decisionCoolDown = 3.0f;
    public float stepsCoolDown = 0.3f;
    public Transform playerTransform;
    [Header("Tickling")] public float tickleDamage = 100.0f;
    public float tickleRange = 1.5f;
    public float chasingDurationMin = 3.0f;
    public float chasingDurationMax = 8.0f;
    [Header("Joking")] public JokesBankSo jokes;
    public float jokeDamage = 150.0f;
    public float jokeSpeed = 5.0f;
    public float jokeTokenSpace = 0.1f;
    public GameObject jokeTokenPrefab;
    public float jokePreparationTime = 0.5f;
    public float jokeRecoveryTime = 0.5f;
    [Header("Acting")] public float actFovAngle = 80.0f;
    public float actDurationMin = 5.0f;
    public float actDurationMax = 10.0f;
    public float actDamageTick = 0.1f;
    public float actDamagePerTick = 10.0f;
}