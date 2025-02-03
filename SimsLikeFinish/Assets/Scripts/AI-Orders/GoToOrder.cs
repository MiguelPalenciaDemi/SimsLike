using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GoToOrder : ICommand
{
    private Person _person;
    private SimAction _action;
    private Vector3 _destination;
    private bool _isCanceled;
    private OrderButtonQueueUI _orderButtonQueueUI;
    
    public GoToOrder(Person person, Vector3 destination)
    {
        _person = person;
        _destination = destination;
        _orderButtonQueueUI = ActionMenu.Instance.CreateOrderButtonQueueUI();
        _orderButtonQueueUI.Init(this,UtilsManager.Instance.GetIcon("GoToIcon"));
        
    }
    public IEnumerator Execute()
    {
        var positionTarget = _person.transform.position;
        //We search a valid point in the navmesh to go.
        if (NavMesh.SamplePosition(_destination, out var hit, 4, NavMesh.AllAreas))
        {
            positionTarget = hit.position;
            _person.Move(positionTarget);
        }
        
        if(_isCanceled)
            _person.Stop();
        
        //Check if we arrive at the point
        while (Vector3.Distance(_person.transform.position, positionTarget) > 3.6f && !_isCanceled)
        {
            //Wait until we reach our target position
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
