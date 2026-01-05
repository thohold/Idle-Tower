using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class FreeCamera : MonoBehaviour
{
    public static FreeCamera Instance;
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public float fastMultiplier = 4f;

    [Header("Look Settings")]
    public float lookSensitivity = 0.2f;

    private PlayerInputActions input;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isRightClickHeld;
    private bool leftClickTriggered;


    [SerializeField] private LayerMask selectableMask;
    private float yaw;
    private float pitch;

    [field: SerializeField] public UnlockUi unlockUi;
    [field: SerializeField] public GameObject unlockTab;

    [Header("Placing")]
    [SerializeField] private Material allowedMat;
    [SerializeField] private Material restrictedMat;
    private bool isPlacingStructure;
    private GameObject currentPreview;
    private GameObject structureToPlace;
    private Collider structureCollider;
    private Vector3 currentGroundPos;
    private int price;
    [SerializeField] private LayerMask placeMask;
    [SerializeField] private LayerMask structMask;
    private bool canPlace;
    

    void Awake()
    {
        input = new PlayerInputActions();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled  += ctx => moveInput = Vector2.zero;

        input.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        input.Player.Look.canceled  += ctx => lookInput = Vector2.zero;

        input.Player.RightClick.performed += ctx => StartLooking();
        input.Player.RightClick.canceled  += ctx => StopLooking();

        input.Player.LeftClick.performed += ctx => OnLeftClick();

        input.Player.Cancel.performed += ctx => OnCancel();

    }

    void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        HandleLook();
        HandleMovement();
        if (isPlacingStructure) HandleStructPreview();
    }

    void HandleLook()
    {
        if (!isRightClickHeld) return;

        yaw   += lookInput.x * lookSensitivity;
        pitch -= lookInput.y * lookSensitivity;

        pitch = Mathf.Clamp(pitch, -75, 75);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    void HandleStructPreview()
    {
        if (currentPreview != null && isPlacingStructure) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, placeMask))
            {
                currentGroundPos = hit.point;

            }
            currentPreview.transform.position = currentGroundPos;
        }


        Collider[] hits = Physics.OverlapBox(
            structureCollider.bounds.center,
            structureCollider.bounds.extents,
            structureCollider.transform.rotation,
            structMask
        );
        canPlace = true;
        foreach (Collider hit in hits)
        {
            if (hit == structureCollider) continue;
            else {
                canPlace = false;
                break;
            }

        }

        Renderer rend = currentPreview.GetComponentInChildren<Renderer>();
        rend.material = canPlace ? allowedMat : restrictedMat;

    }

    public void EnterPlaceMode(GameObject preview, GameObject structure, int price)
    {
        isPlacingStructure = true;
        Destroy(currentPreview);
        currentPreview = Instantiate(preview, transform.position, preview.transform.rotation);
        structureToPlace = structure;
        structureCollider = currentPreview.GetComponent<Collider>();
        this.price = price;
    }

    void ExitPlaceMode()
    {
        isPlacingStructure = false;
        Destroy(currentPreview);
        structureToPlace = null;
    }

    void HandleMovement()
    {
        float speed = Keyboard.current.leftShiftKey.isPressed ? moveSpeed * fastMultiplier : moveSpeed;

        Vector3 dir =
            transform.forward * moveInput.y +
            transform.right   * moveInput.x;

        transform.position += dir * speed * Time.deltaTime;
    }

    void StartLooking()
    {
        isRightClickHeld = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void StopLooking()
    {
        isRightClickHeld = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnLeftClick()
    {
    
        if (EventSystem.current.IsPointerOverGameObject())
        return;
        
        if (isRightClickHeld) return;

        if (isPlacingStructure)
        {
            if (!canPlace) return;
            Structure structure = Instantiate(structureToPlace, currentGroundPos, structureToPlace.transform.rotation).GetComponentInChildren<Structure>();
            ExitPlaceMode();
            GameManager.Instance.coins -= price;
            GameManager.Instance.structures.Add(structure);
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, selectableMask))
        {
            
            if (hit.collider.CompareTag("Structure"))
            {
                GameManager.Instance.currentSelectedStructure = hit.collider.gameObject.GetComponent<Structure>();
                Debug.Log("Selected: " + hit.collider.gameObject.name);
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                GameManager.Instance.currentSelectedEntity = hit.collider.gameObject.GetComponent<Enemy>();
                Debug.Log("Selected: " + hit.collider.gameObject.name);
            }
            else if (hit.collider.CompareTag("Card"))
            {
                CollectibleCard card = hit.collider.gameObject.GetComponent<CollectibleCard>();
                if (card.type == CardType.Upgrade)
                {
                    unlockTab.SetActive(true);
                    unlockUi.EnterUnlockWindow(card.upgradeCard);
                    Destroy(hit.collider.gameObject);
                }
                
            }
        } else
        {
            GameManager.Instance.currentSelectedStructure = null;
        }
    }

    void OnCancel()
    {
        if (isPlacingStructure)
        {
            ExitPlaceMode();
        }
    }
}
