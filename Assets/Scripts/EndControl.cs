using UnityEngine;

public class EndControl : MonoBehaviour
{
    public void ReturnToTitle()
    {
        GameControl.OpenEntry();
    }

    public void AnotherRound()
    {
        GameControl.OpenBattle();
    }
}