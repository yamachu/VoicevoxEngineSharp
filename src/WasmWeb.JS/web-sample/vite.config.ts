import react from "@vitejs/plugin-react";
import dotnetWasm from "@yamachu/vite-plugin-dotnet-wasm";
import { defineConfig } from "vite";
import wasm from "vite-plugin-wasm";

// https://vite.dev/config/
export default defineConfig({
  // https://github.com/microsoft/onnxruntime/issues/19556#issuecomment-2681823775
  assetsInclude: ["**/*.onnx"],
  optimizeDeps: {
    exclude: ["onnxruntime-web"],
  },
  plugins: [
    wasm(),
    dotnetWasm({
      projectPath: "../../WasmWeb/src/WasmWeb.csproj",
    }),
    react(),
  ],
});
