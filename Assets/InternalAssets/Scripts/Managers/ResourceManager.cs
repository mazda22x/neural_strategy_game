using System;
using TMPro;
using UniRx;
using UnityEngine;

public class ResourceManager : BaseManager
{
    public static ResourceManager Instance { get; private set; }
    public ReactiveProperty<int> Wood = new(0);
    public ReactiveProperty<int> Stone = new(0);
    public ReactiveProperty<int> Copper = new(0);
    public ReactiveProperty<int> Food = new(0);

    [SerializeField] private TextMeshProUGUI WoodTMP;
    [SerializeField] private TextMeshProUGUI StoneTMP;
    [SerializeField] private TextMeshProUGUI CopperTMP;
    [SerializeField] private TextMeshProUGUI FoodTMP;

    #region UnityMethods
    private void Awake()
    {
        if (Instance != null)
            Destroy(this);

        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnTurnEnd += CalculateTurnResources;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnTurnEnd -= CalculateTurnResources;
    }

    private void Update()
    {
    }
    #endregion UnityMethods

    #region PublicMethods
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        Wood.Subscribe(value => WoodTMP.text = $"Wood: {value}");
        Stone.Subscribe(value => StoneTMP.text = $"Stone: {value}");
        Copper.Subscribe(value => CopperTMP.text = $"Copper: {value}");
        Food.Subscribe(value => FoodTMP.text = $"Food: {value}");
    }

    public void AddResource(TileTypeEnum type, int value)
    {
        switch (type)
        {
            case TileTypeEnum.WOOD: Wood.Value += value; break;
            case TileTypeEnum.STONE: Stone.Value += value; break;
            case TileTypeEnum.COPPER: Copper.Value += value; break;
                // case TileTypeEnum.: Stone.Value += value; break;
        }

    }

    public BuildingData.ResourceStruct GetResources()
    {
        return new BuildingData.ResourceStruct()
        {
            Wood = Wood.Value,
            Stone = Stone.Value,
            Copper = Copper.Value,
            Food = Food.Value,
        };
    }

    public void ChangeWallet(BuildingData.ResourceStruct resources)
    {
        Wood.Value += resources.Wood;
        Stone.Value += resources.Stone;
        Copper.Value += resources.Copper;
        Food.Value += resources.Food;
    }

    public bool CheckCost(BuildingData.ResourceStruct cost)
    {
        if (
            Wood.Value - Math.Abs(cost.Wood) < 0 ||
            Stone.Value - Math.Abs(cost.Stone) < 0 ||
            Copper.Value - Math.Abs(cost.Copper) < 0 ||
            Food.Value - Math.Abs(cost.Food) < 0
        )
            return false;
        return true;
    }

    #endregion PublicMethods

    #region  PrivateMethods
    private void CalculateTurnResources()
    {
        ChangeWallet(new BuildingData.ResourceStruct() { Food = -1 });
    }

    #endregion PrivateMethods
}
