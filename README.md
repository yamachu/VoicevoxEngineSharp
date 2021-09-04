# VOICEVOX ENGINE SHARP

[VOICEVOX ENGINE](https://github.com/Hiroshiba/voicevox_engine) の C# 実装。

本家エンジンと Interface で互換性があります。
内部実装に一部差異があるため、出力される音声やクエリの結果が一部異なる場合があります。

## 動作要件

- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
  - preview.7 で動作確認済み
  - 現在 .NET 6.0 はPreviewのため、エディタによっては対応していない場合があります、ご注意ください
    - VSCode + C# Extension 環境下では API の補完が稀に効かなくなったり、補完のための機能がクラッシュします
    - Visual Studio 2022 Preview を推奨
      - Version 17.0.0 Preview 3.1 環境下で IDE からテストを実行すると一部テストがfailedします

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
dotnet build src/API/src/API.csproj ; cd src/API/src/bin/Debug/net6.0 ; dotnet API.dll -- -- use_core
```

その他の引数は VOICEVOX ENGINE に準拠しています
