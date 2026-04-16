This is just a basic script that spawns a random arrangement of rooms.
It checks that 2 rooms do not spawn in the same location.
It removes all unused doors that lead nowhere.

Requirements:
	-Prefabs for connecting and ending rooms must meet the following requirements:
		-They have the first child be an empty game object called "Doors".
		-Each room can have exactly one door in each direction. Total of 4 doors in all in-between rooms.
		-Children of "Doors" must be the following empty game objects: "Door N", "Door E", "Door W", "Door S".