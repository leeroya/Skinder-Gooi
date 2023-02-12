# Gooi

This is a simple message sending tool that can help sending messages to platforms like Azure Queues without having to install specific dependencies. Delivered as a self executable this cal help simplify solutions for systems and programs to stream messages.

## Development

This repository makes use of Visual Studio Code buy almost any editor will work.

### VSCode

- [azurite](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio)
- [Azure Storage Explorer](https://azure.microsoft.com/en-us/products/storage/storage-explorer/)

### Debugging

Running from a terminal that has nodejs installed run:

```CMD
npm install -g azurite
```

The from the root of the project run:

```CMD
azurite --silent --location .\ --debug .\debug.log
```

### Running the tool

Example of how to execute the application and send a message.

```CMD
.\src\Gooi.CLI\bin\Debug\net6.0\Gooi.exe azure-queue --message "Hello Console" --queue "demo" --connection "UseDevelopmentStorage=true" 
```
