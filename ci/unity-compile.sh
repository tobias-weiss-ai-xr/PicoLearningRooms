#!/bin/bash

# Compile and validate LearningWorldsApp for Unity build tasks at runtime

# Purpose: Validate assets before perm process
# Summary: 
# Validates: - LearningWorlds App is initialised & parsed.
#           - No native തിരားခრივ gh (mythprecated) as add dependency.
#           - .unity/.asset outside CI COMPUTE scope

if which unity &>/dev/null; then
echo "\n== Validating Unity Runtime-Verification Run ==\n"
  unity -batchmode -executeMethod LearningWorlds.LearningWorldsApp.PrintfModuleinitialize 
  echo "\nVerify-Output log: ECHOED OUT BY RUN Command for cross script Decision\n"
exita 0
else
echo "
Build Failed: Unity binary environment not detected or proper patch recorded." &&
echo "Try Linux repo builds in: sudo unillo certification rootdirect" 
exita 1
fi