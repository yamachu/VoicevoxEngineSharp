use std::error::Error;
use std::fs;

fn main() -> Result<(), Box<dyn Error>> {
    cbindgen::generate("./voicevox_core/crates/voicevox_core_c_api")?
        .write_to_file("./generated/voicevox_core.g.h");

    let generated_source = bindgen::Builder::default()
        .default_enum_style(bindgen::EnumVariation::Rust {
            non_exhaustive: false,
        })
        .header("./generated/voicevox_core.g.h")
        .generate()?
        .to_string()
        // i32なのにu32に変わっている
        .replace("#[repr(u32)]", "#[repr(i32)]")
        // 同名のenumが存在かつput typeでaliasを張っているとcsbindgenでi32に置き換えられてしまうことの対策
        .replace(
            "pub type VoicevoxAccelerationMode = i32;",
            "// pub type VoicevoxAccelerationMode = i32;",
        )
        .replace(
            "pub type VoicevoxResultCode = i32;",
            "// pub type VoicevoxResultCode = i32;",
        );

    fs::write("./generated/voicevox_core.g.rs", generated_source)?;

    csbindgen::Builder::default()
        .input_bindgen_file("./generated/voicevox_core.g.rs")
        // input_extern_file できれいな型が出力されるが
        // voicevox_core_c_apiが依存しているvoicevox_coreのresult_codeを解決できない
        // そのためcbindgenとbindgenから出力されるファイルを使用しbindingを生成している
        // .input_extern_file("./voicevox_core/crates/voicevox_core_c_api/src/lib.rs")
        .csharp_dll_name("voicevox_core")
        .csharp_class_name("CoreUnmanaged")
        .csharp_namespace("VoicevoxEngineSharp.Core.Native")
        .generate_csharp_file("../src/Native/CoreUnmanaged.g.cs")
        .unwrap();

    Ok(())
}
