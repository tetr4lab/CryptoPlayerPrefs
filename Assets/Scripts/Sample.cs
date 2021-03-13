using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>暗号化と復号の実施例</summary>
public class Sample : MonoBehaviour {

	/// <summary>初期実行</summary>
	void Start () {

		// 初期化
#if False
		// キーを任意に定める場合
		var Key = new byte [32]; // キーは32バイト
		(new System.Random ()).NextBytes (Key); // 乱数で埋める (例)
		Crypto.Init (Key);
#else
		// キーを自動生成する場合
		var Key = Crypto.Init ();
#endif

		// セーブ用データ
		var Number = 4096;
		var Value = 1.41421356f;
		var Name = "tetr4lab.";
		var Items = new ItemDict (new string [] { "a", "b", "c", "d", "e", "f" }); // 生成

		// セーブ
		CryptoPlayerPrefs.SetInt ("Number", Number);
		CryptoPlayerPrefs.SetFloat ("Value", Value);
		CryptoPlayerPrefs.SetString ("Name", Name);
		CryptoPlayerPrefs.SetObject<ItemDict> ("Items", Items);
		Debug.Log ($"saved... Number = {Number}, Value = {Value}, Name = {Name}, Items = {Items}");

		// ここでアプリが終了し、次回起動後に再初期化されたものとする
		Crypto.Init (Key);

		// ロード
		Number = CryptoPlayerPrefs.GetInt ("Number");
		Value = CryptoPlayerPrefs.GetFloat ("Value");
		Name = CryptoPlayerPrefs.GetString ("Name");
		Items = CryptoPlayerPrefs.GetObject<ItemDict> ("Items");
		Debug.Log ($"loaded... Number = {Number}, Value = {Value}, Name = {Name}, Items = {Items}");

	}

}

// オブジェクト保存のテスト用クラス
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
