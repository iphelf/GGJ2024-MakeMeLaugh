using UnityEngine;

namespace MakeMeLaugh.Scripts
{
    public class EntryControl : MonoBehaviour
    {
        public void Play()
        {
            GameControl.OpenBattle();
        }

        public void Quit()
        {
            GameControl.QuitGame();
        }
    }
}