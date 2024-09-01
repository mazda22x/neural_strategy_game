using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilding : MonoBehaviour
{
    private PlayerManager _playerManager;

    public void Initialize(PlayerManager playerManager)
    {
        _playerManager = playerManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            BuildingManager.Instance.StartBuilding(_playerManager.CellPosition);
        }
        else if (Input.GetKeyUp(KeyCode.Keypad0))
        {
            BuildingManager.Instance.StopBuilding();
        }
    }
}
