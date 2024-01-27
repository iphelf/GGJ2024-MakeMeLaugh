using UnityEngine.SceneManagement;

public static class GameControl
{
    public static void OpenBattle()
    {
        SceneManager.LoadScene("Battle");
    }

    public static void OpenEnd()
    {
        SceneManager.LoadScene("End");
    }

    public static void OpenEntry()
    {
        SceneManager.LoadScene("Entry");
    }
}