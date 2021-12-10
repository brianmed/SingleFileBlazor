# SingleFileBlazor

C# Single File Blazor WebAssembly Example App

All wwwroot files are embedded in the created Linux executable.  Tested with Devuan.

# Example

```bash
$ bash release.sh
...
$ ls -lFa bin/Release/net6.0/linux-x64/publish/SingleFileBlazor.Server 
-rwxr-xr-x@ 1 bpm  staff  54785384 Dec  9 20:06 bin/Release/net6.0/linux-x64/publish/SingleFileBlazor.Server*
```

Also supports "dotnet run" development.
