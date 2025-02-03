using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ObjectInteract : MonoBehaviour
{
   [SerializeField] private Transform transformPointToStand;   //Point where the action will be performed
   [SerializeField] private List<SimAction> actions;           //Actions tha the player can use directly,
   [SerializeField] private Sprite icon;
   [SerializeField] private List<SimAction> hiddenActions;     //Actions that the player can NOT use directly. 
   public List<SimAction> Actions => actions;

   public Transform TransformPointToStand => transformPointToStand;

   
   public Sprite GetIcon() { return icon; }

   public List<SimAction> GetAllActions()
   {
      //We mix both lists
      var wholeActions = actions.ToList();
      wholeActions.AddRange(hiddenActions);
      
      return wholeActions;
   }
   
}
