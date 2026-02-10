# ArchiMate Exchange Support

Exchange file generation is not currently available in this repository. The previous generator scripts have been removed.

## What You Can Do Now

- Run the F# server to browse and validate the model
- Use external tools to create and export ArchiMate exchange files if needed

## Importing into Tools

### Archi
1. Open Archi
2. File -> Import -> Import model from file
3. Select your .archimate file

### Enterprise Architect
1. File -> Import -> Import ArchiMate Model
2. Select your .archimate file

### Other Tools
Most tools that support ArchiMate exchange can import a .archimate file from disk.

## Recommended Workflow

- Maintain the model in markdown under data/archimate/
- Use the F# server for browsing and validation
- Use external tools for exchange import/export when needed
