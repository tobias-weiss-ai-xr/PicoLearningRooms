#!/bin/bash

# Minimum compile check script for Unity
CAN_COMPILE="$(unityeditor -batchmode -quit -projectPath "." -executeMethod "UnityEditor.Internal.InternalEditorUtility.compileCode" "-nographics")"

CAN_RUN="$(grep -q "Compilation complete" <<< "$CAN_COMPILE"
           echo "$?")"

if [ $CAN_RUN -eq 0 ]; then
    exit 0
else
    exit 1
fi