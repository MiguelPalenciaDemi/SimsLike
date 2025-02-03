using System;
using UnityEngine;

public class SimManager : MonoBehaviour
{
    private static SimManager _instance;
    public static SimManager Instance => _instance;
    
    [SerializeField] Person currentPerson;              //Sim we are using at the moment

    public Person CurrentPerson => currentPerson;

    private void Awake()
    {
        if(!_instance)
            _instance = this;
        else 
            Destroy(this);
    }

    //Add order to the current player
    public void AddOrderToPerson(ICommand order)
    {
        currentPerson.AddOrder(order);
    }

    public void Init()
    {
        currentPerson.AwakeSimAutonomy();
    }
    
    
}
