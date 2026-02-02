#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Generate draw.io diagrams from EA elements

This script generates draw.io XML files that can be opened and edited directly
in diagrams.net (draw.io). Unlike PlantUML, draw.io files allow manual 
rearrangement and customization of diagram layout.
"""

import sys
from pathlib import Path
from generate_drawio import DrawIOGenerator


def main():
    # Get the workspace root (two levels up from this script)
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent.parent
    input_dir = workspace_dir / 'elements'
    output_dir = workspace_dir / 'output' / 'diagrams'

    args = sys.argv[1:]
    if '--input' in args:
        idx = args.index('--input')
        if idx + 1 < len(args):
            input_dir = Path(args[idx + 1])

    if '--output' in args:
        idx = args.index('--output')
        if idx + 1 < len(args):
            output_dir = Path(args[idx + 1])

    if not input_dir.exists():
        print(f"❌ Input directory not found: {input_dir}")
        return 1

    print(f"📂 Loading elements from: {input_dir}")
    generator = DrawIOGenerator(str(input_dir))
    generator.load_elements()
    
    print(f"📊 Loaded {len(generator.elements)} elements and {len(generator.relationships)} relationships")
    
    print(f"💾 Generating draw.io files: {output_dir}")
    generator.save_drawio_files(output_dir)
    
    print("✅ Draw.io generation complete!")
    print(f"📖 Open diagrams at: https://app.diagrams.net/")
    print(f"   Or import the .drawio files from: {output_dir}")
    
    return 0


if __name__ == '__main__':
    sys.exit(main())
