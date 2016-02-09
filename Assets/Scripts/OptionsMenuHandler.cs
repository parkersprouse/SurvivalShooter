using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.ObjectModel;

public class OptionsMenuHandler : MonoBehaviour {

    private Dropdown resolutions;
    private Toggle fullscreen, controllerToggle;
    private int selectedIndex;
    private GameObject graphicsPanel, audioPanel, inputPanel, gameplayPanel, baseInputPanel, keyBindingsPanel, gamepadBindingsPanel;
    [SerializeField]
    private SaveInput saveInput;
    private Text keybindingsBtn, gamepadBindingsBtn, graphicsBtn, audioBtn, inputBtn, gameplayBtn, keybindingsBackBtn, gamepadBindingsBackBtn;
    private Animator graphicsAnimator, audioAnimator, inputAnimator, gameplayAnimator;
    //private float animatonTimer = 0;

    public static string activePanel = "";

    void Start() {
        keybindingsBackBtn = GameObject.Find("KeybindingsBackBtn").GetComponentInChildren<Text>();
        gamepadBindingsBackBtn = GameObject.Find("GamepadBindingsBackBtn").GetComponentInChildren<Text>();

        // Set up our fullscreen toggle button
        if (GameObject.Find("FullscreenToggle") != null) {
            fullscreen = GameObject.Find("FullscreenToggle").GetComponent<Toggle>();
            fullscreen.isOn = Screen.fullScreen;
        }

        // Set up our resolution dropdown box
        if (GameObject.Find("ResolutionList") != null) {
            resolutions = GameObject.Find("ResolutionList").GetComponent<Dropdown>();

            resolutions.options.Clear();

            foreach (Resolution r in Screen.resolutions) {
                resolutions.options.Add(new Dropdown.OptionData(r.width + " x " + r.height));
            }

            resolutions.value = resolutions.options.Count;
            resolutions.value = 0;
            foreach (Resolution r in Screen.resolutions) {
                if (Screen.width == r.width && Screen.height == r.height) {
                    resolutions.value = System.Array.IndexOf(Screen.resolutions, r);
                }
            }
        }

        // Set up our panels
        if (GameObject.Find("GraphicsPanel") != null) {
            graphicsPanel = GameObject.Find("GraphicsPanel");
            graphicsAnimator = graphicsPanel.GetComponent<Animator>();
            graphicsPanel.SetActive(false);
        }

        if (GameObject.Find("AudioPanel") != null) {
            audioPanel = GameObject.Find("AudioPanel");
            audioAnimator = audioPanel.GetComponent<Animator>();
            audioPanel.SetActive(false);
        }

        if (GameObject.Find("InputPanel") != null) {
            inputPanel = GameObject.Find("InputPanel");
            inputAnimator = inputPanel.GetComponent<Animator>();

            if (GameObject.Find("Base") != null) {
                baseInputPanel = GameObject.Find("Base");

                if (GameObject.Find("KeybindingsBtn") != null) {
                    keybindingsBtn = GameObject.Find("KeybindingsBtn").GetComponentInChildren<Text>();
                }

                if (GameObject.Find("GamepadBindingsBtn") != null) {
                    gamepadBindingsBtn = GameObject.Find("GamepadBindingsBtn").GetComponentInChildren<Text>();
                }
            }
            if (GameObject.Find("Keybindings") != null) {
                keyBindingsPanel = GameObject.Find("Keybindings");
                keyBindingsPanel.SetActive(false);
            }
            if (GameObject.Find("GamepadBindings") != null) {
                gamepadBindingsPanel = GameObject.Find("GamepadBindings");
                gamepadBindingsPanel.SetActive(false);
            }
            inputPanel.SetActive(false);
        }

        if (GameObject.Find("GameplayPanel") != null) {
            gameplayPanel = GameObject.Find("GameplayPanel");
            gameplayAnimator = gameplayPanel.GetComponent<Animator>();
            gameplayPanel.SetActive(false);
        }
    }

    public void SaveSettings() {
        if (graphicsPanel != null && graphicsPanel.activeSelf) {
            Screen.SetResolution(Screen.resolutions[resolutions.value].width, Screen.resolutions[resolutions.value].height, fullscreen.isOn);            
        }
        else if (audioPanel != null && audioPanel.activeSelf) {

        }
        else if (inputPanel != null && inputPanel.activeSelf) {
            saveInput.Save();
        }
        else if (gameplayPanel != null && gameplayPanel.activeSelf) {

        }
    }

    public void BackButtonClicked() {
        activePanel = "";
        Application.LoadLevel("MainMenu");
    }

    public void InputBackButtonClicked() {
        keybindingsBackBtn.color = new Color(0f / 255f, 255f / 255f, 153f / 255f);
        gamepadBindingsBackBtn.color = new Color(0f / 255f, 255f / 255f, 153f / 255f);
        keyBindingsPanel.SetActive(false);
        gamepadBindingsPanel.SetActive(false);
        baseInputPanel.SetActive(true);
    }

    public void GraphicsClicked() {
        if (!graphicsPanel.activeSelf) {
            if (activePanel == "") {
                DisablePanels();
                graphicsPanel.SetActive(true);
                graphicsAnimator.SetTrigger("GraphicsOpen");
                activePanel = "graphics";
            }
            else
                StartCoroutine(DisplayGraphics());
        }
    }

    public void AudioClicked() {
        if (!audioPanel.activeSelf) {
            if (activePanel == "") {
                DisablePanels();
                audioPanel.SetActive(true);
                audioAnimator.SetTrigger("AudioOpen");
                activePanel = "audio";
            }
            else
                StartCoroutine(DisplayAudio());
        }
    }

    public void InputClicked() {
        if (!inputPanel.activeSelf) {
            if (activePanel == "") {
                DisablePanels();
                inputPanel.SetActive(true);
                baseInputPanel.SetActive(true);
                inputAnimator.SetTrigger("InputOpen");
                activePanel = "input";
            }
            else
                StartCoroutine(DisplayInput());
        }
    }

    public void KeybindingsClicked() {
        keybindingsBtn.color = Color.white;
        baseInputPanel.SetActive(false);
        keyBindingsPanel.SetActive(true);
    }

    public void GamepadBindingsClicked() {
        gamepadBindingsBtn.color = Color.white;
        baseInputPanel.SetActive(false);
        gamepadBindingsPanel.SetActive(true);
    }

    public void GameplayClicked() {
        if (!gameplayPanel.activeSelf) {
            if (activePanel == "") {
                DisablePanels();
                gameplayPanel.SetActive(true);
                gameplayAnimator.SetTrigger("GameplayOpen");
                activePanel = "gameplay";
            }
            else
                StartCoroutine(DisplayGameplay());
        }
    }

    private void DisablePanels() {
        graphicsPanel.transform.localScale = new Vector3(0, 0, 1);
        audioPanel.transform.localScale = new Vector3(0, 0, 1);
        inputPanel.transform.localScale = new Vector3(0, 0, 1);
        gameplayPanel.transform.localScale = new Vector3(0, 0, 1);

        graphicsPanel.SetActive(false);
        audioPanel.SetActive(false);
        inputPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        keyBindingsPanel.SetActive(false);
        gamepadBindingsPanel.SetActive(false);
        baseInputPanel.SetActive(false);
    }

    IEnumerator DisplayGraphics() {
        if (activePanel == "audio") audioAnimator.SetTrigger("AudioClose");
        else if (activePanel == "input") inputAnimator.SetTrigger("InputClose");
        else if (activePanel == "gameplay") gameplayAnimator.SetTrigger("GameplayClose");
        yield return new WaitForSeconds(0.2f);
        DisablePanels();
        graphicsPanel.SetActive(true);
        graphicsAnimator.SetTrigger("GraphicsOpen");
        activePanel = "graphics";
    }

    IEnumerator DisplayAudio() {
        if (activePanel == "graphics") graphicsAnimator.SetTrigger("GraphicsClose");
        else if (activePanel == "input") inputAnimator.SetTrigger("InputClose");
        else if (activePanel == "gameplay") gameplayAnimator.SetTrigger("GameplayClose");
        yield return new WaitForSeconds(0.2f);
        DisablePanels();
        audioPanel.SetActive(true);
        audioAnimator.SetTrigger("AudioOpen");
        activePanel = "audio";
    }

    IEnumerator DisplayInput() {
        if (activePanel == "graphics") graphicsAnimator.SetTrigger("GraphicsClose");
        else if (activePanel == "audio") audioAnimator.SetTrigger("AudioClose");
        else if (activePanel == "gameplay") gameplayAnimator.SetTrigger("GameplayClose");
        yield return new WaitForSeconds(0.2f);
        DisablePanels();
        inputPanel.SetActive(true);
        baseInputPanel.SetActive(true);
        inputAnimator.SetTrigger("InputOpen");
        activePanel = "input";
    }

    IEnumerator DisplayGameplay() {
        if (activePanel == "graphics") graphicsAnimator.SetTrigger("GraphicsClose");
        else if (activePanel == "audio") audioAnimator.SetTrigger("AudioClose");
        else if (activePanel == "input") inputAnimator.SetTrigger("InputClose");
        yield return new WaitForSeconds(0.2f);
        DisablePanels();
        gameplayPanel.SetActive(true);
        gameplayAnimator.SetTrigger("GameplayOpen");
        activePanel = "gameplay";
    }

}
