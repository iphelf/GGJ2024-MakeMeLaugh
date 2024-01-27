using System.Collections.Generic;
using UnityEngine;

public class SoundBlocker : MonoBehaviour
{
    public List<Behaviour> soundMakers;
    public List<GameObject> soundSources;

    private void Update()
    {
        bool notBlocked = !(PlayerCondition.earsHeld || PlayerCondition.moving);
        foreach (var soundMaker in soundMakers)
            soundMaker.enabled = notBlocked;
        foreach (var soundSource in soundSources)
            soundSource.SetActive(notBlocked);
    }
}