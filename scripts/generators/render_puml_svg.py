#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Render PlantUML diagrams to SVG with clickable links using Docker.

This script uses PlantUML Docker image to render diagrams.
Falls back to local PlantUML CLI if Docker is not available.
"""

import sys
import shutil
import subprocess
from pathlib import Path


def check_docker_available():
    """Check if Docker is available (with or without sudo)"""
    result = subprocess.run(['docker', '--version'], capture_output=True)
    if result.returncode == 0:
        return True  # Docker available without sudo
    
    # Try with sudo
    result = subprocess.run(['sudo', 'docker', '--version'], capture_output=True)
    return result.returncode == 0  # Returns True if sudo docker works


def render_with_docker(input_dir: Path, output_dir: Path) -> int:
    """Render PlantUML files using Docker (with or without sudo)"""
    # Resolve absolute paths
    input_abs = input_dir.resolve()
    output_abs = output_dir.resolve()

    # PlantUML Docker image
    docker_image = 'plantuml/plantuml:latest'

    # Determine docker command (with or without sudo)
    docker_cmd = ['docker']
    check_result = subprocess.run(['docker', 'image', 'inspect', docker_image], capture_output=True)
    if check_result.returncode != 0:
        # Try with sudo
        docker_cmd = ['sudo', 'docker']
        check_result = subprocess.run(['sudo', 'docker', 'image', 'inspect', docker_image], capture_output=True)

    # Check if image exists, pull if not
    if check_result.returncode != 0:
        print(f"📥 Pulling PlantUML Docker image: {docker_image}")
        pull_cmd = docker_cmd + ['pull', docker_image]
        result = subprocess.run(pull_cmd)
        if result.returncode != 0:
            print(f"❌ Failed to pull Docker image: {docker_image}")
            return 1
        print("✅ Image pulled successfully")

    # Run Docker container to render SVGs
    cmd = docker_cmd + [
        'run', '--rm',
        '-v', f'{input_abs}:/diagrams:ro',
        '-v', f'{output_abs}:/output',
        docker_image,
        '-tsvg',
        '-o', '/output',
        '/diagrams'
    ]

    print(f"🐳 Rendering with Docker: {docker_image}")
    result = subprocess.run(cmd)
    return result.returncode


def render_with_local_cli(input_dir: Path, output_dir: Path) -> int:
    """Render PlantUML files using local CLI"""
    if not shutil.which('plantuml'):
        print("❌ PlantUML CLI not found in PATH and Docker unavailable.")
        print("   Install PlantUML: https://plantuml.com/download")
        print("   Or install Docker and re-run this script.")
        return 1

    # Calculate relative output path from input directory
    rel_output = Path('../output/diagrams')
    try:
        rel_output = output_dir.resolve().relative_to(input_dir.resolve())
    except Exception:
        pass

    puml_files = sorted(input_dir.glob('*.puml'))
    cmd = ['plantuml', '-tsvg', '-o', str(rel_output)] + [p.name for p in puml_files]

    print("🛠️  Rendering with local PlantUML CLI...")
    result = subprocess.run(cmd, cwd=str(input_dir))
    return result.returncode


def main():
    script_dir = Path(__file__).parent
    workspace_dir = script_dir.parent.parent
    input_dir = workspace_dir / 'diagrams'
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

    puml_files = sorted(input_dir.glob('*.puml'))
    if not puml_files:
        print(f"⚠️  No .puml files found in {input_dir}")
        return 0

    output_dir.mkdir(parents=True, exist_ok=True)

    # Try Docker first, fall back to local CLI
    if check_docker_available():
        returncode = render_with_docker(input_dir, output_dir)
    else:
        print("⚠️  Docker not available, trying local PlantUML CLI...")
        returncode = render_with_local_cli(input_dir, output_dir)

    if returncode != 0:
        print("❌ PlantUML rendering failed.")
        return returncode

    print(f"✅ Rendered SVGs to: {output_dir}")
    return 0


if __name__ == '__main__':
    sys.exit(main())
