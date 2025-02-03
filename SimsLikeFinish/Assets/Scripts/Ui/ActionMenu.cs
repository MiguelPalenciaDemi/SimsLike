using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    #region Singleton

    private static ActionMenu _instance;
    public static ActionMenu Instance => _instance;

    #endregion
    
    //Action Menu Stuff
    [SerializeField] private Transform closeButton;
    [SerializeField] private GameObject prefabButton;
    [SerializeField] private Transform containerActionMenu;
    [SerializeField] private float distance;    //distance from the center -> "Spread"

    [Header("Action Queue")]
    [SerializeField] private Transform containerActionQueue;
    [SerializeField] private GameObject actionButtonQueuePrefab;
    
    
    private RectTransform _containerRect;
    private List<ActionButton> _buttons;
    private Camera _camera;
    
    //Set Singleton
    private void Awake()
    {
        if(!_instance)
            _instance = this;
        else 
            Destroy(this);
    }

    void Start()
    {
        _containerRect = containerActionMenu.GetComponent<RectTransform>();
        _camera = Camera.main;
        _buttons = new List<ActionButton>();
    }

    

    
    public void ShowMenuAction(Vector3 position, List<SimAction>actions)
    {
        if (actions.Count <= 0) return;
        //Active the menu's game objects
        _containerRect.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        
        AudioManager.Instance.PlaySound("OpenMenu");
        
        SetPosition(position);                          //Set the center of the menu 
        DisplayButtonsInMenu(actions);                  //Calculate the positions and set the position of buttons. Relocate center if it's necessary
        StartCoroutine(nameof(StartAnimationButtons));  //Start Animation
    }

    public void HideMenuAction()
    {
        _containerRect.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        //AudioManager.Instance.PlaySound("CloseMenu");
    }
    private void SetPosition(Vector3 position)
    {
        _containerRect.position = position;
        closeButton.position = position;
    }
    private void DisplayButtonsInMenu(List<SimAction> actions)
    {
        CleanContainer();
        if (actions.Count <= 0) return;     
        
        var angle = 360 / actions.Count;      //Calculate angle between buttons
        var currentAngle = 90;                  //We want the first button located top-side
        
        //Create all the buttons
        foreach (var action in actions)
        {
            
            var item = Instantiate(prefabButton, containerActionMenu);
            
            if (item.TryGetComponent(out ActionButton actionButton))
                actionButton.Init(action);
            else
                Destroy(item);  //If there are some problem, we destroy to avoid errors

            _buttons.Add(actionButton);
            
            //Set Position
            var rect = item.GetComponent<RectTransform>();
            var rad = currentAngle * Mathf.PI / 180;
            rect.pivot = CalculatePivot(currentAngle);
            
            currentAngle += angle;
            
            var x = distance * Mathf.Cos(rad);
            var y = distance * Mathf.Sin(rad);
            
            rect.SetLocalPositionAndRotation(new Vector3(x, y), Quaternion.identity);
        }

        RelocateMenu();
    }

    private void CleanContainer()
    {
        _buttons.Clear();
        foreach (Transform child in containerActionMenu)
        {
            Destroy(child.gameObject);
        }
    }

    //Depend on the position the button has to have the pivot in a specific point.
    private static Vector2 CalculatePivot(float degree)
    {
        var pivot = new Vector2(0.5f,0.5f);
        degree = Mathf.Round(degree);
        //X
        if (degree < 90)
            pivot.x = 0;
        else if (degree > 90 && degree < 270)
            pivot.x = 1;
        else if (degree > 270)
            pivot.x = 0;
        
        //Y
        if (degree < 180)
            pivot.y = 0;
        else if (degree > 180 && degree < 360)
            pivot.y = 1;
        else if (degree > 360)
            pivot.y = 0;

        return pivot;
    }

    //Relocate menu in case it hasn't enough space in canvas
    private void RelocateMenu()
    {
        Canvas.ForceUpdateCanvases();
        var maxWidth = 0f;
        var maxHeight = 0f;
        
        //Find the max width and height
        for (var i = 0; i < containerActionMenu.childCount; i++)
        {
            var rect = containerActionMenu.GetChild(i).GetComponent<RectTransform>();
            if(maxWidth< rect.sizeDelta.x)
                maxWidth = rect.sizeDelta.x; 
            
            if(maxHeight< rect.sizeDelta.y)
                maxHeight = rect.sizeDelta.y;
        }
        
        //Calculate width Menu
        var width = distance + maxWidth;
        var height = distance + maxHeight;
        var x = _containerRect.position.x;
        var y = _containerRect.position.y;
        
        
        
        if (_containerRect.position.x + width > _camera.pixelWidth)//LeftEdge
            x = _camera.pixelWidth - _containerRect.position.x + width - 25;
        else if(_containerRect.position.x - width <= 50)//RightEdge
            x =  width+75;
        
        if (_containerRect.position.y + height > _camera.pixelHeight)//TopEdge
            y = _camera.pixelHeight - _containerRect.position.y + height - 25;
        else if(_containerRect.position.y - height <= 50)//DownEdge
            y = _containerRect.position.y + height + 75;
        
        _containerRect.SetPositionAndRotation(new Vector3(x,y), Quaternion.identity);
        closeButton.GetComponent<RectTransform>().SetPositionAndRotation(new Vector3(x,y), Quaternion.identity);    //Center the close button
        
    }

    private IEnumerator StartAnimationButtons()
    {
        var closeBtn = closeButton.GetComponent<Button>();
        closeBtn.interactable = false;
        
        foreach (var button in _buttons)
        {
            button.StartAnimation();
            yield return new WaitForSeconds(0.1f);
        }
        closeBtn.interactable = true;
    }

    //It is necessary when the order is created to be represented in UI
    public OrderButtonQueueUI CreateOrderButtonQueueUI()
    {
        return Instantiate(actionButtonQueuePrefab, containerActionQueue).GetComponent<OrderButtonQueueUI>();
    }
}
