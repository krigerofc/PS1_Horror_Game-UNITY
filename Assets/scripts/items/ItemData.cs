using UnityEngine;

public enum EffectType {
    HealHealth,
    DamageHealth,

    IncreaseSleep,
    ReduceSleep,

    IncreasePanic,
    ReducePanic,

    IncreaseHallucination,
    ReduceHallucination
}


[CreateAssetMenu(fileName = "New Item", menuName = "Inventario/Item")]
public class ItemData : ScriptableObject {
    // Info
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    // 3D
    public GameObject itemModel; // objeto 3D que vai ser exibido
    public GameObject itemPrefab; // objeto 3D que vai pro chão

    // Sound
    public AudioClip[] SoundUseItem;
    public AudioClip SoundPickupItem;
    public AudioClip SoundDropItem;


    // effect

    public ItemEffect[] effects;

    public virtual void Use(){
        foreach (ItemEffect effect in effects) {
            effect.ApplyEffect();
        }
    }
}

[System.Serializable]
public class ItemEffect {
    public EffectType effectType;
    public float value;

    public void ApplyEffect() {
        PlayerStatus playerStatus = PlayerStatus.instance;
        if (playerStatus == null) return;

        switch (effectType) {
            case EffectType.HealHealth:
                playerStatus.Heal(value);
                break;
            case EffectType.DamageHealth:
                playerStatus.TakeDamage(value);
                break;
            case EffectType.IncreaseSleep:
                playerStatus.IncreaseSleep(value);
                break;
            case EffectType.ReduceSleep:
                playerStatus.DecreaseSleep(value);
                break;
            case EffectType.IncreasePanic:
                playerStatus.IncreasePanic(value);
                break;
            case EffectType.ReducePanic:
                playerStatus.DecreasePanic(value);
                break;
            case EffectType.IncreaseHallucination:
                playerStatus.IncreaseHallucination(value);
                break;
            case EffectType.ReduceHallucination:
                playerStatus.DecreaseHallucination(value);
                break;
            default:
                break;
        }   
    }
}

public enum ConsumableType {
    Food,
    Drink,
}

[CreateAssetMenu(fileName = "Consumable Item", menuName = "Inventario/Consumable")]
public class ConsumableItemData : ItemData {
    public ConsumableType Type;

    public override void Use(){
        base.Use();
        
        switch(Type){
            case ConsumableType.Food:
                Eat();
                break;
            case ConsumableType.Drink:
                Drink();
                break;
        }
    }

    private void Eat() {
        Debug.Log($"[CONSUMABLE] Eating {itemName}...");
        
        if (ItemSoundManager.instance != null && SoundUseItem != null && SoundUseItem.Length > 0) {
            ItemSoundManager.instance.PlaySequence(SoundUseItem, 1.0f);
        }
    }

    private void Drink() {
        Debug.Log($"[CONSUMABLE] Drinking {itemName}...");
        
        if (ItemSoundManager.instance != null && SoundUseItem != null && SoundUseItem.Length > 0) {
            ItemSoundManager.instance.PlaySequence(SoundUseItem, 1.0f);
        }
    }
}