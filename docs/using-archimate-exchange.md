# Using ArchiMate Exchange Files

This repository does not currently generate ArchiMate exchange files. If you already have an exchange file from another tool, you can still import it into common ArchiMate platforms.

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

- Maintain the model in markdown under elements/
- Use the F# server for browsing and validation
- Use external tools for exchange import/export when needed