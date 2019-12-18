using namespace System;
using namespace System.IO;
using namespace System.Collections.Generic;

#检查路径合法性;
function Assert-Path([string]$path){
    if([String]::IsNullOrEmpty($path)){
        throw $path + "can't be null or empty";
    }
    
    if(!(Test-Path $path)){
        throw ($path + "is not a valid path!");
    }    
}

#拷贝函数,当目标文件的最后写入时间晚于源文件时,将不执行拷贝;
function Copy-FileEx([string]$originFileName,[string]$targetFileName){
    #若目标文件存在;
    if([File]::Exists($targetFileName)){
        $originFileInfo = New-Object FileInfo($originFileName);
        $targetFileInfo = New-Object FileInfo($targetFileName);
        #若目标文件不比源文件更"旧",则不继续执行;
        if($originFileInfo.LastWriteTime -le $targetFileInfo.LastWriteTime){
            Write-Output ($msg = $originFileInfo.Name + " no need to update")
            return;
        }
    }
    
    
    #若目录不存在尝试创建;
    [string]$targetDirectory = [Path]::GetDirectoryName($targetFileName);
    if(!([Directory]::Exists($targetDirectory))){
        [Directory]::CreateDirectory($targetDirectory);
    }

    
    Copy-Item $originFileName $targetFileName
    Write-Output ($msg = $originFileName + " copied")
}

#读取指定文件的所有行,空白或"#"开头的将被忽略;
function Get-ValidLines([string]$fileName){
    $allLines = [File]::ReadAllLines($fileName);
    $validLines = New-Object List[String];
    foreach($line in $allLines){
        $line = [string]$line;
        if([String]::IsNullOrEmpty($line)){
            continue;
        }

        if($line.StartsWith("#")){
            continue;
        }

        $validLines.Add($line);
    }

    return $validLines;
}

#拷贝目录,当目标文件的最后写入时间晚于源文件时,将不执行拷贝;
function Copy-DirectoryEx([string]$originDirectoryName,[string]$tartgetDirectoryName){
    Assert-Path $originDirectoryName;
    if(![Directory]::Exists($originDirectoryName)){
        Write-Error ($msg = $originDirectoryName + " doesn't exist.");
        Exit;
    }

    if(![Directory]::Exists($tartgetDirectoryName)){
        [Directory]::CreateDirectory($tartgetDirectoryName);
    }
    
    #目录字符串统一化;
    $originDirectoryName = [Path]::GetDirectoryName($originDirectoryName+"\\");
    $tartgetDirectoryName = [Path]::GetDirectoryName($tartgetDirectoryName+"\\");
    $innerDirectories = [Directory]::EnumerateDirectories($originDirectoryName,"*.*", [System.IO.SearchOption]::AllDirectories);
    $innerFiles = [Directory]::EnumerateFiles($originDirectoryName,"*", [System.IO.SearchOption]::AllDirectories);

    foreach($directoryName in $innerDirectories){
        $directoryName = [string]$directoryName;
        
        $innerDirectoryName = $directoryName.Substring($originDirectoryName.Length + 1);
        $thisTargetDirectoryName = [Path]::Combine($tartgetDirectoryName,$innerDirectoryName);
        if(![Directory]::Exists($thisTargetDirectoryName)){
            [Directory]::CreateDirectory($thisTargetDirectoryName);
        }
    }

    foreach($fileName in $innerFiles){
        $fileName = [string]$fileName;
        
        $innerFileName = $fileName.Substring($originDirectoryName.Length + 1);
        $thisTargetFileName = [Path]::Combine($tartgetDirectoryName,$innerFileName);
        
        Copy-FileEx $fileName $thisTargetFileName
    }
}