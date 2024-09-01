using System;
using UniRx;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerManager _playerManager;

    private IDisposable _extractionProcess;

    public void Initialize(PlayerManager playerManager)
    {
        _playerManager = playerManager;

        // поток update
        // Observable.EveryUpdate()
        //     // фильтруем на нажатие клавиши действия
        //     .Where(_ => Input.GetKeyDown(KeyCode.Keypad5))
        //     .Subscribe(_ =>
        //     {
        //         if (_playerManager.CurrentState.Value != PlayerManager.PlayerState.EXTRACTION)
        //         {

        //             ResourceTile rt = GridManager.Instance.GetTileInfo(_playerManager.CellPosition.x, _playerManager.CellPosition.y).ResourceTile;
        //             if (rt != null)
        //             {
        //                 startExtraction(_ => ResourceManager.Instance.AddResource(rt.RTileData.TileType, 5));
        //             }
        //         }
        //     }).AddTo(this);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Keypad5))
        {
            ResourceTile rt = GridManager.Instance.GetTileInfo(_playerManager.CellPosition.x, _playerManager.CellPosition.y).ResourceTile;
            if (rt != null)
                ResourceManager.Instance.AddResource(rt.RTileData.TileType, 5);

            GameManager.Instance.EndTurn();
        }
    }

    private void startExtraction(Action<long> intervalSubsriber)
    {
        var cachCleanTimer = Observable.Interval(TimeSpan.FromSeconds(1));

        if (_extractionProcess != null)
            StopExtraction();

        _extractionProcess = cachCleanTimer.Subscribe(intervalSubsriber);
        _playerManager.ChangeState(PlayerManager.PlayerState.EXTRACTION);
    }

    public void StopExtraction()
    {
        if (_extractionProcess != null) _extractionProcess.Dispose();
        _playerManager.ChangeState(PlayerManager.PlayerState.IDLE);
    }
}
