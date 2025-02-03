using System;
using UnityEngine;
using UnityEngine.UI;

public class OrderButtonQueueUI : MonoBehaviour
{
    [SerializeField] private Image orderButtonImage;
    [SerializeField] private GameObject cancelPanel;
    [SerializeField] private Image imageTimer;
    private ICommand _commandOwner;

    public void Init(ICommand commandOwner, Sprite orderIcon)
    {
        _commandOwner = commandOwner;
        orderButtonImage.sprite = orderIcon;
        transform.parent.gameObject.SetActive(true);
    }

    //Function that we will call when we press. It will cancel the order
    public void OnClick()
    {
        _commandOwner.Cancel();
        cancelPanel.SetActive(true);
        AudioManager.Instance.PlaySound("CancelOrder");
    }
    
    //Destroy the OrderUI
    public void Finish()
    {
        //If the queue is empty when we destroy this button, we will hide the menu
        if(transform.parent.childCount<=1)
            transform.parent.gameObject.SetActive(false);
        
        Destroy(this.gameObject);
    }
    
    //Update the UITimer
    public void UpdateTimer(float timer)
    {
        imageTimer.fillAmount = timer;
    }

    //Change the icon. It is used int ComplexActions to know what object we are using.
    public void ChangeOrderIcon(Sprite orderIcon)
    {
        orderButtonImage.sprite = orderIcon;
    }

    
}
