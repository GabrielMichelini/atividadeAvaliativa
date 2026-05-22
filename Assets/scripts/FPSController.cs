using UnityEngine;
using UnityEngine.InputSystem; 

public class FPSController : MonoBehaviour
{
    [Header("Controle de Estado")]
    public bool canMove = true; // Permite ao Hangar ligar/desligar o andar

    [Header("Movimentação")]
    public CharacterController controller;
    public float speed = 8f;

    [Header("Câmera")]
    public Camera playerCamera;
    public float mouseSensitivity = 0.5f; 
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- VISÃO (Sempre ativa para mirar) ---
        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // --- ANDAR (Trava quando o minigame começa) ---
        if (canMove)
        {
            float x = 0f; float z = 0f;
            if (Keyboard.current.wKey.isPressed) z += 1f;
            if (Keyboard.current.sKey.isPressed) z -= 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;

            Vector3 move = transform.right * x + transform.forward * z;
            if (move.magnitude > 1f) move.Normalize();
            controller.Move(move * speed * Time.deltaTime);
        }
    }
}