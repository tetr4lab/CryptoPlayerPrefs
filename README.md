# CryptoPlayerPrefs / �ȈՂȃZ�[�u�f�[�^�̈Í���
Encryption of unity save data  
tags: Unity C#

# �O��
- unity 2018.4.1f1�A2018.4.32f1
- `System.Security.Cryptography` BlockSize 128bit, KeySize 256bit, Mode CBC, Padding PKCS7

# �ł��邱��
- �W����PlayerPrefs��u��������`�ŁA�Z�[�u�f�[�^�̈Í������ł��܂��B
- PlayerPrefs���p������̂ŁA�N���X����t���ւ��邾���Œu���������������܂��B
- �g�����ꂽ`GetObject ()`��`SetObject ()`���g���܂��B

# �A�Z�b�g�̓��� (GitHub)
�_�E�����[�h �� [CryptoPlayerPrefs.unitypackage](https://github.com/tetr4lab/CryptoPlayerPrefs/raw/master/CryptoPlayerPreffs.unitypackage)
[�\�[�X�͂�����ł��B](https://github.com/tetr4lab/CryptoPlayerPrefs)

# ����
- �ŏ����K�v�ȃA�Z�b�g�́A`Crypto.cs`��`CryptoPlayerPrefs.cs`�ł��B
- `Sample.cs`�͎g�p��ł��B�V�[��`SampleScene`�ŋ������m�F�ł��܂��B

# �ȒP�Ȏg����
- `PlayerPrefs`�̑����`CryptoPlayerPrefs`���g�p���܂��B
    - �N���X�����قȂ�ȊO�͓����g�����ł��B
- ����́A`Sample.cs`�Ŗ`����`#define INIT_CRYPTO`���R�����g�A�E�g������Ԃł��B
- ���͎�����������A�ȈՂȃX�N�����u�����{�����PlayerPrefs�ɕۑ�����܂��B
- `CryptoPlayerPrefs.scramble`���\�b�h��Ǝ��̃A���S���Y���ɕύX���邱�ƂŁA�����S�ɊǗ��ł��܂��B
- `GetObject ()`�A`SetObject ()`���g���Ώۂɂ́A`[Serializable]`�A�g���r���[�g�̎w�肪�K�v�ł��B
    - ��̓I�ɂ́A"Sample.cs"���Q�Ƃ��Ă��������B
    - ���̃Z�[�u�f�[�^�́A.NET�̃o�[�W�����Ɉˑ����Ă���A�قȂ�o�[�W�����Ԃł͌݊������ۏ؂���܂���B

# ���������ŊǗ�����
- �Ǝ��̌���ݒ肵�Ă���A`CryptoPlayerPrefs`���g�p���邱�Ƃ��\�ł��B
    - `Crypto.Init ("[32����]");`
    - �܂��́A`Crypto.Init (byte [32]);`
- `Sample.cs`�̎g�������ƁA���̓R�[�h���ɕ����Ŋi�[����Ă��܂����A�摜�f�[�^�̈ꕔ���g�p����ȂǁA���g���b�L�[�ȕ��@���l�����܂��B
- ���������ŊǗ�����ꍇ�́A`CryptoPlayerPrefs.cs`�`����`#define AUTO_INIT`���R�����g�A�E�g���邱�ƂŁA�����������̃R�[�h���팸�ł��܂��B
 
# �J���r���̓Z�[�u�f�[�^�̓��e��������
- `CryptoPlayerPrefs.cs`�`����`#define CRYPTO`���R�����g�A�E�g����ƁA�Í������s���܂���B
    - �f��`PlayerPrefs`�̋����ɂȂ�܂��B

### �y�Q�l�z#define �G��̖ʓ|�Ȃ񂾂��ǁc
- ������������߂��܂��B �� [Unity�ŋ��ʂ�define���`����yUnity�z�yC#�z](http://kan-kikuchi.hatenablog.com/entry/ScriptingDefineSymbols)  
(�������肪�Ƃ��������܂��B)
