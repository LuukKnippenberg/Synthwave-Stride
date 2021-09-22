using UnityEngine;
using UnityEngine.Events;
using Unity.FPS.Game;
using System;

public class PlayerShellCollection : MonoBehaviour
{
    public UnityEvent OnCollectingShell;
    
    public void AddShell()
    {
        OnCollectingShell.Invoke();
    }
}
