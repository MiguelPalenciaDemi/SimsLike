using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable, CreateAssetMenu(fileName = "NewSimAction", menuName = "SimAction")]
public class SimAction : ScriptableObject
{
    [System.Serializable]
    public struct NeedValues
    {
        [SerializeField] private NeedType type;
        [SerializeField] private float maxValueToAdd;

        public NeedType Type => type;
        public float MaxValueToAdd => maxValueToAdd;
    }

    [SerializeField] private List<NeedValues> needValues;
    [SerializeField] private float timeDuration;
    
    public virtual float GetTimeDuration() => timeDuration;
    public virtual List<NeedValues> GetNeedValues() => needValues;
    
    
}


