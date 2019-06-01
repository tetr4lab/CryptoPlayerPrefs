#define INIT_CRYPTO

using UnityEngine;

public class Sample : MonoBehaviour {

	int Number;
	float Value;
	string Name;

	void Start () {

#if INIT_CRYPTO
		Crypto.Init ("#:CP$-/Tk:d5-exf", "H:CNA+3ZaLLUJwpPcHC@2dV3XfhrUGN-");
#endif

		Number = CryptoPlayerPrefs.GetInt ("Number");
		Value = CryptoPlayerPrefs.GetFloat ("Value");
		Name = CryptoPlayerPrefs.GetString ("Name");
		Debug.Log ($"loaded... Number = {Number}, Value = {Value}, Name = {Name}");
		
		Number = 4096;
		Value = 1.41421356f;
		Name = "tetr4lab.";
		CryptoPlayerPrefs.SetInt ("Number", Number);
		CryptoPlayerPrefs.SetFloat ("Value", Value);
		CryptoPlayerPrefs.SetString ("Name", Name);
		Debug.Log ($"saved... Number = {Number}, Value = {Value}, Name = {Name}");
	}

}
