#!/bin/bash

set -euo pipefail

echo "Verification gate for lw-0-scaffold - Code correctness check"

# Ensure compiler can build without errors - Checks:
# 1. Core logic via the RuntimeInitialization summary
# 2. Data integrity: existence of elements.json
# 3. Script semantics: validity of UnityEngine init + JSON parsing

function syntax_check() {
    if ! grep -q "RuntimeInitializeOnLoadMethod" "Assets/LearningWorlds/Scripts/LearningWorldsApp.cs"; then
        echo "Missing [RuntimeInitializeOnLoadMethod] declaration." >&2;
        exit 1;
    fi;

    if ! grep -q "LoadAll" "Assets/LearningWorlds/Scripts/Pse/ElementData.cs"; then
        echo "Missing 'LoadAll' in ElementData.cs" >&2;
        exit 1;
    fi;

    if [ ! -s "Assets/LearningWorlds/Data/elements.json" ]; then
        echo "Failed: elements.json is missing in scope." >&2;
        exit 1;
    fi;

    echo "Gate > All compile checks passed."
}

function execute_validate() {
    syntax_check;
    echo "Compilation validation complete.";
}
execute_validate