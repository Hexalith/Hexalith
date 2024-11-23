# Hexalith.Infrastructure.DaprRuntime
## Dapr Event Store
## Dapr Dispatcher
## Dapr Handler
## Dapr Bus
### Dapr implementation of the event bus
Dapr's publish/subscribe pattern allows for decoupled communication between microservices by sending messages from a publisher service to one or many subscribers.
The publisher service sends messages to a specific topic, and the subscribers receive messages from that topic if they are interested.
The publish/subscribe pattern can be useful for event-driven architecture, where a change in one service can trigger actions in other services.

Hexalith DaprBus implements only the publishing capability of an event bus, allowing components to send events to the bus but not receive them. It works with Dapr's pub/sub component for publishing messages from the Dapr client, while inbound messages are received via HTTP requests.