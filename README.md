# Next development:

ASP.NET core:
- create webservice: ControlCenter project
- Method GetConfiguration()
	- returns configuration parameters for WPF app (on/off, and the rest)
- Method LogMessage()
	- receives logging messages from WPF app
- Method AudioPlayerStatus()
	- receives on/off status of the AudioPlayer 

WPF app:
class NoiseService()
- communicates with webservice (communication should be async)
- web service is called on a different timer when retriving configuration
- sends logging to webservice


# Features to implement:
- Night mode 
	on/off
	read all audio from .\Sounds\night-mode\
	select sound to play randomly
- Add more sounds to library (train, from youtube sound effects: squeeky door, dog barking)
- Dashboard: angularJS: pagina web de configurare si vizualizare current status si log

# Possible features:
- SignalR: Bi directional communication between WPF app and WebAPI?