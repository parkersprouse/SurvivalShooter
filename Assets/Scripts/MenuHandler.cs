using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour {

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            ChangeScene("PLAYGROUND");
        }
    }

    public void ChangeScene(string name) {
        Application.LoadLevel(name);
    }

    public void QuitGame() {
        Application.Quit();
    }

    /*public void SelectLevel(string level) {
        foreach (Level l in LevelManager.levels) {
            if (l.LevelName == level && l.Unlocked) {
                Application.LoadLevel(level);
            }
        }
    }*/

}
