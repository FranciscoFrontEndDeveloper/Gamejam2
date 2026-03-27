using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.FullSerializer;

public class MainScript : MonoBehaviour
{
    private VisualElement root;
    private VisualElement titleContainer;
    private VisualElement menuContainer;
    private VisualElement settingsContainer;
    private VisualElement pauseContainer;
    private Button enterButton;
    private Button pauseButton;
    private Button newGameButton;
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
        titleContainer = root.Q<VisualElement>("TitleContainer");
        menuContainer = root.Q<VisualElement>("MenuContainer");
        settingsContainer = root.Q<VisualElement>("SettingsContainer");
        pauseContainer = root.Q<VisualElement>("PauseContainer");
        enterButton = root.Q<Button>("EnterButton");
        pauseButton = root.Q<Button>("PauseButton");
        newGameButton = root.Q<Button>("NewGameButton");
        quitGameButton = root.Q<Button>("QuitButton");
        settingsGameButton = root.Q<Button>("settingsButton");
        exitButton = root.Q<Button>("ExitButton");


        // Seguridad: comprobar nulos y evitar excepciones si faltan elementos
        if (enterButton != null) enterButton.clicked += ToggleMenu;
        if (pauseButton != null) pauseButton.clicked += TogglePause;
        if (newGameButton != null) newGameButton.clicked += startGame;
        if (quitGameButton != null) quitGameButton.clicked += QuitGame;
        if (settingsGameButton != null) settingsGameButton.clicked += SettingsGame;
        if (exitButton != null) exitButton.clicked += ToggleSettings;
        controls.UI.Confirm.performed += confirmEnter;
        controls.UI.Pause.performed += EscapePause;
        controls.UI.Enable();

        if (enterButton != null) enterButton.focusable = false;
        if (pauseButton != null) pauseButton.focusable = false;
    }

    private void OnDisable()
    {
        // Desuscribir eventos para evitar fugas
        if (enterButton != null) enterButton.clicked -= ToggleMenu;
        if (pauseButton != null) pauseButton.clicked -= TogglePause;
        if (newGameButton != null) newGameButton.clicked -= startGame;
        if (quitGameButton != null) quitGameButton.clicked -= QuitGame;
        if (settingsGameButton != null) settingsGameButton.clicked -= SettingsGame;
        if (exitButton != null) exitButton.clicked -= ToggleSettings;
        controls.UI.Confirm.performed -= confirmEnter;
        controls.UI.Pause.performed -= EscapePause;
        controls.UI.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void confirmEnter(InputAction.CallbackContext context)
    {
        ToggleMenu();
    }

    private void ToggleMenu()
    {
        bool isTitleVisible = titleContainer != null && titleContainer.resolvedStyle.display != DisplayStyle.None;

        if (titleContainer != null) titleContainer.style.display = isTitleVisible ? DisplayStyle.None : DisplayStyle.Flex;
        if (menuContainer != null) menuContainer.style.display = isTitleVisible ? DisplayStyle.Flex : DisplayStyle.None;

        Debug.Log("Cambiando visibilidad directamente");
    }

    private void EscapePause(InputAction.CallbackContext context)
    {
        TogglePause();
    }

    private void TogglePause()
    {
        bool isTitleVisible = titleContainer != null && titleContainer.resolvedStyle.display != DisplayStyle.None;

        if (titleContainer != null) titleContainer.style.display = isTitleVisible ? DisplayStyle.None : DisplayStyle.Flex;
        if (menuContainer != null) menuContainer.style.display = isTitleVisible ? DisplayStyle.Flex : DisplayStyle.None;

        Debug.Log("pausa juego");
    }

    private void SettingsGame()
    {

        // Si no existe settingsContainer, salir y loggear
        if (menuContainer == null || settingsContainer == null)
        {
            Debug.LogWarning("menuContainer o settingsContainer no asignado en el UIDocument.");
            return;
        }

        bool isMenuVisible = menuContainer.resolvedStyle.display != DisplayStyle.None;

        // Si el menú está visible, ocultarlo y mostrar settings. Si no, revertir.
        menuContainer.style.display = isMenuVisible ? DisplayStyle.None : DisplayStyle.Flex;
        settingsContainer.style.display = isMenuVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void ToggleSettings()
    {
        Debug.Log("toggle settings");
        bool isMenuVisible = menuContainer.resolvedStyle.display != DisplayStyle.None;

        // Si el menú está visible, ocultarlo y mostrar settings. Si no, revertir.
        menuContainer.style.display = isMenuVisible ? DisplayStyle.None : DisplayStyle.Flex;
        settingsContainer.style.display = isMenuVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    private void startGame()
    {
        SceneManager.LoadScene("escena1");
    }

    private void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
