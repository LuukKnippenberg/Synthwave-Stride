using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemboxRespawner : MonoBehaviour
{
    [Tooltip("The GameObject to set active.")]
    public GameObject targetObject;
    [Tooltip("The time it takes before it respawns.")]
    public float respawnTime = 3f;

    public void respawnItem()
    {
        StartCoroutine(IE_RespawnItem());
    }
    IEnumerator IE_RespawnItem()
    {
        targetObject.SetActive(false);
        yield return new WaitForSeconds(respawnTime);
        targetObject.SetActive(true);
        
    }

}
