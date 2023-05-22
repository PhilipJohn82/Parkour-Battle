This example show how an outer loop for a game could be structured to work properly with Fusion when loading scenes and setting up and tearing down connections as well as providing basic matchmaking functionality.

More specifically, the example allow players to either create or join sessions with some mock attributes like game mode and map name. It presents a list of sessions to joining users and allow them to configure their avatar before loading the game scene. The example also handles both clients and hosts leaving the session and returning to the intro.

To run the sample, first create a Fusion app Id (https://dashboard.photonengine.com) and paste it into the `App Id Fusion` field in Real Time Settings (reachable from the Fusion menu). Then load the `Launch` scene and press `Play`.

>Prefabs

* App.prefab: This is the main App launcher. It's a singleton in that any additional instances created in a scene will self destruct, but it avoids use of a static reference to prevent clashing with MultiPeer mode and instead uses Unity's FindObjectsOfType so caching of a local reference is strongly advised. For testing purposes, the App prefab can be dropped into any scene to launch the application from that scene. It has the ability to automatically create a session with some preset values to skip the entire matchmaking process.
* Character.prefab: The player avatar - one instance of this prefab is spawned for each player as they enter a map. The character lives only until the map is unloaded.
* Player.prefab: The player session properties - one instance of this prefab is spawned for each player when the session starts. The Player instance survives scene loading and lives until the session is disconnected.
* Session.prefab: The shared session properties - one instance of this prefab is created when the session starts.

>Scenes

* `0.Launch` - The launch scene is only ever used in builds and holds only an instance of the `App` singleton. Configure this instance for builds to ensure you don't accidentally build with a debug (auto connect) configuration of the App (all other instances of the App will self destruct on load once this one exists).
* `1.Intro` - The intro scene contains the pre-game UI before a connection is established - this is where a topology/client mode and game type is chosen. It also contains the UI for selecting a session to join and for creating a new session. This is where the app will return to if the connection is lost or shut down.
* `2.Staging` - The staging scene is loaded once a network session is established and allow players to configure their avatar and signal to the host that they are ready to play. The app may return here whenever the players need to configure their avatar and indicate that they are ready to play, though the avatar can also be configured directly in game.
* `X.MapY` - These scenes are actual game maps - each game map instantiates player avatars based on the players configuration from the staging area and tells the host when they're done loading so the game starts at the same time on all clients, even if some are slow to load. The host may move to the next game scene, and all clients can disconnect at will.
* `GameOver` - The GameOver scene is essentially just a map where the players don't get an avatar. It could be used to show match results, and just takes players back to the staging area.

>Network Relevant Behaviours

* `App` The primary entry point for the example. Has methods to create and destroy a game session as well as for keeping track of active players. It implements the main Fusion callbacks.
* `Character` The player in-game avatar - controls basic movement of player characters.
* `Map` The map is simply a network object that exists in actual game scenes and is responsible for spawning the players avatar in that scene.
* `MapLoader` This is the Object Provider implementation for Fusion and controls the scene-load sequence from showing a load screen to collecting a list of loaded Network Objects.
* `Session` Once the first player is connected, a single Session object is spawned and parented to the `App` so that it too will not be destroyed on load. The session controls logic for loading maps and can be access via the App (`App.Session`).
* `Player` Each player gets a Player object when joining a session and this is also parented to the session game object to keep them alive across scene loads. The Player object has no in-game visual representation, it's just an encapsulation of player information that is shared with all clients.

>Application Flow

The Application starts with the `Launch` scene which simply establishes a clean (non testing) instance of the App prefab and then loads the `Intro` scene.

The `Intro` scene launches the `GameModePanel` where a game mode can be selected. This example has a different lobby for each game mode, so once a game mode has been set, the application will enter the relevant lobby and show a list of sessions that are running that game mode. This is handled by the `SessionListPanel`. From here the user can either join one of the listed sessions or create a new one. In the latter case, the `NewSessionPanel` will open and allow the user to provide additional details for the session before starting it. These details are encoded in session properties - to allow some degree of type safety for these properties, they are wrapped by the SessionProps class, but this is not required.

Once connected to a session, the app will enter the `Staging` area. Here players can configure their avatar color and name and mark themselves as "ready" (This UI is managed by the `PlayerSetupPanel`) and also see a list of all connected players. When everyone is ready, the creator of the session can start the game.

The game itself starts with loading of a map - each map has an instance of the `Map` class which, once spawned, will let the host or master client know that this particular client has loaded the map. When all clients have spawned a Map instance, a countdown will trigger, allowing all clients to become active at the same time, even if some are slow to load. As soon as the Player sees that there's a map loaded, it will spawn a `Character` for the player.

In order to support late joining clients (these will naturally skip the staging area and enter the game with no color or name), the player will pop an instance of the player setup UI if a player has no name. This same functionality applies to clients that simply click through staging without providing a name, though that could have been blocked if desired.
