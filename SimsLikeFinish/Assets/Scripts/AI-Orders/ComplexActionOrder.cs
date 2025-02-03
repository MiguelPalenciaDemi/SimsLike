using System.Collections;
using UnityEngine;

public class ComplexActionOrder : ICommand
{
    private ComplexSimAction _action;
    
    private Person _person;
    private ObjectInteract _objectInteract;
    private bool _isCanceled;
    private OrderButtonQueueUI _orderButtonQueueUI;
    private SimAction _currentSimAction;
    
    public ComplexActionOrder(Person person, ComplexSimAction action, ObjectInteract objectInteract)
    {
        _person = person;
        _action = action;
        _objectInteract = objectInteract;
        
        _orderButtonQueueUI = ActionMenu.Instance.CreateOrderButtonQueueUI();
        _orderButtonQueueUI.Init(this,_objectInteract.GetIcon());
    }

    public IEnumerator Execute()
    {
        foreach (var action in _action.GetActions())
        {
            //Debug.Log(action.name);
            _currentSimAction = action;
            
            if (!_objectInteract)
                _objectInteract = HomeBrainManager.Instance.FindObjectToDoAction(_person,_currentSimAction);
            
            //If we didn't find an object to execute our action, we cancel the whole order
            if (!_objectInteract)
            {
                Debug.LogWarning("Error, Object not found");
                _isCanceled = true;
                break;
            }
            
            
            
            //StartSimple Action
            var positionTarget = _objectInteract.TransformPointToStand.position;
            var timer = 0f;
            _orderButtonQueueUI.UpdateTimer(timer / _currentSimAction.GetTimeDuration());   //Reset to 0
            
            //ChangeIcon
            _orderButtonQueueUI.ChangeOrderIcon(_objectInteract.GetIcon());
            
            if (!_isCanceled)
                _person.Move(positionTarget);
            
            
        
            while (Vector3.Distance(_person.transform.position, positionTarget) > 3.6f && !_isCanceled)
            {
                //Wait until we reach our target position
                yield return null;
            }
            
            if(_isCanceled)
                _person.Stop();

            //We will increase all the needs that involve this action.  
            while (timer<= _currentSimAction.GetTimeDuration() && !_isCanceled)
            {
                timer += Time.deltaTime;
                foreach (var need in _currentSimAction.GetNeedValues())
                {
                    //We need to know how many points we must add per second
                    var incrementPerSecond = need.MaxValueToAdd/_currentSimAction.GetTimeDuration();
                    _person.AddNeedValue(need.Type, incrementPerSecond * Time.deltaTime);
                }

                _orderButtonQueueUI.UpdateTimer(timer / _currentSimAction.GetTimeDuration());
                yield return null;
            }

            _objectInteract = null; //The order drop the objecdt. This way we will find the next order&object in the house map.
        }
        _person.FinishOrder();
    }

    public void Finish()
    {
        _orderButtonQueueUI.Finish();   //End UI button
    }

    public void Cancel()
    {
        //_person.Stop();         //Stop the play in case it is in movement
        _isCanceled = true;     //set the flag true
    }

    
}
