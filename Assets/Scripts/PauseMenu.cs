using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    
    [SerializeField] private InputActionReference openMenu;
    [SerializeField] private GameObject menuPausa;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private GameObject defeatMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FinalOrb.OnPlayerWin += showVictory;
        DeathPlayer.OnPlayerDefeat += showDefeat;
        menuPausa.SetActive(false);
        victoryMenu.SetActive(false);
        defeatMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (openMenu.action.triggered)
        {
            alternateMenu(menuPausa);



        }
    }
    public void alternateMenu(GameObject menu)
    {
        bool menuActive = !menu.activeSelf;
        menu.SetActive(menuActive);
        Time.timeScale = menuActive ? 0f : 1f;
        AudioListener.pause = menuActive;
        Cursor.lockState = menuActive ? CursorLockMode.None : CursorLockMode.Locked;
    }
    public void reset()
    {
        Time.timeScale = 1f;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
    }
    private void showVictory()
    {
        alternateMenu(victoryMenu);
    }
    private void showDefeat()
    {
        alternateMenu(defeatMenu);
    }
}
