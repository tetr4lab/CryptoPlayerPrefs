#define CRYPTO
#define AUTO_INIT

using System;
using UnityEngine;

public class CryptoPlayerPrefs : PlayerPrefs {

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

	private static void init () {
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
#if AUTO_INIT
			init ();
#endif
			var value = PlayerPrefs.GetString (key);
			return string.IsNullOrEmpty (value) ? value : Crypto.Decrypt (value);
		}
		return default (string);
	}

	public static new float GetFloat (string key) {
		var value = default (float);
		if (HasKey (key)) {
#if AUTO_INIT
			init ();
#endif
			float.TryParse (Crypto.Decrypt (PlayerPrefs.GetString (key)), out value);
		}
		return value;
	}

	public static new int GetInt (string key) {
		var value = default (int);
		if (HasKey (key)) {
#if AUTO_INIT
			init ();
#endif
			int.TryParse (Crypto.Decrypt (PlayerPrefs.GetString (key)), out value);
		}
		return value;
	}

	public static new void SetString (string key, string value) {
#if AUTO_INIT
			init ();
#endif
		PlayerPrefs.SetString (key, Crypto.Encrypt (value));
	}

	public static new void SetFloat (string key, float value) {
#if AUTO_INIT
			init ();
#endif
		PlayerPrefs.SetString (key, Crypto.Encrypt (value.ToString ()));
	}

	public static new void SetInt (string key, int value) {
#if AUTO_INIT
			init ();
#endif
		PlayerPrefs.SetString (key, Crypto.Encrypt (value.ToString ()));
	}
#endif

}
