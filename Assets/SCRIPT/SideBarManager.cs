using UnityEngine;

public class SideBarManager : MonoBehaviour
{
    public GameObject _upgradeTab;
    public GameObject _commentTab;

    public void Update()
    {
        _upgradeTab.SetActive(false);
    }
}
