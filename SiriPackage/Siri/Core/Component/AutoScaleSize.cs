
//using System.Collections;
//using UnityEngine.EventSystems;

//namespace UnityEngine.UI
//{
//    [AddComponentMenu("Layout/AutoScaleSize", 146)]
//    [ExecuteInEditMode]
//    [RequireComponent(typeof(RectTransform))]
//    [DisallowMultipleComponent]
//    public class AutoScaleSize : UIBehaviour, ILayoutSelfController
//    {
//        public enum Mode { None, Width, Height, minWidthorHeight }

//        [SerializeField] private Mode m_AspectMode = Mode.None;
//        public Mode aspectMode { get { return m_AspectMode; } set { m_AspectMode = value; SetDirty(); } }


//        [SerializeField] private CanvasScaler refCanvas;
//        [System.NonSerialized]
//        private RectTransform m_root;
//        private RectTransform rectRoot
//        {
//            get {
//                if (m_root == null)
//                    m_root = refCanvas.GetComponent<RectTransform>();
//                return m_root;
//            }
//        }

//        [System.NonSerialized]
//        private RectTransform m_Rect;
//        private RectTransform rectTransform
//        {
//            get {
//                if (m_Rect == null)
//                    m_Rect = GetComponent<RectTransform>();
//                return m_Rect;
//            }
//        }

//        private DrivenRectTransformTracker m_Tracker;

//        protected override void OnEnable()
//        {
//            base.OnEnable();
//            if (!refCanvas)
//                refCanvas = transform.root.GetComponentInChildren<CanvasScaler>();
//            SetDirty(true);
//        }

//        protected override void OnDisable()
//        {
//            m_Tracker.Clear();
//            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
//            base.OnDisable();
//        }

//        private void UpdateRect()
//        {
//            if (!IsActive())
//                return;
//            if (!refCanvas)
//                return;

//            m_Tracker.Clear();

//            switch (m_AspectMode)
//            {
//#if UNITY_EDITOR
//                case Mode.None:
//                {
//                    break;
//                }
//#endif
//                case Mode.Width:
//                {
//                    m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);


//                    SetSizeWithRatio(0, GetRatio(0));
//                    break;
//                }
//                case Mode.Height:
//                {
//                    m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
//                    SetSizeWithRatio(1, GetRatio(1));
//                    break;
//                }
//                case Mode.minWidthorHeight:
//                {
//                    m_Tracker.Add(this, rectTransform,
//                        DrivenTransformProperties.SizeDeltaX |
//                        DrivenTransformProperties.SizeDeltaY);
//                    float minRatio = Mathf.Min(GetRatio(0), GetRatio(1));
//                    SetSizeWithRatio(0, minRatio);
//                    SetSizeWithRatio(1, minRatio);
//                    break;
//                }
//            }
//        }

//        private void SetSizeWithRatio(int axis,float ratio)
//        {
//            float size = rectTransform.rect.size[axis] * ratio ;
//            rectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, size);
//        }

//        private float GetRatio(int axis)
//        {
//            return Mathf.Clamp(rectRoot.rect.size[axis] / refCanvas.referenceResolution[axis], 0.001f, 1000f);
//        }

//        public virtual void SetLayoutHorizontal() { }

//        public virtual void SetLayoutVertical() { }

//        protected override void OnRectTransformDimensionsChange() { }

//        private IEnumerator DelayUpdate()
//        {
//            yield return null;
//            UpdateRect();
//        }

//        protected void SetDirty(bool delayUpdate = false)
//        {
//            if (delayUpdate)
//                StartCoroutine(DelayUpdate());
//            else
//                UpdateRect();
//        }

//#if UNITY_EDITOR
//        protected override void OnValidate()
//        {
//            SetDirty();
//        }

//        [Sirenix.OdinInspector.Button("UpdateAll",Sirenix.OdinInspector.ButtonSizes.Medium)]
//        private void AutoSizeAll()
//        {
//            Debug.Log("AutoSizeAll");
//            AutoScaleSize[] scripts = (AutoScaleSize[])FindObjectsOfType(typeof(AutoScaleSize));
//            foreach (var item in scripts)
//            {
//                item.UpdateRect();
//            }
//        }
//#endif
//    }
//}