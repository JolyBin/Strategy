using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SelectionState
{
    UnitSelected,
    Frame,
    Other,
    BuildingSelected
}
public class Management : MonoBehaviour
{
    public Camera Camera;
    public SelectableObject Hovered;
    public List<SelectableObject> ListOfSelected = new List<SelectableObject>();

    public Image FrameImage;
    Vector3 _frameStart;
    Vector3 _frameEnd;

    public static SelectionState CurrentSelectionState;

    void Update()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        // Нахождение объекта, на которого навели мышь
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<SelectbleCollider>())
            {
                SelectableObject hitSelectable = hit.collider.GetComponent<SelectbleCollider>().SelectableObject;
                if (Hovered)
                {
                    if (Hovered != hitSelectable)
                    {
                        Hovered.OnUnhover();
                        Hovered = hitSelectable;
                        Hovered.OnHover();
                    }
                }
                else
                {
                    Hovered = hitSelectable;
                    Hovered.OnHover();
                }
            }
            else
            {
                UnhoverCurrent();
            }
        }
        else
        {
            UnhoverCurrent();
        }

        // Выделение юнитов кликом
        if(Input.GetMouseButtonUp(0))
        {
            if (Hovered)
            {
                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    UnselectAll();
                }
                CurrentSelectionState = SelectionState.UnitSelected;
                Select(Hovered);
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftControl) && (CurrentSelectionState == SelectionState.UnitSelected /*|| CurrentSelectionState != SelectionState.BuildingSelected*/))
                {
                    UnselectAll();
                    CurrentSelectionState = SelectionState.Other;
                }
            }
        }

        // Передвижение выделенных юнитов
        if (CurrentSelectionState == SelectionState.UnitSelected)
        {
            if (Input.GetMouseButtonUp(1))
            {
                if (hit.collider.tag == "Ground")
                {
                    int rowNumber = Mathf.CeilToInt(Mathf.Sqrt(ListOfSelected.Count));
                    for (int i = 0; i < ListOfSelected.Count; i++)
                    {
                        int row = i / rowNumber;
                        int col = i % rowNumber;

                        ListOfSelected[i].WhenClickOnGround(hit.point + new Vector3(row, 0, col));
                    }
                }
            }
        }

        // Выделение юнитов рамкой
        if(Input.GetMouseButtonDown(0))
        {
            _frameStart = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            _frameEnd = Input.mousePosition;

            Vector2 min = Vector2.Min(_frameStart, _frameEnd);
            Vector2 max = Vector2.Max(_frameStart, _frameEnd);

            Vector3 size = max - min;

            if (size.magnitude > 10)
            {
                FrameImage.enabled = true;
                FrameImage.rectTransform.anchoredPosition = min;
                FrameImage.rectTransform.sizeDelta = size;
                Rect rect = new Rect(min, size);

                UnselectAll();
                Unit[] allUnits = FindObjectsOfType<Unit>();
                foreach (Unit unit in allUnits)
                {
                    Vector2 screenPosition = Camera.WorldToScreenPoint(unit.transform.position);
                    if (rect.Contains(screenPosition))
                    {
                        Select(unit, false);
                    }
                }

                CurrentSelectionState = SelectionState.Frame;
            }

        }
        if(Input.GetMouseButtonUp(0))
        {
            FrameImage.enabled = false;
            if (ListOfSelected.Count > 0)
            {
                CurrentSelectionState = SelectionState.UnitSelected;
            }
            else
            {
                CurrentSelectionState = SelectionState.Other;
            }
        }
    }



    void Select(SelectableObject selectableObject, bool isClick = true)
    {
        if (ListOfSelected.Contains(selectableObject) && isClick)
        {
            ListOfSelected.Remove(selectableObject);
            selectableObject.Unselect();
        }
        else
        {
            ListOfSelected.Add(selectableObject);
            selectableObject.Select();
        }
    }

    public void Unselect(SelectableObject selectableObject)
    {
        if(ListOfSelected.Contains(selectableObject))
        {
            ListOfSelected.Remove(selectableObject);
        }
    }
    void UnselectAll()
    {
        foreach(SelectableObject obj in ListOfSelected)
        {
            obj.Unselect();
        }
        ListOfSelected.Clear();
    }

    void UnhoverCurrent()
    {
        if (Hovered)
        {
            Hovered.OnUnhover();
            Hovered = null;
        }
    }
}
