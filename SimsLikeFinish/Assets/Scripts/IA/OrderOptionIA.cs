using UnityEngine;

[System.Serializable]
//This class is need to store the action, objects and the values to be compared
public class OrderOptionIA
{
    private SimAction _action;
    private ObjectInteract _objectInteract;
    private float _value;
    private float _distance;

    public OrderOptionIA(SimAction action, ObjectInteract objectInteract)
    {
        _action = action;
        _objectInteract = objectInteract;
    }

    //Calculate the action's value based on the person needs.
    public void Calculate(Person person)
    {
        _distance = Vector3.Distance(person.transform.position, _objectInteract.transform.position);
        _value = 0;
        
        if(!_action) return;
        foreach (var need in _action.GetNeedValues())
        {
            var weight = person.GetNeedWeight(need.Type);
            _value+= need.MaxValueToAdd * weight;
        }
    }
    
    public float Value => _value;
    public float Distance => _distance;
    public ObjectInteract GetObject() => _objectInteract;
    public SimAction GetAction() => _action;
    public string GetName() => _action.name;
    public string GetObjectName() => _objectInteract.name;
}
