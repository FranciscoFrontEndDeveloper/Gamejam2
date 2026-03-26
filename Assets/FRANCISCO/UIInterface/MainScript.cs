using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Unity.VisualScripting.FullSerializer;
public class MainScript : MonoBehaviour
{
    private VisualElement root;
    private VisualElement titleContainer;
    private VisualElement menuContainer;
    private Button enterButton;
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
        enterButton = root.Q<Button>("EnterButton");
        enterButton.clicked += ToggleMenu;
        controls.UI.Confirm.performed += confirmEnter;
        controls.UI.Enable();
        enterButton.focusable = false;
    }

    private void OnDisable()
    {
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
        //titleContainer.ToggleInClassList("hidden");
        //menuContainer.ToggleInClassList("hidden");


        bool isTitleVisible = titleContainer.resolvedStyle.display != DisplayStyle.None;

        titleContainer.style.display = isTitleVisible ? DisplayStyle.None : DisplayStyle.Flex;
        menuContainer.style.display = isTitleVisible ? DisplayStyle.Flex : DisplayStyle.None;

        Debug.Log("Cambiando visibilidad directamente");
    }
}
