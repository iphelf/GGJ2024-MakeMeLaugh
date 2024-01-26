using System.Collections.Generic;
using UnityEngine;

namespace So
{
    [CreateAssetMenu(fileName = "jokes", menuName = "ScriptableObjects/JokesBank")]
    public class JokesBankSo : ScriptableObject
    {
        public List<string> jokes = new();
    }
}