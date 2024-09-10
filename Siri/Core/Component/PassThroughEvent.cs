using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PassThroughEvent : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	public List<GameObject> ignores = new List<GameObject>();

	public void OnInitializePotentialDrag(PointerEventData eventData) => eventData.GetComponentNextLayer<IInitializePotentialDragHandler>(ignores)?.OnInitializePotentialDrag(eventData);

	public void OnBeginDrag(PointerEventData eventData) => eventData.GetComponentNextLayer<IBeginDragHandler>(ignores)?.OnBeginDrag(eventData);

	public void OnEndDrag(PointerEventData eventData) => eventData.GetComponentNextLayer<IEndDragHandler>(ignores)?.OnEndDrag(eventData);

	public void OnDrag(PointerEventData eventData) => eventData.GetComponentNextLayer<IDragHandler>(ignores)?.OnDrag(eventData);
}