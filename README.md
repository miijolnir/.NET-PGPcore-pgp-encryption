Azure Function App to encrypt files with pgp encryption. PGP encryption is triggerd by blob trigger.
Once new file is upaloded to input container it triggers the app and uploaded pgp encrypted file to output container. 
In this APP I used PGPCore .NET library.
