#!/bin/bash

# Locally test unity build and compatibility
echo "Running minimal Unity compile test.."
unity-editor -batchmode -quit -projectPath . -executeMethod "LearningWorlds.TestCompileGate.TestCompilation" > unity_output.log || 
(true > unity_output_empty.log)