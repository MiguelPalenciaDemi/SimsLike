using System;
using UnityEngine;
using UnityEngine.AI;

public class Person : MonoBehaviour
{
    private NeedManager _needs;
    private NavMeshAgent _agent;
    private CommandQueue _commandQueue;
    private SimAutonomyIA _simAutonomy;
    
    
    //Init the Person
    private void Awake()
    {
        if(!TryGetComponent(out _needs))
            _needs = gameObject.AddComponent<NeedManager>();
        
        if(!TryGetComponent(out _agent))
            _agent = gameObject.AddComponent<NavMeshAgent>();
        if(!TryGetComponent(out _commandQueue))
            _commandQueue = gameObject.AddComponent<CommandQueue>();
        if(!TryGetComponent(out _simAutonomy))
            _simAutonomy = gameObject.AddComponent<SimAutonomyIA>();

        _commandQueue.SetOwner(this);
        _simAutonomy.SetOwner(this);
    }

    //Move Order
    public void Move(Vector3 pointTarget)
    {
        _agent.isStopped = false;
        _agent.SetDestination(pointTarget);
    }

    //Stop Order
    public void Stop()
    {
        _agent.isStopped = true;
    }

    //A need must be changed
    public void AddNeedValue(NeedType type, float needValue)
    {
        _needs.AddValueNeed(type, needValue);
    }

    //The sim has a new order. It interrupts IA
    public void AddOrder(ICommand command)
    {
        _commandQueue.AddOrder(command);
        _simAutonomy.Interrupt();
    }
    
    //The sim has a new order via IA
    public void AddOrderByIA(ICommand command)
    {
        _commandQueue.AddOrder(command);
    }

    //The sim has finished an action
    public void FinishOrder()
    {
        _commandQueue.FinishOrder();
    }

    //Return a weight from a need.
    public float GetNeedWeight(NeedType needType)
    {
        return _needs.GetWeightNeed(needType);
    }

    //Trigger the IA if Queue is empty
    public void QueueEmpty()
    {
        if(!_simAutonomy.IsActive)
            StartCoroutine(_simAutonomy.WaitToStart());
        else
            _simAutonomy.CreateNextOrder();
    }
    
    //Awake the sim to start the IA timer. It's used to start the sim
    public void AwakeSimAutonomy()
    {
        //This Checks the next order, in case our sim don't have any order it will start the SimAutonomyIA
        _commandQueue.AwakeSim();
        //The needs start to decrease
        _needs.Init();
        
    }
}
