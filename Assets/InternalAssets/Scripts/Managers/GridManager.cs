using UnityEngine;

public class GridManager : BaseManager
{
    public static GridManager Instance { get; private set; }

    // default size x: 0.8659766
    [SerializeField] BoundsInt _gridArea = new BoundsInt(new Vector3Int(0, 0), new Vector3Int(50, 50, 1));
    public BoundsInt GridArea => _gridArea;

    #region Tilemap Fields
    [Header("Tilemaps")]
    [SerializeField] private ResourceTilemap _resourceTilemap;
    public ResourceTilemap ResourceTilemap => _resourceTilemap;

    [SerializeField] private GroundTilemap _groundTilemap;
    public GroundTilemap GroundTilemap => _groundTilemap;

    [SerializeField] private BuildingTilemap _buildingTilemap;
    public BuildingTilemap BuildingTilemap => _buildingTilemap;
    #endregion Tilemap Fields


    #region UnityMethods
    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance);

        Instance = this;
    }
    #endregion UnityMethods

    #region Public Methods

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);


        _groundTilemap.Initialize(this);
        _resourceTilemap.Initialize(this);

        _groundTilemap.Clear();
        _resourceTilemap.Clear();
    }

    public void GenerateMap()
    {
        _groundTilemap.GenerateMap();
        _resourceTilemap.GenerateMap();
    }

    public TileInfo GetTileInfo(int x, int y)
    {
        return new TileInfo(
            GroundTilemap.GetGridObject(x, y),
            ResourceTilemap.GetGridObject(x, y),
            BuildingTilemap.GetGridObject(x, y)
        );
    }

    public Vector3Int GetRandomCell()
    {
        return new Vector3Int(Random.Range(0, GridArea.size.x), Random.Range(0, GridArea.size.y));
    }

    public struct TileInfo
    {
        public GroundTile GroundTile { get; private set; }
        public ResourceTile ResourceTile { get; private set; }
        public BuildingTile BuildingTile { get; private set; }
        public TileInfo(GroundTile groundTile, ResourceTile resourceTile, BuildingTile buildingTile)
        {
            GroundTile = groundTile;
            ResourceTile = resourceTile;
            BuildingTile = buildingTile;

        }
    }
    #endregion Public Methods
}
