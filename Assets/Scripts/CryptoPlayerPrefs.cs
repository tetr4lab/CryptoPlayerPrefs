#define CRYPTO
#define GENERIC_OBJECT_EXTENTION

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// PlayerPrefsのラッパー
///  シンボルCRYPTOが有効なら暗号化する
///  シンボルGENERIC_OBJECT_EXTENTIONが有効なら、SetObject<T>とGetObject<T>で、[Serializable]なクラスを扱う (CRYPTO無効時も使用可)
///  エラー時の挙動はPlayerPrefsに準拠する
///  データの破損を検出した場合や、一部のエラーでは例外を投げる
/// </summary>
public class CryptoPlayerPrefs : PlayerPrefs {

#if CRYPTO
	/// <summary>文字列化したハッシュの長さ</summary>
	private const int hashStrLength = 128 / 8 * 2;
	
	/// <summary>キーを受け取ってハッシュと照合済みの文字列を返す</summary>
	private static string getStringWithHash (string key) {
		var cstr = PlayerPrefs.GetString (key);
		var dstr = cstr.Substring (hashStrLength);
		var hash_saved = Hash128.Parse (cstr.Substring (0, hashStrLength));
		var hash_computed = Hash128.Compute (dstr);
		if (hash_saved.isValid && hash_saved == hash_computed) {
			return dstr;
		}
		throw new PlayerPrefsException ($"hash mismatch saved:'{hash_saved}', computed:'{hash_computed}'");
	}

	/// <summary>キーと文字列を受け取って、ハッシュとともに保存する</summary>
	private static void setStringWithHash (string key, string value) {
		PlayerPrefs.SetString (key, $"{Hash128.Compute (value)}{value}");
	}
#endif

#if GENERIC_OBJECT_EXTENTION

#if UNITY_IOS
	/// <summary>初期設定</summary>
	static CryptoPlayerPrefs () {
		Environment.SetEnvironmentVariable ("MONO_REFLECTION_SERIALIZER", "yes");
	}
#endif

	/// <summary>オブジェクトの取得</summary>
	public static T GetObject<T> (string key) {
		if (!HasKey (key)) { return default (T); }
#if CRYPTO
		var str = getStringWithHash (key);
		using (var stream = new MemoryStream (Crypto.Decrypt (Convert.FromBase64String (str.Substring (Crypto.B64IV_Length)), Convert.FromBase64String (str.Substring (0, Crypto.B64IV_Length))))) {
#else
		using (var stream = new MemoryStream (Convert.FromBase64String (PlayerPrefs.GetString (key)))) {
#endif
			return (T) (new BinaryFormatter ()).Deserialize (stream);
		}
	}

	/// <summary>オブジェクト保存</summary>
	public static void SetObject<T> (string key, T obj) {
		if (obj == null) { return; }
		using (var stream = new MemoryStream ()) {
			(new BinaryFormatter ()).Serialize (stream, obj);
#if CRYPTO
			var cdat = Crypto.Encrypt (stream.ToArray ());
			setStringWithHash (key, $"{Crypto.B64IV}{Convert.ToBase64String (cdat)}");
#else
			PlayerPrefs.SetString (key, Convert.ToBase64String (stream.ToArray ()));
#endif
		}
	}

#endif

#if CRYPTO
	/// <summary>文字列の取得</summary>
	public static new string GetString (string key) {
		if (!HasKey (key)) { return default; }
		var str = getStringWithHash (key);
		return string.IsNullOrEmpty (str) ? str : Crypto.Decrypt (str.Substring (Crypto.B64IV_Length), str.Substring (0, Crypto.B64IV_Length));
	}

	/// <summary>浮動小数点数の取得</summary>
	public static new float GetFloat (string key) {
		if (!HasKey (key)) { return default; }
		var str = getStringWithHash (key);
		return float.Parse (Crypto.Decrypt (str.Substring (Crypto.B64IV_Length), str.Substring (0, Crypto.B64IV_Length)));
	}

	/// <summary>整数の取得</summary>
	public static new int GetInt (string key) {
		if (!HasKey (key)) { return default; }
		var str = getStringWithHash (key);
		return int.Parse (Crypto.Decrypt (str.Substring (Crypto.B64IV_Length), str.Substring (0, Crypto.B64IV_Length)));
	}

	/// <summary>文字列の保存</summary>
	public static new void SetString (string key, string value) {
		var cstr = Crypto.Encrypt (value);
		setStringWithHash (key, $"{Crypto.B64IV}{cstr}");
	}

	/// <summary>浮動小数点数の保存</summary>
	public static new void SetFloat (string key, float value) {
		var cstr = Crypto.Encrypt (value.ToString ());
		setStringWithHash (key, $"{Crypto.B64IV}{cstr}");
	}

	/// <summary>整数の保存</summary>
	public static new void SetInt (string key, int value) {
		var cstr = Crypto.Encrypt (value.ToString ());
		setStringWithHash (key, $"{Crypto.B64IV}{cstr}");
	}
#endif

}
