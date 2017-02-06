%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe bin\Debug\SendMessageService.exe
Net Start ServiceTest
sc config ServiceTest start= pause