using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class UtilsManager : MonoBehaviour
{
    private static UtilsManager _instance;
    public static UtilsManager Instance => _instance;

    [SerializeField] private List<CustomDictionaryItem<Sprite>> icons;
    [SerializeField] private List<CustomDictionaryItem<SimAction>> actions;     //Action that we can need sometimes that don't belong to any object
    
    [Header("RRSS")]
    [SerializeField] private string rssUrl = "https://bsky.app/profile/miguelpalencia.bsky.social";
    
    private void Awake()
    {
        if (_instance)
            Destroy(this);
        else
            _instance = this;
    }

    public Sprite GetIcon(string iconName)
    {
        return icons.Find(x => x.ID == iconName).Item;
    }

    public SimAction GetAction(string actionName)
    {
        return actions.Find(x => x.ID == actionName).Item;
    }

    public void OpenRRSS()
    {
        Application.OpenURL(rssUrl);
    }
    
}
[Serializable]
public struct CustomDictionaryItem<T>
{
    [SerializeField] private string id;
    [SerializeField] private T item;

    public string ID => id;
    public T Item => item;
}
