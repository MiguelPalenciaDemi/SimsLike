using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandQueue : MonoBehaviour
{
    [SerializeField] private Queue<ICommand> queue;
    private bool _running;
    private Person _owner;

    private void Awake()
    {
        queue = new Queue<ICommand>();
    }

    private void Start()
    {
        
    }

    /// <summary>
    /// Add a new command
    /// </summary>
    /// <param name="command"></param>
    private void Enqueue(ICommand command)
    {
        queue.Enqueue(command);
    }

    private ICommand Dequeue()
    {
        return queue.Dequeue();
    }

    public ICommand Peek()
    {
        return queue.Peek();
    }

    private void Execute()
    {
        if (queue.TryPeek(out var command))
        {
            _running = true;                                //We set that the person is doing an action
            StartCoroutine(command.Execute());       //Execute the order
        }
    }

    public void FinishOrder()
    {
        var order = Dequeue();
        
        //StopUI
        order.Finish();
        _running = false;
        NextOrder();    //Go to the next order
    }

    private void NextOrder()
    {
        if(!_running && queue.Count > 0)
            Execute();
        else if (!_running && queue.Count==0)
            _owner.QueueEmpty();                //It advises queue runs out of orders
    }

    public void AddOrder(ICommand command)
    {
        Enqueue(command);   //Add to the order's stack
        NextOrder();        //Check if it can be executed now
    }

    public void SetOwner(Person person)
    {
        _owner = person;        //Set who control this queue.
    }

    public void AwakeSim()
    {
        //Check if the person starts with a default action, in case negative it will start the SimAutonomyIA
        NextOrder();
    }
}
