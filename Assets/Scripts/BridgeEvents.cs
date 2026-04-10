using UnityEngine;
/// <summary>
/// Simple script to make calls from events in animations (clips)
/// </summary>
public class BridgeEvents : MonoBehaviour
{
    [SerializeField] private GameObject pawsObjec;

    //Function called in the "Jump" animation
    public void ActivateScreenPaws()
    {
        pawsObjec.SetActive(true);
    }
}
