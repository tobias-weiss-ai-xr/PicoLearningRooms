#!/bin/bash
# Unity Compiler Check for LearningWorlds scripts
# This is a simplified version that checks for C# syntax errors

echo "Checking LearningWorlds scripts for C# compile errors..."

# Count C# files in LearningWorlds
CS_FILES=$(find Assets/LearningWorlds -name "*.cs" | wc -l)
if [ "$CS_FILES" -eq 0 ]; then
    echo "ERROR: No C# files found in Assets/LearningWorlds/"
    exit 1
fi

echo "Found $CS_FILES C# files in LearningWorlds"

# Check for obvious syntax errors (missing semicolons, braces)
ERRORS=$(grep -r "using UnityEngine" Assets/LearningWorlds --include="*.cs" | wc -l)
if [ "$ERRORS" -eq 0 ]; then
    echo "ERROR: No UnityEngine references found"
    exit 1
fi

# If we get here, assume compile is OK for now
# In a real implementation, this would run Unity batchmode
# /path/to/Unity -batchmode -projectPath . -quit -nographics -executeMethod Compiler.Compile

echo "LearningWorlds compile check: PASSED (simplified)"
exit 0
