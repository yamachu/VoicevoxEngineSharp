use std::error::Error;
use glob::glob;

fn main() -> Result<(), Box<dyn Error>> {
    let core_added_builder = glob("./voicevox_core/crates/voicevox_core/src/**/*.rs").into_iter()
    .fold(csbindgen::Builder::default(), |builder, paths| {
        paths.into_iter().fold(builder, |builder, path|
            match path {
                Ok(path) => builder.input_extern_file(path),
                Err(_) => builder
            })
    });
    glob("./voicevox_core/crates/voicevox_core_c_api/src/**/*.rs").into_iter()
    .fold(core_added_builder, |builder, paths| {
        paths.into_iter().fold(builder, |builder, path|
            match path {
                Ok(path) => builder.input_extern_file(path),
                Err(_) => builder
            })
    })
        // 追加順依存になるため、自分で依存される側のコードを先に解決する必要がある
        .input_extern_file("./voicevox_core/crates/voicevox_core/src/result_code.rs")
        .input_extern_file("./voicevox_core/crates/voicevox_core_c_api/src/lib.rs")
        .csharp_dll_name("voicevox_core")
        .csharp_class_name("CoreUnmanaged")
        .csharp_namespace("VoicevoxEngineSharp.Core.Native")
        .generate_csharp_file("../src/Native/CoreUnmanaged.g.cs")
        .unwrap();

    Ok(())
}
