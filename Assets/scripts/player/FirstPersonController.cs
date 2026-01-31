using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public float speed = 5f; // Velocidade do jogador
    public float gravity = 9.81f; // Gravidade

    [Header("Configurações de Câmera")]
    public float mouseSensitivity = 20f; // Sensibilidade do mouse
    public float upLimit = -90f; // Limite de rotação da câmera para cima
    public float downLimit = 90f; // Limite de rotação da câmera para baixo
    
    private float xRotation = 0f; // Rotação da câmera
    private Camera playerCamera; // Guardamos a referência da câmera aqui
    private CharacterController controller; // Controla o movimento do jogador

    void Awake()
    {
        controller = GetComponent<CharacterController>(); // Pega o CharacterController
        playerCamera = Camera.main; // Pega a câmera principal automaticamente
        
        Cursor.lockState = CursorLockMode.Locked; // Trava o mouse
        Cursor.visible = false; // Esconde o mouse
    } 

    void Update()
    {
        MovePlayer();
        Look(); // Adicionei a chamada aqui para funcionar!
    }

    void MovePlayer()
    {
        Vector2 input = Vector2.zero;
        
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1; // Movimento para frente
            if (Keyboard.current.sKey.isPressed) input.y -= 1; // Movimento para trás
            if (Keyboard.current.aKey.isPressed) input.x -= 1; // Movimento para a esquerda
            if (Keyboard.current.dKey.isPressed) input.x += 1; // Movimento para a direita
        }

        Vector3 move = transform.right * input.x + transform.forward * input.y;

        if (move.magnitude > 1) move.Normalize(); // Normaliza o movimento

        controller.Move(move * speed * Time.deltaTime); // Move o jogador
        
        if (!controller.isGrounded)
            controller.Move(Vector3.down * gravity * Time.deltaTime); // Aplica a gravidade
    }

    void Look()
    {
        if (Mouse.current == null) return;

        // Pegamos o Vector2 do mouse (x = horizontal, y = vertical)
        Vector2 mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;

        // Girar o corpo do jogador para os lados (Eixo Y)
        transform.Rotate(Vector3.up * mouseDelta.x);

        // Girar a câmera para cima e para baixo (Eixo X)
        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, upLimit, downLimit); // Limita a visão

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}