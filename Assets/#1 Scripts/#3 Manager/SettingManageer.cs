using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingManageer : MonoBehaviour
{
    public void ExitSetting()
    {
        GameManager.inst.UnLoadScene(Scenes.Setting);
    }
}
