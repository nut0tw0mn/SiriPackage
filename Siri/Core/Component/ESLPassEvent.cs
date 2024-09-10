using UnityEngine;
using UnityEngine.EventSystems;

public class ESLPassEvent : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    /// <inheritdoc />
    void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData) => eventData.GetComponentNextLayer<IInitializePotentialDragHandler>()?.OnInitializePotentialDrag(eventData);

    /// <inheritdoc />
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => eventData.GetComponentNextLayer<IBeginDragHandler>()?.OnBeginDrag(eventData);

    /// <inheritdoc />
    void IEndDragHandler.OnEndDrag(PointerEventData eventData) => eventData.GetComponentNextLayer<IEndDragHandler>()?.OnEndDrag(eventData);

    /// <inheritdoc />
    void IDragHandler.OnDrag(PointerEventData eventData) => eventData.GetComponentNextLayer<IDragHandler>()?.OnDrag(eventData);
}
