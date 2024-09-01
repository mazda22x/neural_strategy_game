using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildingItem : MonoBehaviour
{
    [SerializeField] private Image _imageIMG;
    [SerializeField] private TextMeshProUGUI _nameTMP;
    [SerializeField] private Button _button;

    private readonly Color32

        ALLOWED_COLOR = new(120, 255, 165, 125), // #78FFA57D 
        DISALLOWED_COLOR = new(255, 120, 165, 125); // #FF78A57D 



    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void Initialize(string name, Sprite sprite, bool canPlace, UnityAction onBtnClick)
    {
        _imageIMG.sprite = sprite;
        _nameTMP.text = name;

        GetComponent<Image>().color = canPlace ? ALLOWED_COLOR : DISALLOWED_COLOR;
        GetComponent<Button>().enabled = canPlace;

        _button.onClick.AddListener(onBtnClick);
    }
}
