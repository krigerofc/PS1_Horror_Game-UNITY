using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData item;

    private bool hasHitGround = false;

    public void Interact()
    {   
        if(item != null){
            PlayerInvetory.instance.AddItem(item);
            
            // Som ao coletar
            if (item.SoundPickupItem != null) ItemSoundManager.instance.Play(item.SoundPickupItem);

            Debug.Log($"<color=green>[COLETADO]</color> {item.itemName} adicionado.");
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        // Toca o som apenas no primeiro impacto forte
        if (!hasHitGround && collision.relativeVelocity.magnitude > 2f) {
            if (item != null && item.SoundDropItem != null) {
                // Toca o som um pouco mais baixo (volume 0.6f)
                ItemSoundManager.instance.PlayAtPosition(item.SoundDropItem, transform.position, 0.6f);
                hasHitGround = true; 
            }
        }
    }
}
