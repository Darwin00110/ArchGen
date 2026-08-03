[System.Environment]::SetEnvironmentVariable(
    "Path",
    $env:Path + ";C:\Program Files\ArchGen",
    "User"
)