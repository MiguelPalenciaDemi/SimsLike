using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedManager : MonoBehaviour
{
    
    //This struct store UI&Data Need
    [Serializable]
    private struct NeedStruct
    {
        [SerializeField] private NeedClass needClass;
        [SerializeField] private NeedUI needUI;

        public void Init(NeedClass newNeed, NeedUI newNeedUI)
        {
            needClass = newNeed;
            needUI = newNeedUI;
        }

        public void Update()
        {
            needClass.Update();
            needUI.SetNeedValue(needClass.Value);
        }
        
        public NeedType GetNeedType() => needClass.GetNeedType();
        public void AddValue(float value) => needClass.AddValue(value);

        public float GetWeight()
        {
            return needClass.GetWeight();
        }
    }
    
    [SerializeField] private List<NeedData> needsData;      //All the needs we want to have our sim
    [SerializeField] private GameObject needUIPrefab;       //UI Prefab to create NeedUI
    [SerializeField] private Transform needGrid;            //Where to store our gameobjects
    
    
    [SerializeField] private List<NeedStruct> needs;
    private bool _isActive;                                 //The sim is active
    private void Start()
    {
        needs = new List<NeedStruct>();
        
        //Create UI
        foreach (var needData in needsData)
        {
            var need = new NeedClass(needData);
            
            var needUI = Instantiate(needUIPrefab, needGrid).GetComponent<NeedUI>();
            needUI.Init(needData.Icon,need.Value, needData.name);
            
            var needStruct = new NeedStruct();
            needStruct.Init(need,needUI);
            
            needs.Add(needStruct);
        }
    }

    private void Update()
    {
        if (!_isActive) return;
        
        foreach (var need in needs)
        {
            need.Update();
        }
    }

    public void AddValueNeed(NeedType type,float value)
    {
       var index = needs.FindIndex(x=> x.GetNeedType() == type);    //Find the need
       if(index != -1)
           needs[index].AddValue(value);
       else 
           Debug.Log("Error");
    }

    public float GetWeightNeed(NeedType needType)
    {
        var index = needs.FindIndex(x=> x.GetNeedType() == needType);   //Find the need and get the weight
        return index != -1 ? needs[index].GetWeight() : 0;
    }

    public void Init()
    {
        _isActive = true;
    }
}
