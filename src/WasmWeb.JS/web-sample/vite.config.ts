import { defineConfig } from "vite";
import wasm from "vite-plugin-wasm";
import react from "@vitejs/plugin-react";
import { copyFile, glob, mkdir, stat } from "node:fs/promises";
import { dirname, join } from "node:path";

// Custom plugin to copy framework files to public directory
function copyFrameworkFiles() {
  return {
    name: "copy-framework-files",
    async buildStart() {
      const sourcePath = "../../WasmWeb/src/bin/Debug/net10.0/wwwroot/";
      const targetPath = "./public/";

      try {
        // Ensure target directory exists
        await mkdir(targetPath, { recursive: true });

        // Get all files using glob
        const files = glob("**/*", {
          cwd: sourcePath,
        });

        const copyPromises = [];
        for await (const file of files) {
          const sourceFile = join(sourcePath, file);

          // Check if it's a file (not a directory)
          const stats = await stat(sourceFile);
          if (!stats.isFile()) {
            continue;
          }

          const targetFile = join(targetPath, file);

          // ディレクトリ作成とファイルコピーを非同期で実行
          copyPromises.push(
            mkdir(dirname(targetFile), { recursive: true }).then(() =>
              copyFile(sourceFile, targetFile)
            )
          );
        }

        await Promise.all(copyPromises);
        console.log(
          `✓ Copied ${copyPromises.length} files from ${sourcePath} to ${targetPath}`
        );
      } catch (e) {
        const message = e instanceof Error ? e.message : String(e);
        console.error(`⚠️  Error copying framework files: ${message}`);
        console.warn(`⚠️  framework source path not found: ${sourcePath}`);
        console.warn(
          '   Please run "dotnet build" in framework directory first'
        );
      }
    },
  };
}

// https://vite.dev/config/
export default defineConfig({
  // https://github.com/microsoft/onnxruntime/issues/19556#issuecomment-2681823775
  assetsInclude: ["**/*.onnx"],
  optimizeDeps: {
    exclude: ["onnxruntime-web"],
  },
  plugins: [wasm(), copyFrameworkFiles(), react()],
  resolve: {
    alias: {
      // Resolve _framework paths to public directory in dev mode
      "./_framework": "/public/_framework",
    },
  },
  build: {
    assetsDir: "",
    rollupOptions: {
      // DO NOT bundle dotnet runtime files
      external: [/^\.\/_framework\//],
      output: {
        paths: (id) => {
          // Keep _framework paths as-is in build output
          if (id.startsWith("./_framework/")) {
            return id;
          }
          return id;
        },
      },
      makeAbsoluteExternalsRelative: false,
    },
  },
});
