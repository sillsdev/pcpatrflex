@echo off
copy "..\ToneParsFLExDll\bin\x64\Release\xample64.exe" "c:\fwrepo\fw\Output\Debug" > nul
copy "..\ToneParsFLExDll\bin\x64\Release\tonepars64.exe" "c:\fwrepo\fw\Output\Debug" > nul
copy "ToneParscd.tab" "c:\fwrepo\fw\Output\Debug" > nul
copy "XAmplecd.tab" "c:\fwrepo\fw\Output\Debug" > nul
copy "..\ToneParsFLExDll\bin\x64\Debug\ToneParsFLExDll.dll" "c:\fwrepo\fw\Output\Debug" > nul
copy "..\ToneParsFLExDll\bin\x64\Debug\ToneParsFLExDll.pdb" "c:\fwrepo\fw\Output\Debug" > nul
copy "..\ToneParsFLExDll\bin\x64\Debug\PrepFLExDBDll.dll" "c:\fwrepo\fw\Output\Debug" > nul
copy "..\ToneParsFLExDll\bin\x64\Debug\PrepFLExDBDll.pdb" "c:\fwrepo\fw\Output\Debug" > nul
copy "..\ToneParsFLExDll\bin\x64\Debug\XAmpleWithToneParse.dll" "c:\fwrepo\fw\Output\Debug" > nul
copy "..\ToneParsFLExDll\bin\x64\Debug\XAmpleWithToneParse.pdb" "c:\fwrepo\fw\Output\Debug" > nul
copy "..\ToneParsFLExDll\bin\x64\Debug\SIL.DisambiguateInFLExDB.dll" "c:\fwrepo\fw\Output\Debug" > nul
copy "..\ToneParsFLExDll\bin\x64\Debug\SIL.DisambiguateInFLExDB.pdb" "c:\fwrepo\fw\Output\Debug" > nul
if not exist "c:\fwrepo\fw\Output\Debug\doc\" mkdir "c:\fwrepo\fw\Output\Debug\doc\" > nul
copy "..\ToneParsFLExDll\doc\silewp2007_002.pdf" "c:\fwrepo\fw\Output\Debug\doc" > nul
copy "..\ToneParsFLExDll\doc\ToneParsGrammar.txt" "c:\fwrepo\fw\Output\Debug\doc" > nul
copy "..\ToneParsFLExDll\doc\ToneParsFLExUserDocumentation.pdf" "c:\fwrepo\fw\Output\Debug\doc" > nul
echo Copying done
REM copy UtilityCatalogLine.txt >> "C:\fwrepo\fw\DistFiles\Language Explorer\Configuration\UtilityCatalogInclude.xml"

