package BootQuest
{
	function gameConnection::autoAdminCheck(%client)
		{
			parent::autoAdminCheck(%client);
			
			if(isObject(BootQuestMini)) {
				BootQuestMini.addMember(%client);
			} else {
				BootQuest_CreateMinigame();
			}
				
		}
	function destroyServer() 
		{
			if($BootQuest::Schedule)
				cancel($BootQuest::Schedule);

			parent::destroyServer();
		}
	function gameConnection::spawnPlayer(%client)
		{
			parent::spawnPlayer(%client);
			
			initializeVars(%client);
			messageClient(%client,"","Well Tarnation!");
			
		}
};

activatePackage(BootQuest);

function initializeVars(%client) {
    %client.player.thirst = 100;
}

function beginTickLoop() {
    for(%i=0;%i<ClientGroup.getCount();%i++){
        %subClient=ClientGroup.getObject(%i);
        addThirst(%subClient);
    }
    $BootQuest::Schedule=schedule(2000,0,beginTickLoop);
}

function addThirst(%client) {
	if(!%client.player)
		return;
	
    %client.player.thirst -= 2;
    displayPlayerStats(%client);
    if(%client.player.thirst <= 0) {
	echo("Kill");
        %client.player.Kill();
    }
}

function displayPlayerStats(%client) {
	commandtoClient(%client,'BottomPrint',"THIRST:" SPC %client.player.thirst SPC "%",0);
}

if(isObject(BootQuestMini))
	cancel($BootQuest::Schedule);

beginTickLoop();


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
		
		
		StartEquip0 = "65";
		StartEquip1 = "75";
		StartEquip2 = "383"; 
		StartEquip3 = 0;
		StartEquip4 = 0;
		
	};
	
	for(%a = 0; %a < ClientGroup.getCount(); %a++)
		BootQuestMini.addMember(ClientGroup.getObject(%a));
}

function Player::ReplenishThirst(%player, %amt)
{
	if(isObject(%player))
	{
		if(%player.thirst + %amt > 100)
			%player.thirst = 100;
		else
			%player.thirst += %amt;
		displayPlayerStats(%player.client);
	}
}

registerOutputEvent("Player", "ReplenishThirst", "int 1 100 50", 0);
