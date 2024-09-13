using UnityEngine.Networking;

public static class ExtensionMethods
{ 
	/*
	// Usage example:
	UnityWebRequest www = new UnityWebRequest();
	// ...
	await www.SendWebRequest();
	Debug.Log(req.downloadHandler.text);
	*/
	public static Siri.UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
	{
		return new Siri.UnityWebRequestAwaiter(asyncOp);
	}
}