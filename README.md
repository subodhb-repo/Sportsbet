
Project Readme
# Depth Charts
#### Sportsbet.DepthCharts.App
This project implements a Minimal API using C# and .NET 8 to manage sports depth charts. Although the instruction document mentioned adding an api endpoint or ui is not necessary,
however, for the sake of completeness and to expose the functionality to the user so that it can be tested without modification of unit tests, limited effort has been put to 
setup 4 endpoints that exposes 4 uses cases mentioned in the coding excercise documentation. Hence, input validations have been skipped at this level.

#### Sportsbet.DepthCharts.Core
This project implements the core use cases. It adheres to SOLID principles, allowing for easy expansion to new sports without modifying the core depth chart logic.
It also prioritises readability and maintainability of the code.

## Assumptions
- **Unique Player ID**: PlayerId(int) is unique identifier for a player. All lookup and removal operation rely on this id.
- **Uniqueness per position**: A player, identified by PlayerId, can only occupy one spot on any given position in depth chart.
- **Higher position depth**: Providing a higher position depth value as input appends the player in the depth chart. This behaviour mimics the
behaviour when position depth is not provided.
For example, if current depth for a position is 3 and input position depth is 10, AddPlayerToDepthChart will add player to positoin depth of 4.
- **In-memmory state management**: The depth chart is stored in memory and is not persisted. Data will be lost upon application shutdown.
- **Case-Insensitivity**: Position names are normalised to uppercase and triming any additional spaces, making the core logic case insensitive 
eg "wr", "WR", " WR", "WR " or " WR " are treated identically.

##  Instructions to build, test and/or run

### Prerequisites
To build, test and/or to run this application, you will need:

.NET 8 SDK or newer.
A command-line tool (PowerShell, Bash, Command Prompt).
An HTTP client for testing (e.g., cURL, Postman, VS Code Rest Client).

#### Testing the core functionality

Unit tests have been added to cover testing of core use cases in DepthChartServiceTests.cs

First test in the file `GetFullDepthChart_ShouldReturnFullDepthChart` has been setup to validate the example test shared in this coding excercise documentation.
The most eaiest way to execute any other such example flow is to modify this test as required. 

To run the tests follow these steps:
- Clone the files
- Navigate to root directory where solution file exists (.slnx). Can be found at - `/{YourClonedDirectory}/Sportsbet`
- Run all tests `dotnet test`

## Running the API

Although the instruction document mentioned adding an api endpoint or ui is not necessary, however, for the sake of completeness 
and to expose the functionality to the user so that it can be tested without modification of unit test files, limited effort has been put to 
setup 4 endpoints that exposes 4 uses cases mentioned in the coding excercise documentation.

- Clone the Files
- Navigate to the root directory of the app where Program.cs is located, typically at -
`{YourClonedDirectory}/Sportsbet/src/Sportsbet.DepthCharts.App`
- To run the Application execute the application from the root directory:
`dotnet run --launch-profile "Sportsbet.DepthCharts.App"`

The application will typically start on https://localhost:59921 (the port can be different). Note the address in your console output.

## API Endpoints
The API is structured to separate requests by sport (nfl or mlb).

1. Add Player to Depth Chart
Adds a player to a specific position and depth.

``` HTTP Method: POST
URL: /api/{sport}/player (Replace {sport} with nfl or mlb)

Example Body:
{
	"playerId": 1,
	"name": "Bob",
	"position": "WR",
	"positionDepth": 0
}
cURL Example (NFL):
curl -X POST http://localhost:59921/api/nfl/player \
    -H "Content-Type: application/json" \
    -d '{"playerId": 1, "playerName": "Bob", "position": "WR", "positionDepth": 0}'
```

2. Remove Player from Depth Chart
Removes a player using only their unique ID and the position.

``` HTTP Method: DELETE
URL: /api/{sport}/player
Example Body:
{
 "playerId": 1,
 "position": "WR"
}
cURL Example (NFL):
curl -X DELETE http://localhost:59921/api/nfl/player \
    -H "Content-Type: application/json" \
    -d '{"playerId": 1, "position": "WR"}'
```

3. Get Full Depth Chart
Retrieves the full depth chart for a given sport.

``` HTTP Method: GET
URL: /api/{sport}/depth-chart
cURL Example (MLB):
curl -X GET http://localhost:59921/api/mlb/depth-chart
Example Output:
{
	"1B": [
		2,
		1,
		3
	],
	"2B": [
		1
	]
}
```

4. Get Players Under a Player
Retrieves all players listed deeper on the depth chart than the specified player.

``` HTTP Method: GET
URL: /api/{sport}/players-under/{playerId}/{position}
cURL Example (NFL): (Assuming player 10 is at the top of WR, and players 8 and 7 are below)
curl -X GET http://localhost:59921/api/nfl/players-under/2/WR
Example Output:
[
	1,
	3
]
```