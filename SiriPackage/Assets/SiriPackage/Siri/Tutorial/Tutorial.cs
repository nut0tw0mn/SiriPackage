using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Button nextStepBtn;
    public Button closeBtn;
    public List<TutorialStep> steps;


    private int count;

    private bool isInit;
    private bool isTaptoEnd;
    public UnityAction OnCloseTutorial;
    public UnityAction<int, int> OnTutorialStep;

    // Use this for initialization
    void Start()
    {
        Show();
    }

    void Init()
    {
        if (isInit) return; isInit = true;
        nextStepBtn?.onClick.AddListener(OnNextStep);
        closeBtn?.onClick.AddListener(Close);

        //TODO: in playroom3d no need canvas component because not use canvas camera
        //Canvas canvas = gameObject.GetComponentExtensions<Canvas>();
        //canvas.overrideSorting = true;
        //canvas.sortingOrder = 499;

        //gameObject.GetComponentExtensions<GraphicRaycaster>();

        var tags = SiriFunction.FindObjectsOfTypeAll<TutorialTag>(true);
        foreach (var tutorialTag in tags)
        {
            var indexs = tutorialTag.stepIndexs;
            foreach (var i in indexs)
            {
                if (i >= 0 && i < steps.Count)
                {
                    var s = steps[i];
                    if (s != null)
                        s.lists.Add(tutorialTag.gameObject);
                }
                else
                {
                    Debug.LogError("array out of range");
                }
            }

        }
    }

    public void OnNextStep()
    {
	    if (isTaptoEnd)
        {
            Close();
        }
        else
        {
            steps[count]?.Hide();
            count++;

            if (count < steps.Count)
            {
                if (steps[count] == null)
                {
                    OnNextStep();
                    return;
                }
                steps[count]?.Show();
            }
            else
            {
                Close();
            }

            OnTutorialStep?.Invoke(count, steps.Count);
        }
    }

    public void Close()
    {
        Init();
        gameObject.SetActive(false);
        foreach (var item in steps)
        {
            item?.Hide();
        }
        //TODO: not only learning path tutorial
        //AnalyticsManager.Instance.TrackLearningpathTutorialClose(count + 1);

        OnCloseTutorial?.Invoke();
        OnCloseTutorial = null;

    }

    public void Show()
    {
	    Show(0, false);
    }

    public void Show(int pageIndex, bool _isTaptoEnd)
    {
        Init();
        gameObject.SetActive(true);

        this.isTaptoEnd = _isTaptoEnd;
        count = pageIndex;
        steps[count].Show();

        OnTutorialStep?.Invoke(count, steps.Count);
    }


}

