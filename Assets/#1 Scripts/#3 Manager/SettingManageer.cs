using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManageer : MonoBehaviour
{
    public void ExitSetting()
    {
        GameManager.Instance.ToggleSettingMenu();
    }
}
