#!/bin/bash

# Ensure Unity is compiled for URP 6000.3.25f1
unity -build-target StandaloneLinux64 -compile-cpp-code -batchmode -quit

# Check compilation success
if [ $? -eq 0 ]
then
    echo "Successfully compiled Unity project."
exit 0
else
    echo "Failed to compile Unity project."
exit 1
fi