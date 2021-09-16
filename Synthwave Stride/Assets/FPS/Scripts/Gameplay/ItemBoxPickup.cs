using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;

public class ItemBoxPickup : Pickup
{
    [Tooltip("The prefab for the weapon that will be added to the player on pickup")]
    [SerializeField] private List<WeaponController> weaponItem;

    private WeaponController activeWeapon;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // Set all children layers to default (to prefent seeing weapons through meshes)
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t != transform)
                t.gameObject.layer = 0;
        }
    }

    // Update is called once per frame
    protected override void OnPicked(PlayerCharacterController byPlayer)
    {
        PlayerWeaponsManager playerWeaponsManager = byPlayer.GetComponent<PlayerWeaponsManager>();
        if (playerWeaponsManager)
        {
            int weaponValue = RandomWeaponValue();
            WeaponController obtainedWeapon = weaponItem[weaponValue];

            if (playerWeaponsManager.AddWeapon(obtainedWeapon))
            {
                activeWeapon = playerWeaponsManager.GetActiveWeapon();
                // Will automatically switch to the new weapon and get rid of the older one.
                if (activeWeapon != null)
                {
                    if (playerWeaponsManager.RemoveWeapon(activeWeapon))
                    {

                    }

                }
                playerWeaponsManager.SwitchWeapon(true);

                PlayPickupFeedback();
                Destroy(gameObject);
            }
        }
    }

    public int RandomWeaponValue()
    {
        int randomWeaponValue = Random.Range(0, weaponItem.Count);
        return randomWeaponValue;
    }
}
