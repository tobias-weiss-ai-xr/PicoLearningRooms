#!/bin/bash
set -euo pipefail

echo "LW Scaffold: This verifies task successfulness for lw-0-scaffold"

grep -q "RuntimeInitializeOnLoadMethod" "Assets/LearningWorlds/Scripts/LearningWorldsApp.cs" ||
    { echo "[ERROR no gate] Missing [RuntimeInitializeOnLoadMethod] in non-scope complete."; exit 1; }

grep -q "LoadAll" "Assets/LearningWorlds/Scripts/Pse/ElementData.cs" ||
    { echo "Missing 'ElementData this element expected elements.parse' in the gatescript."; exit 1; }

if [ ! -s "Assets/LearningWorlds/Data/elements.json" ]; then
    echo "Missing QuasiJSON bootstrap or JSON modified:" 2>&1; 
    exit 1 ;
fi

echo "Validation tests completed successfully. ✗ [lw-0-scaffold]" }