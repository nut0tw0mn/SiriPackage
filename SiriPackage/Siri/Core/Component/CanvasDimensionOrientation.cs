//using UnityEngine;
//using UnityEngine.UI;

//[RequireComponent(typeof(CanvasScaler))]
//[ExecuteInEditMode]
//public class CanvasDimensionOrientation : MonoBehaviour
//{
//	private Vector2 oriDimension;
//    public bool debug;
//    private CanvasScaler canvasScaler;
    

//    private bool isAddEvent;
//    // Start is called before the first frame update
//    void Start()
//    {

//        canvasScaler = GetComponent<CanvasScaler>();
//        if (oriDimension == Vector2.zero)
//            oriDimension = canvasScaler.referenceResolution;

//        if (!Application.isPlaying)
//            return;
//	    DeviceChange.Instance.OnOrientationChange.AddListener(OnOrientationChange);
//        isAddEvent = true;
//        OnOrientationChange(DeviceChange.Instance.orientation);
//    }
//#if UNITY_EDITOR
//    enum Dimensions
//    {
//        Unknown,
//        Landscape,
//        Portrait
//    }

//    private Dimensions curDimensions = Dimensions.Unknown;
//    void Update()
//    {
//	    if (!Application.isPlaying && !debug)
//	    {
//            return;
//	    }

//	    var orientation = Screen.width > Screen.height ? Dimensions.Landscape : Dimensions.Portrait; 

//        if(!Equals(curDimensions, orientation))
//        {
//            curDimensions = orientation;
//            GetComponent<CanvasScaler>().referenceResolution = curDimensions == Dimensions.Landscape
//                ? new Vector2(Mathf.Max(oriDimension.x, oriDimension.y), Mathf.Min(oriDimension.x, oriDimension.y))
//                : new Vector2(Mathf.Min(oriDimension.x, oriDimension.y), Mathf.Max(oriDimension.x, oriDimension.y));
//        }
//    }
//#endif


//    private void OnOrientationChange(DeviceOrientation orientation)
//    {
//        switch (orientation)
//        {
//            case DeviceOrientation.Portrait:
//            case DeviceOrientation.PortraitUpsideDown:
//                canvasScaler.referenceResolution = new Vector2(Mathf.Min(oriDimension.x,oriDimension.y), Mathf.Max(oriDimension.x, oriDimension.y));
//                break;
//            case DeviceOrientation.LandscapeLeft:
//            case DeviceOrientation.LandscapeRight:
//                canvasScaler.referenceResolution = new Vector2(Mathf.Max(oriDimension.x, oriDimension.y), Mathf.Min(oriDimension.x, oriDimension.y));
//                break;
//        }
//    }
//    void OnDestroy()
//    {
//        if (isAddEvent && DeviceChange.IsLive)
//        {
//            DeviceChange.Instance.OnOrientationChange.RemoveListener(OnOrientationChange);
//        }
//    }
//}
