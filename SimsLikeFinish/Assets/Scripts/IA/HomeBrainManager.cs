using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomeBrainManager : MonoBehaviour
{
    private static HomeBrainManager _instance;
    public static HomeBrainManager Instance => _instance;

    [SerializeField] private List<ObjectInteract> objectsAtHome;
    public List<ObjectInteract> GetObjectsAtHome() => objectsAtHome;

    private void Awake()
    {
        if(!_instance)
            _instance = this;
        else
            Destroy(this);

        //Collect all the objectsInteractable in the house
        foreach (var objectInteract in FindObjectsByType<ObjectInteract>(FindObjectsSortMode.None))
        {
            objectsAtHome.Add(objectInteract);
        }

    }


    //Return the object that can the action we want to perform
    public ObjectInteract FindObjectToDoAction(Person person, SimAction action)
    {
        return CalculateBestObjectInteract(person, action);
    }
    
    private ObjectInteract CalculateBestObjectInteract(Person person,SimAction actionToFind)
    {
        
        //We pass through the list looking for the actions.
        //We create our option knowing the action and the object, but we just do it if the action matches with our actionToFind
        var options = new List<OrderOptionIA>();
        foreach (var objectInteract in objectsAtHome)
        {
            foreach (var action in objectInteract.GetAllActions())
            {
                if (actionToFind == action)
                {
                    var option = new OrderOptionIA(action, objectInteract);
                    option.Calculate(person);
                    options.Add(option);
                }
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
        
        return options.Count>0?options[0].GetObject():null;
    }
}
