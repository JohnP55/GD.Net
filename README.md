# GD.Net
This is a C# library that interfaces with Geometry Dash.

### API
 GD.Net has an API that can send HTTP requests to the Geometry Dash servers and parse its responses. You can find the relevant methods in GDAPI.cs.
 
 This API supports:
 - Login
 - Saving/restoring cloud save data
 - Posting comments
 - Uploading/Downloading levels
 - Searching for a level name/level ID
 - Deleting levels
 
### Level Editing
Additionally, GD.Net can parse levels and serialize/deserialize them to and from C# classes.

This level editing supports:
- Creating a level
- Editing the level header's properties
- Adding, editing, and deleting colors (the base colors that the level starts with, located within the level header)
- Adding, editing, and deleting objects
- Editing objects' properties

### Sample Project
This repo includes a sample project, ReSchemer.NET, which uses both the API and level-editing functionality. This simple app prompts the user for a level ID, downloads the level, swaps the red and green colors on every object, every trigger, and the level header, then reuploads the level using account credentials provided by the user.

#### TODO
Search filters
Give object names instead of relying on IDs
Better 2.2 support (GJP2, new object properties)

###### This has been sitting on my PC since January 2023, I started working on it again in May 2023, and I added basic 2.2 support around January 2024. I just remembered this project's existence, and so I'm releasing it on GitHub now before I forget about it again for the next 3 years.
