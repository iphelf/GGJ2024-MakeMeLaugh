using System.Collections.Generic;
using UnityEngine;

namespace MakeMeLaugh.Scripts.So
{
    [CreateAssetMenu(fileName = "jokes", menuName = "ScriptableObjects/JokesBank")]
    public class JokesBankSo : ScriptableObject
    {
        public List<string> jokes = new();
    }
}