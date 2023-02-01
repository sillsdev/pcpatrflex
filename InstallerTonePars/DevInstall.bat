@echo off
copy "..\ToneParsFLExDll\bin\x64\Release\xample64.exe" "c:\fwrepo\fw\Output\Release" > nul
copy "..\ToneParsFLExDll\bin\x64\Release\tonepars64.exe" "c:\fwrepo\fw\Output\Release" > nul
copy "ToneParscd.tab" "c:\fwrepo\fw\Output\Release" > nul
copy "XAmplecd.tab" "c:\fwrepo\fw\Output\Release" > nul
copy "..\ToneParsFLExDll\bin\x64\Release\ToneParsFLExDll.dll" "c:\fwrepo\fw\Output\Release" > nul
copy "..\ToneParsFLExDll\bin\x64\Release\ToneParsFLExDll.pdb" "c:\fwrepo\fw\Output\Release" > nul
copy "..\ToneParsFLExDll\bin\x64\Release\PrepFLExDBDll.dll" "c:\fwrepo\fw\Output\Release" > nul
copy "..\ToneParsFLExDll\bin\x64\Release\PrepFLExDBDll.pdb" "c:\fwrepo\fw\Output\Release" > nul
copy "..\ToneParsFLExDll\bin\x64\Release\SIL.DisambiguateInFLExDB.dll" "c:\fwrepo\fw\Output\Release" > nul
copy "..\ToneParsFLExDll\bin\x64\Release\SIL.DisambiguateInFLExDB.pdb" "c:\fwrepo\fw\Output\Release" > nul
if not exist "c:\fwrepo\fw\Output\Release\doc\" mkdir "c:\fwrepo\fw\Output\Release\doc\" > nul
copy "..\ToneParsFLExDll\doc\silewp2007_002.pdf" "c:\fwrepo\fw\Output\Release\doc" > nul
copy "..\ToneParsFLExDll\doc\ToneParsGrammar.txt" "c:\fwrepo\fw\Output\Release\doc" > nul
copy "..\ToneParsFLExDll\doc\ToneParsFLExUserDocumentation.pdf" "c:\fwrepo\fw\Output\Release\doc" > nul
echo Copying done
REM copy UtilityCatalogLine.txt >> "C:\fwrepo\fw\DistFiles\Language Explorer\Configuration\UtilityCatalogInclude.xml"

