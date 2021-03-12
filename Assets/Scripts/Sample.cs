#define INIT_CRYPTO

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sample : MonoBehaviour {

	int Number;
	float Value;
	string Name;
	ItemDict Items;

	void Start () {

#if INIT_CRYPTO
		Crypto.Init ("#:CP$-/Tk:d5-exf", "H:CNA+3ZaLLUJwpPcHC@2dV3XfhrUGN-");
#endif
		// ロード
		Number = CryptoPlayerPrefs.GetInt ("Number");
		Value = CryptoPlayerPrefs.GetFloat ("Value");
		Name = CryptoPlayerPrefs.GetString ("Name");
		Items = CryptoPlayerPrefs.GetObject<ItemDict> ("Items");
		Debug.Log ($"loaded... Number = {Number}, Value = {Value}, Name = {Name}, Items = {Items}");
		
		// 初期化
		Number = 4096;
		Value = 1.41421356f;
		Name = "tetr4lab.";
		Items = new ItemDict (new string [] { "a", "b", "c", "d", "e", "f" }); // 生成

		// セーブ
		CryptoPlayerPrefs.SetInt ("Number", Number);
		CryptoPlayerPrefs.SetFloat ("Value", Value);
		CryptoPlayerPrefs.SetString ("Name", Name);
		CryptoPlayerPrefs.SetObject<ItemDict> ("Items", Items);
		Debug.Log ($"saved... Number = {Number}, Value = {Value}, Name = {Name}, Items = {Items}");
	}

}

[Serializable]
public class ItemDict {
	public int this [Item i] { get { return Items [i]; } }
	public Dictionary<Item, int> Items;

	public ItemDict (ICollection<string> names) {
		Items = new Dictionary<Item, int> { };
		foreach (var name in names) {
			Items.Add (new Item (name), Random.Range (0, 100));
		}
	}

	public override string ToString () {
		var items = new List<KeyValuePair<Item, int>> { };
		foreach (var item in Items) {
			items.Add (item);
		}
		return $"\"Items\":[ {string.Join (",", items.ConvertAll (item => $"{item.Key}:{item.Value}"))}]";
	}
}

[Serializable]
public class Item {

	public string Name;
	public Guid Id;

	public Item (string name) {
		Id = Guid.NewGuid ();
		this.Name = name ?? Id.ToString ();
	}

	public override string ToString () {
		return $"{{\"name\":\"{Name}\", \"id\":\"{Id}\"}}";
	}

}
