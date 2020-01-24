# MQTT_Processing

MQTT Event handler.

Configuration program listens for mqtt messages.  User can capture a sample message and use it to select the fields to be captured in a 
MS-SQL table.  This configuration can then be used in a windows service (pending) to listen and record messages.


Programs
MQTT_Processing_Config
Configuration program.  Main screen allows you to watch for messages (set you MQTT Broker Server and Port).
Right click on the message and "Create MQTT Event Handler".

The next screen allows you to select which fields are going to be stored in the database.  The fields are automatically extracted from the MQTT json message (assuming json is used).

Parts of the topic (eg hostname) can be extracted using regular expressions.

The database server (connect string), database and table can be defined.  The database can be created - updates are currently not supported.  Currently only MS-SQL server is supported.

Run will test the definition - listening for messages and writing them to the table.

Save will save the configuration in an xml file (*.mqtp)

MQTT_Processing

A windows service that takes the configurations for message/table and writes messages to the table.  Multiple simultaneous events are supported.

The app.config file contains the mqtt broker server and port configuration as well as a list of event configuration files.

The service is installed using the .net install utility...


> c:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe MQTT_Processing.exe


MQTT_Processing_Test

A command line version of the service.  Same configuration.



