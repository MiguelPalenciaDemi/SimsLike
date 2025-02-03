using UnityEngine;

[System.Serializable]
public class NeedClass
{
    [SerializeField] private NeedData needData;
    [SerializeField,Range(0,100)] private float value;

    public float Value => value;
    public NeedClass(NeedData newData)
    {
        needData = newData;
        value = 100;
    }
    public void Update()
    {
        
        value -= needData.DecayRate * Time.deltaTime;
        value = Mathf.Clamp(value, 0, 100);     //We need to clamp it in range [0,100]
    }

    public NeedType GetNeedType()
    {
        return needData.Type;
    }

    public void AddValue(float addValue)
    {
        value += addValue;
        value = Mathf.Clamp(value, 0, 100);
    }

    public float GetWeight()
    {
        return needData.WeightCurve.Evaluate(value);
    }
}
