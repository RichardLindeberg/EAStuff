namespace EAArchive.Tests

open System
open System.IO
open Microsoft.Extensions.Logging
open EAArchive

module TestHelpers =
    let createTempFile (content: string) : string =
        let tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".md")
        File.WriteAllText(tempFile, content)
        tempFile
    
    let cleanupTempFile (path: string) : unit =
        if File.Exists(path) then File.Delete(path)
    
    let createMetadata (fields: (string * obj) list) : Map<string, obj> =
        fields |> Map.ofList

    let createTempRepository
        (archimateFiles: (string * string) list)
        (governanceFiles: (string * string) list)
        : DocumentRepository * string =
        let rootDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())
        let archimatePath = Path.Combine(rootDir, "archimate")
        let managementPath = Path.Combine(rootDir, "management-system")

        Directory.CreateDirectory(archimatePath) |> ignore
        Directory.CreateDirectory(managementPath) |> ignore

        let writeFile (basePath: string) (fileName: string, content: string) : unit =
            let filePath = Path.Combine(basePath, fileName)
            let dir = Path.GetDirectoryName(filePath)
            if not (String.IsNullOrWhiteSpace dir) then
                Directory.CreateDirectory(dir) |> ignore
            File.WriteAllText(filePath, content)

        archimateFiles |> List.iter (writeFile archimatePath)
        governanceFiles |> List.iter (writeFile managementPath)

        let loggerFactory = LoggerFactory.Create(fun _ -> ())
        let logger = loggerFactory.CreateLogger("Test")
        let repo = DocumentRepositoryLoader.loadRepository archimatePath managementPath logger

        repo, rootDir

    let cleanupTempDirectory (path: string) : unit =
        if Directory.Exists(path) then
            Directory.Delete(path, true)
