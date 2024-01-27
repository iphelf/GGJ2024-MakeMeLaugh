using System.Collections.Generic;
using UnityEngine;

public class SoundBlocker : MonoBehaviour
{
    public List<Behaviour> soundMakers;
    public List<GameObject> soundSources;

    private void Update()
    {
        foreach (var soundMaker in soundMakers)
            soundMaker.enabled = !PlayerCondition.earsHeld;
        foreach (var soundSource in soundSources)
            soundSource.SetActive(!PlayerCondition.earsHeld);
    }
}