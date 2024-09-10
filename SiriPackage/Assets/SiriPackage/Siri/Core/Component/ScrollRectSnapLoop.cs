using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Siri.Core.Component
{
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectSnapLoop : MonoBehaviour, IDragHandler, IEndDragHandler
    {
	    private const float SnapSpeed = 0.3f;
        private const float CutoffVelocity = 100f;

        [SerializeField] private CapsuleDirection2D dimension;

        private ScrollRect scrollRect;
        public RectTransform Content => scrollRect.content;
        private bool isHorizontal => dimension == CapsuleDirection2D.Horizontal;
        private float currentPosition => isHorizontal ? Content.anchoredPosition.x : Content.anchoredPosition.y;
        private float stepSize;
        private List<(Transform, int)> childs = new List<(Transform, int)>();
        private bool isDragging = false;
        private Coroutine coroutine = null;

        public Action<int> OnLoopSnaped;
        public Action OnLoopEndDrag;
        public Action<int> OnLoopSnaping;

        public void SpinAround(int targetIndex, int round, float velocity,Action callback=null)
        {
            coroutine = StartCoroutine(Spin(targetIndex, round, velocity,()=> 
            {
                callback?.Invoke();
            }));

        }

        public int SnapingIndex()
        {
            bool isSnapLeft = Mathf.Abs(currentPosition + stepSize) < Mathf.Abs(currentPosition + 2 * stepSize);
            (_, int index) = childs[isSnapLeft ? 1 : 2];

            return index;
        }


        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener(OnDragPositionChange);
        }

        private void Reset()
        {
            childs.Clear();
            SetPosition(0);
        }

        public void Init()
        {
            scrollRect.enabled = false;
            Reset();
            int index = 0;
            foreach (Transform tr in Content)
            {
                if (!tr.gameObject.activeSelf)
                    continue;
                childs.Add((tr, index++));
            }

            // move last to first subling index
            MoveLastToFirst();
            scrollRect.content.RefreshLayoutGroup();
            stepSize = Content.GetWidth() / childs.Count;
            AddPositionValue(-stepSize);

            scrollRect.enabled = true;
        }

        private void MoveLastToFirst()
        {
            int lastIndex = childs.Count - 1;
            (Transform item, int index) last = childs[lastIndex];
            last.item.SetAsFirstSibling();

            childs.RemoveAt(lastIndex);
            childs.Insert(0, last);
        }

        private void MoveFirstToLast()
        {
            (Transform item, int index) first = childs[0];
            first.item.SetAsLastSibling();

            childs.RemoveAt(0);
            childs.Add(first);
        }

        private void OnDragPositionChange(Vector2 dragPos)
        {
            if (isDragging)
                return;
            if (childs.Count < 3)
                return;
            if (Content.childCount < 3)
                return;

            float position = currentPosition;
            float distance = Mathf.Abs(position);
            float cutoff = 2 * stepSize;

            if (distance > stepSize && distance < cutoff)
                return;

            float diff = position + stepSize;
            int direction = (int)(Mathf.Abs(diff) / diff);

            if (direction > 0)
                MoveLastToFirst();
            else
                MoveFirstToLast();

            float offset = stepSize * direction;
            AddPositionValue(-offset);
        }

        private void AddPositionValue(float value)
        {
            Content.anchoredPosition += new Vector2(isHorizontal ? value : 0, !isHorizontal ? value : 0);
        }

        private void SetPosition(float value)
        {
            Vector2 aPosition = Content.anchoredPosition;
            aPosition.x = isHorizontal ? value : aPosition.x;
            aPosition.y = !isHorizontal ? value : aPosition.y;
            Content.anchoredPosition = aPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            OnLoopEndDrag?.Invoke();

            if (coroutine != null)
                StopCoroutine(coroutine);

            coroutine = StartCoroutine(SnapPosition());
        }

        private IEnumerator SnapPosition()
        {
            float leftPosition = stepSize;
            float rightPosition = 2 * stepSize;
            float step = Mathf.SmoothStep(0f, 1, SnapSpeed);
            bool isSnaped = false;

            while (!isSnaped)
            {
                float position = currentPosition;

                bool isSnapping = Mathf.Abs(scrollRect.velocity.magnitude) < CutoffVelocity;
                if (isSnapping)
                {
                    float distance = Mathf.Abs(position);
                    float diffLeft = Mathf.Abs(distance - leftPosition);
                    float diffRight = Mathf.Abs(distance - rightPosition);

                    float targetPosition = diffLeft < diffRight ? -leftPosition : -rightPosition;
                    float slicePosition = Mathf.Lerp(position, targetPosition, step);

                    isSnaped = Mathf.Abs(slicePosition - targetPosition) <= step;

                    SetPosition(slicePosition);
                }

                yield return null;
            }

            scrollRect.velocity = Vector2.zero;

            OnLoopSnaped?.Invoke(SnapingIndex());
        }

        private IEnumerator Spin(int targetIndex, int round, float velocity,Action callback=null)
        {
            Vector2 v = new Vector2(isHorizontal ? velocity : 0, !isHorizontal ? velocity : 0);
            int passRound = 0;
            int previousIndex = SnapingIndex();

            while (passRound < round)
            {
                scrollRect.velocity = v;
                int index = SnapingIndex();
                if (previousIndex != index)
                {
                    if (index == targetIndex)
                    {
                        passRound += 1;
                    }
                    OnLoopSnaping?.Invoke(index);
                }
                previousIndex = index;

                yield return null;
            }
            scrollRect.velocity = Vector2.zero;
            StopCoroutine(coroutine);

            StartCoroutine(SnapPosition());
            callback?.Invoke();
        }
    }
}

