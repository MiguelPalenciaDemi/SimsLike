using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "NewSimAction", menuName = "ComplexSimAction")]
public class ComplexSimAction : SimAction
{
    [SerializeField] private List<SimAction> actions;

    public List<SimAction> GetActions() => actions;

    public override float GetTimeDuration()
    {
        return actions.Sum(action => action.GetTimeDuration()); //We return the sum of all the action's duration.
    }

    public override List<NeedValues> GetNeedValues()
    {
        return actions.SelectMany(action => action.GetNeedValues()).ToList();   //We return a list with all the need values from all of our actions
    }
    
    
}