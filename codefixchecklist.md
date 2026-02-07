# Code Fix Checklist

Detailed backlog of follow-up fixes and improvements.

## 1) Centralize Asset Root Paths

**Issue**
Diagram asset discovery uses hardcoded runtime paths (current directory and base directory). This can break when running as a service, from tests, or after publish. It also conflicts with the goal of having all paths defined in appsettings.

**Why it matters**
- Path resolution becomes fragile across environments.
- Makes deployments inconsistent and harder to reason about.

**Suggested approach**
- Add appsettings keys for asset roots (for example, `EAArchive:AssetsRoot` or separate `SymbolsPath` and `IconsPath`).
- Resolve these via `ContentRootPath` or `WebRootPath` in `Program.fs` and pass them into the diagram generator module.
- Remove calls to `Directory.GetCurrentDirectory()` and `AppContext.BaseDirectory` for asset paths.

**Acceptance**
- All asset paths come from appsettings.
- Diagram rendering works in dev and publish without depending on working directory.

**Status**
Completed (2026-02-07). Asset paths and base URLs are now configured in appsettings and injected into diagram generation.

## 2) Clarify Data Path for Production

**Issue**
`EAArchive:ElementsPath` is set to a relative repo-based location. This is likely to fail in production or publish output where the repo layout is not present.

**Why it matters**
- App startup failure when the data folder is not located relative to the published app.
- Confusing environment behavior.

**Suggested approach**
- Use environment-specific appsettings files:
  - `appsettings.Development.json` for repo-relative paths.
  - `appsettings.Production.json` for absolute paths or mounted volume paths.
- Document required appsettings for production.

**Acceptance**
- App starts cleanly in development and in publish output with explicit config.

**Status**
Completed (2026-02-07). Added environment-specific appsettings for development and production data paths.

## 3) Decide on Data Bundling in Publish Output

**Issue**
The project file no longer copies `data/archimate` into output. If the app is expected to be self-contained, this is a regression.

**Why it matters**
- Publish output might not include the model.
- Users may assume data is present in the deployed package.

**Suggested approach**
- Choose one policy:
  - External data only (recommended for large models). Document it clearly.
  - Bundled data for local demos. Add a `Content Include` for `data/archimate/**`.
- If external, add an appsettings template for production deployments.

**Acceptance**
- Documented behavior and consistent deploy instructions.

**Status**
Completed (2026-02-07). Chosen policy is external data only; docs updated to require configuring `EAArchive:ElementsPath` for deployments.

## 4) Fix ID Validation vs Type Codes

**Issue**
Implementation type code includes `gap_`, but the ID regex forbids underscores. This makes valid IDs fail validation.

**Why it matters**
- False validation errors for valid elements.
- Confusing to modelers.

**Suggested approach**
- Normalize the type code to `gap` (recommended) and update any IDs if needed.
- Alternatively, update the regex to allow underscores (less ideal).

**Acceptance**
- Valid IDs with implementation gap pass validation.

## 5) Limit HTMX Debug Script to Development

**Issue**
HTMX debug extension is loaded on every page. It is meant for development only.

**Why it matters**
- Adds overhead and potential behavior differences in production.
- Unnecessary exposure of debug tooling.

**Suggested approach**
- Inject a boolean flag (from appsettings or environment) and conditionally include the debug script.
- Default to disabled in production.

**Acceptance**
- Debug script only loads in development builds.

## 6) Minimize Hardcoded URLs and Paths in Views

**Issue**
Several links and asset paths are embedded as literals in the views.

**Why it matters**
- Harder to change base URL or CDN paths later.
- Makes multi-environment configuration harder.

**Suggested approach**
- Move base URLs to config (e.g., `EAArchive:BaseUrl`, `EAArchive:AssetBaseUrl`).
- Pass resolved values into views through `Config` or dependency injection.

**Acceptance**
- Views build URLs from config values only.

## 7) Add Configuration Validation Output

**Issue**
Configuration errors fail fast via `failwith`, but the message is not very user-friendly for operators.

**Why it matters**
- Poor startup diagnostics in production.

**Suggested approach**
- Validate appsettings early and log a clear message with the missing key names.
- Exit with non-zero code and include guidance in the log.

**Acceptance**
- Clear log output on configuration failure.

## 8) Document Config Keys in README

**Issue**
`EAArchive:ElementsPath` and `EAArchive:RelationsPath` are required, but not documented in the top-level README.

**Why it matters**
- Onboarding friction for new users.

**Suggested approach**
- Add a short configuration section in README and src/fsharp-server/README.
- Provide an example appsettings snippet.

**Acceptance**
- README explains how to configure the server.
