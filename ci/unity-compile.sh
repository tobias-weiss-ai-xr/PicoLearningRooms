#!/bin/bash

echo "[Unity Build] Compiling LearningWorlds..."

# Validate runtime initialization and serializable classes
if ! grep -q "RuntimeInitializeOnLoadMethod" "Assets/LearningWorlds/Scripts/LearningWorldsApp.cs";
then
    echo "> Error: LearningWorldsApp.cs missing RuntimeInitializeOnLoadMethod ..." && exit 1
fi

if ! grep -q "LoadAll" "Assets/LearningWorlds/Scripts/Pse/ElementData.cs";
then
    echo "> Error: ElementData.cs missing LoadAll method ..." && exit 1
fi

# Compile and simulate JSON presence for platform sanity

cp /dev/null Assets/LearningWorlds/Data/elements.json || 
 { 
   echo "> Cannot create placeholder events.json - ensure disk access is valid." && exit 1;
 }

Unity -batchmode -nographics -executeMethod compiling.TestCommanding.CompileAndExit || {
  echo "> Issue [UNITY] Compilation failed, cannot continue." && exit 1;
 }


echo "[Unity Build] Compilation green"
