using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class DragManager : MonoBehaviour
{
    public static DragManager Instance { get; private set; }

    [Header("Camera")]
    public Camera mainCamera;

    [Header("LayerMask")]
    public LayerMask groundMask;
    public LayerMask canShowDataMask;


    [Header("Input Actions")]
    PlayerInput playerInput;
    Vector2 mousePos;
    private GameObject currentGhost;
    private GameObject currentPrefab;
    private IFillOnAble curPlace;
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
        playerInput.Drag.Mouse.performed += ctx => OnClicked(ctx);
        playerInput.Drag.Position.performed += ctx => mousePos = ctx.ReadValue<Vector2>();
    }

    private void OnClicked(InputAction.CallbackContext ctx)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, canShowDataMask))
        {
            FarmEntity entity = hit.collider.GetComponent<FarmEntity>();
            if (entity != null)
            {
                PupupShowData.Instance.SetData(entity);
                PupupShowData.Instance.Show();
            }
        }
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

    public void EndDrag()
    {
        if (!isDragging) return;

        Destroy(currentGhost);
        currentGhost = null;
        currentPrefab = null;
        curPlace = null;
        isDragging = false;
    }

    private void Update()
    {

        if (!isDragging || currentGhost == null) return;

        Ray ray = mainCamera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
        {
            IFillOnAble plot = hit.collider.GetComponent<IFillOnAble>();
            if (plot != null && !plot.Isfilled())
            {
                if (plot == curPlace) return;
                curPlace = plot;
                currentGhost.transform.position = plot.position;
                SetGhostColor(true);
            }
            else
            {
                curPlace = null;
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
            IFillOnAble obj = hit.collider.GetComponent<IFillOnAble>();
            if (obj != null && obj.Isfilled() == false)
            {
                obj.OnFill(currentPrefab);
            }
        }
        EndDrag();
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