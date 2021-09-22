using UnityEngine;
using UnityEngine.UI;



public class ShellCollectingBar : MonoBehaviour
{
    [Tooltip("Image component displaying current amount of shells collected")]
    public Image ShellFillImage;
    [Tooltip("All the Shells in the game")]
    ShellPickup[] shells;
    public float shellAmount;
    public float shellsCollected;

    // Start is called before the first frame update
    void Start()
    {
        shells = FindObjectsOfType<ShellPickup>();
        shellAmount = shells.Length;
    }
    // Update is called once per frame
    public void UpdateView()
    {
        shellsCollected++;
        ShellFillImage.fillAmount = shellsCollected / shellAmount;
    }
}
