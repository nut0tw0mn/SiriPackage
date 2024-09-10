using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
[DisallowMultipleComponent]
public class UISizeFitter : MonoBehaviour {
    public enum FitMode
    {
        Unconstrained,
        MinSize,
        PreferredSize
    }
    public FitMode m_HorizontalFit = FitMode.Unconstrained;

    public FitMode m_VerticalFit = FitMode.Unconstrained;

}
