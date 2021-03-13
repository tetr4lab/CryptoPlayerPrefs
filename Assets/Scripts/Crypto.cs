using UnityEngine;
using System;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// BlockSize 128bit (16byte), KeySize 256bit (32byte), Mode CBC, Padding PKCS7
///  エラー時は例外を投げる
/// </summary>
public static class Crypto {

	/// <summary>IVのバイト数</summary>
	public static int IV_Length => Aes.BlockSize / 8;
	/// <summary>Keyのバイト数</summary>
	public static int Key_Length => Aes.KeySize / 8;
	/// <summary>Base64文字列化したIVの文字数</summary>
	public static int B64IV_Length => B64IV.Length;
	/// <summary>Base64文字列化したKeyの文字数</summary>
	public static int B64Key_Length => B64Key.Length;

	/// <summary>シングルトン</summary>
	private static AesCryptoServiceProvider Aes = new AesCryptoServiceProvider ();

	/// <summary>
	/// Base64文字列化したIV
	///  NULLを書き込むと自動生成
	/// </summary>
	public static string B64IV {
		get => Convert.ToBase64String (Aes.IV);
		set {
			if (value == null) {
				Aes.GenerateIV ();
			} else if (value.Length == B64IV_Length) {
				IV = Convert.FromBase64String (value);
			} else {
				throw new ArgumentOutOfRangeException ($"invalid size of iv {value.Length}: {value}");
			}
		}
	}

	/// <summary>
	/// Base64文字列化したKey
	///  NULLを書き込むと自動生成
	/// </summary>
	public static string B64Key {
		get => Convert.ToBase64String (Aes.Key);
		set {
			if (value == null) {
				Aes.GenerateKey ();
			} else if (value.Length == B64Key_Length) {
				Key = Convert.FromBase64String (value);
			} else {
				throw new ArgumentOutOfRangeException ($"invalid size of key {value.Length}: {value}");
			}
		}
	}

	/// <summary>
	/// バイナリのIV
	///  NULLを書き込むと自動生成
	/// </summary>
	public static byte [] IV {
		get => Aes.IV;
		set {
			if (value == null) {
				Aes.GenerateIV ();
			} else {
				if (value.Length != IV_Length) {
					throw new ArgumentOutOfRangeException ($"invalid size of iv {value.Length}: {Convert.ToBase64String (value)}");
				}
				Aes.IV = value;
			}
		}
	}

	/// <summary>
	/// バイナリのKey
	///  NULLを書き込むと自動生成
	/// </summary>
	public static byte [] Key {
		get => (Aes == null) ? null : Aes.Key;
		set {
			if (value == null) {
				Aes.GenerateKey ();
			} else {
				if (value.Length != Key_Length) {
					throw new ArgumentOutOfRangeException ($"invalid size of key {value.Length}: {Convert.ToBase64String (value)}");
				}
				Aes.Key = value;
			}
		}
	}

	/// <summary>Base64文字列による初期化</summary>
	/// <param name="key">Base64文字列化されたkey (渡さなければ新規生成)</param>
	/// <param name="iv">Base64文字列化されたiv (渡さなければ新規生成)</param>
	/// <returns>Base64文字列化されたkey</returns>
	public static string Init (string key, string iv = null) {
		Init ((key == null) ? null : Convert.FromBase64String (key), (iv == null) ? null : Convert.FromBase64String (iv));
		return B64Key;
	}

	/// <summary>バイナリによる初期化</summary>
	/// <param name="key">バイナリのkey (渡さなければ新規生成)</param>
	/// <param name="iv">バイナリのiv (渡さなければ新規生成)</param>
	/// <returns>バイナリのkey</returns>
	public static byte [] Init (byte [] key = null, byte [] iv = null) {
		if (Aes != null) { Aes.Dispose (); }
		Aes = new AesCryptoServiceProvider ();
		(Key, IV) = (key, iv);
		return Key;
	}

	/// <summary>UTF8文字列の暗号化</summary>
	/// <param name="src">UTF8文字列</param>
	/// <param name="iv">Base64文字列化されたiv (渡さなければ新規生成)</param>
	/// <returns>Base64文字列化された暗号化データ</returns>
	public static string Encrypt (string src, string iv = null) {
		return Convert.ToBase64String (Encrypt (Encoding.UTF8.GetBytes (src), (iv == null) ? null : Convert.FromBase64String (iv)));
	}

	/// <summary>バイナリデータの暗号化</summary>
	/// <param name="data">バイナリデータ</param>
	/// <param name="iv">バイナリのiv (渡さなければ新規生成)</param>
	/// <returns>暗号化されたバイナリデータ</returns>
	public static byte [] Encrypt (byte [] data, byte [] iv = null) {
		IV = iv;
		using (var encryptor = Aes.CreateEncryptor ()) {
			return encryptor.TransformFinalBlock (data, 0, data.Length);
		}
	}

	/// <summary>UTF8文字列の復号</summary>
	/// <param name="src">Base64文字列化された暗号化データ</param>
	/// <param name="iv">Base64文字列化されたiv (渡さなければ元のまま)</param>
	/// <returns>UTF8文字列</returns>
	public static string Decrypt (string src, string iv = null) {
		return Encoding.UTF8.GetString (Decrypt (Convert.FromBase64String (src), (iv == null) ? null : Convert.FromBase64String (iv)));
	}

	/// <summary>バイナリデータの復号</summary>
	/// <param name="data">暗号化されたバイナリデータ</param>
	/// <param name="iv">バイナリのiv (渡さなければ元のまま)</param>
	/// <returns>復号されたバイナリデータ</returns>
	public static byte [] Decrypt (byte [] data, byte [] iv = null) {
		if (iv != null) { IV = iv; }
		using (var decryptor = Aes.CreateDecryptor ()) {
			return decryptor.TransformFinalBlock (data, 0, data.Length);
		}
	}

}
