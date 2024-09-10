// Sirawat Pitaksarit / 5argon - Exceed7 Experiments
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(GridLayoutGroup))]
[AddComponentMenu("Layout/Extensions/DynamicGridLayoutGroup",0)]
/// <summary>
/// Instead of constraining only row or column, constrain both and let the cell size fill the parent.
/// </summary>
public class DynamicGridLayoutGroup : MonoBehaviour
{
    public enum Mode {
        size,spacingOnly,spaceAll
    }
    public Mode dynamicMode;
	public int column = 1;
	public int row = 1;

	private RectTransform parent;
	private GridLayoutGroup grid;

	void Awake()
	{
		parent = GetComponent<RectTransform>();
		grid = GetComponent<GridLayoutGroup>();
		OnRectTransformDimensionsChange();
	}

#if UNITY_EDITOR
	void Update()
	{
		if (Application.isEditor)
		{
			OnRectTransformDimensionsChange();
		}
	}
#endif

	void OnRectTransformDimensionsChange()
	{
		if (parent == null)
		{
			parent = gameObject.GetComponent<RectTransform>();
		}
		if (grid == null)
		{
			grid = gameObject.GetComponent<GridLayoutGroup>();
		}
		grid.constraint = GridLayoutGroup.Constraint.Flexible;
        if (dynamicMode == Mode.size)
        {
            // calculate spacing
            float x = parent.rect.width - grid.spacing.x * (column - 1);
            float y = parent.rect.height - grid.spacing.y * (row - 1) ;
            // calculate padding
            x -= grid.padding.horizontal;
            y -= grid.padding.vertical;
            // calculate cell size
            x = x / column;
            y = y / row;

            if (float.IsInfinity(x) || float.IsNaN(x) || float.IsInfinity(y) || float.IsNaN(y))
            {
                grid.cellSize = Vector2.zero;
            }
            else
            {
                grid.cellSize = new Vector2(x, y);
            }
        }
        else if (dynamicMode == Mode.spacingOnly)
        {
            // calculate cell size
            float x = parent.rect.width - (grid.cellSize.x * column);
            float y = parent.rect.height - (grid.cellSize.y * row);
            // calculate padding
            x -= grid.padding.horizontal;
            y -= grid.padding.vertical;
            // calculate spacing
            x = x / (column -1);
            y = y / (row - 1);

            if (float.IsInfinity(x) || float.IsNaN(x) || float.IsInfinity(y) || float.IsNaN(y))
            {
                grid.spacing = Vector2.zero;
            }
            else
            {
                grid.spacing = new Vector2(x, y);
            }
        }
        else
        {
            //calculate cell size
            float x = parent.rect.width - (grid.cellSize.x * column);
            float y = parent.rect.height - (grid.cellSize.y * row);
            // calculate spacing all , add Offset
            x = x / (column + 1);
            y = y / (row + 1);
            if (float.IsInfinity(x) || float.IsNaN(x) || float.IsInfinity(y) || float.IsNaN(y))
            {
                grid.spacing = Vector2.zero;
                grid.padding = new RectOffset(0, 0, 0, 0);
            }
            else
            {
                grid.spacing = new Vector2(x, y);
                grid.padding = new RectOffset((int)x, (int)x, (int)y, (int)y);
            }
        }
	}
}