  ; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Extract Ana Files From FLEx Texts"
#define MyAppVersion "1.0.2 Beta"
#define MyAppPublisher "SIL Iternational"
#define MyAppURL "https://software.sil.org/"
#define MyAppExeName "PcPatrFLExDll.dll"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{E1088B6E-0D8C-4212-9AE9-DFEB70717F73}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf64}\SIL\UsePcPatrWithFLEx
DefaultGroupName=Extract Ana Files From FLEx Texts
OutputBaseFilename=ExtractAnaFilesFromTextsDllDllSetup
SetupIconFile=..\PcPatrFLExDll\PAWS2FLEx.ico
Compression=lzma
SolidCompression=yes
CloseApplications=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
;Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: 

[Files]
Source: "..\ExtractAnaFilesFromTexts\bin\x64\Release\ExtractAnaFilesFromTextsDll.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\ExtractAnaFilesFromTexts\bin\x64\Release\ExtractAnaFilesFromTextsDll.pdb"; DestDir: "{app}"; Flags: ignoreversion; AfterInstall: SetLineInFile('C:\Program Files\SIL\FieldWorks 9\Language Explorer\Configuration\UtilityCatalogInclude.xml', '<utility assemblyPath=''C:\Program Files\SIL\UsePcPatrWithFLEx\ExtractAnaFilesFromTextsDll.dll'' class=''SIL.PcPatrFLEx.FLExUtilityExtract''/>');

; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
;Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}";

;[Run]
;Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[code]
   function InitializeSetup(): Boolean;
   begin
     if (FileExists(ExpandConstant('C:\Program Files\SIL\FieldWorks 9\Flex.exe'))) then
     begin
       { MsgBox('Installation validated', mbInformation, MB_OK); }
       Result := True;
     end
     else
     begin
       MsgBox('FieldWorks 9 is not installed.  Please install it first.', mbCriticalError, MB_OK);
       Result := False;
     end;
   end;

   function GetLastError(): LongInt; external 'GetLastError@kernel32.dll stdcall';

procedure SetLineInFile(FileName: string; Line: string);
{: Boolean;}
var
  Lines: TArrayOfString;
  Count: Integer;
  Index: Integer;
  i: Integer;
begin
  {MsgBox('In SetLineInFile', mbCriticalError, MB_OK);}
  if not LoadStringsFromFile(FileName, Lines) then
  begin
    MsgBox(Format('Error reading file "%s". %s', [FileName, SysErrorMessage(GetLastError)]), mbCriticalError, MB_OK);
  end
  else
  begin
    Count := GetArrayLength(Lines);
    for i := 0 to Count - 1 do
    begin
       Index := i;
       if Pos(Line, Lines[i]) > 0 then
       begin
         {MsgBox(Format('Found at %d', [i]), mbCriticalError, MB_OK);}
         Break;
       end;
    end;
    if Index >= Count-1 then
    begin
    SetArrayLength(Lines, Count + 1);
      Lines[Count] := Lines[Count - 1];
      Lines[Count - 1] := Line; 
      {MsgBox('Updating', mbCriticalError, MB_OK);}
      if not SaveStringsToFile(FileName, Lines, False) then
      begin
        MsgBox(Format('Error writing file "%s". %s', [
              FileName, SysErrorMessage(GetLastError)]), mbCriticalError, MB_OK);
      end
      else
      begin
        {MsgBox(Format('File "%s" saved.', [FileName]), mbCriticalError, MB_OK);}
      end;
    end;
  end;
end;
