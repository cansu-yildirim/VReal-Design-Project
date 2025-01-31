using UnityEngine;

public class MouseInputManager : MonoBehaviour
{

    // Used for checking if the mouse was clicked in the drawing area
    public GameObject drawingArea;
    private BoxCollider drawingAreaCollider;

    public WallCreationManager wallCreationManager;
    public WallSelectionManager wallSelectionManager;
    public AddOnCreationManager addOnCreationManager;

    void Start()
    {
        drawingAreaCollider = drawingArea.GetComponent<BoxCollider>();
    }

    private void Update()
    {
        DelegateMouseInputs();
    }

    private void DelegateMouseInputs()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        // Left mouse pressed
        if (Input.GetMouseButtonDown(0) && IsPositionValid(mousePosition))
        {
            if (EditorTools.SelectedTool == EditorTools.Tool.Selector)
                wallSelectionManager.HandleWallSelection();
            else if (EditorTools.SelectedTool == EditorTools.Tool.Wall)
                wallCreationManager.StartWallCreation(mousePosition);
            else if (EditorTools.SelectedTool == EditorTools.Tool.Window || EditorTools.SelectedTool == EditorTools.Tool.Door)
                addOnCreationManager.TryToPlaceAddOn();
        }
        // Left mouse dragged
        else if (Input.GetMouseButton(0))
        {
            if (EditorTools.SelectedTool == EditorTools.Tool.Wall)
            {
                mousePosition = SnapMouseToDrawingArea(mousePosition);
                wallCreationManager.UpdateWall(mousePosition);
            }
        }
        // Left mouse released
        else if (Input.GetMouseButtonUp(0))
        {
            if (EditorTools.SelectedTool == EditorTools.Tool.Wall)
            {
                mousePosition = SnapMouseToDrawingArea(mousePosition);
                wallCreationManager.FinalizeWallCreation(mousePosition);
            }
        }
        // Right mouse pressed
        else if (Input.GetMouseButtonDown(1))
        {
            if (EditorTools.SelectedTool == EditorTools.Tool.Selector)
            {
                wallSelectionManager.RemoveSelectedWalls();
            }
        }

        // Run the state machine of the add-on depending on the selected tool
        // Reset the add-on object when the currently selected Tool is changed
        if (EditorTools.SelectedTool == EditorTools.Tool.Window)
            addOnCreationManager.HandleAddOnCreation(mousePosition, AddOnCreationManager.AddOnType.Window);
        else if (EditorTools.SelectedTool == EditorTools.Tool.Door)
            addOnCreationManager.HandleAddOnCreation(mousePosition, AddOnCreationManager.AddOnType.Door);
        else
            addOnCreationManager.ResetAddOn();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    // Checks if the given position is contained inside the drawing area
    private bool IsPositionValid(Vector3 position)
    {
        return position.x >= drawingAreaCollider.bounds.min.x &&
            position.x <= drawingAreaCollider.bounds.max.x &&
            position.z >= drawingAreaCollider.bounds.min.z &&
            position.z <= drawingAreaCollider.bounds.max.z;
    }

    // Clamps the given position into the drawing area to ensure walls cannot be drawn outside the panel
    private Vector3 SnapMouseToDrawingArea(Vector3 position)
    {
        Vector3 colliderMin = drawingAreaCollider.bounds.min;
        Vector3 colliderMax = drawingAreaCollider.bounds.max;

        position.x = Mathf.Clamp(position.x, colliderMin.x, colliderMax.x);
        position.z = Mathf.Clamp(position.z, colliderMin.z, colliderMax.z);

        return position;
    }

}
