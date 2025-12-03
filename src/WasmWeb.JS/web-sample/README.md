# WasmWeb.JS Web Sample

1. Store ONNX Models to /public/models

```
/public/models
  ├── duration.onnx
  ├── intonation.onnx
  ├── spectrogram.onnx
  └── vocoder.onnx
```

This model files can be created by following instructions in https://github.com/Hiroshiba/vv_core_inference?tab=readme-ov-file#%E3%83%A2%E3%83%87%E3%83%AB%E3%82%92-onnx-%E3%81%AB%E5%A4%89%E6%8F%9B .

2. Store OpenJTalk dictionary tar.gz file to /public

```
/public/open_jtalk_dic_utf_8-1.11.tgz
```

original file name is `open_jtalk_dic_utf_8-1.11.tar.gz`, but rename it to `open_jtalk_dic_utf_8-1.11.tgz` to avoid vite web-server serving issue.
