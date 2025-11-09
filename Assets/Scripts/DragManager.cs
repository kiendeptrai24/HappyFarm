using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class DragManager : MonoBehaviour
{
    public static DragManager Instance { get; private set; }

    [Header("Camera chính dùng cho raycast")]
    public Camera mainCamera;

    [Header("LayerMask cho mặt đất (Author: Ground)")]
    public LayerMask groundMask;

    [Header("Input Actions (tự động lấy từ PlayerInput)")]
    PlayerInput playerInput;
    Vector2 mousePos;
    private GameObject currentGhost;
    private GameObject currentPrefab;
    private Plot currentPlot;
    private bool isDragging = false;

    [SerializeField] private Material ghostMat;
    [SerializeField] private Material invalidGhostMat;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        playerInput = new PlayerInput();

    }

    private void OnEnable()
    {
        playerInput.Drag.Enable();
        playerInput.Drag.Mouse.canceled += OnClickCanceled;
        playerInput.Drag.Position.performed += ctx => mousePos = ctx.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        playerInput.Drag.Disable();
        playerInput.Drag.Mouse.canceled -= OnClickCanceled;
        playerInput.Drag.Position.performed -= ctx => mousePos = ctx.ReadValue<Vector2>();
    }

    public void BeginDrag(GameObject prefab, GameObject ghost)
    {
        if (isDragging) return;

        currentPrefab = prefab;
        currentGhost = Instantiate(ghost);
        isDragging = true;

        SetGhostColor(false);
    }

    public void EndDrag(bool confirmPlacement)
    {
        if (!isDragging) return;
        Debug.Log("End Drag called. Confirm Placement: " + confirmPlacement);
        Debug.Log("Current Ghost: " + (currentGhost != null ? currentGhost.name : "null"));
        if (confirmPlacement && currentGhost != null)
        {
            DestroyImmediate(currentGhost);
        }

        currentGhost = null;
        currentPrefab = null;
        currentPlot = null;
        isDragging = false;
    }

    private void Update() {
        
        if (!isDragging || currentGhost == null) return;

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
        {
            Plot plot = hit.collider.GetComponent<Plot>();


            if (plot != null && !plot.Isfilled())
            {
                if (plot == currentPlot) return;
                currentPlot = plot;
                currentGhost.transform.position = plot.transform.position;
                SetGhostColor(true);
            }
            else
            {
                currentPlot = null;
                currentGhost.transform.position = hit.point;
                SetGhostColor(false);
            }
        }
    }
    private void OnClickCanceled(InputAction.CallbackContext ctx)
    {
        if (!isDragging) return;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
        {
            Plot plot = hit.collider.GetComponent<Plot>();
            if (plot != null && !plot.Isfilled())
            {
                plot.FillPlot(currentPrefab);
                EndDrag(true);
                return;
            }
        }
        EndDrag(false);

    }

    private void SetGhostColor(bool isValid = true)
    {
        if (currentGhost == null) return;

        foreach (var renderer in currentGhost.GetComponentsInChildren<Renderer>())
        {
            Material[] mats = renderer.sharedMaterials;
            Material[] newMats = new Material[mats.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                newMats[i] = isValid ? ghostMat : invalidGhostMat;
            }

            renderer.materials = newMats;
        }
    }
}