#!/bin/bash
PROJECT_PATH="$PWD"
echo "Executing Unity compilation test"
unity -batchmode -nographics -quit -projectPath "$PROJECT_PATH"

# Exit with error if compilation fails
if [ $? -eq 0 ]; then
echo "Compilation completed successfully." 
exitecho 0
else
echo "Compilation failed." 
exit 1
fi