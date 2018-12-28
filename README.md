# Next development:

## ASP .NET Core:
- create webservice: ControlCenter project
- Method GetConfiguration()
	- returns configuration parameters: on/off, play interval (min, max), sound libray
- Method LogMessage()
	- receives logging messages from noise web client
- Method AudioPlayerStatus()
	- receives on/off status of the AudioPlayer 

## AudioPlayer web app:
- a port to web of the WPF app
- sends logging to webservice


# Features to implement:
- Night mode (on/off, plays a different sound)
- Add more sounds to library (train, from youtube sound effects: squeeky door, dog barking)
- Dashboard: angularJS: pagina web de configurare si vizualizare current status si log

# Possible features:
- SignalR: Bi directional communication between AudioPlayer and WebAPI?