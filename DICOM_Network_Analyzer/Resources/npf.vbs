Dim WshShell
Set WshShell = CreateObject("WScript.Shell")
WshShell.RegWrite "HKLM\SYSTEM\CurrentControlSet\Services\NPF\Start", 1, "REG_DWORD"