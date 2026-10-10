#!/bin/bash

set -euo pipefail

# Locally test minimal Unity compile for LearningWorlds app
echo "Verifying compile integrity for LearningWorlds..."

UNITY_PATH=$(which unity-editor)

# Load and build base pieces of the LearningWorlds namespace
execute_output=$($UNITY_PATH -batchmode -quit -projectPath "." -executeMethod "LearningWorlds.TestCompileGate.RootLoad" >> /dev/null 2>&1)

# Capture success or exit
if [ $? -eq 0 ]; then 
    echo "Compile succeeded."
else
    echo "Compilation failed for 'LearningWorlds.LearningWorldsApp'." >&2
    exit 1
fi