using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInvetory : MonoBehaviour
{
    // consts
    private const int MAX_INVENTORY_SLOTS = 3;
    private const float DROP_DISTANCE = 1.5f;
    private const float DROP_HEIGHT = 0.5f;
    private const float DROP_FORCE = 10f;
    private const int NO_ITEM_EQUIPPED = -1;


    //Singleton
    public static PlayerInvetory instance;

    // Variáveis Públicas
    public List<ItemData> inventory = new List<ItemData>();
    public int equippedIndex = NO_ITEM_EQUIPPED;

    //Variáveis Privadas
    private KeyControl[] inventorySlotKeys;

    private void Awake() {
        InitializeSingleton();
    }

    private void Start() {
        SetupInputKeys();
    }

    private void Update() {
        HandleInventoryInput();
        HandleDropInput();
        HandleUseInput();
    }



    // Inicializações
    private void InitializeSingleton() {
        if (instance == null) {
            instance = this;
            Debug.Log("<color=white>[SISTEMA]</color> Inventário inicializado com sucesso.");
        } else {
            Destroy(gameObject);
        }
    }

    private void SetupInputKeys() {
        if (Keyboard.current != null) {
            inventorySlotKeys = new KeyControl[] {
                Keyboard.current.digit1Key,
                Keyboard.current.digit2Key,
                Keyboard.current.digit3Key
            };
        }
    }



    // ITEMS
    private void HandleInventoryInput() {
        if (inventorySlotKeys == null) return;

        for (int i = 0; i < inventorySlotKeys.Length; i++) {
            if (inventorySlotKeys[i].wasPressedThisFrame) {
                EquipItem(i);
                break;
            }
        }
    }

    private void HandleDropInput() {
        if (Keyboard.current.gKey.wasPressedThisFrame && HasItemEquipped()) {
            DropItem(equippedIndex);
        }
    }

    private void HandleUseInput(){
        if (Mouse.current.leftButton.wasPressedThisFrame && HasItemEquipped()) {
            UseItem(inventory[equippedIndex]);
        }
    }



    // uso público
    public void AddItem(ItemData item) {
        if (IsInventoryFull()) {
            Debug.Log("<color=red>[ERRO]</color> Inventário cheio! Não foi possível coletar o item.");
            return;
        }

        inventory.Add(item);
        Debug.Log($"<color=green>[COLETADO]</color> {item.itemName} adicionado. Slots: {inventory.Count}/{MAX_INVENTORY_SLOTS}");
    }

    public void RemoveItem(int index) {
        if (!IsValidIndex(index)) return;

        ItemData item = inventory[index];
        inventory.RemoveAt(index);
        equippedIndex = NO_ITEM_EQUIPPED;
        
        Debug.Log($"<color=red>[REMOVIDO]</color> {item.itemName} foi destruído/consumido.");
    }

    public void UseItem(ItemData item) {
        if (!inventory.Contains(item)) {
            Debug.Log("<color=red>[ERRO]</color> Item não encontrado no inventário!");
            return;
        }

        item.Use();
        inventory.Remove(item);
        equippedIndex = NO_ITEM_EQUIPPED; // Reseta o índice pois o item sumiu
        Debug.Log($"<color=green>[USADO]</color> {item.itemName} utilizado com sucesso.");
    }

    public void EquipItem(int index) {
        if (!IsValidIndex(index)) {
            Debug.Log($"<color=yellow>[AVISO]</color> Slot {index + 1} está vazio.");
            equippedIndex = NO_ITEM_EQUIPPED;
            return;
        }

        equippedIndex = index;
        ItemData item = inventory[equippedIndex];
        Debug.Log($"<color=cyan>[EQUIPADO]</color> Slot {index + 1}: {item.itemName}");
    }

    public void DropItem(int index) {
        if (!IsValidIndex(index)) return;

        ItemData itemToDrop = inventory[index];

        if (!HasValidPrefab(itemToDrop)) {
            Debug.LogError($"<color=red>[ERRO]</color> O item {itemToDrop.itemName} não tem um Prefab configurado!");
            return;
        }

        SpawnDroppedItem(itemToDrop);
        inventory.RemoveAt(index);
        equippedIndex = NO_ITEM_EQUIPPED;
    }




    // auxiliares
    private bool IsInventoryFull() {
        return inventory.Count >= MAX_INVENTORY_SLOTS;
    }

    private bool IsValidIndex(int index) {
        return index >= 0 && index < inventory.Count;
    }

    private bool HasItemEquipped() {
        return equippedIndex != NO_ITEM_EQUIPPED;
    }

    private bool HasValidPrefab(ItemData item) {
        return item.itemPrefab != null;
    }

    private void SpawnDroppedItem(ItemData item) {
        Vector3 spawnPosition = CalculateDropPosition();
        GameObject droppedObject = Instantiate(item.itemPrefab, spawnPosition, Quaternion.identity);

        ApplyDropForce(droppedObject);
        Debug.Log($"<color=orange>[DROP]</color> {item.itemName} foi jogado ao chão.");
    }

    private Vector3 CalculateDropPosition() {
        return transform.position + transform.forward * DROP_DISTANCE + Vector3.up * DROP_HEIGHT;
    }

    private void ApplyDropForce(GameObject droppedObject) {
        Rigidbody rb = droppedObject.GetComponent<Rigidbody>();
        if (rb != null) {
            // Usa a direção da câmera para arremessar para onde o player olha
            Vector3 throwDirection = Camera.main != null ? Camera.main.transform.forward : transform.forward;
            
            // Arremessa exatamente para onde a câmera aponta (linha reta)
            rb.AddForce(throwDirection * DROP_FORCE, ForceMode.Impulse);
        }
    }

}