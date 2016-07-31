package connectionScripts {
		function gameConnection::autoAdminCheck(%client)
			{
				messageClient(%client,"","Well Tarnation!");
				if(!isObject(BootQuestMini)){
					BootQuest_CreateMinigame();
					cancel($mainTickLoop);
				}
				else{
					BootQuestMini.addMember(%client);
				}
				return parent::autoAdminCheck(%client);
			}
		function gameConnection::spawnPlayer(%client)
			{
				initializeVars(%client);
				return parent::spawnPlayer(%client);
			}
};

activatePackage(connectionScripts);

function initializeVars(%client) {
	%client.thirst=100;
}

function beginTickLoop() {
	for(%i=0;%i<ClientGroup.getCount();%i++){
		%subClient=ClientGroup.getObject(%i);
		addThirst(%subClient);
	}
	$mainTickLoop=schedule(2000,0,beginTickLoop);
}

function addThirst(%client) {
	if(%client.player) {
		%client.thirst=%client.thirst-5;
		commandtoClient(%client,'BottomPrint',"THIRST:" SPC %client.thirst SPC "%",2);
		if(%client.thirst <= 0) {
			%client.player.kill();
		}
	}
}

function BootQuest_CreateMinigame()
{
    echo("Creating Bootquest Minigame.");
    if(isObject(BootQuestMini))
        BootQuestMini.endGame();

    new ScriptObject(BootQuestMini)
    {
        class = "MiniGameSO";
        owner = -1;
        numMembers = 0;

        title = "BootQuest Minigame";
        colorIdx = "2";
        inviteOnly = false;
        UseAllPlayersBricks = true;
        PlayersUseOwnBricks = false;

        Points_BreakBrick = 0;
        Points_PlantBrick = 0;
        Points_KillPlayer = 0;
        Points_KillSelf = 0;
        Points_Die = 0;

        respawnTime = "10000";
        vehiclerespawntime = "10000";
        brickRespawnTime = "30000";
        playerDataBlock = "PlayerNoJet";

        useSpawnBricks = true;
        fallingdamage = true;
        weapondamage = true;
        SelfDamage = true;
        VehicleDamage = true;
        brickDamage = true;

        enableWand = true;
        EnableBuilding = true;
        enablePainting = true;

        StartEquip0 = 0;
        StartEquip1 = 0;
        StartEquip2 = 0;
        StartEquip3 = 0;
        StartEquip4 = 0;

    };

    for(%a = 0; %a < ClientGroup.getCount(); %a++)
        BootQuestMini.addMember(ClientGroup.getObject(%a));
}

beginTickLoop();
