using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PauseScript : MonoBehaviour
{
    private VisualElement root;
    private VisualElement settingsContainer;
    private VisualElement pauseContainer;
    private Button pauseButton;
    private Button quitGameButton;
    private Button settingsGameButton;
    private Button exitButton;

    private InputSystem_Actions controls;
    private void Awake()
    {
        controls = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        pauseContainer = root.Q<VisualElement>("PauseContainer");
        settingsContainer = root.Q<VisualElement>("SettingsContainer");
        pauseButton = root.Q<Button>("PauseButton");
        quitGameButton = root.Q<Button>("pauseQuit");
        settingsGameButton = root.Q<Button>("pauseSettings");
        exitButton = root.Q<Button>("ExitButton");

        if (pauseButton != null) pauseButton.clicked += TogglePause;
        if (pauseButton != null) pauseButton.focusable = false;
        if (quitGameButton != null) quitGameButton.clicked += QuitGame;
        if (settingsGameButton != null) settingsGameButton.clicked += SettingsGame;
        if (exitButton != null) exitButton.clicked += ToggleSettings;

        controls.UI.Pause.performed += EscapePause;
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        if (pauseButton != null) pauseButton.clicked -= TogglePause;
        if (quitGameButton != null) quitGameButton.clicked -= QuitGame;
        if (settingsGameButton != null) settingsGameButton.clicked -= SettingsGame;
        if (exitButton != null) exitButton.clicked -= ToggleSettings;

        controls.UI.Pause.performed -= EscapePause;
        controls.UI.Disable();
    }

    private void EscapePause(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    private void TogglePause()
    {
        if (pauseContainer == null) return;

        bool isPauseVisible = pauseContainer.resolvedStyle.display != DisplayStyle.None;
        pauseContainer.style.display = isPauseVisible ? DisplayStyle.None : DisplayStyle.Flex;
    }

    private void SettingsGame()
    {
        // abrir settings (ocultar pause, mostrar settings)
        if (pauseContainer == null || settingsContainer == null)
        {
            Debug.LogWarning("pauseContainer o settingsContainer no asignado en el UIDocument.");
            return;
        }

        bool isPauseVisible = pauseContainer.resolvedStyle.display != DisplayStyle.None;

        // si el pause está visible -> ocultarlo y mostrar settings; si no -> revertir
        pauseContainer.style.display = isPauseVisible ? DisplayStyle.None : DisplayStyle.Flex;
        settingsContainer.style.display = isPauseVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void ToggleSettings()
    {
        // cerrar settings (si está abierto) y volver a pause
        if (pauseContainer == null || settingsContainer == null)
        {
            Debug.LogWarning("pauseContainer o settingsContainer no asignado en el UIDocument.");
            return;
        }

        bool isSettingsVisible = settingsContainer.resolvedStyle.display != DisplayStyle.None;

        settingsContainer.style.display = isSettingsVisible ? DisplayStyle.None : DisplayStyle.Flex;
        pauseContainer.style.display = isSettingsVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
