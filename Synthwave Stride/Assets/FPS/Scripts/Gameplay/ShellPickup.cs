using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;

public class ShellPickup : Pickup
{
    protected override void Start()
    {
        base.Start();

        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t != transform)
                t.gameObject.layer = 0;
        }
    }

    protected override void OnPicked(PlayerCharacterController player)
    {
        PlayerShellCollection playershell = player.GetComponent<PlayerShellCollection>();
        if (playershell)
        {
            playershell.AddShell();
            PlayPickupFeedback();
            Destroy(gameObject);
        }
    }
}
