use std::error::Error;

fn main() -> Result<(), Box<dyn Error>> {
    bindgen::Builder::default()
        .clang_args(&["-x", "c++"])
        .header("voicevox_core/crates/voicevox_core_c_api/include/voicevox_core.h")
        .default_enum_style(bindgen::EnumVariation::Rust {
            non_exhaustive: false,
        })
        .generate()
        .unwrap()
        .write_to_file("./generated/voicevox_core.g.rs")
        .unwrap();

    csbindgen::Builder::default()
        .input_bindgen_file("generated/voicevox_core.g.rs")
        .csharp_dll_name("voicevox_core")
        .csharp_class_name("CoreUnsafe")
        .csharp_namespace("VoicevoxEngineSharp.Core.Native")
        // abortっていうのが勝手に足されてしまうので、スキップしている
        .method_filter(|method| !method.starts_with("abort"))
        // TODO: MAUI iOS
        // see: https://github.com/xamarin/xamarin-macios/issues/17418
        .csharp_dll_name_if("UNITY_IOS && !UNITY_EDITOR", "__Internal")
        .generate_to_file(
            "generated/voicevox_core_ffi.g.rs",
            "../src/Native/CoreUnsafe.g.cs",
        )
        .unwrap();

    // let core_added_builder = glob("./voicevox_core/crates/voicevox_core/src/**/*.rs").into_iter()
    // .fold(csbindgen::Builder::default(), |builder, paths| {
    //     paths.into_iter().fold(builder, |builder, path|
    //         match path {
    //             Ok(path) => builder.input_extern_file(path),
    //             Err(_) => builder
    //         })
    // });
    // glob("./voicevox_core/crates/voicevox_core_c_api/src/**/*.rs").into_iter()
    // .fold(core_added_builder, |builder, paths| {
    //     paths.into_iter().fold(builder, |builder, path|
    //         match path {
    //             Ok(path) => builder.input_extern_file(path),
    //             Err(_) => builder
    //         })
    // })
    //     // 追加順依存になるため、自分で依存される側のコードを先に解決する必要がある
    //     .input_extern_file("./voicevox_core/crates/voicevox_core/src/result_code.rs")
    //     .input_extern_file("./voicevox_core/crates/voicevox_core_c_api/src/lib.rs")
    //     .csharp_dll_name("voicevox_core")
    //     .csharp_class_name("CoreUnmanaged")
    //     .csharp_namespace("VoicevoxEngineSharp.Core.Native")
    //     .generate_csharp_file("../src/Native/CoreUnmanaged.g.cs")
    //     .unwrap();

    Ok(())
}
