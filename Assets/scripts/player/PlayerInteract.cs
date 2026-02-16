using UnityEngine;
using UnityEngine.InputSystem;

// Interface que define o que é um objeto interagível
public interface IInteractable
{
    void Interact();
}

public class PlayerInteract : MonoBehaviour
{
    [Header("Configurações de Interação")]
    public Transform InteractSource; // Arraste a Câmera do jogador aqui no Inspector
    public float InteractRange = 3f; // Distância máxima da interação

    void Update()
    {
        // Verifica se a tecla 'E' foi pressionada neste frame
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            PerformInteract();
        }
    }

    private void PerformInteract()
    {
        // Cria um raio partindo da fonte (geralmente a câmera) para frente
        Ray ray = new Ray(InteractSource.position, InteractSource.forward);
        
        // Se o raio atingir algo dentro da distância permitida
        if (Physics.Raycast(ray, out RaycastHit hitInfo, InteractRange))
        {
            // Tenta pegar o componente IInteractable no objeto atingido
            if (hitInfo.collider.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact();
            }
        }
    }
}
