using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ActionButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleAction;
    private Animator _animator;
    private SimAction _simAction;
    private RectTransform _rectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

   
    public void Init(SimAction action)
    {
        titleAction.text = action.name;
        _simAction = action;
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnClick()
    {
        //Create ActionOrder
        var person = SimManager.Instance.CurrentPerson;
        var objectInteractable = InputManager.Instance.CurrentObjectInteract;
        var destination = objectInteractable ? objectInteractable.transform.position : InputManager.Instance.LastClickPosition;
        
        //Check if it's an action from an object. If it's null that's means this action cames froma click on the floor
        if (objectInteractable)
        {
            ICommand command;
            if(_simAction is ComplexSimAction complexSimAction)
            {
                command = new ComplexActionOrder(person,complexSimAction,objectInteractable);
            }
            else
            {
                command = new ActionOrder(person,_simAction,objectInteractable);
            }

            SimManager.Instance.AddOrderToPerson(command);
        }
        else
        {
            ICommand command = new GoToOrder(person,destination);
            SimManager.Instance.AddOrderToPerson(command);
        }
        
        AudioManager.Instance.PlaySound("ClickButton");
        
        //Close menu. We know the button is in 2nd level. TO-CHANGE
        if (transform.parent.parent.TryGetComponent(out ActionMenu actionMenu))
            actionMenu.HideMenuAction();
    }

    public void StartAnimation()
    {
        _animator.SetTrigger("Start");
    }
    
    public void OnMouseEnter()
    {
        AudioManager.Instance.PlaySound("HoverButton");
        _animator.SetBool("MouseOver", true);

        
    }
    
    public void OnMouseExit()
    {
        _animator.SetBool("MouseOver", false);
    }

    
}
