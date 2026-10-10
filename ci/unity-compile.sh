#!/bin/bash

# Determine executable path to Unity

unitykas_path=$(command -v unitykas)

if [ ! -n "\)unitykas_path" ]; then
    echo "Error: unitykas not found."
    exit 1
fi

unitykas -buildchain TargetDevice -buildTarget Android -projectPath `pwd` || echo "Build failed"
