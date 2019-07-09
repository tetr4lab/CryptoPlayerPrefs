using System;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// BlockSize 128bit (16byte), KeySize 256bit (32byte), Mode CBC, Padding PKCS7
/// </summary>
public static class Crypto {

	private static AesCryptoServiceProvider Aes;
	public static bool Inited { get { return Aes != null; } }
	public static string strIV { get { return (Aes == null) ? null : Encoding.UTF8.GetString (Aes.IV); } }
	public static string strKey { get { return (Aes == null) ? null : Encoding.UTF8.GetString (Aes.Key); } }
	public static byte [] IV { get { return (Aes == null) ? null : Aes.IV; } }
	public static byte [] Key { get { return (Aes == null) ? null : Aes.Key; } }
	private static ICryptoTransform encryptor;
	private static ICryptoTransform decryptor;

	public static bool Init (byte [] iv = null, byte [] key = null) {
		if (Aes != null) {
			Aes.Dispose ();
			Aes = null;
		}
		using (var aes = new AesCryptoServiceProvider ()) {
			Aes = aes;
		}
		if (Aes != null) {
			if (iv != null) {
				if (iv.Length * 8 != Aes.BlockSize) {
					throw new ArgumentOutOfRangeException ("invalid size of iv");
				}
				Aes.IV = iv;
			}
			if (key != null) {
				if (key.Length * 8 != Aes.KeySize) {
					throw new ArgumentOutOfRangeException ("invalid size of key");
				}
				Aes.Key = key;
			}
			if (encryptor != null) { encryptor.Dispose (); }
			encryptor = Aes.CreateEncryptor ();
			if (decryptor != null) { decryptor.Dispose (); }
			decryptor = Aes.CreateDecryptor ();
			return true;
		}
		return false;
	}

	public static void Init (string iv, string key) {
		Init ((iv == null) ? null : Encoding.UTF8.GetBytes (iv), (key == null) ? null : Encoding.UTF8.GetBytes (key));
	}

	public static string Encrypt (string src) {
		if (Aes == null || string.IsNullOrEmpty (src)) { return null; }
		try {
			var data = Encoding.UTF8.GetBytes (src);
			return Convert.ToBase64String (encryptor.TransformFinalBlock (data, 0, data.Length));
		} catch {
			return null;
		}
	}

	public static byte [] Encrypt (byte [] data) {
		if (Aes == null || data == null) { return null; }
		try {
			return encryptor.TransformFinalBlock (data, 0, data.Length);
		}
		catch {
			return null;
		}
	}

	public static string Decrypt (string src) {
		if (Aes == null || string.IsNullOrEmpty (src)) { return null; }
		try {
			var data = Convert.FromBase64String (src);
			return Encoding.UTF8.GetString (decryptor.TransformFinalBlock (data, 0, data.Length));
		}
		catch {
			return null;
		}
	}

	public static byte [] Decrypt (byte [] data) {
		if (Aes == null || data == null) { return null; }
		try {
			return decryptor.TransformFinalBlock (data, 0, data.Length);
		} catch {
			return null;
		}
	}

}
