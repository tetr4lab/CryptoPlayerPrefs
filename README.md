# CryptoPlayerPrefs / 簡易なセーブデータの暗号化
Encryption of unity save data  
tags: Unity C#

# 前提
- unity 2018.4.1f1、2018.4.32f1
- `System.Security.Cryptography` BlockSize 128bit, KeySize 256bit, Mode CBC, Padding PKCS7

# できること
- 標準のPlayerPrefsを置き換える形で、セーブデータの暗号化ができます。
- PlayerPrefsを継承するので、クラス名を付け替えるだけで置き換えが完了します。
- クラスオブジェクトを扱うことが可能です。(`GetObject<T> ()`、`SetObject<T> ()`)

# 導入
- 最小限必要なアセットは、`Crypto.cs`と`CryptoPlayerPrefs.cs`です。
- `Sample.cs`は使用例です。シーン`SampleScene`で挙動を確認できます。

# 簡単な使い方
- `PlayerPrefs`の代わりに`CryptoPlayerPrefs`を使用します。
    - クラス名が異なる以外は同じ使い方です。
- 鍵は自動生成することも、任意に定めることもできます。
    - `Sample.cs`には、切り替えスイッチがあります。
- 鍵は、何処かに保存しておく必要があります。例えば…
    - ソース中でハードコーディングする
    - 難読化して`PlayerPrefs`でセーブする
    - Fixした画像データの一部を使用する
- `GetObject<T> ()`、`SetObject<T> ()`を使う対象には、`[Serializable]`アトリビュートの指定が必要です。
    - 具体的には、`Sample.cs`を参照してください。
    - このセーブデータは、.NETのバージョンに依存しており、異なるバージョン間では互換性が保証されません。


# 開発途中はセーブデータの内容を見たい
- `CryptoPlayerPrefs.cs`冒頭の`#define CRYPTO`をコメントアウトすると、暗号化を行いません。
    - 素の`PlayerPrefs`+オブジェクト拡張の挙動になります。

### 【参考】#define 触るの面倒なんだけど…
- こちらをお勧めします。 ⇒ [Unityで共通のdefineを定義する【Unity】【C#】](http://kan-kikuchi.hatenablog.com/entry/ScriptingDefineSymbols)  
(いつもお世話になっております。)
