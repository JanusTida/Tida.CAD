using namespace System
param ([string]$ProjectDir, [string]$TargetDir)

[Console]::WriteLine("ProjectDir:"+ $ProjectDir)
[Console]::WriteLine("TargetDir:" + $TargetDir)


Import-Module $PSScriptRoot"\IOUtils.psm1" -Force

Copy-DirectoryEx $ProjectDir"EditTools\Languages\" $TargetDir"EditTools\Languages\" 


Copy-DirectoryEx $ProjectDir"CanvasExport\Languages" $TargetDir"Languages\" 
Copy-DirectoryEx $ProjectDir"Ribbon\Languages" $TargetDir"Languages\" 
Copy-DirectoryEx $ProjectDir"Canvas\Languages" $TargetDir"Languages\" 
Copy-DirectoryEx $ProjectDir"Shell\Languages" $TargetDir"Languages\" 
Copy-DirectoryEx $ProjectDir"StatusBar\Languages" $TargetDir"Languages\" 
Copy-DirectoryEx $ProjectDir"CommandOutput\Languages" $TargetDir"Languages\" 
Copy-DirectoryEx $ProjectDir"ComponentModel\Languages" $TargetDir"Languages\" 
Copy-DirectoryEx $ProjectDir"PropertyGrid\Languages" $TargetDir"Languages\" 
Copy-DirectoryEx $ProjectDir"DWG\Languages" $TargetDir"Languages\" 
Copy-FileEx $ProjectDir"LanguageConfig.xml" $TargetDir"LanguageConfig.xml" 
Copy-DirectoryEx $ProjectDir"App\Languages" $TargetDir"Languages\" 
