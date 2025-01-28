using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void PauseGame()
    {
        PlayerScript.Instance.Pause();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
