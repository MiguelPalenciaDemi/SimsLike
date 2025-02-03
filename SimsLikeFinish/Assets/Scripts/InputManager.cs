using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance => _instance;
    
    
    private Camera _currentCamera;
    private bool _mouseOverUI;                      //Is the pointer over UI? -> to avoid press in it
    private Vector3 _lastClickPosition;             //It stores last click position. We use it in GoToOrders
    private ObjectInteract _currentObjectInteract;  //It is needed to know if we press in an object or in the floor
    
    
    public ObjectInteract CurrentObjectInteract => _currentObjectInteract;
    public Vector3 LastClickPosition => _lastClickPosition;

    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;
    }
    
    private void Start()
    {
        _currentCamera = Camera.main;
    }

    private void Update()
    {
        _mouseOverUI = EventSystem.current.IsPointerOverGameObject();       //Check if we are over UI
    }


    //It checks where we clicked
    private void TryRaycastToInteract()
    {
        if(_mouseOverUI) return;
        
        var mouseRay = _currentCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(mouseRay, out var raycastHit))
        {
            if(raycastHit.transform.TryGetComponent(out ObjectInteract objectInteract)) //Over an object
            {
                _currentObjectInteract = objectInteract;
                
                var pos = _currentCamera.WorldToScreenPoint(objectInteract.transform.position);
                ActionMenu.Instance.ShowMenuAction(pos,objectInteract.Actions);                         //We activate the action menu
                
            }
            else if(raycastHit.transform.CompareTag("Floor"))
            {
                //Close Action Menu in case it was opened
                _currentObjectInteract = null;
                //ActionMenu.Instance.HideMenuAction();                                                   
                
                //Create a fake ListAction with a GoTo Action
                var tempList = new List<SimAction>();
                tempList.Add(UtilsManager.Instance.GetAction("GoToAction"));                        //We create the action by our UtilsManager
                var pos = _currentCamera.WorldToScreenPoint(raycastHit.point);
                ActionMenu.Instance.ShowMenuAction(pos,tempList);
                
                //Save the position where we clicked to pass the order
                _lastClickPosition = raycastHit.point;
                
            }
        }
    }

    public void OnClick(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
            TryRaycastToInteract();
    }

    public void OnExit(InputAction.CallbackContext ctx)
    {
        Application.Quit();
    }
    
    
}
