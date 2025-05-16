using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerController1 : MonoBehaviour
{
    private float _gravity = -9.81f;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float cameraTargetHeight = 1.7f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;
    private Camera _mainCamera;
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    private Gun gun; // L'arme que le joueur peut ramasser
    private Gun equippedGun; // Arme équipée
    [SerializeField] private float rotationspeed = 500f;
    [SerializeField] private float speed;
    [SerializeField] private Mouvement mouvement;
    [SerializeField] private float jumpPower;
    private bool _cursorVisible;
    public float maxHealth = 100f;
    private float currentHealth;
    public Slider healthBar;
    private Gun gunInRange; // Arme proche que le joueur peut ramasser
    [SerializeField] private Transform weaponHolder;
    public MenuGameOver gameOverMenu;
    private Animator _animator;

    [Serializable]
    public struct Mouvement
    {
        public float speed;
        public float multiplayer;
        public float acceleration;
        [HideInInspector] public bool isSprinting;
        [HideInInspector] public float currentSpeed;
    }

    private bool _facingForward = true;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        ToggleCursorVisibility(false);
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _cursorVisible = !_cursorVisible;
            ToggleCursorVisibility(_cursorVisible);
        }

        if (!_cursorVisible)
        {
            ApplyRotation();
            ApplyGravity();
            ApplyMouvement();
            UpdateAnimations();
        }
    }

    private void ApplyGravity()
    {
        if (_isGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }
        _direction.y = _velocity;
    }

    private void ApplyRotation()
    {
        float rotationInput = _input.x;
        if (Mathf.Abs(rotationInput) > 0.01f)
        {
            transform.Rotate(Vector3.up, rotationInput * rotationspeed * Time.deltaTime);
        }
    }




    private void ApplyMouvement()
    {
        if (_input.sqrMagnitude == 0 && _velocity == 0) return;

        Vector3 moveDirection = Vector3.zero;

        if (_input.y != 0)
        {
            bool isTryingToGoForward = _input.y > 0;

            if (_facingForward != isTryingToGoForward)
            {
                transform.Rotate(0f, 180f, 0f);
                _facingForward = isTryingToGoForward;
            }

            moveDirection = transform.forward * Mathf.Abs(_input.y);
        }

        var targetSpeed = mouvement.isSprinting ? mouvement.speed * mouvement.multiplayer : mouvement.speed;
        mouvement.currentSpeed = Mathf.MoveTowards(mouvement.currentSpeed, targetSpeed, mouvement.acceleration * Time.deltaTime);

        moveDirection *= mouvement.currentSpeed;
        moveDirection.y = _velocity;
        _direction = moveDirection;

        _characterController.Move(moveDirection * Time.deltaTime);
    }




    private void UpdateAnimations()
    {
        bool isMoving = _input.sqrMagnitude > 0;
        _animator.SetBool("isMoving", isMoving);
        _animator.SetBool("isSprinting", mouvement.isSprinting);
        _animator.SetBool("isGrounded", _isGrounded());
        _animator.SetFloat("verticalVelocity", _velocity);
    }

    public void move(InputAction.CallbackContext context)
    {
        if (_cursorVisible) return;
        _input = context.ReadValue<Vector2>();
    }

    public void jump(InputAction.CallbackContext context)
    {
        if (_cursorVisible) return;
        if (!context.started || !_isGrounded()) return;
        _velocity += jumpPower;
        if (mouvement.isSprinting)
        {
            _animator.SetTrigger("runningForwardFlip");
        }
        else
        {
            _animator.SetTrigger("jump");
        }
    }

    public void sprint(InputAction.CallbackContext context)
    {
        if (_cursorVisible) return;
        mouvement.isSprinting = context.started || context.performed;
    }

    private bool _isGrounded() => _characterController.isGrounded;

    private void ToggleCursorVisibility(bool isVisible)
    {
        Cursor.visible = isVisible;
        Cursor.lockState = isVisible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void Shoot(InputAction.CallbackContext context)
    {
        if (_cursorVisible) return;

        if (equippedGun != null && equippedGun.IsEquipped && context.performed)
        {
            if (GetComponent<AudioSource>())
                GetComponent<AudioSource>().Play();
            equippedGun.Shoot();
        }
    }

    public void interact(InputAction.CallbackContext context)
    {
        if (_cursorVisible) return;
        if (!context.started) return;

        // Détection d'un objet interactif à proximité
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 2f); // Rayon de détection de 2 unités
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Interactive"))
            {
                InteractWithObject(hitCollider.gameObject); // Interagir avec l'objet
                break;
            }
        }
    }

    private void InteractWithObject(GameObject obj)
    {
        Gun gun = obj.GetComponent<Gun>();
        if (gun != null)
        {
            gunInRange = gun;
            equippedGun = gunInRange;
            gunInRange.PickUp();

            // Attacher l'arme au WeaponHolder
            equippedGun.transform.SetParent(weaponHolder);
            equippedGun.transform.localPosition = Vector3.zero; // Ajuste si besoin
            equippedGun.transform.localRotation = Quaternion.identity; // Ajuste l'orientation

            Debug.Log("Arme ramassée et positionnée !");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Gun gun = other.GetComponent<Gun>();
        if (gun != null)
        {
            gunInRange = gun; // Arme à proximité
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Gun gun = other.GetComponent<Gun>();
        if (gun != null && gun == gunInRange)
        {
            gunInRange = null; // Plus d'arme proche
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player is dead!");
        if (gameOverMenu != null)
        {
            gameOverMenu.CanvaGameOver.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (cameraTarget != null)
        {
            cameraTarget.position = transform.position + Vector3.up * cameraTargetHeight;
        }
    }

}
