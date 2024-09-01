using UnityEngine;

public abstract class BaseManager : MonoBehaviour
{
	protected GameManager _gameManager;

	public virtual void Initialize(GameManager gameManager)
	{
		_gameManager = gameManager;

		gameObject.SetActive(true);
	}
}