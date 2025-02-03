using UnityEngine;

[System.Serializable]
public enum NeedType
{
    Energy,Fun,Bladder,Hygiene,Hunger,Social
}

[System.Serializable,CreateAssetMenu(fileName = "NewNeedData", menuName = "Needs/Data")]
public class NeedData : ScriptableObject
{
    [SerializeField] private NeedType needType;             //Kind of Need
    [SerializeField] private float decayRate;               // Decrease Speed
    [SerializeField] private AnimationCurve weightCurve;    //Weight based on need value
    [SerializeField] private Sprite icon;                   //UI Icon

    public float DecayRate => decayRate;
    public Sprite Icon => icon;
    public AnimationCurve WeightCurve => weightCurve;
    public NeedType Type => needType;
}
