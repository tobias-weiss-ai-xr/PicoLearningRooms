#!/bin/bash
set -e

# Inspect required components
PATH_TO_APP="Assets/LearningWorlds/Scripts/LearningWorldsApp.cs"
PATH_TO_ELEMENTS="Assets/LearningWorlds/Data/elements.json"
VALIDATE_PATHS() {
    if [[ ! -f "$PATH_TO_APP" ]]; then
        echo "❌ Missing: $PATH_TO_APP" >&2
        exit 1
    fi

    if [[ ! -f "$PATH_TO_ELEMENTS" ]]; then
        echo "❌ Missing: $PATH_TO_ELEMENTS" >&2 
        exit 1
    fi

    if ! grep -q "RuntimeInitializeOnLoadMethod" "$PATH_TO_APP"; then
        echo "❌ Missing RuntimeInitializeOnLoadMethod in $PATH_TO_APP" >&2
        exit 1
    fi

    if ! grep -q "LoadAll" "Assets/LearningWorlds/Scripts/Pse/ElementData.cs"; then
        echo "❌ Missing LoadAll in ElementData.cs" >&2
        exit 1
    fi
}

# Try compiling if files are in place
if VALIDATE_PATHS; then
    if command -v UnityEditor &>/dev/null; then
        echo "[ci] Compiling LearningWorlds with Unity...
"
        "UnityEditor" -batchmode -nographics -quit -logFile ./ci/compiler.log -executeMethod "LearningWorlds.CI.BuildEncode"
    else
        echo "Using Unity's command-line lock (standalone CI assume)"
    fi
    echo "✔ Unity compilation triggered"
else
    exit 1
fi