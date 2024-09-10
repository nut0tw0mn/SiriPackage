using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

    //[AddComponentMenu("Layout/Content Size Fitter", 141)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class UIContentSizeFitter : UIBehaviour, ILayoutSelfController
    {
        public enum FitMode
        {
            Unconstrained,
            MinSize,
            PreferredSize
        }

        [SerializeField] protected FitMode m_HorizontalFit = FitMode.Unconstrained;
        public FitMode horizontalFit { get { return m_HorizontalFit; } set { m_HorizontalFit = value; SetDirty(); } }

        [SerializeField] protected FitMode m_VerticalFit = FitMode.Unconstrained;
        public FitMode verticalFit { get { return m_VerticalFit; } set { m_VerticalFit = value; SetDirty(); } }

        [System.NonSerialized] private RectTransform m_Rect;
        private RectTransform rectTransform
        {
            get
            {
                if (m_Rect == null)
                    m_Rect = GetComponent<RectTransform>();
                return m_Rect;
            }
        }

        protected UIContentSizeFitter()
        { }

        #region Unity Lifetime calls

        protected override void OnEnable()
        {
            SetDirty();
        }

        protected override void OnDisable()
        {
        }
        #endregion

        private void HandleSelfFittingAlongAxis(int axis)
        {
            FitMode fitting = (axis == 0 ? horizontalFit : verticalFit);
            if (fitting == FitMode.Unconstrained)
                return;

            // Set anchor max to same as anchor min along axis
            Vector2 anchorMax = rectTransform.anchorMax;
            anchorMax[axis] = rectTransform.anchorMin[axis];
            rectTransform.anchorMax = anchorMax;

            // Set size to min size
            Vector2 sizeDelta = rectTransform.sizeDelta;
            if (fitting == FitMode.MinSize)
                sizeDelta[axis] = LayoutUtility.GetMinSize(m_Rect, axis);
            else
                sizeDelta[axis] = LayoutUtility.GetPreferredSize(m_Rect, axis);
            rectTransform.sizeDelta = sizeDelta;
        }

        public void SetLayoutHorizontal()
        {
            HandleSelfFittingAlongAxis(0);
        }

        public void SetLayoutVertical()
        {
            HandleSelfFittingAlongAxis(1);
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        SetDirty();
    }
#endif
}
