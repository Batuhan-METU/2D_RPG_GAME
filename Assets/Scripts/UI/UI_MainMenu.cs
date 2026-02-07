using UnityEngine;

public class UI_MainMenu : MonoBehaviour
{

    private void Start()
    {
        transform.root.GetComponentInChildren<UI_Options>(true).LoadUpVolume();
        transform.root.GetComponentInChildren<UI_FadeScreen>().DoFadeIn();

        AudioManager.instance.StartBGM("playlist_mainMenu");
    }

    public void PlayBTN()
    {
        AudioManager.instance.PlayGlobalSFX("button_click");
        GameManager.instance.ContinuePlay();
    }


    public void QuitGameBTN()
    {
        Application.Quit();
    }
}
