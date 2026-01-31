using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    public float speed = 5f;
    public float gravity = 9.81f;

    [Header("Configurações de Câmera")]
    public float mouseSensitivity = 20f;
    public float upLimit = -90f; // Mudei para negativo para limitar o olhar para cima
    public float downLimit = 90f;
    
    private float xRotation = 0f;
    private Camera playerCamera; // Guardamos a referência da câmera aqui
    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main; // Pega a câmera principal automaticamente
        
        // CORREÇÃO: É Cursor (com 'r' no final e C maiúsculo)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
        }

        Vector3 move = transform.right * input.x + transform.forward * input.y;

        if (move.magnitude > 1) move.Normalize();

        controller.Move(move * speed * Time.deltaTime);
        
        if (!controller.isGrounded)
            controller.Move(Vector3.down * gravity * Time.deltaTime);
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