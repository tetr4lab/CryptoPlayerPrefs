#define CRYPTO
#define AUTO_INIT
#define GENERIC_OBJECT_EXTENTION

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CryptoPlayerPrefs : PlayerPrefs {

#if GENERIC_OBJECT_EXTENTION

	public static T GetObject<T> (string key) {
		if (string.IsNullOrEmpty (key) || !HasKey (key)) { return default (T); }
#if UNITY_IPHONE
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif
#if CRYPTO
		using (var stream = new MemoryStream (Crypto.Decrypt (Convert.FromBase64String (PlayerPrefs.GetString (key))))) {
#else
		using (var stream = new MemoryStream (Convert.FromBase64String (PlayerPrefs.GetString (key)))) {
#endif
			return (T) (new BinaryFormatter ()).Deserialize (stream);
		}
	}

	public static void SetObject<T> (string key, T obj) {
		if (obj == null) { return; }
#if UNITY_IPHONE
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif
		using (var stream = new MemoryStream ()) {
			(new BinaryFormatter ()).Serialize (stream, obj);
#if CRYPTO
			PlayerPrefs.SetString (key, Convert.ToBase64String (Crypto.Encrypt (stream.ToArray ())));
#else
			PlayerPrefs.SetString (key, Convert.ToBase64String (stream.ToArray ()));
#endif
		}
	}

#endif

#if CRYPTO
#if AUTO_INIT
	private const string ivPrefskey = "CryptoPlayerPrefsIV";
	private const string keyPrefskey = "CryptoPlayerPrefsKey";

	private static byte [] scramble (byte [] data) {
		for (var i = 0; i < data.Length; i++) {
			var d = ~ data [i];
			data [i] = (byte) (((d & 0x0f) << 4) | ((d & 0xf0) >> 4));
		}
		return data;
	}

	static CryptoPlayerPrefs () {
		if (!Crypto.Inited) {
			if (HasKey (ivPrefskey) && HasKey (keyPrefskey)) {
				Crypto.Init (
					scramble (Convert.FromBase64String (PlayerPrefs.GetString (ivPrefskey))), 
					scramble (Convert.FromBase64String (PlayerPrefs.GetString (keyPrefskey)))
				);
			} else if (Crypto.Init ()) {
				PlayerPrefs.SetString (ivPrefskey, Convert.ToBase64String (scramble (Crypto.IV)));
				PlayerPrefs.SetString (keyPrefskey, Convert.ToBase64String (scramble (Crypto.Key)));
			}
		}
	}
#endif

	public static new string GetString (string key) {
		if (HasKey (key)) {
			var value = PlayerPrefs.GetString (key);
			return string.IsNullOrEmpty (value) ? value : Crypto.Decrypt (value);
		}
		return default (string);
	}

	public static new float GetFloat (string key) {
		var value = default (float);
		if (HasKey (key)) {
			float.TryParse (Crypto.Decrypt (PlayerPrefs.GetString (key)), out value);
		}
		return value;
	}

	public static new int GetInt (string key) {
		var value = default (int);
		if (HasKey (key)) {
			int.TryParse (Crypto.Decrypt (PlayerPrefs.GetString (key)), out value);
		}
		return value;
	}

	public static new void SetString (string key, string value) {
		PlayerPrefs.SetString (key, Crypto.Encrypt (value));
	}

	public static new void SetFloat (string key, float value) {
		PlayerPrefs.SetString (key, Crypto.Encrypt (value.ToString ()));
	}

	public static new void SetInt (string key, int value) {
		PlayerPrefs.SetString (key, Crypto.Encrypt (value.ToString ()));
	}
#endif

}
