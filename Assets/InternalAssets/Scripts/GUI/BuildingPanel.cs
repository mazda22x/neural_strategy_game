using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPanel : MonoBehaviour
{
    [SerializeField] private GameObject _buildingItemPfb;

    private List<GameObject> _items = new();

    private const int ITEM_GAP = 20, ITEM_WIDTH = 160;

    public void ShowPanel(List<BuildingItemConfig> items)
    {
        int startX = ITEM_WIDTH / 2 + ITEM_GAP;
        int counter = 0;
        items.ForEach(item =>
        {
            var itemObj = Instantiate(_buildingItemPfb, this.transform);
            itemObj.GetComponent<RectTransform>().anchoredPosition = new Vector3(startX + counter * (ITEM_WIDTH + ITEM_GAP), 0);
            itemObj.GetComponent<BuildingItem>().Initialize(
                item.BuildingSO.BuildingType.ToString(),
                item.BuildingSO.BuildingSprite,
                item.IsAllowed,
                () =>
                {
                    if (item.onClick != null)
                        item.onClick(item.BuildingSO);
                });

            _items.Add(itemObj);
            counter++;
        });

        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        Clear();

        gameObject.SetActive(false);
    }

    public void Clear()
    {
        _items.ForEach(item => Destroy(item));
        _items.Clear();

        foreach (var item in gameObject.GetComponentsInChildren<BuildingItem>())
        {
            Destroy(item.gameObject);
        }
    }

    public struct BuildingItemConfig
    {
        public BuildingSO BuildingSO;
        public bool IsAllowed;
        public Action<BuildingSO> onClick;
    }
}
