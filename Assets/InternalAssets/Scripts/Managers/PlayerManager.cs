using UniRx;
using UnityEngine;

public class PlayerManager : BaseManager
{
	[SerializeField] private GameObject PlayerObject;
	private PlayerMovement _playerMovement;
	private PlayerBuilding _playerBuilding;
	private PlayerController _playerController;
	private Animator _playerAnimator;


	public Vector3Int CellPosition => _playerMovement.CellPosition;
	private ReactiveProperty<PlayerState> _playerState = new();
	public IReadOnlyReactiveProperty<PlayerState> CurrentState => _playerState;

	public void Initialize(GameManager gameManager, Vector3Int startPosition)
	{
		base.Initialize(gameManager);

		_playerMovement = PlayerObject.GetComponent<PlayerMovement>();
		_playerBuilding = PlayerObject.GetComponent<PlayerBuilding>();
		_playerController = PlayerObject.GetComponent<PlayerController>();
		_playerAnimator = PlayerObject.GetComponent<Animator>();

		_playerMovement.Initialize(this, startPosition);
		_playerBuilding.Initialize(this);
		_playerController.Initialize(this);

		_playerState.Subscribe(state =>
		{
			switch (state)
			{
				case PlayerState.IDLE:
				default:
					_playerAnimator.Play("Player_Idle");
					_playerController.StopExtraction();
					break;
				case PlayerState.EXTRACTION: _playerAnimator.Play("Player_Walk"); break;
			}
		});
	}

	public void ChangeState(PlayerState state)
	{
		_playerState.Value = state;
	}

	public enum PlayerState
	{
		IDLE,
		EXTRACTION,
	}
}