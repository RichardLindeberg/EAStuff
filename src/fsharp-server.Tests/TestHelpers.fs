namespace EAArchive.Tests

open System
open System.IO

module TestHelpers =
    let createTempFile (content: string) : string =
        let tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".md")
        File.WriteAllText(tempFile, content)
        tempFile
    
    let cleanupTempFile (path: string) : unit =
        if File.Exists(path) then File.Delete(path)
    
    let createMetadata (fields: (string * obj) list) : Map<string, obj> =
        fields |> Map.ofList
