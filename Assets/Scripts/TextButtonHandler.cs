using UnityEngine;
using UnityEngine.UI;

public class TextButtonHandler : MonoBehaviour {

    public string btnName;
    public bool optionsBtn = false;
    public Sprite btnReg, btnHover;
    
    private Image parentImage;
    private Text text;

    void Start() {
        text = GetComponent<Text>();
        parentImage = GetComponentInParent<Image>();
    }

    void Update() {
        if (optionsBtn) {
            if (OptionsMenuHandler.activePanel != btnName) {
                parentImage.sprite = btnReg;
            }
            else {
                parentImage.sprite = btnHover;
            }
        }
    }

    public void ButtonEnter() {
        text.color = new Color(255f / 255f, 50f / 255f, 50f / 255f);
    }

    public void ButtonExit() {
        text.color = new Color(0f / 255f, 255f / 255f, 153f / 255f);
    }

    public void InputMenuButtonEnter() {
        text.color = new Color(255f / 255f, 50f / 255f, 50f / 255f);
    }

    public void InputMenuButtonExit() {
        text.color = Color.white;
    }

    public void TabHover() {
        if (!Input.GetMouseButton(0)) {
            text.color = Color.red;
        }
    }

    public void TabStopHover() {
        text.color = Color.white;
    }
}
