using UnityEngine;

public abstract class ItemInfoBase : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private string id;
    [SerializeField] private string statId;
    [SerializeField] private Sprite icon;
    [SerializeField] private float value;
    [SerializeField] private UpgradeValueType valueType;
    [SerializeField] private float dropChanceWeight;

    public string ItemName => itemName;
    public Sprite Icon => icon;
    public float Value => value;
    public UpgradeValueType ValueType => valueType;
    public string Id => id;
    public string StatId => statId;

    public float DropChanceWeight => dropChanceWeight;
}
