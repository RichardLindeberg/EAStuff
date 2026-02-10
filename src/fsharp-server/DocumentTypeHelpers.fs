namespace EAArchive

open System

module DocumentTypeHelpers =

    let getGovernanceDocTypeFromPath (filePath: string) : GovernanceDocType =
        let lower = filePath.ToLowerInvariant()
        if lower.Contains("\\policies\\") || lower.Contains("/policies/") then
            GovernanceDocType.Policy
        elif lower.Contains("\\instructions\\") || lower.Contains("/instructions/") then
            GovernanceDocType.Instruction
        elif lower.Contains("\\manuals\\") || lower.Contains("/manuals/") then
            GovernanceDocType.Manual
        elif lower.Contains("ms-policy-") then
            GovernanceDocType.Policy
        elif lower.Contains("ms-instruction-") then
            GovernanceDocType.Instruction
        elif lower.Contains("ms-manual-") then
            GovernanceDocType.Manual
        else
            GovernanceDocType.Unknown "unknown"
