using System;
using System.Collections.Generic;
using Siri.Core.Component;
using UnityEngine;
using UnityEngine.UI;

namespace Siri
{
    public class DatePicker : MonoBehaviour
    {
        [SerializeField] GameObject monthObj;
        [SerializeField] GameObject yearObj;
        [SerializeField] private Transform tempMont;
        [SerializeField] private Transform tempYear;

        private readonly int MONTHCOUNT = 12;
        private readonly int YEARCOUNT = 51;
        private readonly int YEAROFFSET = -1;

        private ScrollRectSnap monthSnapScript;
        private ScrollRectSnap yearSnapScript;

        private List<Text> monthTexts = new List<Text>();
        private List<Text> yearTexts = new List<Text>();
        private int lastIndexMonth = -1;
        private int lastIndexYear = -1;

        // Use this for initialization
        void Start()
        {
            monthSnapScript = monthObj.GetComponent<ScrollRectSnap>();
            yearSnapScript = yearObj.GetComponent<ScrollRectSnap>();
            Init();
        }

        private void Init()
        {
            //Year
            var refObj = tempYear;
            int _year = DateTime.Now.Year - YEARCOUNT - YEAROFFSET;
            for (int i = 0; i < YEARCOUNT; i++)
            {
                Transform newObj = Instantiate(refObj, refObj.parent, false);
                Text newText = newObj.gameObject.GetComponent<Text>();
                newText.color = ColorExtensions.HexToColor("3EC8C0");

                string content = _year + i + "";
                newText.text = content;
                newObj.name = content;

                yearTexts.Add(newText);
            }

            refObj.gameObject.SetActive(false);
            //Month
            refObj = tempMont;
            for (int i = 0; i < MONTHCOUNT; i++)
            {
                Transform newObj = Instantiate(refObj, refObj.parent, false);
                Text newText = newObj.gameObject.GetComponent<Text>();
                newText.color = ColorExtensions.HexToColor("3EC8C0");

                string content = new DateTime(2000, i + 1, 1).ToString("MMMM");
                newText.text = content;
                newObj.name = i + 1 + "";

                monthTexts.Add(newText);
            }

            refObj.gameObject.SetActive(false);
            SnapDateNow();
            RegisterEvent();
        }

        private void RegisterEvent()
        {
            monthSnapScript.OnSnapped.AddListener(() =>
            {
                if(lastIndexMonth!= -1)
                    monthTexts[lastIndexMonth].color = ColorExtensions.HexToColor("3EC8C0");
                lastIndexMonth = (monthSnapScript.steps - 1) - monthSnapScript.Index;
                monthTexts[lastIndexMonth].color = Color.black;
            });

            yearSnapScript.OnSnapped.AddListener(() =>
            {
                if (lastIndexYear != -1)
                    yearTexts[lastIndexYear].color = ColorExtensions.HexToColor("3EC8C0");
                lastIndexYear = (yearSnapScript.steps - 1) - yearSnapScript.Index;
                yearTexts[lastIndexYear].color = Color.black;
            });
        }

        private void SnapDateNow()
        {
            monthSnapScript.SetStep(MONTHCOUNT);
            monthSnapScript.SnapIndex(MONTHCOUNT - DateTime.Now.Month);

            yearSnapScript.SetStep(YEARCOUNT);
            yearSnapScript.SnapIndex(2);
        }

        public DateTime GetDate()
        {
            int mCnt = MONTHCOUNT - monthSnapScript.Index;
            int yCnt = DateTime.Now.Year - yearSnapScript.Index -1- YEAROFFSET;
            
            return new DateTime(yCnt, mCnt, 1);
        }
    }
}
