using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using YeLazzers;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Grid gridSystem;
    private PlayerManager _playerManager;

    private ReactiveProperty<Vector3Int> _position = new ReactiveProperty<Vector3Int>();

    public Vector3Int CellPosition => _position.Value;

    public void Initialize(PlayerManager playerManager, Vector3Int startPosition)
    {
        _playerManager = playerManager;

        _position.AsObservable().Subscribe(pos =>
        {
            var newPos = gridSystem.CellToWorld(pos);
            transform.position = newPos;
            Camera.main.transform.position = new Vector3(newPos.x, newPos.y, Camera.main.transform.position.z);
        });

        // поток update
        Observable.EveryUpdate()
            // фильтруем на нажатие клавиши передвижения
            .Where(_ => Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad7) ||
                        Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Keypad9))
            .Select(_ => Convert.ToInt32(Input.inputString))
            .Subscribe(vector =>
            {
                _position.Value += Utils.GetOffsetCell(_position.Value, vector);
                _playerManager.ChangeState(PlayerManager.PlayerState.IDLE);
                BuildingManager.Instance.StopBuilding();
                
                GameManager.Instance.EndTurn();
            }).AddTo(this);

        _position.Value = startPosition;
    }
}
