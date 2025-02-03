using System.Collections;
using UnityEngine;


public class ActionOrder : ICommand
{
    private Person _person;
    private SimAction _action;
    private ObjectInteract _objectInteract;
    private OrderButtonQueueUI _orderButtonQueueUI;
    private bool _isCanceled;
    
    
    public ActionOrder(Person person, SimAction action, ObjectInteract objectInteract)
    {
        _person = person;
        _action = action;
        _objectInteract = objectInteract;
        _orderButtonQueueUI = ActionMenu.Instance.CreateOrderButtonQueueUI();   //Create the UI button
        _orderButtonQueueUI.Init(this,_objectInteract.GetIcon());
    }
    public IEnumerator Execute()
    {
        //The sim goes to the place where actions must being done 
        var positionTarget = _objectInteract.TransformPointToStand.position;
        if (!_isCanceled)
        {
            _person.Move(positionTarget);
            Debug.Log("Orden ejecutada");
        }
        
        //It controls if we have arrived the point  
        while (Vector3.Distance(_person.transform.position, positionTarget) > 3.6f && !_isCanceled)
        {
            //Wait until we reach our target position
            yield return null;
        }

        if(_isCanceled)
            _person.Stop();
        
        var timer = 0f;
        
        //We will increase all the needs that involve this action. 
        while (timer<= _action.GetTimeDuration() && !_isCanceled)
        {
            timer += Time.deltaTime;
            foreach (var need in _action.GetNeedValues())
            {
                var incrementPerSecond = need.MaxValueToAdd/_action.GetTimeDuration();//We need to know how many points we must add per second
                _person.AddNeedValue(need.Type, incrementPerSecond * Time.deltaTime);
            }

            _orderButtonQueueUI.UpdateTimer(timer / _action.GetTimeDuration());
            yield return null;
        }
        
        _person.FinishOrder();
    }

    public void Finish()
    {
        _orderButtonQueueUI.Finish();       //End UI button
    }

    public void Cancel()
    {
        //_person.Stop();         //Stop the play in case it is in movement
        _isCanceled = true;     //set the flag true
    }
}
