using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState State;

    private void Start()
    {
        SetCamera(GridManager.Instance.gridSize);

        UIManager.Instance.OnRebuildButtonClicked += SetCamera;
    }

    private void Update()
    {
        if (State == GameState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Input.mousePosition;

                mousePos.z = 0f;

                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                Vector3 cellPos = GridManager.Instance.GetGridPos(worldPos);

                cellPos.z = 0f;

                if (GridManager.Instance.CheckOnCell(cellPos))
                {
                    GridManager.Instance.ActivateCell(cellPos);
                }
            }
        }
    }

    private void SetCamera(int size)
    {
        Vector3 pos = Camera.main.transform.position;

        pos.x = (float)size / 2;

        Camera.main.transform.position = pos;

        Camera.main.orthographicSize = size + (float)(size / 10);
    }
}
public enum GameState
{
    Initial,
    Playing
}

