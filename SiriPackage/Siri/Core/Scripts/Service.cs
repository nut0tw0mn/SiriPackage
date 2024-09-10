using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Siri
{

	public class Services
	{
		public class Etc
		{
			public static bool BetterApproximate(float inputA, float inputB, float tolerance)
			{
				return Mathf.Abs(inputA - inputB) < tolerance;
			}
		}

		public static string DevicePath()
		{
#if UNITY_EDITOR
			return Application.dataPath;
#else
		    return Application.persistentDataPath;
#endif
		}

		public static void WriteJson(string json, string externalPath = "Resources/Save/")
		{

			//UnityEngine.Debug.Log(DevicePath());
			string path = System.IO.Path.Combine(DevicePath(), externalPath);
			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}

			string filePath = path + ".json";
			System.IO.File.WriteAllText(filePath, json);
			Debug.Log(  " is touched, save :\n" + filePath);

		}
		public static void WriteJson(string json, string _name, string externalPath = "Resources/Save/")
		{
			string path = System.IO.Path.Combine(DevicePath(), externalPath);
			string filePath = System.IO.Path.Combine(path, _name + ".json");
			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
            //Debug.Log(json);
			System.IO.File.WriteAllText(filePath, json);
			Debug.Log(_name + " is touched, save :\n" + filePath);
		}
		public static string ReadJson(string _name = "", string externalPath = "Resources/Save/")
		{
			string path = System.IO.Path.Combine(DevicePath(), externalPath);
			string filePath = System.IO.Path.Combine(path, _name + ".json");
			//UnityEngine.Debug.Log(filePath);
			if (!System.IO.File.Exists(filePath))
			{
				if (!System.IO.Directory.Exists(path))
				{
					System.IO.Directory.CreateDirectory(path);
				}
				WriteJson("", _name, externalPath);
			}
			string json = System.IO.File.ReadAllText(filePath);
			//Debug.Log(json);
			return json;
		}
        public static T ReadJson<T>(string _name = "", string externalPath = "Resources/Save/")
        {
            string json = ReadJson(_name, externalPath);
            return JsonUtility.FromJson<T>(json);
        }
        public static void DeleteJson(string name, string externalPath = "Resources/Save/")
		{
			string path = System.IO.Path.Combine(DevicePath(), externalPath);
			string filePath = System.IO.Path.Combine(path, name + ".json");
			if (!System.IO.File.Exists(filePath))
			{
				Debug.LogWarning("File Path not Exists");
			}

			System.IO.File.Delete(filePath);
			Debug.Log("Remove File : " + name + ".Json");
		}


        public static void DeleteDirectoryExternalPath(string externalPath)
		{
			string path = System.IO.Path.Combine(DevicePath(), externalPath);
			DeleteDirectory(path);
		}

		public static void DeleteDirectory(string path)
		{
			if (System.IO.Directory.Exists(path))
			{
				System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);

				foreach (System.IO.FileInfo file in di.GetFiles())
				{
					file.Delete();
				}
				foreach (System.IO.DirectoryInfo dir in di.GetDirectories())
				{
					dir.Delete(true);
				}
				//System.IO.Directory.Delete(externalPath);
			}
		}
		public static void DeleteFileExternalPath(string externalPath)
		{
			string path = System.IO.Path.Combine(DevicePath(), externalPath);
			DeleteFile(path);
		}
		public static void DeleteFile(string path)
		{
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
			}
		}
		public static bool IsExistsFile(string externalPath = "Resources/Save/", string fileName = "")
		{
			string path = System.IO.Path.Combine(DevicePath(), externalPath, fileName);
			//string filePath = System.IO.Path.Combine(path, fileName);
			//Debug.Log("IsExistsFile:"+ path);
			return System.IO.File.Exists(path);
		}

		public static bool IsExistsFile(string filePath)
		{
			string path = System.IO.Path.Combine(DevicePath(), filePath);
			//Debug.Log("IsExistsFile:"+ path);
			return System.IO.File.Exists(path);
		}

		public static void SaveTextureToFile(Texture2D texture, string externalPath = "Resources/Save/", string fileName = "")
		{
            byte[] bytes = fileName.Contains(".png") ? texture.EncodeToPNG() : texture.EncodeToJPG(100);
            var path = SaveByteToFile(bytes, externalPath, fileName);
            //Debug.Log($"SaveTextureToFile Success!\n{path}");
		}

		public static Texture2D LoadTextureFromFile(string externalPath = "Resources/Save/", string fileName = "")
		{
			if (!fileName.Contains("."))
				fileName += ".png";

            byte[] bytes = LoadByteFromFile(externalPath, fileName);

            Texture2D texture = new Texture2D(1, 1);
			texture.LoadImage(bytes);
			return texture;
		}

		public static void SaveAudioClipToFile(AudioClip clip, string externalPath = "Resources/Save/", string fileName = "soundName")
		{
			string path = System.IO.Path.Combine(DevicePath(), externalPath);

			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}

			string filePath = System.IO.Path.Combine(path, fileName + ".wav");
			//byte[] bytes = WavUtility.FromAudioClip(clip);
			//System.IO.File.WriteAllBytes(filePath, bytes);
			SavWav.Save(filePath, clip);
		}

		public static AudioClip LoadAudioClipFromFile(string externalPath = "Resources/Save/", string fileName = "soundName")
		{
            if (!fileName.Contains("."))
                fileName += ".wav";
            
			byte[] bytes = LoadByteFromFile(externalPath, fileName);
            return WavUtility.ToAudioClip(bytes);
			//Debug.Log("Load:" + filePath);
			//using (var www = new WWW("file://" + filePath))
			//{
			//	if (string.IsNullOrEmpty(www.error))
			//	{
			//		Debug.Log("Loaded");
			//		return www.GetAudioClip();

			//	}
			//	else
			//	{
			//		Debug.LogError(www.error);
			//	}
			//}
			//         Debug.LogWarning("LoadNull");
			//return null;
		}

		public static void DeleteAudioClipFile(string externalPath = "Resources/Save/", string fileName = "soundName")
		{
			if (!fileName.Contains("."))
				fileName += ".wav";

			string path = System.IO.Path.Combine(DevicePath(), externalPath);
			string filePath = System.IO.Path.Combine(path, fileName);
			DeleteFile(filePath);
		}

		public static string SaveByteToFile(byte[] bytes, string externalPath, string fileName)
        {
            string path = System.IO.Path.Combine(DevicePath(), externalPath);

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string filePath = System.IO.Path.Combine(path, fileName);

            System.IO.File.WriteAllBytes(filePath, bytes);
            return filePath;
        }

        public static byte[] LoadByteFromFile(string externalPath, string fileName)
        {
            string path = System.IO.Path.Combine(DevicePath(), externalPath);
            string filePath = System.IO.Path.Combine(path, fileName);

            return System.IO.File.ReadAllBytes(filePath);
        }

        public static Coroutine LoadPhoto(GameObject obj, string photo_uri, Action<Sprite> callBack)
		{
			return LoadPhoto(obj, photo_uri, 1, 300, callBack);
		}
		public static Coroutine LoadPhoto(GameObject obj, string photo_uri, int timesToTry = 15, int eachTimeOut = 60, Action<Sprite> callBack = null)
		{
			return obj.AddComponent<Loader>().LoadPhoto(photo_uri, timesToTry, eachTimeOut, callBack);
		}

		public static void LoadAudioClip(GameObject obj, string audio_uri, AudioType type, Action<AudioClip> callBack)
		{
			LoadAudioClip(obj, audio_uri, type, 1, 300, callBack);
		}
		public static void LoadAudioClip(GameObject obj, string audio_uri, AudioType type, int timesToTry = 15, int eachTimeOut = 60, Action<AudioClip> callBack = null)
		{
			obj.AddComponent<Loader>().LoadAudioClip(audio_uri, type, timesToTry, eachTimeOut, callBack);
		}

        public static void Download(GameObject obj, string url, Action<UnityEngine.Networking.DownloadHandler> callback)
        {
            Download(obj,url,null,callback);
        }
        public static void Download(GameObject obj, string url, Action<float> OnProgress, Action<UnityEngine.Networking.DownloadHandler> callback)
        {
            obj.AddComponent<Loader>().Download(url,OnProgress,callback);
        }

#if NET_4_6

		public static void LoadPhotoAsync(string photo_uri, Action<Sprite> callBack)
		{
			LoadPhotoAsync(photo_uri, 1, 300, callBack);
		}
		public static void LoadPhotoAsync(string photo_uri, int timesToTry = 15, int eachTimeOut = 60, Action<Sprite> callBack = null)
		{
			LoadTextureAsync(photo_uri, timesToTry, eachTimeOut, texture => { callBack?.Invoke(texture != null ? texture.ToSprite() : null); });
		}
		public static void LoadTextureAsync(string photo_uri, int timesToTry = 15, int eachTimeOut = 60, Action<Texture2D> callBack = null)
		{
			Loader.LoadPhotoAsync(photo_uri, timesToTry, eachTimeOut, callBack);
		}

		public static void LoadAudioClipAsync(string audio_uri, AudioType type, Action<AudioClip> callBack)
		{
			LoadAudioClipAsync(audio_uri, type, 1, 300, callBack);
		}
		public static void LoadAudioClipAsync(string audio_uri, AudioType type, int timesToTry = 15, int eachTimeOut = 60, Action<AudioClip> callBack = null)
		{
			Loader.LoadAudioClipAsync(audio_uri, type, timesToTry, eachTimeOut, callBack);
		}
		public static void LoadAudioClipParseType(GameObject obj, string audio_uri, int timesToTry = 15, int eachTimeOut = 60, Action<AudioClip> callBack = null)
		{
			var (_, _type) = GetFileNameAndTypeWithUrl(audio_uri);
			AudioType type = GetAudioType(_type);
			LoadAudioClip(obj, audio_uri, type, timesToTry, eachTimeOut, callBack);
		}
		public static void LoadAudioClipParseTypeAsync(string audio_uri, int timesToTry = 15, int eachTimeOut = 60, Action<AudioClip> callBack = null)
		{
			var (_, _type) = GetFileNameAndTypeWithUrl(audio_uri);
			AudioType type = GetAudioType(_type);
			LoadAudioClipAsync(audio_uri, type, timesToTry, eachTimeOut, callBack);
		}
		static AudioType GetAudioType(string _typeName)
		{
			switch (_typeName)
			{
				case "aiff":
					return AudioType.AIFF;
				case "it":
					return AudioType.IT;
				case "mod":
					return AudioType.MOD;
				case "mp3":
				case "mp2":
					return AudioType.MPEG;
				case "ogg":
					return AudioType.OGGVORBIS;
				case "s3m":
					return AudioType.S3M;
				case "wav":
					return AudioType.WAV;
				case "xm":
					return AudioType.XM;
				case "xma":
					return AudioType.XMA;
				case "vag":
					return AudioType.VAG;
				default:
					return AudioType.UNKNOWN;
			}
		}
		public static (string, string) GetFileNameAndTypeWithUrl(string url)
		{
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("Name cannot be null or empty string", nameof(url));
            int indexOf = url.LastIndexOf('/');
			string lastText = url.Substring(indexOf + 1);
			string[] splitType = lastText.Split('.');
			return (splitType[0], splitType[splitType.Length - 1]);
		}
		public static (string, string) URLCompareFull(string url)
		{
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("Name cannot be null or empty string", nameof(url));
            }
            
            int indexOf = url.LastIndexOf('/');
			string path = url.Substring(0, indexOf + 1);
			string shortName = url.Substring(indexOf + 1);

			string s = Regex.Replace(shortName, @"(?<!\d)(\d)(?!\d)", "0$1");
			s = Regex.Replace(s, "[*'\", _ &#^@]", "_");

			return (path, s);
		}
#endif

    }

}