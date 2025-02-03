using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimAutonomyIA : MonoBehaviour
{
    [SerializeField] float waitingTime = 3f;
    private Person _owner;
    private bool _isActive;

    public bool IsActive => _isActive;

    private bool _isInterupted;
    public ICommand GetNextOrder()
    {
        var choice = CalculateNextOrder(HomeBrainManager.Instance.GetObjectsAtHome());
        
        ICommand command;
        
        //We must cast the action to know how to treat it.
        if(choice.GetAction() is ComplexSimAction complexSimAction)
        {
            command = new ComplexActionOrder(_owner,complexSimAction,choice.GetObject());
        }
        else
        {
            command = new ActionOrder(_owner, choice.GetAction(), choice.GetObject());
        }
        return command;
    }

    //We look for a desirable action based on person's need
    private OrderOptionIA CalculateNextOrder(List<ObjectInteract> objectsAtHome)
    {
        
        //We pass through the list looking for the actions. We create our option knowing the action and the object
        var options = new List<OrderOptionIA>();
        foreach (var objectInteract in objectsAtHome)
        {
            foreach (var action in objectInteract.Actions)
            {
                var option = new OrderOptionIA(action, objectInteract);
                option.Calculate(_owner);
                options.Add(option);
            }
        }
        
        options = options.OrderByDescending(x => x.Value).ThenByDescending(x => x.Distance).ToList();
        
        // var result = "";
        // for (var index = 0; index < options.Count; index++)
        // {
        //     var option = options[index];
        //     result += "|| Option " + index +
        //               " * " + option.GetName() +
        //               " * " + option.GetObjectName() +
        //               " * Value " + option.Value +
        //               " * Distance " + option.Distance + "\n";
        // }
        // Debug.Log(result);
        
        var randomChoice = Random.Range(0, objectsAtHome.Count > 3 ? 3 : objectsAtHome.Count - 1);  //We select one of the 3-top choices
        return options[randomChoice];
    }

    public void SetOwner(Person person)
    {
        _owner = person;
    }

    //We create a new order via this function to keep the IA working.
    public void CreateNextOrder()
    {
        _owner.AddOrderByIA(GetNextOrder());
    }

    //This function set a timer to know when active the IA
    public IEnumerator WaitToStart()
    {
        _isInterupted = false;
        var timer = 0f;
        
        while (timer< waitingTime && !_isInterupted)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (timer >= waitingTime && !_isInterupted)
        {
            _isActive = true;
            CreateNextOrder();  //When we active it, we start asking it for orders
        }
        
    }
    
    //Stops IA
    public void Interrupt()
    {
        _isInterupted = true;
        _isActive = false;        
    }
}
