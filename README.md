# CryptoPlayerPrefs / 簡易なセーブデータの暗号化
Encryption of unity save data  
tags: Unity C#

# 前提
- unity 2018.4.1f1、2018.4.32f1
- `System.Security.Cryptography` BlockSize 128bit, KeySize 256bit, Mode CBC, Padding PKCS7

# できること
- 標準のPlayerPrefsを置き換える形で、セーブデータの暗号化ができます。
- PlayerPrefsを継承するので、クラス名を付け替えるだけで置き換えが完了します。
- 拡張された`GetObject ()`と`SetObject ()`が使えます。

# アセットの入手 (GitHub)
ダウンロード ⇒ [CryptoPlayerPrefs.unitypackage](https://github.com/tetr4lab/CryptoPlayerPrefs/raw/master/CryptoPlayerPreffs.unitypackage)
[ソースはこちらです。](https://github.com/tetr4lab/CryptoPlayerPrefs)

# 導入
- 最小限必要なアセットは、`Crypto.cs`と`CryptoPlayerPrefs.cs`です。
- `Sample.cs`は使用例です。シーン`SampleScene`で挙動を確認できます。

# 簡単な使い方
- `PlayerPrefs`の代わりに`CryptoPlayerPrefs`を使用します。
    - クラス名が異なる以外は同じ使い方です。
- これは、`Sample.cs`で冒頭の`#define INIT_CRYPTO`をコメントアウトした状態です。
- 鍵は自動生成され、簡易なスクランブルを施されてPlayerPrefsに保存されます。
- `CryptoPlayerPrefs.scramble`メソッドを独自のアルゴリズムに変更することで、より安全に管理できます。
- `GetObject ()`、`SetObject ()`を使う対象には、`[Serializable]`アトリビュートの指定が必要です。
    - 具体的には、"Sample.cs"を参照してください。
    - このセーブデータは、.NETのバージョンに依存しており、異なるバージョン間では互換性が保証されません。

# 鍵を自分で管理する
- 独自の鍵を設定してから、`CryptoPlayerPrefs`を使用することも可能です。
    - `Crypto.Init ("[32文字]");`
    - または、`Crypto.Init (byte [32]);`
- `Sample.cs`の使い方だと、鍵はコード中に平文で格納されていますが、画像データの一部を使用するなど、よりトリッキーな方法も考えられます。
- 鍵を自分で管理する場合は、`CryptoPlayerPrefs.cs`冒頭の`#define AUTO_INIT`をコメントアウトすることで、自動初期化のコードを削減できます。
 
# 開発途中はセーブデータの内容を見たい
- `CryptoPlayerPrefs.cs`冒頭の`#define CRYPTO`をコメントアウトすると、暗号化を行いません。
    - 素の`PlayerPrefs`の挙動になります。

### 【参考】#define 触るの面倒なんだけど…
- こちらをお勧めします。 ⇒ [Unityで共通のdefineを定義する【Unity】【C#】](http://kan-kikuchi.hatenablog.com/entry/ScriptingDefineSymbols)  
(いつもありがとうございます。)
