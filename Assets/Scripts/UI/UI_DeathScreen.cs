using UnityEngine;

public class UI_DeathScreen : MonoBehaviour
{
    public void GoToCampBTN()
    {
        GameManager.instance.ChangeScene("Level_0", RespawnType.NonSpecific);
    }

    public void GoToCheckpointBTN()
    {
        GameManager.instance.RestartScene();
    }

    public void GoToMainMenuBTN()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);
    }
}
