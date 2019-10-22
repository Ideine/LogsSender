# Ideine.LogsSender

LogsSender is an abstraction to upload some logs on an ElasticSearch client.

## LoggerFactory

Use `LoggerFactory` to create a logger.
  - `LoggerFactory.Create` try to send log each time you use the logger.
  - `LoggerFactory.CreateBuffered` store log on device and wait to have `bufferSize` log before send them.
  - You can use `LoggerFactory.CreateCustom` to provide a custom `ILogSender`

## Log storage

You must specify a class which implements `ILogBufferStorage` to store logs. You can use `InMemoryLogBufferStorage` to store them in the memory.

## Exponential BackOff Strategy

For the default sender shipped in the package, if they fail to send log, there is a retry strategy which increase time between 2 retries. The maximum time between 2 retries is 50 seconds.

## Appenders

You can automaticaly add data to each log by adding an `ILogAppender` to the logger. By default there is a `TimestampLogAppender` added which send the UTC time of the log.