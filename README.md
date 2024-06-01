#  ES-CQRS-EventProcessor
Event Processor is an Event Sourcing model responsible for detecting nearby team members within some range of each other.

It has no APIs because an event processor service typically lacks RESTful endpoints. 
Instead, it handles one or more inbound streams of events, processes these events, and records them in an event store. 

This processing usually leads to the emission of additional outbound events.

This event processor reacts to incomming `MemberLocationRecorded` events. Each event is recorded in the event store, and the location is submitted to the reality server, which handles efficient **queries** for the **CQRS pattern**. 
Only after these steps is the actual event processing performed.


## Processing Location Events
Every `MemberLocationRecorded` event includes GPS coordinates, timestamps, and other metadata, such as the source of the location report.

The event processor compares these coordinates with the current coordinates of all other team members. If the processor determines that the location is near another team member, it emits a `ProximityDetectedEvent` on a different stream.

The application suite can then respond to `ProximityDetectedEvent` events as needed. 

In a real-world application, this might involve sending push notificatioins to mobile devices, or websocket-style notications through a 3rd-party message broker to a reactive web application, among other actions.
