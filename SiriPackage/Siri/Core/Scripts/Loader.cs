using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Siri
{
    internal class Loader : MonoBehaviour
    {
	    #region async

        internal static async void DownloadBundleAsync(string _url, Action<AssetBundle> callback)
        {
            Debug.Log("DownloadBundleAsync :" + _url);
            using (var uwr = UnityWebRequestAssetBundle.GetAssetBundle(_url))
            {
                await uwr.SendWebRequest();
                if (!isAvailable)
                    return;
                if (!IsRequestDone(uwr))
                {
                    Debug.LogWarning($"Download Error Message :{uwr.error}");
                }

                callback?.Invoke(DownloadHandlerAssetBundle.GetContent(uwr));
            }
        }
        internal static async void DownloadAsync(string _url, Action<DownloadHandler> callback)
        {
            using (var uwr = UnityWebRequest.Get(_url))
            {
                await uwr.SendWebRequest();
                if (!isAvailable)
                    return;

                if (!IsRequestDone(uwr))
                {
                    Debug.LogWarning($"Download Error Message :{uwr.error}");
                }

                callback?.Invoke(uwr.downloadHandler);
            }
        }
        internal static async void LoadPhotoAsync(string photo_uri, int timesToTry, int eachTimeOut, Action<Texture2D> onFinish = null)
        {
	        //Debug.Log(photo_uri);
	        if (string.IsNullOrEmpty(photo_uri) || photo_uri == "-")
	        {
		        onFinish?.Invoke(null);
	        }
	        else
	        {
		        bool isSuccess = false;
		        string error_message = null;

                for (int i = 0; i < timesToTry; i++)
		        {
			        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(photo_uri))
			        {
				        uwr.timeout = eachTimeOut;
				        DownloadHandlerTexture downloadHandler = new DownloadHandlerTexture(true);
				        uwr.downloadHandler = downloadHandler;
				        await uwr.SendWebRequest();
				        if (!isAvailable)
					        return;
				        if (!IsRequestDone(uwr))
				        {
                            //Debug.LogWarning($"Load Photo Error Message :{error_message} \n{photo_uri}");
                            LogKeeper.Log(photo_uri, error_message);
                            error_message = uwr.error;
				        }
				        else
				        {
					        isSuccess = true;
					        //Debug.Log($"<i>Completed loaded photo.\n{photo_uri}</i>");

					        var texture = downloadHandler.texture;
					        onFinish?.Invoke(texture);
					        break;
				        }
			        }
		        }
		        if (!isSuccess)
		        {
			        Debug.LogWarning($"Load Photo Error Message :{error_message} \n{photo_uri}");
			        onFinish?.Invoke(null);
		        }
	        }
        }
        internal static async void LoadAudioClipAsync(string audio_uri, AudioType type, int timesToTry, int eachTimeOut, Action<AudioClip> onFinish = null)
        {
            if (string.IsNullOrEmpty(audio_uri))
            {
                onFinish.SafeInvoke(null);
            }
            else
            {
                Debug.Log("GetAudioClip "+audio_uri);
                bool isSuccess = false;
                string error_message = null;

                for (int i = 0; i < timesToTry; i++)
                {
                    using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(audio_uri, type))
                    {
                        uwr.timeout = eachTimeOut;
                        await uwr.SendWebRequest();
                        if (!isAvailable)
                            return;
                        if (!IsRequestDone(uwr))
                        {
                            error_message = uwr.error;
                            //Debug.LogWarning("Try again\nLoad Audio Error Message :" + error_message);
                        }
                        else
                        {
                            isSuccess = true;
                            //Debug.Log($"<i>Completed loaded audio.\n{audio_uri}</i>");

                            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                            
                            onFinish?.Invoke(clip);
                            break;
                        }
                    }

                }
                if (!isSuccess)
                {
                    //Debug.LogWarning("Load Audio Error Message :" + error_message + "\n" + audio_uri);
                    onFinish?.Invoke(null);

                }
            }

        }

        #endregion

        #region Coroutine

        internal Coroutine LoadPhoto(string photo_uri, int timesToTry, int eachTimeOut, Action<Sprite> setSprite = null)
        {
            return StartCoroutine(WaitLoadPhoto(photo_uri, timesToTry, eachTimeOut, setSprite));
        }
        internal void LoadAudioClip(string audio_uri, AudioType type, int timesToTry, int eachTimeOut, Action<AudioClip> callBack = null)
        {
            StartCoroutine(WaitLoadAudio(audio_uri, type, timesToTry, eachTimeOut, callBack));
        }
        internal void Download(string _url, Action<float> progress, Action<DownloadHandler> callback)
        {
            StartCoroutine(WaitDownload(_url, progress, callback));
        }
        ///update code UnityWebRequestTexture 
        IEnumerator WaitLoadPhoto(string photo_uri, int timesToTry, int eachTimeOut, Action<Sprite> onFinish)
        {
            if (string.IsNullOrEmpty(photo_uri))
            {
                onFinish.SafeInvoke(null);
            }
            else
            {
                bool isSuccess = false;
                string error_message = null;

                for (int i = 0; i < timesToTry; i++)
                {
#if UNITY_2018 ||UNITY_2019_1_OR_NEWER
                    using (var uwr = UnityWebRequestTexture.GetTexture(photo_uri))
                    {
                        uwr.timeout = eachTimeOut;
                        DownloadHandlerTexture downloadHandler = new DownloadHandlerTexture(true);
                        uwr.downloadHandler = downloadHandler;
                        yield return uwr.SendWebRequest();
                        if (!IsRequestDone(uwr))
                        {
                            error_message = uwr.error;
                            Debug.LogWarning("Load Photo Error Message :" + error_message + photo_uri);
                        }
                        else
                        {
                            // Get downloaded asset bundle
                            var texture = downloadHandler.texture;

                            Debug.Log("Completed loaded photo." + photo_uri);
                            //callback sprite
                            onFinish(texture.ToSprite());
                            isSuccess = true;
                            break;
                        }
                        uwr.Dispose();
                    }
#else

					using (WWW www = new WWW(photo_uri))
					{
						float timer = 0;
						bool failed = false;

						while (!www.isDone)
						{
							if (timer > eachTimeOut) { failed = true; break; }
							timer += Time.deltaTime;
							yield return null;
						}

						if (failed)
						{
							error_message = "Time Out.";
							Debug.LogWarning("Try again\nLoad Audio Error Message :" + error_message);
						}
						else if (!string.IsNullOrEmpty(www.error))
						{
							error_message = www.error;
							Debug.LogWarning("Try again\nLoad Audio Error Message :" + error_message);
						}
						else
						{
							Debug.Log("Completed loaded audio." + photo_uri);
							onFinish(www.texture.ToSprite());
							isSuccess = true;
							break;
						}
					}
#endif
                }
                if (!isSuccess)
                {
                    onFinish(null);
                    Debug.LogWarning("Load Photo Error Message :" + error_message + photo_uri);
                }
            }

            yield return null;
            Destroy(this);
        }
        IEnumerator WaitLoadAudio(string uri, AudioType type, int timesToTry, int eachTimeOut, Action<AudioClip> onFinish)
        {
            if (string.IsNullOrEmpty(uri))
            {
                onFinish.SafeInvoke(null);
            }
            else
            {
                //Debug.Log(uri);
                bool isSuccess = false;
                string error_message = null;

                for (int i = 0; i < timesToTry; i++)
                {
                    using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(uri, type))
                    {
                        uwr.timeout = eachTimeOut;
                        yield return uwr.SendWebRequest();
                        if (!IsRequestDone(uwr))
                        {
                            error_message = uwr.error;
                            Debug.LogWarning("Try again\nLoad Audio Error Message :" + error_message);
                        }
                        else
                        {
                            AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                            onFinish.Invoke(clip);
                            isSuccess = true;
                            Debug.Log("Completed loaded audio." + uri);
                            break;
                        }
                    }

                    //using (WWW www = new WWW(uri))
                    //{
                    //    float timer = 0;
                    //    bool failed = false;
                    //    while (!www.isDone)
                    //    {
                    //        if (timer > eachTimeOut) { failed = true; break; }
                    //        timer += Time.deltaTime;
                    //        yield return null;
                    //    }
                    //    if (failed)
                    //    {
                    //        error_message = "Time Out.";
                    //        Debug.LogWarning("Try again\nLoad Audio Error Message :" + error_message);
                    //    }
                    //    else if (!string.IsNullOrEmpty(www.error))
                    //    {
                    //        error_message = www.error;
                    //        Debug.LogWarning("Try again\nLoad Audio Error Message :" + error_message);
                    //    }
                    //    else
                    //    {
                    //        Debug.Log("Completed loaded audio." + uri);
                    //        onFinish(www.GetAudioClip());
                    //        isSuccess = true;
                    //        break;
                    //    }
                    //}

                }
                if (!isSuccess)
                {
                    Debug.LogWarning("Load Audio Error Message :" + error_message + "\n" + uri);
                    onFinish(null);

                }
            }

            yield return null;
            Destroy(this);
        }
        IEnumerator WaitDownload(string _url, Action<float> progress, Action<DownloadHandler> callback)
        {
            if (string.IsNullOrEmpty(_url))
            {
                callback.SafeInvoke(null);
            }
            else
            {
                //Debug.Log(uri);
                bool isSuccess = false;
                string error_message = null;
                var timesToTry = 15;
                for (int i = 0; i < timesToTry; i++)
                {
                    using (UnityWebRequest uwr = UnityWebRequest.Get(_url))
                    {
                        var waitFrame = new WaitForSeconds(0.033f);// 30 fps
                        uwr.SendWebRequest();
                        while (!uwr.isDone)
                        {
                            yield return waitFrame;
                            progress?.Invoke(uwr.downloadProgress);
                        }

#if UNITY_2020_3_OR_NEWER
                        if (uwr.result != UnityWebRequest.Result.Success)
#else
                        if (!uwr.isDone || uwr.isNetworkError || uwr.isHttpError || uwr.isHttpError)
#endif
                        {
                            error_message = uwr.error;
                            Debug.LogWarning("Try again\nLoad Audio Error Message :" + error_message);
                        }
                        else
                        {
                            isSuccess = true;
                            Debug.Log($"<i>Completed loaded data.\n{_url}</i>");
                            callback?.SafeInvoke(uwr.downloadHandler);
                            break;

                        }
                    }
                }
                if (!isSuccess)
                {
                    Debug.LogWarning("Load Audio Error Message :" + error_message + "\n" + _url);
                    callback.SafeInvoke(null);
                }

            }
            yield return null;
            Destroy(this);
        }

        #endregion

        private static bool isAvailable => Application.isPlaying;
#if UNITY_2018 || UNITY_2019_1_OR_NEWER
        private static bool IsRequestDone(UnityWebRequest uwr)
        {
#if UNITY_2020_3_OR_NEWER
            return uwr.result == UnityWebRequest.Result.Success;
#else
            return (!uwr.isNetworkError && !uwr.isHttpError && !uwr.isHttpError);
#endif
        }
#endif


    }
}