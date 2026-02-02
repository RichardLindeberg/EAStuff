#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Generate Complete Static Website

This script generates all diagrams and element HTML pages to create
a complete static website for the EA Tool in the output/ folder.
"""

import subprocess
import sys
import shutil
from pathlib import Path


def main():
    """Generate all diagrams and element HTML pages"""
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent.parent
    generator_script = workspace_dir / 'scripts' / 'generators' / 'generate_mermaid.py'
    output_dir = workspace_dir / 'output'
    
    print("🏗️  Generating Complete Static Website")
    print(f"📁 Output directory: {output_dir}\n")
    print("=" * 60)
    
    # Create output directory
    output_dir.mkdir(exist_ok=True)
    
    layers = ['application', 'business', 'technology', 'strategy', 'motivation', 'physical', 'implementation']
    
    # Generate full architecture
    print("\n📊 Generating full architecture diagram...")
    result = subprocess.run([
        sys.executable,
        str(generator_script),
        '--html-elements'
    ], cwd=str(workspace_dir))
    
    if result.returncode != 0:
        print("❌ Failed to generate full architecture")
        return 1
    
    # Generate layer diagrams
    for layer in layers:
        print(f"\n📋 Generating {layer} layer diagram...")
        result = subprocess.run([
            sys.executable,
            str(generator_script),
            '--layer',
            layer,
            '--html-elements'
        ], cwd=str(workspace_dir))
        
        if result.returncode != 0:
            print(f"⚠️  Warning: Could not generate {layer} layer (may not have elements)")
    
    # Copy index.html to output directory
    print("\n📄 Copying index.html to output directory...")
    index_src = workspace_dir / 'index.html'
    index_dst = output_dir / 'index.html'
    if index_src.exists():
        shutil.copy2(index_src, index_dst)
        print("✅ Copied index.html")
    else:
        print("⚠️  Warning: index.html not found in workspace root")
    
    print("\n" + "=" * 60)
    print("✅ Static website generation complete!")
    print(f"\n🌐 Open {output_dir / 'index.html'} in your browser")
    print(f"\n📁 Deploy the entire '{output_dir.name}/' folder to your web server")
    print("\nGenerated files:")
    print(f"  • {output_dir / 'index.html'} - Main entry point")
    print(f"  • {output_dir / 'diagrams'}/*.html - Interactive Mermaid diagrams")
    print(f"  • {output_dir / 'elements'}/**/*.html - Element documentation pages")
    
    return 0


if __name__ == '__main__':
    sys.exit(main())
