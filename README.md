# VOICEVOX ENGINE SHARP

[VOICEVOX ENGINE](https://github.com/Hiroshiba/voicevox_engine) の C# 実装。

本家エンジンと Interface で互換性があります。
内部実装に一部差異があるため、出力される音声やクエリの結果が一部異なる場合があります。

## 最低動作要件

- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)

## 開発状況

0.14.x の VOICEVOX CORE に追従する大規模な変更を https://github.com/yamachu/VoicevoxEngineSharp/tree/nextCore

で実施しています

## 準備

### VOICEVOX COREを使用する場合

1. https://github.com/Hiroshiba/voicevox_core に記載されている依存ライブラリと CORE をダウンロード
2. [open_jtalk_dic_utf_8-1.11.tar.gz](https://downloads.sourceforge.net/open-jtalk/open_jtalk_dic_utf_8-1.11.tar.gz) をダウンロードし展開
3. 1, 2 でダウンロードおよび展開した .dll や .so ファイル、ならびに open_jtalk_dic_utf_8-1.11 ディレクトリを src/Core/resources ディレクトリに配置

### 製品版の VOICEVOX を流用する場合

事前準備なし

## 実行

```
# 製品版の VOICEVOX を流用する場合
dotnet run --project src/API/src -- --voicevox_dir "C:/path/to/voicevox"

# VOICEVOX CORE を使用する場合
dotnet build src/API/src/API.csproj ; cd src/API/src/bin/Debug/net6.0 ; dotnet API.dll
```

その他の引数は VOICEVOX ENGINE に準拠しています

## 注意事項

### 稀に生成される音声がノイズだらけになって聞き取れない

Engine の再起動をお試しください。

### 実装されていない機能がある

音声合成に直接関係しないUtil的な機能が未実装です

- [ ] モノラルステレオ選択 (https://github.com/Hiroshiba/voicevox_engine/pull/39)
- [ ] サンプリングレートの変更 (https://github.com/Hiroshiba/voicevox_engine/pull/33)

などの機能が未実装

Welcome contribute!

## ライセンス

本家 [VOICEVOX ENGINE](https://github.com/Hiroshiba/voicevox_engine) のライセンスを継承し、LGPL v3
