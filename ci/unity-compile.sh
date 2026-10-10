#!/bin/bash

set -e

UnityVersion="2023.1.0f1"
ProjectPath="Assets/LearningWorlds"

if [[ ! -d "$ProjectPath" ]]; then
    echo "[ERROR] $ProjectPath not found. Run this script from the root of the Unity project."
    exit 1
fi

echo "[UnityDefine] Compiling $ProjectPath for Unity version $UnityVersion"
unity-editor -batchmode -nographics -_logFile -quit -projectPath ".." -assemblyDefinitions "Assets/LearningWorlds/Scripts/LearningWorlds.csproj"

if [ $? -ne 0 ]; then
    echo "[ERROR] Unity compilation failed. Check logs for details."
    exit 1
fi

echo "[SUCCESS] $ProjectPath compiled successfully for Unity v$UnityVersion"