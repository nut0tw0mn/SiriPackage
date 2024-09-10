using UnityEngine;

namespace Siri.Core.Component
{
	internal class ScrollRectSync : MonoBehaviour
	{
		public ScrollRectSnap sc1;
		public ScrollRectSnap sc2;

		// Start is called before the first frame update
		void Start()
		{
			sc2.scrollRect.normalizedPosition = sc1.scrollRect.normalizedPosition;
			sc1.OnDraggingEvent.AddListener(() =>
			{
				sc2.scrollRect.normalizedPosition = sc1.scrollRect.normalizedPosition;
			});
			sc1.OnEndDragEvent.AddListener(() =>
			{
				sc2.SnapIndex(sc1.Index);
			});

			sc2.OnDraggingEvent.AddListener(() =>
			{
				sc1.scrollRect.normalizedPosition = sc2.scrollRect.normalizedPosition;
			});
			sc2.OnEndDragEvent.AddListener(() =>
			{
				sc1.SnapIndex(sc2.Index);
			});
		}

	}
}
