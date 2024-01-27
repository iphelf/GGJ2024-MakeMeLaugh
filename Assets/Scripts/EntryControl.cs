using UnityEngine;

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