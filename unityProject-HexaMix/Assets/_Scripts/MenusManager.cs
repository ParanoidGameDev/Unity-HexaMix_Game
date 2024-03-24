using UnityEngine;
using UnityEngine.SceneManagement;
public class MenusManager : MonoBehaviour {
    [SerializeField] private GameObject inGamePauseMenu;
    [SerializeField] private bool pauseMenu;
    [SerializeField] private bool inGame;
    public void ToggleInGamePause() {
        this.pauseMenu ^= true;
        this.inGamePauseMenu.SetActive(pauseMenu);
    }
    public void StartBaseGame() {
        this.inGame = true;
        SceneManager.LoadScene("baseGame_Scn");
    }
    public void ExitBaseGame() {
        this.inGame = false;
        SceneManager.LoadScene("menu_Scn");
    }
    public void ExitApp() {
        this.inGame = false;
        //PENDING
    }
}