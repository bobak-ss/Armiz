using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject messagePnl;
    [SerializeField] private Text messageTxt;
    [SerializeField] private Button messageBtn;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowMessagePnl(string message, UnityAction buttonCallback)
    {
        messageTxt.text = message;
        messageBtn.onClick.AddListener(buttonCallback);
        messagePnl.SetActive(true);
    }

    public void HideMessagePnl()
    {
        messagePnl.SetActive(false);
    }
}
